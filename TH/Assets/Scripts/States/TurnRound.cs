using System;

public class TurnRound : BetRound {
	public TurnRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.betTurnRiverMultiplier;

		this.betMaxLimit = new Bet(0);
		this.betMaxLimit.inBetMath = Settings.betMathLimit;

		// turn bet rounds
		var turns = game.source.GetTurns ();
		SetPatternAndHisAlternatives (turns);
	}
	
	public override void FirstAction() {
		game.ui.audio.PlayOneShot(game.ui.soundDeal);
		game.cards [3].FaceUp = true;
	}

	public override void LastAction ()
	{
		base.LastAction ();
		game.state = new RiverRound (game);
	}

	public override void BetSubRounds ()
	{
		base.BetSubRounds ();
	}
}
