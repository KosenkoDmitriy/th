using System;
using System.Collections.Generic;

public class EndGame : BetRound {
	public EndGame(Game game) {
		this.game = game;

		if (Settings.isDebug)
			game.ui.DebugLog ("EndGame()");

		game.isGameRunning = false;
		game.isGameEnd = true;

		// display all community cards
		for (int i = 0; i < Settings.playerHandMaxSize; i++) {
			var card = game.cards[i];
				card.FaceUp = true;
		}

		// hide cards for folded players, show cards for active players
		foreach(var player in game.players) {
			foreach (var card in player.handPreflop.getCards()) {
				if (player.isFolded) {
					card.isHidden = true;
				} else {
					card.FaceUp = true;
				}
				if (player.isReal) card.FaceUp = true;
			}
		}

		// display hand combination for active players only
		foreach(var player in game.players) {
			if (!player.isFolded || Settings.isDev || (player.isFolded && player.isReal)) {
				string winHandString = player.GetHandStringFromHandObj();
				player.lblAction.text = winHandString; // show player's hand
			} else {
				player.lblAction.text = "";
			}
		}

		// preparation active players for winners detection
		var players = new List<Player> ();
		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			players.Add(player);
		}
		game.WinInfo (players);
	}


	public override void LastAction () {}

	public override void BetSubRounds () {}

	public override void FirstAction() {}
}
