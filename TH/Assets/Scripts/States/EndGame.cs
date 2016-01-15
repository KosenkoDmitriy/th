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
		foreach (var player in game.players) {
			foreach (var card in player.handPreflop.getCards()) {
				if (player.id != Settings.playerRealIndex)
				if (player.isFolded) {
					card.isHidden = true;
				} else {
					card.FaceUp = true;
				}
			}
		}

		// display hand combinations
		foreach (var player in game.players) {
			string winHandString = player.GetHandStringFromHandObj();
			//			player.lblAction.text = winHandString; // show player's hand
			player.lblName.text = winHandString; // show player's hand
		}

		// split the pot between win players
		game.winners = game.GetWinnersAndSetWinPercentage ();

		string winString = "";
		double winAmount = game.potAmount;

		// virtual players
		if (game.winners.Count > 1) {
			winAmount = game.potAmount/game.winners.Count;

			winString += game.winners[0].GetHandStringFromHandObj() + '\n';
			winString += string.Format("the pot was split in {0} ways\n".ToUpper(), game.winners.Count);
			winString += string.Format("(each player win {0} credits):\n".ToLower(), winAmount.to_s());
			int no = 0;
			foreach(var player in game.winners) {
				no++;
				player.betTotal += winAmount;
				player.lblCredits.text = player.betTotal.to_s();
				winString += string.Format ("{0}) {1}\n", no, player.name);
			}
		} else if (game.winners.Count == 1) { // one win player
			Player player = game.winners[0];
			if (player.isReal) game.ui.audio.PlayOneShot (game.ui.soundWin);
			player.betTotal += winAmount;
			player.lblCredits.text = player.betTotal.to_s();
			winString += string.Format ("{2}\n\n{0} win\n {1} credits\n".ToUpper (), player.name, winAmount.to_s (), player.GetHandStringFromHandObj ());
		}

		string winBonusString = "";
		if (game.player.isReal) {
			winBonusString = GetAndSetBonusString(game.player, winAmount);
			if (!string.IsNullOrEmpty (winBonusString)) {
				winString += winBonusString;
			}
		}

		game.potAmount = 0;
		game.ui.lblPot.GetComponent<UnityEngine.UI.Text> ().text = game.potAmount.to_s();

		game.ui.lblWinInfo.GetComponent<UnityEngine.UI.Text> ().text = winString;

		game.ui.HideDynamicPanels ();
		game.ui.panelWin.SetActive (true);

//		LastAction ();
//		game.state.isWaiting = true;
	}

	private string GetAndSetBonusString(Player player, double winAmount) {
		// check for bet bonus
		string winBonusString = "";
		if (Settings.betBonus > 0 && player.isReal) {
			if (game.ui.payTable != null) {
				double winBonus = game.ui.payTable.GetAndSelectBonusWin (player);
				if (winBonus > 0) {
					game.ui.audio.PlayOneShot (game.ui.soundVideoWin);
					player.betTotal += winBonus;
					player.lblCredits.text = player.betTotal.to_s();
					winBonusString = string.Format ("\n{0} win bonus {1} credits\n", player.name, winBonus.to_b ());
				}
			}
		}
		return winBonusString;
	}

	public override void LastAction () {}

	public override void BetSubRounds () {}

	public override void FirstAction() {}
}
