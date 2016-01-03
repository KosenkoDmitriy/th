using System;

//using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class Game
{
	public Game (GameUI ui)
	{
		this.ui = ui;
		GameState = new GameStates ();
		PatternState = new PatternStates ();
		cards = new List<Card> ();
		players = GetPlayers ();
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

	public interface IPatternState
	{
		void CheckFold_Fold (Game game); // always fold
		void CheckCall_Call (Game game); // always call
		void CheckCall1_Fold (Game game);

		void CheckCall2_Call (Game game);

		void CheckCall3_Call (Game game);

		void CheckRaise_Raise (Game game);

		void CheckRaise1_Raise (Game game);

		void CheckRaise2_Raise (Game game);

		void Raise_Raise (Game game); // always raise
		void RaiseCall1_Raise (Game game);

		void RaiseCall2_Raise (Game game);

		void OpenCall1_Fold (Game game);

		void OpenCall2_Fold (Game game);
	}

	public class GameStates : IGameState
	{
		double betToStayInGame;
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
			betToStayInGame = betTotalInThisRound = 0;

			foreach (var player in game.players) {
				foreach (var card in player.cards) {
					card.FaceUp = true;
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
			game.ui.HideDynamicPanels ();
			game.ui.panelInitBet.SetActive (true);
		}

		public void Check (Game game)
		{
			NextRound (game);

		}

		public void Call (Game game)
		{
			betToStayInGame += game.ui.betAmount;
			betTotalInThisRound += game.ui.betAmount;

			NextRound (game);

		}

		public void Raise (Game game)
		{
			betToStayInGame += game.ui.betAmount;
			betTotalInThisRound += game.ui.betAmount;

			NextRound (game);

		}

		public void Fold (Game game)
		{
			EndGame (game);
//			NextRound (game);
		}

		private void NextRound (Game game)
		{

			if (roundCount >= roundMaxCount) {
				roundCount = 0;
				EndGame (game);
				return;
			}
			
			if (subRoundCount >= subRoundMaxCount) {
				subRoundCount = 0;
				betTotalInThisRound = 0;
			}
			
			if (Settings.isDebug)
				game.ui.DebugLog ("NextRound() current round: " + roundCount + "/" + roundMaxCount);

			int i = 0;
			foreach (var player in game.players) {
				if (player.credits <= 0 || game.ui.betAmount <= 0) {
					game.ui.btnCheck.GetComponent<Button> ().interactable = true;
					game.ui.btnCall.GetComponent<Button> ().interactable = false;
				} else {
					game.ui.btnCheck.GetComponent<Button> ().interactable = false;
					game.ui.btnCall.GetComponent<Button> ().interactable = true;
					player.credits -= game.ui.betAmount;
					game.potAmount += game.ui.betAmount;
					game.ui.creditLabels [i].GetComponent<Text> ().text = player.credits.ToString ();
					game.ui.lblPot.GetComponent<Text> ().text = game.potAmount.ToString ();
					i++;
				}
			}

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

			Card card = null;
			Image image = null;
			// TODO:
			// Bet round 1
			// check players hand strength
			// chose pattern and alternative patterns
			
			if (subRoundCount == 0) {
				if (source == null)
					source = new Constants ();
				game.deck = new Deck ();
				game.deck.Shuffle ();
//				var deckCards = source.GetDeckCards();
//				var hand = new Hand();
				foreach (var player in game.players) {
//					player.hand = hand.GetHandByPlayerNo(player.no);
					player.handString = "22"; // TODO: 
					var preflops = source.GetPreflops ();
					foreach (var preflop in preflops) {
						if (preflop.position == player.no) {
							if (preflop.hand == player.handString) {
								for (int i = 1; i <= Settings.playerHandSize; i++) {
									card = game.deck.Deal ();
									var cardImg = GameObject.Find ("player" + player.no + "hold" + i);
									if (cardImg) {
										card.setImage (cardImg.GetComponent<Image> ());
										if (player.no == 0 || Settings.isDebug)
											card.FaceUp = true;
										else
											card.FaceUp = false;
									}
									player.cards.Add (card);
								}

								player.pattern = preflop.pattern;
								player.alt_patterns = preflop.alt_patterns;
								player.patternCurrent = player.GetAndSetPatternRandomly ();
								player.actionCurrent = player.GetCurrentAction (betToStayInGame, betTotalInThisRound);
								
								break;
							}
						}
					}
				}

				game.isGameRunning = true;
				game.isGameEnd = false;


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
					player.hand = GetBestPlayerHand (player, game.cards);
				}

//				foreach (var player in game.ui.cardsOfPlayer) {
//					//				int rand = new Random(0, game.ui.cardsAll.Count);
//					player.sprite = game.ui.cardsAll [1];
//				}

			}
		}

		public Hand GetBestPlayerHand (Player player, List<Card> cards)
		{
			var count = 20;
			Hand hand = null;
			player.hands = new List<Hand> ();

			for (int x = 0; x <= count; x++) {//iterate through all possible 5 card hands
				hand = new Hand();
				switch (x) {
				case 0:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);

						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
					}
					break;
				case 1:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);
						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [3]);
					}
					break;
				case 2:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);
						hand.Add (cards [0]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
					}
					break;
				case 3:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
					}
					break;
				case 4:
					{
						hand.Add (player.cards [0]);
						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
					}
					break;
				case 5:
					{
						hand.Add (player.cards [1]);
						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
					}
					break;
				case 6:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);
						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [4]);
					}
					break;
				case 7:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);
						hand.Add (cards [0]);
						hand.Add (cards [2]);
						hand.Add (cards [4]);
					}
					break;
				case 8:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [4]);
					}
					break;
				case 9:
					{
						hand.Add (player.cards [0]);
						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [4]);
					}
					break;
				case 10:
					{
						hand.Add (player.cards [1]);
						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [4]);
					}
					break;
				case 11:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);
						hand.Add (cards [0]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				case 12:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);
						hand.Add (cards [1]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				case 13:
					{
						hand.Add (player.cards [0]);
						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				case 14:
					{
						hand.Add (player.cards [1]);
						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				case 15:
					{
						hand.Add (player.cards [0]);
						hand.Add (player.cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				case 16:
					{
						hand.Add (player.cards [0]);
						hand.Add (cards [0]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				case 17:
					{
						hand.Add (player.cards [1]);
						hand.Add (cards [0]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				case 18:
					{
						hand.Add (player.cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				case 19:
					{
						hand.Add (player.cards [1]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				case 20:
					{
						hand.Add (cards [0]);
						hand.Add (cards [1]);
						hand.Add (cards [2]);
						hand.Add (cards [3]);
						hand.Add (cards [4]);
					}
					break;
				}
				hand = HandCombination.getBestHand(hand); 
				player.hands.Add (hand);
			}

			Hand bestHand = player.hands.FirstOrDefault();
//			bestHand = HandCombination.getBestHand (bestHand);
			foreach (var item in player.hands) {
//				hand = HandCombination.getBestHand(item);
				if (hand > bestHand) {
					bestHand = hand;
				}
			}
			return bestHand;
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

	public class PatternStates : IPatternState
	{
		public void CheckFold_Fold (Game game)
		{

		}

		public void CheckCall_Call (Game game)
		{

		}

		public void CheckCall1_Fold (Game game)
		{

		}

		public void CheckCall2_Call (Game game)
		{

		}

		public void CheckCall3_Call (Game game)
		{

		}

		public void CheckRaise_Raise (Game game)
		{

		}

		public void CheckRaise1_Raise (Game game)
		{

		}

		public void CheckRaise2_Raise (Game game)
		{

		}

		public void Raise_Raise (Game game)
		{

		}

		public void RaiseCall1_Raise (Game game)
		{

		}

		public void RaiseCall2_Raise (Game game)
		{

		}

		public void OpenCall1_Fold (Game game)
		{

		}

		public void OpenCall2_Fold (Game game)
		{

		}
	}

}
