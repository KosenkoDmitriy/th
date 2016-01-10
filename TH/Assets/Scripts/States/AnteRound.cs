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
