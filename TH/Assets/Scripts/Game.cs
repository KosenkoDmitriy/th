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
				player.name = "YOU";
			} else {
				player.isReal = false;
			}
			player.betTotal = new Bet(Settings.playerCreditsInNumberOfBets);

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

	public void WinInfo(List<Player> players) {
		Game game = this; //TODO
		game.winners = game.GetWinnersAndSetWinPercentage (players);

		string winString = "";
		Bet winAmount = game.potAmount;
		
		// virtual players
		if (game.winners.Count > 1) {
			winAmount = game.potAmount/game.winners.Count;
			
			winString += game.winners[0].GetHandStringFromHandObj() + '\n';
			winString += string.Format("the pot was split in {0} ways\n".ToUpper(), game.winners.Count);
			winString += string.Format("(each player win {0} credits):\n".ToLower(), winAmount.inCredits.f());
			int no = 0;
			foreach(var player in game.winners) {
				no++;
				player.betTotal.inBet += winAmount.inBet;
				player.lblCredits.text = player.betTotal.inCredits.f();
				winString += string.Format ("{0}) {1}\n", no, player.name);
			}
		} else if (game.winners.Count == 1) { // one win player
			Player player = game.winners[0];
			if (player.isReal) game.ui.audio.PlayOneShot (game.ui.soundWin);
			player.betTotal += winAmount;
			player.lblCredits.text = player.betTotal.inCredits.f();
			winString += string.Format ("{2}\n\n{0} win\n {1} credits\n".ToUpper (), player.name, winAmount.inCredits.f(), player.GetHandStringFromHandObj ());
		}
		
		string winBonusString = "";
		if (game.player.isReal) {
			winBonusString = GetAndSetBonusString(game.player);
			if (!string.IsNullOrEmpty (winBonusString)) {
				winString += winBonusString;
			}
		}
		
//		game.potAmount = 0;
//		game.ui.lblPot.GetComponent<UnityEngine.UI.Text> ().text = game.potAmount.to_s();
		
		game.ui.lblWinInfo.GetComponent<UnityEngine.UI.Text> ().text = winString;
		
		game.ui.HideDynamicPanels ();
		game.ui.panelWin.SetActive (true);
	}
	
	public string GetAndSetBonusString(Player player) {
		Game game = this; // TODO
		// check for bet bonus
		string winBonusString = "";
		if (Settings.betBonus > 0 && player.isReal) {
			if (game.ui.payTable != null) {
				double winBonus = game.ui.payTable.GetAndSelectBonusWin (player);
				if (winBonus > 0) {
					game.ui.audio.PlayOneShot (game.ui.soundVideoWin);
					player.betTotal.inBet += winBonus;
					player.lblCredits.text = player.betTotal.inCredits.f();
					winBonusString = string.Format ("\n{0} win bonus {1} credits\n", player.name, winBonus.to_b ());
				}
			}
		}
		return winBonusString;
	}

	public List<Player> GetWinners(List<Player> players) {
		
		Hand winHandMax = players[0].hand;
		
		// detect max win hand
		foreach (var player in players) {
			if (player.hand > winHandMax && !player.isFolded) {
				winHandMax = player.hand;
			}
		}
		
		// detect winners
		List<Player> winners = new List<Player>();
		foreach (var player in players) {
			if (winHandMax == player.hand && !player.isFolded) {
				winners.Add(player);
			}
		}
		
		return winners;
	}
	
	public List<Player> GetWinnersAndSetWinPercentage(List<Player> players) {
		List<Player> winners = GetWinners (players);
		
		// start calculating the win percentage/hand strength
		if (winners.Count > 0) {
			double winPercentage = 100/winners.Count;
			foreach (var item in winners) {
				foreach (var player in players) {
					if (player.id == item.id && player.name == item.name) {
						item.winPercent = winPercentage;
						player.winPercent = winPercentage;
						player.isWinner = true;
					}
				}
			}
		}
		// end calculating the win percentage/hand strength
		
		return winners;
	}

	public List<Player> winners;

	public Constants source;
	public States states;
	public BetRound state;

	public Bet betAmountAnt, betAmount, potAmount;

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
}
