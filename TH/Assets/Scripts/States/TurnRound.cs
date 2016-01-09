using System;

public class TurnRound : BetRound {
	public TurnRound(Game game) {
		this.game = game;
		this.betMin = Settings.betTurnRiver;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
	}
	
	public override void FirstAction() {
		game.cards [3].FaceUp = true;
	}
	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new RiverRound (game);
	}
	public override void BetSubRounds ()
	{
		base.BetSubRounds ();
	}
}
