using System;
using UnityEngine.UI;

public class PreflopRound : BetRound {
	public PreflopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
	}

	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new FlopRound (game);
	}

	public override void FirstAction ()
	{
		//		base.FirstAction ();
		isWaiting = true;
		game.ui.StartCoroutine (game.ui.DealCards ());
		// isWaiting = false (must be in the end line of coroutine)
	}

	public override void BetSubRounds () {

	}
}
