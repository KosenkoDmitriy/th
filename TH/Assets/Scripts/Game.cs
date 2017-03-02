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
			player.balanceInCredits = Settings.playerCredits;

			player.chip = GameObject.Find("Chip"+i).GetComponent<Image>();
			player.dealer = GameObject.Find("Dealer"+i).GetComponent<Image>();

			player.winImage = GameObject.Find("win"+i).GetComponentInChildren<Image>();
			if (player.id == dealerIndex)
				player.isDealer = true; 
			else
				player.isDealer = false;
			player.lblAction = GameObject.Find ("lblBetPlayer"+i).GetComponent<Text>();
			player.lblCredits = GameObject.Find ("lblCreditPlayer"+i).GetComponent<Text>();
			player.lblName = GameObject.Find("lblPlayerName"+i).GetComponent<Text>();
			player.lblCurBet = GameObject.Find("lblCurBet"+i).GetComponent<Text>();

			player.isWinHidden = true;

			player.isChipHidden = true;
			player.lblCurBet.text = "";

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
		playerList.AddRange(playersAfterDealer);
		playerList.AddRange(playersBeforeDealer);

		int j = 0;
		foreach (var player in playerList) {
			if (j > 0) {
				player.avatar = GameObject.Find("Avatar"+j).GetComponent<Image>();
				string path = "avatars/avatar"+(j);//Settings.avatarDefault;
				player.avatar.sprite = Resources.Load<Sprite>(path);
			}
			player.position = j;	// for math (first to act player)
			j++;
		}

		dealerIndex++;

		return playerList;
	}

	public void WinInfo(List<Player> players) {
		Game game = this;
		game.winners = game.GetWinners (players);

		string winString = "";
		double winAmount = game.potAmount;
		
		// virtual players
		if (game.winners.Count > 1) {
			winAmount = game.potAmount/game.winners.Count;
			
			winString += game.winners[0].GetHandStringFromHandObj() + '\n';
			winString += string.Format("the pot was split in {0} ways\n".ToUpper(), game.winners.Count);
			winString += string.Format("(each player win {0} credits):\n".ToLower(), winAmount.f());
			int no = 0;
			foreach(var player in game.winners) {
				no++;
				player.balanceInCredits += winAmount;
				player.lblCredits.text = player.balanceInCredits.f();
				winString += string.Format ("{0}) {1}\n", no, player.name);
				if (player.isReal) game.ui.WinBalance(winAmount.ToString());
			}
		} else if (game.winners.Count == 1) { // one win player
			Player player = game.winners[0];
			if (player.isReal) {
				game.ui.audio.PlayOneShot (game.ui.soundWin);
				game.ui.WinBalance(winAmount.ToString());
			}
			player.balanceInCredits += winAmount;
			player.lblCredits.text = player.balanceInCredits.f();
			winString += string.Format ("{2}\n\n{0} win\n {1} credits\n".ToUpper (), player.name, winAmount.f(), player.GetHandStringFromHandObj ());
		}

		foreach(var player in game.winners)
			player.isWinHidden = false;

		string winBonusString = "";
//		if (game.player.isReal && game.player.isWinner && !game.player.isFolded) { // only active winner player will get bonus
		if (game.player.isReal) { // real player should be able to lose the hand and still win the bonus bet
				winBonusString = GetAndSetBonusString(game.player, game.winners.Count);
			if (!string.IsNullOrEmpty (winBonusString)) {
				winString += '\n' + winBonusString;
			}
		}

		Settings.playerCredits = game.player.balanceInCredits;

//		game.potAmount = 0;
//		game.ui.lblPot.GetComponent<UnityEngine.UI.Text> ().text = game.potAmount.f();

		game.ui.lblWinInfo.text = winString;
		game.ui.lblWinBonusInfo.text = winBonusString;

		game.ui.HideDynamicPanels ();
		game.ui.panelWin.SetActive (true);

		game.ui.GetBalance(false);
	}
	
	public string GetAndSetBonusString(Player player, int winnersCount) {
		Game game = this; // TODO
		// check for bet bonus
		string winBonusString = "";
		if (Settings.betBonus > 0 && player.isReal) {
			if (game.ui.payTable != null) {
				double winBonus = game.ui.payTable.GetAndSelectBonusWin (player);
				if (winBonus > 0) {
					/*
					 * bonus table value (btv)
					 * 50 credits = btv x 5
					 * 40 credits = btv x 4
					 * 30 credits = btv x 3
					 * 20 credits = btv x 2
					 * 10 credits = btv x 1
					 */
					//winBonus *= game.ui.payTable.selectedCol; // where selectedCol is multiplier 
					game.ui.WinBalance(winBonus.ToString());
//					if (winnersCount > 1) { 
//						winBonus /= 2;
//						winBonusString = string.Format ("{0} win half of bonus {1} credits\n", player.name, winBonus.to_b ());
//					} else {
					//winBonusString = string.Format ("{0} win bonus: {1} x {2} = {3} credits\n", player.name, (winBonus / game.ui.payTable.selectedCol).f (), game.ui.payTable.selectedCol, winBonus.f ());
					winBonusString = string.Format ("{0} win bonus: {1} credits\n", player.name, winBonus.f ());
					//					}

					game.ui.audio.PlayOneShot (game.ui.soundVideoWin);

					player.balanceInCredits += winBonus;// * Settings.betCreditsMultiplier;
					player.lblCredits.text = player.balanceInCredits.f();
				}
			}
		}
		return winBonusString;
	}

	public List<Player> GetWinners(List<Player> players) {
		
		Hand winHandMax = players[0].hand;
		
		// detect max win hand
		foreach (var player in players) {
			if (!player.isFolded && player.hand > winHandMax)
				winHandMax = player.hand;
		}
		
		List<Player> winners = new List<Player>();
		foreach (var player in players) {
			if (winHandMax == player.hand && !player.isFolded) {
				player.winPercent = 100;
				player.isWinner = true;
				winners.Add(player);
			}
		}
		
		return winners;
	}
	
	public List<Player> GetPlayersAndSetWinPercentage(List<Player> players) {
		if (this.winners == null || this.winners.Count == 0) {
			this.winners = GetWinners (players);
		}

		// set win percetage for winners
		foreach (var winner in winners)
			foreach (var player in players)
				if (player.id == winner.id) {
					player.winPercent = winner.winPercent;
					player.isWinner = winner.isWinner;
				}

		// sort by low hand strength
		List<Player> playersSorted = new List<Player> ();
		playersSorted.AddRange (players);

		Player tempPlayer = null;
		for(int i = 0; i < playersSorted.Count - 1; i++)
		{
			for(int j = i + 1; j < playersSorted.Count; j++)
			{
				if (playersSorted[i].hand < playersSorted[j].hand)
				{
					tempPlayer = playersSorted[i];
					playersSorted[i] = playersSorted[j];
					playersSorted[j] = tempPlayer;
				}
			}
		}

		List<Player> loosers = new List<Player> ();
		foreach (var player in playersSorted) {
			if (player.hand < winners[0].hand) {
				loosers.Add(player);
			}
		}

		// win percentage for losers
		if (loosers != null || loosers.Count > 0) {
		var dp = 100 / loosers.Count;
		int no = 1;
		foreach (var player in loosers) {
			if (!player.isWinner) {
				double winPercent = 100 - no * dp;
				player.winPercent = winPercent;
				no++;
			}
		}

		foreach (var l in loosers)
			foreach (var player in players)
				if (player.id == l.id)
					player.winPercent = l.winPercent;
		}
		return players;
	}

	public List<Player> winners;

	public Constants source;
	public States states;
	public BetRound state;

	public Bet betAmount = new Bet(0);
	public double potAmount;

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
