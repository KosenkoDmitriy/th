using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class Game
{
	public Game (GameUI ui)
	{
		this.ui = ui;
		GameState = new GameStates ();
		PatternState = new PatternStates ();
		cards = new List<Card> ();
		players = GetPlayers ();
//		playerReal = players.First ();
//		players.Remove (playerReal);
	}

	public List<Player> GetPlayers ()
	{
		var players = new List<Player> ();
		for (int i = 0; i < Settings.playerSize; i++) {
			var player = new Player ();
			player.name = "Player #" + i;
			player.no = i;
			player.credits = Settings.playerCredits;
			players.Add (player);
		}
		return players;
	}

	double potAmount;
	public Deck deck;
//	public Player playerReal;
	public List<Player> players;
	public List<Card> cards;
	public GameUI ui;
	public bool isGameRunning;
	public bool isGameEnd;

	public IMathState MathState { get; private set; }

	public IGameState GameState { get; private set; }

	public IPatternState PatternState { get; private set; }

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
					if (player.no != 0)
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

//			GameState = new GameStates ();
//			PatternState = new PatternStates ();
//
			game.cards = new List<Card> ();
			game.players = game.GetPlayers ();
		}

		public void Check (Game game)
		{
			NextRound (game);
		}

		public void Call (Game game)
		{
//			betToStayInGame += game.ui.betAmount;
//			betTotalInThisRound += game.ui.betAmount;

			NextRound (game);
		}

		public void Raise (Game game)
		{
//			betToStayInGame += game.ui.betAmount;
//			betTotalInThisRound += game.ui.betAmount;

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
				betTotalInThisRound = betCurrentToStayInGame = 0;
				EndGame (game);
				return;
			}
			
			if (subRoundCount >= subRoundMaxCount) {
				subRoundCount = 0;
				betTotalInThisRound = betCurrentToStayInGame = 0;
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
			// TODO: check players hand strength

			Card card = null;
			Image image = null;

			// start preflop bet round 0
			if (subRoundCount == 0) {
				if (source == null)
					source = new Constants ();
				game.deck = new Deck ();
				game.deck.Shuffle ();

				foreach (var player in game.players) {
					for (int i = 1; i <= Settings.playerHandSizePreflop; i++) {
						card = game.deck.Deal ();
						var cardImg = GameObject.Find ("player" + player.no + "hold" + i);
						if (cardImg) {
							card.setImage (cardImg.GetComponent<Image> ());
							if (player.no == 0 || Settings.isDebug)
								card.FaceUp = true;
							else
								card.FaceUp = false;
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
					if (preflop.position == player.no) {
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

			}
			// end preflop bet round 0

			// the same for all preflop bet rounds
			int index = 0;
			foreach (var player in game.players) {
				if (!player.isFolded) // active virtual players only
				{
					player.patternCurrent = player.GetAndSetPatternRandomly ();
					player.actionCurrent = player.GetCurrentAction (betCurrentToStayInGame, betTotalInThisRound);
					//TODO: handle player's current action

					if (player.actionCurrent == "FOLD") {
						player.isFolded = true;
						foreach (var pcard in player.handPreflop.getCards()) {
							pcard.FaceUp = true;
						}
					} else if (player.actionCurrent == "CHECk") {
//						player.credits -= game.ui.betAmount;
//						betToStayInGame += game.ui.betAmount;
//						betTotalInThisRound += game.ui.betAmount;
						int multiplier = 1;
					} else if (player.actionCurrent == "CALL") {
						int multiplier = 1;
//						int multiplier = player.patternCurrent.betMaxCallOrRaise; //TODO:
						player.credits -= game.ui.betAmount * multiplier;
						betCurrentToStayInGame += game.ui.betAmount * multiplier;
						betTotalInThisRound += game.ui.betAmount * multiplier;
						game.potAmount += game.ui.betAmount * multiplier;
					} else if (player.actionCurrent == "RAISE") {
						int multiplier = 1;
//						int multiplier = player.patternCurrent.betMaxCallOrRaise; //TODO
						player.credits -= game.ui.betAmount * multiplier;
						betCurrentToStayInGame += game.ui.betAmount * multiplier;
						betTotalInThisRound += game.ui.betAmount * multiplier;
						game.potAmount += game.ui.betAmount * multiplier;
					}

					// TODO: will refactor (credit label)
					game.ui.players[index].lblCredits.text = player.credits.ToString ();
					game.ui.players[index].lblAction.text = player.actionCurrent;
					game.ui.lblPot.GetComponent<Text> ().text = game.potAmount.ToString ();
					index++;
				}
			}

			// tips for real player as enable/disable buttons //TODO:
			index = 0;
			var playerReal = game.players[index]; //real players
			if (playerReal.credits <= 0 || game.ui.betAmount <= 0) {
				game.ui.btnCheck.GetComponent<Button> ().interactable = true;
				game.ui.btnCall.GetComponent<Button> ().interactable = false;
			} else {
				game.ui.btnCheck.GetComponent<Button> ().interactable = false;
				game.ui.btnCall.GetComponent<Button> ().interactable = true;
				game.ui.players[index].lblCredits.text = playerReal.credits.ToString ();
				game.ui.lblPot.GetComponent<Text> ().text = game.potAmount.ToString ();
			}
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

}
