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
		List<Player> playersAllIn = before;
		playersAllIn.AddRange (after);

		do {
			double minAllIn = GetMinBetTotal (playersAllIn);
			Pot pot = new Pot();
			foreach (var player in playersAllIn) {
				player.betAlreadyInvestedInCurrentSubRound += minAllIn;
				player.betTotal -= minAllIn;
				pot.pot = game.potAmount + betTotal;
				pot.maxWinIfWin += minAllIn;

				if (player.betTotal <= 0) {
					pot.players.Add(player);
					playersAllIn.Remove (player);
				}
			}
		} while (playersAllIn.Count > 0);

		//TODO: detect winners
		foreach (var pot in pots) {
			game.winners = game.GetWinnersAndSetWinPercentage (pot.players); 
		}


//		PlayerCollection coll = new PlayerCollection ();
//		for(int i = 0; i < players
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
