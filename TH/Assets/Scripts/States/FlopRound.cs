using System;

public class FlopRound : BetRound {
	public FlopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
	}
	public override void FirstAction() {
		game.cards [0].FaceUp = true;
		game.cards [1].FaceUp = true;
		game.cards [2].FaceUp = true;
	}
	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new TurnRound (game);
	}
	public override void BetSubRounds ()
	{
		base.BetSubRounds ();
	}
}
