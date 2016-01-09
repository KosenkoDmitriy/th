using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;


public class States {

	public States(Game game) {
		isDone = false;
		state = new InitGame (game);

		rounds = new List<BetRound> () {
			new AnteRound(game),
			new PreflopRound(game),
			new FlopRound(game),
			new TurnRound(game),
			new RiverRound(game),
			new EndGame(game), // win panel (when close it > InitGame()
		};

		enumerator = rounds.GetEnumerator ();

//		foreach (var round in rounds) {
//			var item = round;
//			var str = item.ToString();
////			round.SubRound();
//		}

//		using (var round = rounds.GetEnumerator())
//		{
//			while (round.MoveNext())
//			{
//				// Do something with round.Current.
////				round.Current.SubRound();
//				var item = round.Current;
//				var str = item.ToString();
//			}
//		}

	}

	
	public void Next() {
		if (enumerator.MoveNext ()) {
			state = enumerator.Current;
			//			state.SubRound ();
			isDone = false;
		} else {
			isDone = true;
		}
	}

	IEnumerable<BetRound> rounds;
	private IEnumerator<BetRound> enumerator;
	public BetRound state;
	public bool isDone;

}



public interface IBetRoundState
{
	void SubRound ();
}

public abstract class AbstractBetRound {
	protected int subRoundMaxSize;
	protected int subRoundCurrent;
	protected double betMin;
	protected Game game;
	protected double betToStayInGame, pot;
}

public class BetRound : AbstractBetRound, IBetRoundState {
	public bool isWaiting; // wait for corountine

	public BetRound() {
		Init ();
	}

	public BetRound(Game game) {
		Init ();
		this.game = game;
	}

	private void Init() {
		this.subRoundMaxSize = Settings.betSubRoundMinSize;
		this.betMin = Settings.betPreflopFlop;
	}
	
	#region IBetRoundState implementation
	
	public virtual void SubRound ()
	{
		if (subRoundCurrent == 0) {
			FirstAction ();
		} else if (subRoundCurrent <= subRoundMaxSize && subRoundMaxSize > 0) {
			BetSubRounds ();
		} else {
			LastAction(); //next state (preflop, turn, river)
			subRoundCurrent = 0;
		}
		subRoundCurrent++;
	}

	public virtual void FirstAction() {
//		isWaiting = true;
//		// do something (deal cards, display win info, reinit game, ui animation
//		isWaiting = false;
	}

	public virtual void BetSubRounds() {
		// bet sub rounds
	}

	public virtual void LastAction() {
		// bet sub rounds
	}
	
	#endregion
}

public class AnteRound : BetRound {
	#region IBetRoundState implementation
	
	public AnteRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMinSize;
		this.betMin = Settings.betAnte;
	}

	public override void BetSubRounds ()
	{
//		base.BetSubRounds ();
		// TODO
	}
	public override void LastAction ()
	{
//		base.LastAction ();
		game.state = new PreflopRound (game);
	}

	#endregion
}
public class PreflopRound : BetRound {
	public PreflopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
	}
	public override void BetSubRounds ()
	{
		//		base.BetSubRounds ();
		// TODO
	}
	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new FlopRound (game);
	}
	public override void FirstAction ()
	{
//		base.FirstAction ();
		isWaiting = true;
		game.ui.StartCoroutine(game.ui.DealCards());
		// isWaiting = false (must be in the end line of coroutine)
		return;

		game.ui.DebugLog ("Preflop()");
		
		// start preflop bet round 0
		if (subRoundCurrent == 0) {
			
		}
		// end preflop bet round 0
		
		// the same for all preflop bet rounds
		foreach (var player in game.players) {

			foreach (var pcard in player.handPreflop.getCards()) {
				if (player.isReal || player.isFolded || Settings.isDebug) {
					pcard.FaceUp = true;
				} else {
					pcard.FaceUp = false;
				}
			}

			if (!player.isFolded) // active virtual players only
			{
				player.patternCurrent = player.GetAndSetPatternRandomly ();
				player.actionCurrent = player.GetCurrentAction (betToStayInGame, player.bet);	// betTotalInThisRound);
				//TODO: handle player's current action
				
				if (player.actionCurrent == "FOLD") {
					player.lblAction.text = player.actionCurrent;
					player.lblCredits.text = player.credits.to_s();
					player.isFolded = true;
					foreach (var pcard in player.handPreflop.getCards()) {
						pcard.FaceUp = true;
					}
				} else if (player.actionCurrent == "CHECK") {
					Update (game, player);
					
				} else if (player.actionCurrent == "CALL") {
					Update (game, player);

				} else if (player.actionCurrent == "RAISE") {
					int multiplier = player.patternCurrent.betMaxCallOrRaise; //TODO
					player.credits -= game.betAmount * multiplier;
					betToStayInGame += game.betAmount * multiplier;
					betToStayInGame += betToStayInGame;
					Update (game, player);
				}
			}
		}
		
		//TODO: tips for real player as enable/disable buttons
		int index = 0;
		var playerReal = game.players[index]; //real player
		if (playerReal.credits <= 0 || game.betAmount <= 0) {
			game.ui.btnCheck.GetComponent<Button> ().interactable = true;
			game.ui.btnCall.GetComponent<Button> ().interactable = false;
		} else {
			game.ui.btnCheck.GetComponent<Button> ().interactable = false;
			game.ui.btnCall.GetComponent<Button> ().interactable = true;
			playerReal.lblCredits.text = playerReal.credits.ToString ();
			game.ui.lblPot.GetComponent<Text> ().text = game.potAmount.ToString ();
		}
	}

	private void Update(Game game, Player player) {
		int multiplier = 1;
		//			int multiplier = player.patternCurrent.betMaxCallOrRaise; //TODO
		player.credits -= game.betAmount * multiplier;
		betToStayInGame += game.betAmount * multiplier;
		pot += game.betAmount * multiplier;
		game.potAmount += game.betAmount * multiplier;
		
		// TODO: will refactor (credit label)
		player.lblCredits.text = player.credits.to_s();
		player.lblAction.text = player.actionCurrent;
		
		game.ui.lblPot.GetComponent<Text> ().text = game.potAmount.to_s();
		game.ui.lblBet.GetComponent<Text> ().text = game.betAmount.to_s();
		game.ui.lblRaise.GetComponent<Text> ().text = betToStayInGame.to_s(); // TODO:
		game.ui.lblCall.GetComponent<Text> ().text = betToStayInGame.to_s();
	}

}
public class FlopRound : BetRound {
	public FlopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
	}
	public override void FirstAction() {
		game.cards [0].FaceUp = true;
		game.cards [1].FaceUp = true;
		game.cards [2].FaceUp = true;
	}
	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new TurnRound (game);
	}
	public override void BetSubRounds ()
	{
		base.BetSubRounds ();
	}
}
public class TurnRound : BetRound {
	public TurnRound(Game game) {
		this.game = game;
		this.betMin = Settings.betTurnRiver;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
	}

	public override void FirstAction() {
		game.cards [3].FaceUp = true;
	}
	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new RiverRound (game);
	}
	public override void BetSubRounds ()
	{
		base.BetSubRounds ();
	}
}
public class RiverRound : BetRound {
	public RiverRound(Game game) {
		this.game = game;
		this.betMin = Settings.betTurnRiver;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
	}

	public override void FirstAction() {
		game.cards [4].FaceUp = true;
	}
	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new EndGame (game);
	}
	public override void BetSubRounds ()
	{
		base.BetSubRounds ();
	}
}
public class EndGame : BetRound {
	public EndGame(Game game) {
		this.game = game;
	}

	public override void LastAction ()
	{
		//		base.LastAction ();
//		game.state = new InitGame (game);
	}

	public override void BetSubRounds ()
	{
//		base.BetSubRounds ();
	}

	public override void FirstAction() {
		if (Settings.isDebug)
			game.ui.DebugLog ("EndGame()");
		game.isGameRunning = false;
		game.isGameEnd = true;
		
//		roundCount = subRoundCount = 0;
//		betCurrentToStayInGame = betTotalInThisRound = 0;
		
		foreach (var player in game.players) {
			foreach (var card in player.handPreflop.getCards()) {
				if (player.id != Settings.playerRealIndex)
				if (player.isFolded) {
					//						card.setImage(game.ui.cardBg); // hide
					card.isHidden = true;
				} else {
					card.FaceUp = true;
				}
			}
		}
		
		game.ui.HideDynamicPanels ();
		game.ui.panelWin.SetActive (true);
	}

	#region IBetRoundState implementation
	
	public void SubRound ()
	{
		throw new NotImplementedException ();
	}
	
	#endregion
}

public class InitGame : BetRound {
	public override void LastAction ()
	{
//		base.LastAction ();
		game.state = new AnteRound (game);
	}
	public InitGame(Game game2) {
		this.game = game2;
		game.ui.ClearAll ();
		
		game.ui.HideDynamicPanels ();
		game.ui.panelInitBet.SetActive (true);
		
		game.players = game.InitPlayers ();
		game.cards = new List<Card> ();
		
		
		
		
		Card card = null;
		Image image = null;
		
		if (game.source == null)
			game.source = new Constants ();
		game.deck = new Deck ();
		game.deck.Shuffle ();
		
		// chips
		foreach (var player in game.players) {
			player.SetChipRandomly();
			player.lblName.text = player.name;
			player.lblCredits.text = player.credits.to_s();
			player.lblAction.text = "";
		}
		
		
		foreach (var player in game.players) {
			for (int i = 1; i <= Settings.playerHandSizePreflop; i++) {
				card = game.deck.Deal ();
				var cardImg = GameObject.Find ("player" + player.id + "hold" + i);
				if (cardImg) {
					card.setImage (cardImg.GetComponent<Image> ());
					//						if (player.id == Settings.playerRealIndex || Settings.isDebug)
					//							card.FaceUp = true;
					//						else
					//							card.FaceUp = false;
				}
				player.hand.getCards().Add (card);
			}
			player.handPreflop = player.hand;
			player.handPreflopString = player.GetHandPreflopString();
		}
		
		game.isGameRunning = true;
		game.isGameEnd = false;
		
		// preflop bet rounds
		var preflops = game.source.GetPreflops ();
		foreach (var player in game.players)
		foreach (var preflop in preflops) {
			if (preflop.position == player.position) {
				if (preflop.hand == player.handPreflopString || preflop.hand == player.handPreflopStringReversed) {
					
					player.pattern = preflop.pattern;
					player.alt_patterns = preflop.alt_patterns;
					
					break;
				} else {
					player.pattern = game.source.GetPatternByName(Settings.defaultPreflopPattern);
					// TODO: alt_patterns? 
				}
			}
		}
		
		// flop
		for (int i = 1; i <= 3; i++) {
			card = game.deck.Deal ();
			image = GameObject.Find ("flop" + i).GetComponent<Image> ();
			card.setImage (image);
			if (Settings.isDebug)
				card.FaceUp = true;
			//					else
			//						card.FaceUp = false;
			game.cards.Add (card);
		}
		// turn
		card = game.deck.Deal ();
		image = GameObject.Find ("turn").GetComponent<Image> ();
		card.setImage (image);
		if (Settings.isDebug)
			card.FaceUp = true;
		//				else
		//					card.FaceUp = false;
		game.cards.Add (card);
		
		// river
		card = game.deck.Deal ();
		image = GameObject.Find ("river").GetComponent<Image> ();
		card.setImage (image);
		if (Settings.isDebug)
			card.FaceUp = true;
		//				else
		//					card.FaceUp = false;
		game.cards.Add (card);
		
		foreach (var player in game.players) {
			player.hand = player.GetBestPlayerHand (game.cards);
		}
		
		//				foreach (var player in game.ui.cardsOfPlayer) {
		//					player.sprite = game.ui.cardsAll [1];
		//				}
		//TODO: calculate win percentage/hand strength
		
		
		// using in update of the game loop
		game.playerCollection = new PlayerCollection ();
		foreach (var p in game.players) {
			game.playerCollection[p.position] = p;
		}
		
		game.playerIterator = new PlayerIterator(game.playerCollection);
	}

}

