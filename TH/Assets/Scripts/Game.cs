using System;

//using UnityEngine;
using System.Collections.Generic;

public class Game
{
	public Game (GameUI ui)
	{
		this.ui = ui;
		MathState = new MathStates ();
		GameState = new GameStates ();
		PatternState = new PatternStates ();
	}

	public int GameLevel = 0; // preflop, flop, turn, river
	public int pftr = 0;
	List<Player> players;
	public int betRound = 0; // for BetRound
	public int betCurrentRound = 0; // for gamelevel

	public GameUI ui;
	public bool isGameRunning;
	public bool isGameEnd;

	public IMathState MathState { get; private set; }
	public IGameState GameState { get; private set; }
	public IPatternState PatternState { get; private set; }


	public interface IMathState
	{
		void Preflop (Game game);
		void Flop (Game game);
		void Turn (Game game);
		void River (Game game);
		void BetRound (Game game);
	}

	public interface IGameState
	{
		void InitGame (Game game);

		void StartGame (Game game);

		void EndGame (Game game);
//
//		void Call();
//		void Check();
//		void Raise();
//		void AllIn();
//		void Fold ();
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

	public class MathStates : IMathState
	{
		public void Preflop (Game game)
		{
			game.ui.DebugLog ("Preflop()");

			game.GameLevel = 0;

			// TODO:
			// Bet round 1
			// check players hand strength
			// chose pattern and alternative patterns

			if (game.betCurrentRound == 0) {
				foreach (var player in game.ui.cardsOfPlayer) {
					//				int rand = new Random(0, game.ui.cardsAll.Count);
					player.sprite = game.ui.cardsAll [1];
				}
			} 

			if (!isBetRound (game)) {
				game.MathState.Flop (game);	
			}
		}

		public void Flop (Game game)
		{
			game.GameLevel = 1;
			game.ui.DebugLog ("Flop()");

			game.ui.cardsPublic [0].sprite = game.ui.cardsAll [2];
			game.ui.cardsPublic [1].sprite = game.ui.cardsAll [3];
			game.ui.cardsPublic [2].sprite = game.ui.cardsAll [4];

			if (!isBetRound (game)) {
				game.MathState.Turn (game);
			}
		}

		public void Turn (Game game)
		{
			game.GameLevel = 2;
			game.ui.DebugLog ("Turn()");

			game.ui.cardsPublic [3].sprite = game.ui.cardsAll [5];

			if (!isBetRound (game)) {
				game.MathState.River (game);
			}
		}

		public void River (Game game)
		{
			game.GameLevel = 3;
			game.ui.DebugLog ("River()");

			game.ui.cardsPublic [4].sprite = game.ui.cardsAll [6];

			if (!isBetRound (game)) {
				game.GameState.EndGame (game);
			}
		}

		public bool isBetRound (Game game)
		{
//			game.ui.HideDynamicPanels ();
			if (game.ui.betAmount <= 0 || game.betCurrentRound >= Settings.betRoundCount - 1) {
				game.ui.panelGame.SetActive(true);
				game.betCurrentRound = 0;
//				game.ui.panelGame.SetActive(true);
				return false;
			}
//			game.ui.panelInitBet.SetActive(true);
			game.betCurrentRound++;
			return true;
		}

		public bool isBetRound2 (Game game)
		{
			if (game.ui.betAmount <= 0 || game.betRound >= Settings.betRoundCount - 1) {
				if (game.pftr == 0) {
					game.MathState.Preflop(game);
				} else if (game.pftr == 1) {
					game.GameState.EndGame(game);
//					game.MathState.Flop(game);
				} else if (game.pftr == 2) {
					game.MathState.Turn(game);
				} else if (game.pftr == 3) {
					game.MathState.River(game);
				}

				game.betRound = 0;
				if (game.pftr >= 4) {
					game.pftr = 0;
				} else {
					game.pftr++;
				}
				return false;
			}

			game.betRound++;
			return true;
		}

		public void BetRound(Game game) {

			if (isBetRound2 (game)) {
				game.ui.HideDynamicPanels ();
				game.ui.panelGame.SetActive (true);
			}
			else if (game.isGameEnd) {
				game.ui.HideDynamicPanels ();
				game.ui.panelWin.SetActive (true);
			}
		}
	}
	
	public class GameStates : IGameState
	{

		public void EndGame (Game game)
		{
			game.isGameRunning = false;
			game.isGameEnd = true;
			game.ui.HideDynamicPanels ();
			game.ui.panelWin.SetActive (true);
		}

		public void StartGame (Game game)
		{
			game.isGameEnd = false;
			game.isGameRunning = true;
			game.betCurrentRound = 0;
//			game.MathState.Preflop (game);
		}

		public void InitGame (Game game)
		{
			game.ui.HideDynamicPanels ();
			game.ui.panelInitBet.SetActive (true);
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
