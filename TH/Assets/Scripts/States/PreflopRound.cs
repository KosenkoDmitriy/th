using System;

public class PreflopRound : BetRound {
	public PreflopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.bePreflopFlopMultiplier;
		this.betMaxLimit = new Bet(Settings.betCurrentMultiplier);

		game.ui.btnBetBonus.GetComponent<UnityEngine.UI.Button> ().interactable = false; // enable "bet bonus" button
	}
	
	public override void FirstAction ()
	{
		game.state.isWaiting = true;
		game.ui.StartCoroutine (game.ui.DealCards ());	// game.state.isWaiting = false (must be in the end line of coroutine)
	}

	public override void LastAction ()
	{
		base.LastAction ();
		game.state = new FlopRound (game);
	}

	public override void BetSubRounds () {
		base.BetSubRounds ();
	}
}
