using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class AllInRound : BetRound {

	public AllInRound(Game game, Player playerFirstToAllIn, double betDx) {
		this.game = game;
		this.playerFirstToAllIn = playerFirstToAllIn;
		this.betBeforeAllIn = betDx;
		this.subRoundMaxSize = 1;

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
			player.betAlreadyInvestedInCurrentSubRound -= betBeforeAllIn;
			player.betTotal += betBeforeAllIn;
			game.potAmount += player.betAlreadyInvestedInCurrentSubRound;

			player.lblCredits.text = player.betTotal.to_s();
			game.ui.lblPot.text = game.potAmount.to_s();
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

				game.ui.HideDynamicPanels();
				game.ui.panelGame.SetActive(true);
				game.ui.btnCall.GetComponent<Button>().interactable = false; 	//.SetActive(false);
				game.ui.btnCheck.GetComponent<Button>().interactable = false;	//.SetActive(false);
				game.ui.btnRaise.GetComponent<Button>().interactable = false;	//.SetActive(false);
				game.ui.btnFold.GetComponent<Button>().interactable = false;	//.SetActive(false);
				game.ui.btnAllIn.GetComponentInChildren<Text>().text = "CONTINUE";

			} else {
				if (player.isWinner) {
					if (Settings.isDev) player.actionCurrentString += "> ALL IN (w)"; else player.actionCurrentString = "ALL IN";
					player.lblAction.text = player.actionCurrentString;

					player.actionFinal = new AllIn(player, betMax);
				} else {
					player.actionCurrentString = "FOLD";
					player.lblAction.text = player.actionCurrentString;

					player.actionFinal = new Fold(player, betMax);
				}
				player.actionFinal.Do(game);
			}

		}
	}
	
	private List<Player> GetList(List<Player> playersAllIn) {
		minAllIn = GetMinBetTotal (playersAllIn);
		Pot pot = new Pot();
		var players = new List<Player>();
		foreach (var player in playersAllIn) {
			player.betTotal -= minAllIn;
			pot.maxWinIfWin += minAllIn;
			if (player.betTotal <= 0) {
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
			if (player.betTotal > maxAllIn) {
				maxAllIn = player.betTotal;
			}
		}

		double minAllIn = maxAllIn;
		foreach (var player in playersAllIn) {
			if (player.betTotal < minAllIn && player.betTotal > 0) {
				minAllIn = player.betTotal;
			}
		}
		return minAllIn;
	}

	
	public override void LastAction () {
		game.state.isWaiting = true;

		game.winners = game.GetWinnersAndSetWinPercentage (playersAllIn);
		
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
		
		//TODO: detect winners
		string winInfo = "";

//		foreach (var pot in pots) {
//			pot.pot = game.potAmount - pot.maxWinIfWin;
//		}

		// main pot
		double winPotAmount = game.potAmount/game.winners.Count;
//		winInfo += "Main Pot:\n";
		foreach(var player in game.winners) {
			player.betTotal += winPotAmount;
			player.lblCredits.text = player.betTotal.to_s();
//			winInfo += string.Format("{0} win {1}\n", player.name, winPotAmount.to_s());
		}

//		if (pots.Count > 0) {
////			winInfo += "\nOther Pots:\n";
//			winInfo += "\nmore info:\n";
//		}

		// others pots
		int no = 1;
		foreach (var pot in pots) {
			var tempWinners = game.GetWinnersAndSetWinPercentage (pot.players);
			double winAmount = pot.maxWinIfWin / tempWinners.Count;
			foreach(var winer in game.winners) {
				winer.ShowCards(game);

				foreach(var player in tempWinners) {
					if (winer.id == player.id) {
						player.betTotal += winAmount;
						player.lblCredits.text = player.betTotal.to_s();
//						winInfo += string.Format("{2}) {0} win {1}\n", player.name, winAmount.to_s(), no);
//						winInfo += string.Format("{0} win {1}\n", player.name, winAmount.to_s());
						no++;
					}
				}
			}
		}

//		foreach (var player in game.winners) {
//			winInfo += string.Format ("{0} win {1} credits\n", player.name, player.betTotal.to_s ());
//		}
		
//		game.ui.panelWin.SetActive (true);
//		game.ui.lblWinInfo.text = winInfo;

		game.WinInfo (game.winners);


		game.ui.panelGame.SetActive(true);
		game.ui.btnCall.GetComponent<Button>().interactable = true; 	//.SetActive(false);
		game.ui.btnCheck.GetComponent<Button>().interactable = true;	//.SetActive(false);
		game.ui.btnRaise.GetComponent<Button>().interactable = true;	//.SetActive(false);
		game.ui.btnFold.GetComponent<Button>().interactable = true;		//.SetActive(false);
		game.ui.btnAllIn.GetComponentInChildren<Text>().text = "ALL IN";
		game.ui.panelGame.SetActive(false);

		playerFirstToAllIn = null;

	}

	Player player;//current
//	Game game;
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
