using System;

public class FlopRound : BetRound {

	public FlopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
		Settings.betCurrentMultiplier = Settings.bePreflopFlopMultiplier;

		// flop bet rounds
		items = game.source.GetFlops ();

		// disable bonus bet feature
		if (game.ui.btnBonusBetSet) game.ui.btnBonusBetSet.GetComponent<UnityEngine.UI.Button>().interactable = false;
		if (game.ui.betBonusDropdown) game.ui.betBonusDropdown.GetComponent<UnityEngine.UI.Dropdown>().interactable = false;

		if (game.ui.btnBetBonus) game.ui.btnBetBonus.GetComponent<UnityEngine.UI.Button> ().interactable = false; // enable "bet bonus" button
		if (game.ui.btnBetBonusRepeat) game.ui.btnBetBonusRepeat.GetComponent<UnityEngine.UI.Button>().interactable = false;
		if (!Settings.btnBetBonusIsDone) game.ui.btnBonusBetRepeatClick(); // automatically repeating a bonus bet each new hand
		// disable bonus bet feature

		UpdatePattern ();
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
		if (Settings.isLog) game.player.Log(false, true, string.Format("Flop BetSubRounds() {0}/{1}", subRoundCount, subRoundMaxSize));
		base.BetSubRounds ();
	}
}
