using System;

public class EndGame : BetRound {
	public EndGame(Game game) {
		this.game = game;
	}
	
	public override void LastAction ()
	{
		base.LastAction ();
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
		
		game.ui.HideDynamicPanels ();
		game.ui.panelWin.SetActive (true);
	}
}
