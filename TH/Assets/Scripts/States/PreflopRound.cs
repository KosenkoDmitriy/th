using System;

public class PreflopRound : BetRound {
	public PreflopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.betPreflopFlopMultiplier;
	}

	public override void LastAction ()
	{
		base.LastAction ();
		game.state = new FlopRound (game);
	}

	public override void FirstAction ()
	{
//		base.FirstAction ();
		game.state.isWaiting = true;
		game.ui.StartCoroutine (game.ui.DealCards ());
		// game.state.isWaiting = false (must be in the end line of coroutine)

		game.playerIterator = new PlayerIterator (game.playerCollection);
	}

	public override void BetSubRounds () {
		base.BetSubRounds ();
	}
}
