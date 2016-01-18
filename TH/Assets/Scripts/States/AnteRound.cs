using System;

public class AnteRound : BetRound {

	public AnteRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betAnteSubRoundMinSize;
		Settings.betCurrentMultiplier = Settings.betAnteMultiplier;
	}

	public override void FirstAction () {}

	public override void BetSubRounds ()
	{
		//		base.BetSubRounds ();
		if (!game.state.isWaiting) {
			player = game.playerIterator.Next ();
			if (player == null) {
				subRoundCount++; // or LastAction();
				return;
			} else {
				if (player.isReal) {
					game.state.isWaiting = true;

					if (game.isGameRunning) {
						game.ui.panelGame.SetActive (true);
						game.ui.panelInitBet.SetActive (false);
					} else {
						game.ui.panelGame.SetActive (false);
						game.ui.panelInitBet.SetActive (true);
					}
				} else {
					player.actionFinal = new Call(player, betMax);
					player.actionFinal.Do (game, player);
				}
			}
		}
	}

	public override void LastAction ()
	{
		base.LastAction ();
		game.state = new PreflopRound (game);
	}

	
	Player player;
}
