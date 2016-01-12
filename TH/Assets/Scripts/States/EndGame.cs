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
			winString += string.Format("the pot was split in {0} ways:\n".ToUpper(), game.winners.Count);
			int no = 0;
			foreach(var player in game.winners) {
				player.betTotal += winAmount;
				no++;
				winString += string.Format("{0}) {1} win {2} credits\n", no, player.name, player.betTotal);
				player.lblCredits.text = player.betTotal.to_s();
			}
		} else { // if (game.winners.Count == 1) { // one win player
			Player player = game.winners[0];
			winString = string.Format("{0} win\n {1} credits \n{2}".ToUpper(), player.name, game.potAmount.to_s(), player.GetHandStringFromHandObj());
			player.betTotal += winAmount;
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

	public override void LastAction ()
	{
		//		base.LastAction ();
		//		game.state = new InitGame (game);
	}

	public override void BetSubRounds ()
	{
		//		base.BetSubRounds ();
	}

	public override void FirstAction() {}
}
