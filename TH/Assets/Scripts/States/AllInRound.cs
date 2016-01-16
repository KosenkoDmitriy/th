using System;
using System.Collections.Generic;

public class AllInRound : BetRound {
	Player playerFirstToAllIn;
	Game game;
	List<Pot> pots;

	public AllInRound(Game game, Player playerFirstToAllIn, double betDx) {
		this.game = game;
		this.playerFirstToAllIn = playerFirstToAllIn;
		pots = new List<Pot>();

		playerFirstToAllIn.isAllIn = true;

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
			player.betAlreadyInvestedInCurrentSubRound -= betDx;
			player.betTotal += betDx;
		}

		// reorder players - first all in player
		List<Player> playersAllIn = new List<Player>();
		playersAllIn.AddRange (before);
		playersAllIn.AddRange (after);

		game.winners = game.GetWinnersAndSetWinPercentage (playersAllIn);

		List<Player> players = playersAllIn;

		double minAllIn = GetMinBetTotal (playersAllIn);

		do {
			var list = GetList(playersAllIn);
			if (list.Count <= 0)
				break;
		} while (true);

		//TODO: detect winners
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


//		PlayerCollection coll = new PlayerCollection ();
//		for(int i = 0; i < players
	}
	double minAllIn;
	private List<Player> GetList(List<Player> playersAllIn) {
		minAllIn = GetMinBetTotal (playersAllIn);
		Pot pot = new Pot();
		var players = new List<Player>();
		foreach (var player in playersAllIn) {
			player.betAlreadyInvestedInCurrentSubRound += minAllIn;
			player.betTotal -= minAllIn;
			pot.pot = game.potAmount + player.betTotal;
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


	public override void LastAction () {}
	
	public override void BetSubRounds () {}
	
	public override void FirstAction() {}
}

public class Pot {
	public Pot() {
		players = new List<Player>();
	}
	public List<Player> players;
	public double pot;
	public double maxWinIfWin;
}
