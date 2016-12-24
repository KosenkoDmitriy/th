using System;

public class RiverRound : BetRound {
	public RiverRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.betTurnRiverMultiplier;

		this.betMaxLimit = new Bet(0);
		this.betMaxLimit.inBetMath = Settings.betMathLimit;

		// rivers bet rounds
		items = game.source.GetRivers ();
		UpdatePattern ();
	}
	
	public override void FirstAction() {
		base.FirstAction ();
		game.ui.audio.PlayOneShot(game.ui.soundDeal);
		game.cards [4].FaceUp = true;
	}

	public override void LastAction ()
	{
		//if (!base.IsOneActivePlayer()) {
			base.LastAction ();
			game.state = new EndGame (game);
		//}
	}

	public override void BetSubRounds ()
	{
		if (Settings.isLog) game.player.Log(false, true, string.Format("River BetSubRounds() {0}/{1}", subRoundCount, subRoundMaxSize));
		base.BetSubRounds ();
	}
}
