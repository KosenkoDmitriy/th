using System;

public class AnteRound : BetRound {
	#region IBetRoundState implementation
	
	public AnteRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betAnteSubRoundMinSize;
		this.betMin = Settings.betAnte;
	}
	
	public override void BetSubRounds ()
	{
		//		base.BetSubRounds ();
		// TODO
		foreach (var player in game.players) {
			player.actionFinal = player.GetFinalAction (betMin, player.betAlreadyInvestedBeforeAction);
			if (!game.state.isWaiting) {
				if (player.isReal) {
					game.state.isWaiting = true;
					game.player = player;
					game.ui.panelGame.SetActive(true);
					game.ui.panelInitBet.SetActive(false);
//					if (player.isFolded) {
//						game.state = new EndGame(game);
//					}
				} else {
					player.actionFinal.Do (game);
					//			betToStayInGame += player.bet;
				}
			}
		}

		game.state.isWaiting = true;
		game.ui.StartCoroutine (game.ui.UpdatePlayers());
	}
	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new PreflopRound (game);
	}
	
	#endregion
}
