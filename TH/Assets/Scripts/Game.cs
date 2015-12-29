﻿using System;

//using UnityEngine;
using System.Collections.Generic;

public class Game
{
	public Game (GameUI ui)
	{
		this.ui = ui;
		GameState = new GameStates ();
		PatternState = new PatternStates ();
	}

	List<Player> players;
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
		void Call(Game game);
		void Check(Game game);
		void Raise(Game game);
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
		int roundCount = 0;
		readonly int roundMaxCount = Settings.maxRoundCount;
		int subRoundCount = 0;
		readonly int subRoundMaxCount = Settings.maxSubRoundCount;

		public void EndGame (Game game)
		{
			if (Settings.isDebug) game.ui.DebugLog ("EndGame()");
			game.isGameRunning = false;
			game.isGameEnd = true;

			roundCount = 0;
			subRoundCount = 0;

			game.ui.HideDynamicPanels ();
			game.ui.panelWin.SetActive (true);
		}

		public void StartGame (Game game)
		{
			if (Settings.isDebug) game.ui.DebugLog ("StartGame()");

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

		public void Check(Game game) {
			NextRound (game);

		}

		public void Call(Game game) {
			NextRound (game);

		}

		public void Raise(Game game) {
			NextRound (game);

		}

		public void Fold(Game game) {
			EndGame (game);
//			NextRound (game);
		}

		private void NextRound(Game game) {

			if (roundCount >= roundMaxCount) {
				roundCount = 0;
				EndGame(game);
				return;
			}
			
			if (subRoundCount >= subRoundMaxCount) {
				subRoundCount = 0;
			}
			
			if (Settings.isDebug)
				game.ui.DebugLog ("NextRound() current round: " + roundCount + "/" + roundMaxCount);
			
			if (roundCount >= 0 && roundCount < subRoundMaxCount) {
				Preflop(game);
			} else if (subRoundMaxCount <= roundCount && roundCount < subRoundMaxCount * 2) {
				Flop(game);
			} else if (subRoundMaxCount * 2 <= roundCount && roundCount < subRoundMaxCount * 3) {
				Turn(game);
			} else if (subRoundMaxCount * 3 <= roundCount && roundCount < subRoundMaxCount * 4) {
				River(game);
			}

			roundCount++;
			subRoundCount++;
		}

		public void Preflop (Game game)
		{
			game.ui.DebugLog ("Preflop()");
			
			// TODO:
			// Bet round 1
			// check players hand strength
			// chose pattern and alternative patterns
			
			if (subRoundCount == 0) {
				game.isGameRunning = true;
				game.isGameEnd = false;
				foreach (var player in game.ui.cardsOfPlayer) {
					//				int rand = new Random(0, game.ui.cardsAll.Count);
					player.sprite = game.ui.cardsAll [1];
				}
			}
		}
		
		public void Flop (Game game)
		{
			game.ui.DebugLog ("Flop()");
			
			if (subRoundCount == 0) {
				game.ui.cardsPublic [0].sprite = game.ui.cardsAll [2];
				game.ui.cardsPublic [1].sprite = game.ui.cardsAll [3];
				game.ui.cardsPublic [2].sprite = game.ui.cardsAll [4];
			}
		}
		
		public void Turn (Game game)//, bool isFromPrev)
		{
			game.ui.DebugLog ("Turn()");
			
			if (subRoundCount == 0) {
				game.ui.cardsPublic [3].sprite = game.ui.cardsAll [5];
			}
		}
		
		public void River (Game game)
		{
			game.ui.DebugLog ("River()");
			if (subRoundCount == 0) {
				game.ui.cardsPublic [4].sprite = game.ui.cardsAll [6];
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
