using System;
using System.Collections.Generic;

public class EndGame : BetRound {
	public EndGame(Game game) {
		this.game = game;
	

		if (Settings.isDebug)
			game.ui.DebugLog ("EndGame()");

		game.isGameRunning = false;
		game.isGameEnd = true;
		
		//		roundCount = subRoundCount = 0;
		//		betCurrentToStayInGame = betTotalInThisRound = 0;

		// display/hide cards
		foreach (var player in game.players) {
			foreach (var card in player.handPreflop.getCards()) {
				if (player.id != Settings.playerRealIndex)
				if (player.isFolded) {
					//						card.setImage(game.ui.cardBg); // hide
					card.isHidden = true;
				} else {
					card.FaceUp = true;
				}
			}
		}

		// split the pot between win players
		string winHandString = "";
		foreach (var player in game.players) {
			winHandString = player.GetHandStringFromHandObj();
			player.lblAction.text = winHandString; // show player's hand
		}

		string winString = "";
		double winAmount = game.potAmount/game.winners.Count;
		if (game.winners.Count > 1) {
			winString += winHandString + '\n';
			winString += string.Format("the pot was split in {0} ways\n".ToUpper(), game.winners.Count);
			winString += string.Format("(each player win {0} credits):\n".ToLower(), winAmount.to_s());
			int no = 0;
			foreach(var player in game.winners) {
				player.betTotal += winAmount;
				no++;
				winString += string.Format("{0}) {1} \n", no, player.name);
				player.lblCredits.text = player.betTotal.to_s();
			}
		} else { // if (game.winners.Count == 1) { // one win player
			Player player = game.winners[0];

			game.ui.audio.PlayOneShot(game.ui.soundWin);
			if (Settings.betBonus > 0) {
				if (game.ui.payTable != null) {
					double winBonus = game.ui.payTable.GetAndSelectBonusWin(player);
					if (winBonus > 0) {
//						winString += string.Format("Bonus: {0}\n", winBonus);
						player.betTotal += winBonus;
						game.ui.audio.PlayOneShot(game.ui.soundVideoWin);
					}
				}
			}
			player.betTotal += winAmount;

			winString += string.Format("{0} win\n {1} credits \n ({2})".ToUpper(), player.name, player.betTotal, player.GetHandStringFromHandObj());

			player.lblCredits.text = player.betTotal.to_s();
		}

		game.potAmount = 0;
		game.ui.lblPot.GetComponent<UnityEngine.UI.Text> ().text = game.potAmount.to_s();

		//TODO: calculate bonus payouts

		game.ui.lblWinPlayerName.GetComponent<UnityEngine.UI.Text> ().text = winString;

		game.ui.HideDynamicPanels ();
		game.ui.panelWin.SetActive (true);

//		LastAction ();
//		game.state.isWaiting = true;
	}

	public override void LastAction () {}

	public override void BetSubRounds () {}

	public override void FirstAction() {}
}
