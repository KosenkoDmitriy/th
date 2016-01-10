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
//		GameState = new GameStates ();
		dealerIndex = Settings.dealerIndex;
	}

	public List<Player> InitPlayers ()
	{
		if (dealerIndex >= Settings.playerSize) {
			dealerIndex = 0;
		}

		var players = new List<Player> ();
		for (int i = 0; i < Settings.playerSize; i++) {
			var player = new Player ();
			player.name = "Player #" + i;
			player.id = i;
			if (player.id == Settings.playerRealIndex) {
				this.player = player;
				player.isReal = true;
			} else {
				player.isReal = false;
			}
			player.betTotal = Settings.playerCreditsInNumberOfBets;

			player.chip = GameObject.Find("Chip"+i).GetComponent<Image>();
			//			player.SetChipRandomly();
			player.dealer = GameObject.Find("Dealer"+i).GetComponent<Image>();
			if (player.id == dealerIndex) player.isDealer = true;// else player.isDealer = false;
			player.lblAction = GameObject.Find ("lblBetPlayer"+i).GetComponent<Text>();
			player.lblCredits = GameObject.Find ("lblCreditPlayer"+i).GetComponent<Text>();
			player.lblName = GameObject.Find("lblPlayerName"+i).GetComponent<Text>();

			players.Add (player);
		}


		// player's sorting by dealer position/index
		var playersBeforeDealer = new List<Player> ();
		var playersAfterDealer = new List<Player> ();
		foreach(var player in players) {
			if (player.id <= dealerIndex) {
				playersBeforeDealer.Add (player);
			} else {
				playersAfterDealer.Add (player);
			}
		}

		var playerList = new List<Player> ();
		playerList.AddRange (playersAfterDealer);
		playerList.AddRange( playersBeforeDealer);

		int j = 0;
		foreach (var player in playerList) {
			player.position = j;	// for math (first to act player)
			j++;
		}

		dealerIndex++;

		return playerList;
	}



	public Constants source;
	public States states;
	public BetRound state;

	public double betAmountAnt, betAmount, potAmount;

	public PlayerIterator playerIterator;
	public PlayerCollection playerCollection;
	public int dealerIndex; // dealer = position + 1

	public Deck deck;
	public List<Player> players;
	public Player player;
	public List<Card> cards;
	public GameUI ui;
	public bool isGameRunning;
	public bool isGameEnd;
	public IBetRoundState BetRound { get; private set; }
	public IGameState GameState { get; private set; }
}
