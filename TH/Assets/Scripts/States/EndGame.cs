using System;
using System.Collections.Generic;

public class EndGame : BetRound {
	public EndGame(Game game) {
		this.game = game;
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
	
	public override void FirstAction() {
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

		// TODO: split the pot between win players
		double winPercent = 0;
		foreach (var player in game.players) {
			if (winPercent <= 0) winPercent = player.winPercent;
			if (player.winPercent > winPercent) {
				winPercent = player.winPercent;
			}
		}
		List<Player> winPlayers = new List<Player>();
		string winHandString = "";
		foreach (var player in game.players) {
			if (winPercent == player.winPercent) {
				winPlayers.Add(player);
				winHandString = player.handWinBestString;
			}
			player.lblAction.text = player.handWinBestString; // show player's hand
		}
		string winString = "";
		double winAmount = game.potAmount/winPlayers.Count;
		if (winPlayers.Count > 1) {
			winString += string.Format("the pot was split in {0} ways:\n".ToUpper(), winPlayers.Count);
			int no = 0;
			foreach(var player in game.players) {
				no++;
				winString += string.Format("{0}) {1} {2}\n", no, player.name, winHandString);
				player.betTotal += winAmount;
				player.lblCredits.text = player.betTotal.to_s();
			}
		} else { // one win player
			Player player = winPlayers[0];
			winString = string.Format("{0} win\n {1}".ToUpper(), player.name, player.handWinBestString);
			player.betTotal += winAmount;
			player.lblCredits.text = player.betTotal.to_s();
		}
		game.potAmount = 0;
		game.ui.lblPot.GetComponent<UnityEngine.UI.Text> ().text = game.potAmount.to_s();
		//TODO: calculate bonus payouts

		game.ui.lblWinPlayerName.GetComponent<UnityEngine.UI.Text> ().text = winString;

		game.ui.HideDynamicPanels ();
		game.ui.panelWin.SetActive (true);
	}
}
