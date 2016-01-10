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
			player.actionFinal = player.GetFinalAction (10, player.betAlreadyInvestedBeforeAction);
			player.actionFinal.Do ();
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
