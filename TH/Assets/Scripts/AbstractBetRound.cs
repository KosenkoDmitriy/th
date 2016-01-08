using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;


public interface IBetRoundState
{
	void SubRound ();
}


public abstract class AbstractBetRound {
	int subRoundMaxSize;
	int subRoundCurrent;
}

public class BetRound : AbstractBetRound, IBetRoundState {
	#region IBetRoundState implementation

	public void SubRound ()
	{
		throw new NotImplementedException ();
	}

	#endregion


}

public class AnteRound : BetRound {
	#region IBetRoundState implementation

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
	#region IBetRoundState implementation

	public void SubRound ()
	{
		throw new NotImplementedException ();
	}

	#endregion
 }
public class RiverRound : BetRound {
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
	#region IBetRoundState implementation

	public void SubRound ()
	{
		throw new NotImplementedException ();
	}

	#endregion
 }

public class States {
	public States() {
		StateCollection collection = new StateCollection ();
		collection [0] = new InitGame ();
		collection [1] = new AnteRound ();
		collection [2] = new PreflopRound ();
		collection [3] = new FlopRound ();
		collection [4] = new TurnRound ();
		collection [5] = new RiverRound ();
		collection [6] = new EndGame ();

		StateIterator stateIterator = new StateIterator (collection);
		stateIterator.Step = 1;

		for (BetRound item = stateIterator.First(); !stateIterator.IsDone; item = stateIterator.Next())
		{
			item.SubRound();
		}
		
		// Wait for user ui interaction
		// game.isWaiting = true;
	}
}

public interface IGameState
{
	void InitGame (Game game);
	
	void StartGame (Game game);
	
	void EndGame (Game game);
	//
	void Call (Game game);
	
	void Check (Game game);
	
	void Raise (Game game);
	//		void AllIn(Game game);
	void Fold (Game game);
	//
	//		void Bet();
	//		void BetBonus();
}

public class GameStates : IGameState
{
	double betCurrentToStayInGame;
	double betTotalInThisRound;
	double betRaiseInThisRound;
	int roundCount;
	readonly int roundMaxCount;
	int subRoundCount;
	readonly int subRoundMaxCount;
	
	public GameStates ()
	{
		roundMaxCount = Settings.maxRoundCount;
		subRoundMaxCount = Settings.maxSubRoundCount;
	}
	
	public void EndGame (Game game)
	{
		if (Settings.isDebug)
			game.ui.DebugLog ("EndGame()");
		game.isGameRunning = false;
		game.isGameEnd = true;
		
		roundCount = subRoundCount = 0;
		betCurrentToStayInGame = betTotalInThisRound = 0;
		
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
	
	public void StartGame (Game game)
	{
		if (Settings.isDebug)
			game.ui.DebugLog ("StartGame()");
		
		game.isGameEnd = false;
		game.isGameRunning = true;
		roundCount = 0;
		//			game.MathState.Preflop (game);
	}
	
	public void InitGame (Game game)
	{
		game.ui.ClearAll ();
		
		game.ui.HideDynamicPanels ();
		game.ui.panelInitBet.SetActive (true);
		
		game.players = game.InitPlayers ();
		game.cards = new List<Card> ();
		
		
		
		
		Card card = null;
		Image image = null;
		if (source == null)
			source = new Constants ();
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
		var preflops = source.GetPreflops ();
		foreach (var player in game.players)
		foreach (var preflop in preflops) {
			if (preflop.position == player.position) {
				if (preflop.hand == player.handPreflopString || preflop.hand == player.handPreflopStringReversed) {
					
					player.pattern = preflop.pattern;
					player.alt_patterns = preflop.alt_patterns;
					
					break;
				} else {
					player.pattern = source.GetPatternByName(Settings.defaultPreflopPattern);
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
	
	public void Check (Game game)
	{
		NextRound (game);
	}
	
	public void Call (Game game)
	{
		//			betToStayInGame += game.betAmount;
		//			betTotalInThisRound += game.betAmount;
		
		NextRound (game);
	}
	
	public void Raise (Game game)
	{
		//			betToStayInGame += game.betAmount;
		//			betTotalInThisRound += game.betAmount;
		
		NextRound (game);
	}
	
	public void Fold (Game game)
	{
		EndGame (game);
	}
	
	private void NextRound (Game game)
	{
		
		if (roundCount >= roundMaxCount) {
			roundCount = subRoundCount = 0;
			betRaiseInThisRound = betTotalInThisRound = betCurrentToStayInGame = 0;
			EndGame (game);
			return;
		}
		
		if (subRoundCount >= subRoundMaxCount) {
			subRoundCount = 0;
			betRaiseInThisRound = betTotalInThisRound = betCurrentToStayInGame = 0;
		}
		
		if (Settings.isDebug)
			game.ui.DebugLog ("NextRound() current round: " + roundCount + "/" + roundMaxCount);
		
		if (roundCount >= 0 && roundCount < subRoundMaxCount) {
			Preflop (game);
		} else if (subRoundMaxCount <= roundCount && roundCount < subRoundMaxCount * 2) {
			Flop (game);
		} else if (subRoundMaxCount * 2 <= roundCount && roundCount < subRoundMaxCount * 3) {
			Turn (game);
		} else if (subRoundMaxCount * 3 <= roundCount && roundCount < subRoundMaxCount * 4) {
			River (game);
		}
		
		roundCount++;
		subRoundCount++;
	}
	
	Constants source;
	
	public void Preflop (Game game)
	{
		game.ui.DebugLog ("Preflop()");
		
		// start preflop bet round 0
		if (subRoundCount == 0) {
			
		}
		// end preflop bet round 0
		
		// the same for all preflop bet rounds
		foreach (var player in game.players) {
			if (!player.isFolded) // active virtual players only
			{
				player.patternCurrent = player.GetAndSetPatternRandomly ();
				player.actionCurrent = player.GetCurrentAction (betCurrentToStayInGame, betTotalInThisRound);
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
					betRaiseInThisRound += game.betAmount * multiplier;
					betCurrentToStayInGame += betRaiseInThisRound;
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
	
	public void Update(Game game, Player player) {
		int multiplier = 1;
		//			int multiplier = player.patternCurrent.betMaxCallOrRaise; //TODO
		player.credits -= game.betAmount * multiplier;
		betCurrentToStayInGame += game.betAmount * multiplier;
		betTotalInThisRound += game.betAmount * multiplier;
		game.potAmount += game.betAmount * multiplier;
		
		// TODO: will refactor (credit label)
		player.lblCredits.text = player.credits.to_s();
		player.lblAction.text = player.actionCurrent;
		
		game.ui.lblPot.GetComponent<Text> ().text = game.potAmount.to_s();
		game.ui.lblBet.GetComponent<Text> ().text = game.betAmount.to_s();
		game.ui.lblRaise.GetComponent<Text> ().text = betRaiseInThisRound.to_s(); // TODO:
		game.ui.lblCall.GetComponent<Text> ().text = betCurrentToStayInGame.to_s();
	}
	
	public void Flop (Game game)
	{
		game.ui.DebugLog ("Flop()");
		
		if (subRoundCount == 0) {
			game.cards [0].FaceUp = true;
			game.cards [1].FaceUp = true;
			game.cards [2].FaceUp = true;
			//				game.ui.cardsPublic [0].sprite = game.ui.cardsAll [2];
			//				game.ui.cardsPublic [1].sprite = game.ui.cardsAll [3];
			//				game.ui.cardsPublic [2].sprite = game.ui.cardsAll [4];
		}
	}
	
	public void Turn (Game game)//, bool isFromPrev)
	{
		game.ui.DebugLog ("Turn()");
		
		if (subRoundCount == 0) {
			game.cards [3].FaceUp = true;
			//				game.ui.cardsPublic [3].sprite = game.ui.cardsAll [5];
		}
	}
	
	public void River (Game game)
	{
		game.ui.DebugLog ("River()");
		
		if (subRoundCount == 0) {
			game.cards [4].FaceUp = true;
			//				game.ui.cardsPublic [4].sprite = game.ui.cardsAll [6];
		}
	}
}

