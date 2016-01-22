using System;

public class RiverRound : BetRound {
	public RiverRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.betTurnRiverMultiplier;
		this.betMax = Settings.betCurrentMultiplier * Settings.betLimitTurnRiver;

		// rivers bet rounds
		var rivers = game.source.GetRivers ();
		SetPatternAndHisAlternatives (rivers);
	}
	
	public override void FirstAction() {
		game.ui.audio.PlayOneShot(game.ui.soundDeal);
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
