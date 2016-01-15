using System;

public class AnteRound : BetRound {

	public AnteRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betAnteSubRoundMinSize;
		this.betMin = Settings.betDxMath;
		Settings.betCurrentMultiplier = Settings.betAnteMultiplier;
	}

	public override void FirstAction () {}

	public override void BetSubRounds ()
	{
		//		base.BetSubRounds ();
		if (!game.state.isWaiting) {
			var player = game.player;
			player = this.game.playerIterator.Next ();
			if (player == null) {
				subRoundCount++; // or LastAction();
			} else {
				if (player.isReal) {
					game.state.isWaiting = true;
					game.player = player;

					if (game.isGameRunning) {
						game.ui.panelGame.SetActive (true);
						game.ui.panelInitBet.SetActive (false);
					} else {
						game.ui.panelGame.SetActive (false);
						game.ui.panelInitBet.SetActive (true);
					}
				} else {
					player.actionFinal = player.GetFinalAction (betMin, isCanToRaise);

					if (player.actionTip.isFold) { // always call in ante round
						player.isFolded = false;
						player.actionFinal = new Call(player, betMin);
					}
					player.actionFinal.Do (game);
				}
				game.ui.UpdatePlayer(player);
			}
		}
	}

	public override void LastAction ()
	{
		base.LastAction ();
		game.state = new PreflopRound (game);
	}
	
}
