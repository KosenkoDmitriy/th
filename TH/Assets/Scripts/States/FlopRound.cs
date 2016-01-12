using System;

public class FlopRound : BetRound {
	public FlopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.betPreflopFlopMultiplier;

		// flop bet rounds
		var flops = game.source.GetFlops ();
		SetPatternAndHisAlternatives (flops);
		
		game.playerIterator = new PlayerIterator(game.playerCollection);
	}

	public override void FirstAction() {
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
		base.BetSubRounds ();
	}
}
