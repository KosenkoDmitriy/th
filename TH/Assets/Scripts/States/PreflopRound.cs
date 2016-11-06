using System;

public class PreflopRound : BetRound {
	public PreflopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.bePreflopFlopMultiplier;

		// disable bonus bet feature
		if (game.ui.btnBonusBetSet) game.ui.btnBonusBetSet.GetComponent<UnityEngine.UI.Button>().interactable = false;
		if (game.ui.betBonusDropdown) game.ui.betBonusDropdown.GetComponent<UnityEngine.UI.Dropdown>().interactable = false;

		if (game.ui.btnBetBonus) game.ui.btnBetBonus.GetComponent<UnityEngine.UI.Button> ().interactable = false; // enable "bet bonus" button
		if (game.ui.btnBetBonusRepeat) game.ui.btnBetBonusRepeat.GetComponent<UnityEngine.UI.Button>().interactable = false;
		if (!Settings.btnBetBonusIsDone) game.ui.btnBonusBetRepeatClick(); // automatically repeating a bonus bet each new hand

		UpdatePattern (); // preflop bet rounds
	}

	public override void UpdatePattern() {
		var items = game.source.GetPreflops ();
		SetPatternAndHisAlternativesForPreflop (items);
	}

	public override void FirstAction ()
	{
//		game.ui.DealPreflopCards (); // without waiting
//		base.FirstAction (); or game.playerIterator = new PlayerIterator (game.playerCollection); // reset playerIterator after DealCards();

		game.state.isWaiting = true;
		game.ui.StartCoroutine (game.ui.DealCards ());	// game.state.isWaiting = false (must be in the end line of coroutine)
	}

	public override void LastAction ()
	{
		base.LastAction ();
		game.state = new FlopRound (game);
	}

	public override void BetSubRounds () {
		if (Settings.isDev) game.player.Log(false, true, string.Format("Preflop BetSubRounds() {0}/{1}", subRoundCount, subRoundMaxSize));
		base.BetSubRounds ();
	}
}
