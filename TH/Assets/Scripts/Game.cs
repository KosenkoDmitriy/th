using System;
//using UnityEngine;
using System.Collections.Generic;

public class Game {

	List<Player> players;
	public int BetRound = 0;
	public GameUI ui;

	public Game(GameUI ui) {
		this.ui = ui;
		MathState = new MathStates ();
		GameState = new GameStates ();
		PatternState = new PatternStates ();
	}

	public IMathState MathState { get; private set; }
	public IGameState GameState { get; private set; }
	public IPatternState PatternState { get; private set; }


	public interface IMathState {
		void Preflop(Game game);
		void Flop(Game game);
		void Turn(Game game);
		void River(Game game);
		void BetRound1(Game game);
		void BetRound2(Game game);
		void BetRound3(Game game);
		void BetRound4(Game game);
	}


	public interface IGameState {
		void InitGame(Game game);
		void StartGame(Game game);
		void EndGame(Game game);
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


	public interface IPatternState {
		void CheckFold_Fold(Game game); // always fold
		void CheckCall_Call(Game game); // always call
		void CheckCall1_Fold(Game game);
		void CheckCall2_Call(Game game);
		void CheckCall3_Call(Game game);
		void CheckRaise_Raise(Game game);
		void CheckRaise1_Raise(Game game);
		void CheckRaise2_Raise(Game game);
		void Raise_Raise(Game game); // always raise
		void RaiseCall1_Raise(Game game);
		void RaiseCall2_Raise(Game game);
		void OpenCall1_Fold(Game game);
		void OpenCall2_Fold(Game game);
	}


	public class MathStates : IMathState {
		public void Preflop(Game game) {
			game.ui.HideDynamicPanels ();
			game.ui.panelGame.SetActive (true);


			foreach (var player in game.ui.cardsOfPlayer) {
//				int rand = new Random(0, game.ui.cardsAll.Count);
				player.sprite = game.ui.cardsAll[1];
			}
			// Bet round 1
			// check players hand strength
			// chose pattern and alternative patterns
		}
		public void BetRound1(Game game) {
			game.ui.HideDynamicPanels ();
			game.ui.panelInitBet.SetActive (true);
		}
		public void BetRound2(Game game) {
			game.ui.HideDynamicPanels ();
//			game.ui.panelInitBet.SetActive (true);
			game.MathState.Turn(game);
		}
		public void BetRound3(Game game) {
			game.ui.HideDynamicPanels ();
//			game.ui.panelInitBet.SetActive (true);
			game.MathState.River(game);
		}
		public void BetRound4(Game game) {
			game.ui.HideDynamicPanels ();
			game.ui.panelInitBet.SetActive (true);
		}
		public void Flop(Game game) {
			game.ui.cardsPublic [0].sprite = game.ui.cardsAll [2];
			game.ui.cardsPublic [1].sprite = game.ui.cardsAll [3];
			game.ui.cardsPublic [2].sprite = game.ui.cardsAll [4];
//			game.MathState.BetRound2(game);
			game.MathState.Turn(game);

			game.BetRound++;
		}
		public void Turn(Game game) {
			game.ui.cardsPublic [3].sprite = game.ui.cardsAll [5];
//			game.MathState.BetRound3(game);
			game.MathState.River(game);
		}
		public void River(Game game) {
			game.ui.cardsPublic [4].sprite = game.ui.cardsAll [6];
//			game.MathState.BetRound4(game);
			game.GameState.EndGame (game);
		}
	}

	
	public class GameStates : IGameState {
		public void EndGame(Game game) {
			game.ui.HideDynamicPanels ();
			game.ui.panelWin.SetActive (true);
		}
		public void StartGame(Game game) {
			game.ui.HideDynamicPanels ();
			game.ui.panelGame.SetActive (true);
		}
		public void InitGame(Game game) {
			game.ui.HideDynamicPanels ();
			game.ui.panelInitBet.SetActive (true);
		}
	}


	public class PatternStates : IPatternState {
		public void CheckFold_Fold(Game game) {

		}
		public void CheckCall_Call(Game game) {

		}
		public void CheckCall1_Fold(Game game) {

		}
		public void CheckCall2_Call(Game game) {

		}
		public void CheckCall3_Call(Game game) {

		}
		public void CheckRaise_Raise(Game game) {

		}
		public void CheckRaise1_Raise(Game game) {

		}
		public void CheckRaise2_Raise(Game game) {

		}
		public void Raise_Raise(Game game) {

		}
		public void RaiseCall1_Raise(Game game) {

		}
		public void RaiseCall2_Raise(Game game) {

		}
		public void OpenCall1_Fold(Game game) {

		}
		public void OpenCall2_Fold(Game game) {

		}
	}

}
