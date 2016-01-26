using System;

public class FlopRound : BetRound {
	public FlopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.bePreflopFlopMultiplier;

		// flop bet rounds
		var flops = game.source.GetFlops ();
		SetPatternAndHisAlternatives (flops);
	}

	public override void FirstAction() {
		base.FirstAction ();
		game.ui.audio.PlayOneShot(game.ui.soundDeal);
		game.cards [0].FaceUp = true;
		game.cards [1].FaceUp = true;
		game.cards [2].FaceUp = true;
	}

	public override void LastAction ()
	{
		base.LastAction ();
		game.state = new TurnRound (game);
	}

	public override void BetSubRounds ()
	{
		if (Settings.isDev) game.player.Log(true, false, string.Format("Flop BetSubRounds() {0}/{1}", subRoundCount, subRoundMaxSize));
		base.BetSubRounds ();
	}
}
