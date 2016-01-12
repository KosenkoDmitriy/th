using System;

public class RiverRound : BetRound {
	public RiverRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.betTurnRiverMultiplier;

		// rivers bet rounds
		var rivers = game.source.GetRivers ();
		SetPatternAndHisAlternatives (rivers);

		game.playerIterator = new PlayerIterator(game.playerCollection);
	}
	
	public override void FirstAction() {
		game.cards [4].FaceUp = true;
	}

	public override void LastAction ()
	{
		base.LastAction ();
		game.state = new EndGame (game);
	}

	public override void BetSubRounds ()
	{
		base.BetSubRounds ();
	}
}
