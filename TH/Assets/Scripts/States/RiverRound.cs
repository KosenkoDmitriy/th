using System;

public class RiverRound : BetRound {
	public RiverRound(Game game) {
		this.game = game;
		this.betMin = Settings.betTurnRiver;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
	}
	
	public override void FirstAction() {
		game.cards [4].FaceUp = true;
	}
	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new EndGame (game);
	}
	public override void BetSubRounds ()
	{
		base.BetSubRounds ();
	}
}
