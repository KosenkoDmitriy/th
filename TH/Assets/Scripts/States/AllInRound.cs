using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class AllInRound : BetRound {

	public AllInRound(Game game, Player playerFirstToAllIn, double betDx) {
		this.game = game;
		this.playerFirstToAllIn = playerFirstToAllIn;
		this.betBeforeAllIn = betDx;
		this.subRoundMaxSize = 1;
		this.betMaxLimit = new Bet(Settings.betCurrentMultiplier);

		pots = new List<Pot>();
//		playerFirstToAllIn.isAllIn = true;

		double betTotal = 0;
		List<Player> before = new List<Player> ();
		List<Player> after = new List<Player> ();
		for (Player player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			if (!player.isFolded) {
				if(player.position < playerFirstToAllIn.position) {
					before.Add(player);
				} else if (player.position > playerFirstToAllIn.position) {
					after.Add(player);
				}
			}
		}
		
		//return back last betToStayInGame
		foreach (var player in before) {
			player.betInvested  -= betBeforeAllIn;
			player.balanceInCredits += betBeforeAllIn;
			game.potAmount += player.betInvested.inCredits;

			player.lblCredits.text = player.balanceInCredits.f();
			game.ui.lblPot.text = game.potAmount.f();
		}
		
		// reorder players - first all in player
		List<Player> players = new List<Player>();
		players.AddRange (before);
		players.AddRange (after);

		var playerCollection = new PlayerCollection ();
		int i = 0;
		foreach (var player in players) {
			playerCollection[i] = player;
			i++;
		}
		playerIterator = new PlayerIterator (playerCollection);

		playersAllIn = new List<Player>();

	}

	public override void FirstAction() {}
	
	public override void BetSubRounds () {
		if (!game.state.isWaiting) {
			player = playerIterator.Next();

			if (player == null) { // || (player.id == playerFirstToAllIn.id && player.isReal && player.isAllIn)) {
				subRoundCount++;
//				LastAction();
				return;
			} else if (player.isReal) {
				game.state.isWaiting = true;
//				game.player = player;
				if (Settings.isDev) game.ui.lblBet.text = string.Format("c:{0} m:{1}", Settings.betCurrent, game.state.betMax);
			} else {
				if (player.isWinner) {
					if (Settings.isDev) player.actionCurrentString += "> " + Settings.aAllIn + " (w)"; else player.actionCurrentString = Settings.aAllIn;
					player.lblAction.text = player.actionCurrentString;

					player.actionFinal = new AllIn(player, betMax, new Bet(0));
				} else {
					player.actionCurrentString = Settings.aFold;
					player.lblAction.text = player.actionCurrentString;

					player.actionFinal = new Fold(player, betMax, new Bet(0));
				}
				player.actionFinal.Do(game, player);
			}

		}
	}
	
	private List<Player> GetList(List<Player> playersAllIn) {
		minAllIn = GetMinBetTotal (playersAllIn);
		Pot pot = new Pot();
		var players = new List<Player>();
		foreach (var player in playersAllIn) {
			player.balanceInCredits -= minAllIn;
			pot.maxWinIfWin += minAllIn;
			if (player.balanceInCredits <= 0) {
				pot.players.Add(player);
			} else {
				players.Add(player);
			}
		}
		pots.Add (pot);
		return players;
	}
	
	private double GetMinBetTotal(List<Player> playersAllIn) {
		// detect player with min credits/betTotal
		double maxAllIn = 0;
		foreach (var player in playersAllIn) {
			if (player.balanceInCredits > maxAllIn) {
				maxAllIn = player.balanceInCredits;
			}
		}

		double minAllIn = maxAllIn;
		foreach (var player in playersAllIn) {
			if (player.balanceInCredits < minAllIn && player.balanceInCredits > 0) {
				minAllIn = player.balanceInCredits;
			}
		}
		return minAllIn;
	}

	public override void LastAction () {
//		LastActionDetailed (); // main pot, other pots (win info with details)
//		return;

		game.state.isWaiting = true;

		// display all community cards
		for (int i = 0; i < Settings.playerHandMaxSize; i++) {
			var card = game.cards[i];
			card.FaceUp = true;
		}

		var potAmountOld = game.potAmount;
		foreach (var player in playersAllIn) {
			player.ShowCards(game);

			game.potAmount += player.balanceInCredits;

			player.balanceInCredits = 0;
			player.lblCredits.text = player.balanceInCredits.f();
		}

		game.ui.lblPot.text = string.Format("{0} + {1} = {2}", potAmountOld.f (), game.potAmount.f (), (potAmountOld + game.potAmount).f() );
		game.potAmount += potAmountOld;
		
		game.WinInfo (playersAllIn);


		game.ui.panelWin.SetActive (true);
//		game.ui.lblWinInfo.text = winInfo;
		
		game.ui.panelGame.SetActive(true);
		game.ui.btnCall.GetComponent<Button>().interactable = true; 	//.SetActive(false);
		game.ui.btnCheck.GetComponent<Button>().interactable = true;	//.SetActive(false);
		game.ui.btnRaise.GetComponent<Button>().interactable = true;	//.SetActive(false);
		game.ui.btnFold.GetComponent<Button>().interactable = true;		//.SetActive(false);
		game.ui.btnAllIn.GetComponentInChildren<Text>().text = Settings.aAllIn;
		game.ui.panelGame.SetActive(false);
		
		playerFirstToAllIn = null;
	}

	public void LastActionDetailed () {
		game.state.isWaiting = true;

		game.winners = game.GetWinners (playersAllIn);
		
		double minAllIn = GetMinBetTotal (playersAllIn);

		// display all community cards
		for (int i = 0; i < Settings.playerHandMaxSize; i++) {
			var card = game.cards[i];
			card.FaceUp = true;
		}

		do {
			var list = GetList (playersAllIn);
			if (list.Count <= 0)
				break;
		} while (true);
		
		// detect winners
		string winInfo = "";

		// main pot
		double winPotAmount = game.potAmount/game.winners.Count;
		List<Player> winList = new List<Player> ();
		
		if (game.winners.Count > 0)
			winInfo += string.Format("{0} \n\n", game.winners [0].GetHandStringFromHandObj () );
		
		winInfo += "Main Pot:\n";
		foreach(var player in game.winners) {
			player.balanceInCredits += winPotAmount;
			player.lblCredits.text = player.balanceInCredits.f();
			winInfo += string.Format("{0} win {1}\n", player.name, winPotAmount.f());
			winList.Add (player);
//TODO
//			if (player.isReal) {
//				game.ui.audio.PlayOneShot (game.ui.soundWin);
//				// check for bonus
//				string winBonusString = game.GetAndSetBonusString(game.player);
//				if (!string.IsNullOrEmpty (winBonusString)) {
//					winInfo += winBonusString + '\n';
//				}
//			}
		}

		// others pots
		if (pots.Count > 0)
			winInfo += "\nOther Pots:\n";

		int no = 1;
		foreach (var pot in pots) {
			var tempWinners = game.GetWinners (pot.players);
			double winAmount = 0;
			winAmount = pot.maxWinIfWin / tempWinners.Count;
			foreach(var player in tempWinners) {
				player.ShowCards(game);

//				foreach(var winer in game.winners) {
//					if (winer.id == player.id) {
						player.balanceInCredits += winAmount;
						player.lblCredits.text = player.balanceInCredits.f();
//					}
//				}

				winInfo += string.Format("{2}) {0} win {1}\n", player.name, winAmount.f(), no);
				no++;
			}
		}

		game.ui.panelWin.SetActive (true);
		game.ui.lblWinInfo.text = winInfo;

		game.ui.panelGame.SetActive(true);
		game.ui.btnCall.GetComponent<Button>().interactable = true; 	//.SetActive(false);
		game.ui.btnCheck.GetComponent<Button>().interactable = true;	//.SetActive(false);
		game.ui.btnRaise.GetComponent<Button>().interactable = true;	//.SetActive(false);
		game.ui.btnFold.GetComponent<Button>().interactable = true;		//.SetActive(false);
		game.ui.btnAllIn.GetComponentInChildren<Text>().text = Settings.aAllIn;
		game.ui.panelGame.SetActive(false);

		playerFirstToAllIn = null;
	}

	Player player;//current
	List<Pot> pots;
	double betBeforeAllIn;
	double minAllIn;
	PlayerIterator playerIterator;
}

public class Pot {
	public Pot() {
		players = new List<Player>();
	}
	public List<Player> players;
	public double pot;
	public double maxWinIfWin;
}
