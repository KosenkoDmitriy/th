using System;

public class AnteRound : BetRound {
	#region IBetRoundState implementation
	
	public AnteRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betAnteSubRoundMinSize;
		this.betMin = Settings.betAnte;
	}

	public override void FirstAction ()
	{
//		base.FirstAction ();
		this.game.playerIterator = new PlayerIterator(this.game.playerCollection);
	}

	public override void BetSubRounds ()
	{
		//		base.BetSubRounds ();
		// TODO
		var player = this.game.playerIterator.Next ();
		if (player == null) {
			LastAction ();
		} else {
			player.actionFinal = player.GetFinalAction (betMin, player.betAlreadyInvestedBeforeAction);
			if (!game.state.isWaiting) {
				if (player.isReal) {
					game.state.isWaiting = true;
					game.player = player;
					game.ui.panelGame.SetActive (true);
					game.ui.panelInitBet.SetActive (false);
//					if (player.isFolded) {
//						game.state = new EndGame(game);
//					}
				} else {
					player.actionFinal.Do (game);
//					betToStayInGame += player.bet;
				}
				game.ui.UpdatePlayer(player);
			}
		}

//		game.state.isWaiting = true;
//		game.ui.StartCoroutine (game.ui.UpdatePlayers());
	}
	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new PreflopRound (game);
	}
	
	#endregion
}
