using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class States {
	IEnumerable<BetRound> rounds;
	private IEnumerator<BetRound> enumerator;
	public BetRound state;
	public bool isDone;

	public void Next() {
		if (enumerator.MoveNext ()) {
			state = enumerator.Current;
			state.SubRound ();
			isDone = false;
		} else {
			isDone = true;
		}
	}

//	public BetRound GetCurrentState() {
//		return state;
//	}

	public States(Game game) {
		isDone = false;
		rounds = new List<BetRound> () {
			new AnteRound(),
			new PreflopRound(),
			new FlopRound(),
			new TurnRound(),
			new RiverRound(),
			new EndGame(), // win panel (when close it > InitGame()
		};
		enumerator = rounds.GetEnumerator ();
		state = new InitGame (game);

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
}



public interface IBetRoundState
{
	void SubRound ();
}

public abstract class AbstractBetRound {
	public int subRoundMaxSize;
	public int subRoundCurrent;
	public double bet;
}

public class BetRound : AbstractBetRound, IBetRoundState {
	
	public BetRound() {
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		this.bet = Settings.betPreflopFlop;
	}
	
	#region IBetRoundState implementation
	
	public void SubRound ()
	{
		throw new NotImplementedException ();
	}
	
	#endregion
}

public class AnteRound : BetRound {
	#region IBetRoundState implementation
	
	public AnteRound() {
		this.subRoundMaxSize = Settings.betAnteSubRoundMaxSize;
		this.bet = Settings.betAnte;
	}
	
	public void SubRound ()
	{
		throw new NotImplementedException ();
	}
	
	#endregion
}
public class PreflopRound : BetRound {
	#region IBetRoundState implementation
	
	public void SubRound ()
	{
		throw new NotImplementedException ();
	}
	
	#endregion
}
public class FlopRound : BetRound {
	#region IBetRoundState implementation
	
	public void SubRound ()
	{
		throw new NotImplementedException ();
	}
	
	#endregion
}
public class TurnRound : BetRound {
	public TurnRound() {
		this.bet = Settings.betTurnRiver;
	}
	#region IBetRoundState implementation
	
	public void SubRound ()
	{
		throw new NotImplementedException ();
	}
	
	#endregion
}
public class RiverRound : BetRound {
	public RiverRound() {
		this.bet = Settings.betTurnRiver;
	}
	#region IBetRoundState implementation
	
	public void SubRound ()
	{
		throw new NotImplementedException ();
	}
	
	#endregion
}
public class EndGame : BetRound {
	#region IBetRoundState implementation
	
	public void SubRound ()
	{
		throw new NotImplementedException ();
	}
	
	#endregion
}
public class InitGame : BetRound {
	Game game;
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
//	public InitGame(Game game) {
//		game.states = new States ();
//	}
	#region IBetRoundState implementation
	
	public void SubRound ()
	{
		throw new NotImplementedException ();
	}
	
	#endregion
}

