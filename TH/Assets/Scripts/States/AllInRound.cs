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
				} else if (player.position >= playerFirstToAllIn.position) {
					after.Add(player);
				}
			}
		}
		
		//return back last betToStayInGame
		foreach (var player in before) {
			player.betAlreadyInvestedInCurrentSubRound -= betBeforeAllIn;
			player.betTotal += betBeforeAllIn;
			game.potAmount += player.betAlreadyInvestedInCurrentSubRound;
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

			if (player == null) {
				subRoundCount++;
//				LastAction();
				return;
			}

//			if (player.id == playerFirstToAllIn.id && player.isAllIn) {
//				if (playersAllIn.Count > 0) {
//					subRoundCount ++;
//					LastAction();
//				}
//			}

			if (player.isReal) {
				game.state.isWaiting = true;

				game.ui.HideDynamicPanels();
				game.ui.panelGame.SetActive(true);
				game.ui.btnCall.GetComponent<Button>().interactable = false; 	//.SetActive(false);
				game.ui.btnCheck.GetComponent<Button>().interactable = false;	//.SetActive(false);
				game.ui.btnRaise.GetComponent<Button>().interactable = false;	//.SetActive(false);

			} else {
				if (player.isWinner) {
					player.actionFinal = new AllIn(player, betMax);
				} else {
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
//			player.betAlreadyInvestedInCurrentSubRound += minAllIn;
			player.betTotal -= minAllIn;
//			pot.pot = game.potAmount + player.betTotal;
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
		double minAllIn = playerFirstToAllIn.betTotal;
		foreach (var player in playersAllIn) {
			if (player.betTotal < minAllIn) {
				minAllIn = player.betTotal;
			}
		}
		return minAllIn;
	}

	
	public override void LastAction () {
		game.state.isWaiting = true;

		game.winners = game.GetWinnersAndSetWinPercentage (playersAllIn);
		
		double minAllIn = GetMinBetTotal (playersAllIn);
		
		do {
			var list = GetList (playersAllIn);
			if (list.Count <= 0)
				break;
		} while (true);
		
		//TODO: detect winners
		string winInfo = "ALL IN";

		// main pot
		double winPotAmount = game.potAmount/game.winners.Count;
		foreach(var player in game.winners) {
			player.betTotal += winPotAmount;
			player.lblCredits.text = player.betTotal.to_s();
		}

		// others pots
		foreach (var pot in pots) {
			var tempWinners = game.GetWinnersAndSetWinPercentage (pot.players);
			double winAmount = pot.maxWinIfWin/tempWinners.Count;
			foreach(var winer in game.winners) {
				foreach(var player in tempWinners) {
					if (winer.id == player.id) {
						player.betTotal += winAmount;
						player.lblCredits.text = player.betTotal.to_s();
					}
				}
			}
		}

		
		game.ui.panelGame.SetActive(true);
		game.ui.btnCall.GetComponent<Button>().interactable = true; 	//.SetActive(false);
		game.ui.btnCheck.GetComponent<Button>().interactable = true;	//.SetActive(false);
		game.ui.btnRaise.GetComponent<Button>().interactable = true;	//.SetActive(false);
//		game.ui.btnFold.GetComponent<Button>().interactable = true;	//.SetActive(false);
		game.ui.panelGame.SetActive(false);


		game.ui.panelWin.SetActive (true);
		game.ui.lblWinInfo.text = winInfo;

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
