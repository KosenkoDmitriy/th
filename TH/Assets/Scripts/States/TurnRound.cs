using System;

public class TurnRound : BetRound {
	public TurnRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.betTurnRiverMultiplier;

		this.betMaxLimit = new Bet(0);
		this.betMaxLimit.inBetMath = Settings.betMathLimit;

		// turn bet rounds
		items = game.source.GetTurns ();
		UpdatePattern ();
	}
	
	public override void FirstAction() {
		base.FirstAction ();
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
		if (Settings.isDev) game.player.Log(false, true, string.Format("Turn BetSubRounds() {0}/{1}", subRoundCount, subRoundMaxSize));
		base.BetSubRounds ();
	}
}
