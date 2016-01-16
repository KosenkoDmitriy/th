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
		for (int i = 0; i < 5; i++) {
			var card = game.cards[i];
//			if (!card.FaceUp)
				card.FaceUp = true;
		}

		//		roundCount = subRoundCount = 0;
		//		betCurrentToStayInGame = betTotalInThisRound = 0;

		// display/hide cards
		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			foreach (var card in player.handPreflop.getCards()) {
				if (player.id != Settings.playerRealIndex)
				if (player.isFolded) {
					card.isHidden = true;
				} else {
					card.FaceUp = true;
				}
			}
		}

		// display hand combinations for any player
		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			string winHandString = player.GetHandStringFromHandObj();
			player.lblAction.text = winHandString; // show player's hand
		}

		// split the pot between win players
		var players = new List<Player> ();
		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			players.Add(player);
		}
		game.WinInfo (players);

//		LastAction ();
//		game.state.isWaiting = true;
	}


	public override void LastAction () {}

	public override void BetSubRounds () {}

	public override void FirstAction() {}
}
