using System;

public class TurnRound : BetRound {
	public TurnRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.betTurnRiverMultiplier;

		// turn bet rounds
		var turns = game.source.GetTurns ();
		SetPatternAndHisAlternatives (turns);
		
		game.playerIterator = new PlayerIterator(game.playerCollection);
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
