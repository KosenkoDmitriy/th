using System;

public class AnteRound : BetRound {

	public AnteRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betAnteSubRoundMinSize;
		Settings.betCurrentMultiplier = Settings.bePreflopFlopMultiplier;
		this.betMaxLimit = new Bet(Settings.betCurrentMultiplier);
	}

	public override void FirstAction () {}

	public override void BetSubRounds ()
	{
		//		base.BetSubRounds ();
		if (!game.state.isWaiting) {
			var player = game.playerIterator.Next ();
			if (player == null) {
				subRoundCount++; // or LastAction();
				return;
			} else {
				if (player.isReal) {
					game.state.isWaiting = true;
					game.player = player;

					Bet dt = player.betInvested - game.state.betMax;

					if (dt > 0) {
						game.ui.lblCall.text = Settings.betNull.f();
						game.ui.lblRaise.text = dt.inCredits.f();
					} else if (dt == 0) {
						game.ui.lblCall.text = Settings.betNull.f();
						game.ui.lblRaise.text = Settings.betNull.f();
					} else if (dt < 0) {
						dt *= -1;
						game.ui.lblCall.text = dt.inCredits.f();
					}

					if (game.isGameRunning) {
						game.ui.panelGame.SetActive (true);
						game.ui.panelInitBet.SetActive (false);
					} else {
						game.ui.panelGame.SetActive (false);
						game.ui.panelInitBet.SetActive (true);
					}
				} else {
					if (player.position == 0 && !player.isReal) {
						game.isGameRunning = true;
						if (betMax <= 0) {
							betMax.inBet = (double)new Random().Next(1, (int)(betMaxLimit.inBet + 1));
						}
						player.actionFinal = new Raise(player, betMax);
					} else {
						player.actionFinal = new Call(player, betMax);
					}
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
	
}
