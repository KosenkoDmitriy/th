using System;

public class TurnRound : BetRound {
	public TurnRound(Game game) {
		this.game = game;
		this.betMin = Settings.betTurnRiver;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;

		// turn bet rounds
		var turns = game.source.GetTurns ();
		SetPatternAndHisAlternatives (turns);
		
		game.playerIterator = new PlayerIterator(game.playerCollection);
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
