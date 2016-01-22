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
			var player = game.playerIterator.Next ();
			if (player == null) {
				subRoundCount++; // or LastAction();
				return;
			} else {
				if (player.isReal) {
					game.state.isWaiting = true;
					game.player = player;

					double dt = player.betAlreadyInvestedInCurrentSubRound - game.state.betMaxToStayInGame;
//					if (Settings.isDev) game.ui.lblBet.text = string.Format("c:{0} m:{1}", Settings.betCurrent, game.state.betMaxToStayInGame);
					
					if (dt > 0) {
						game.ui.lblCall.text = Settings.betNull.to_s();
						game.ui.lblRaise.text = dt.to_s ();
					} else if (dt == 0) {
						game.ui.lblCall.text = Settings.betNull.to_s();
						game.ui.lblRaise.text = Settings.betNull.to_s();
					} else if (dt < 0) {
						dt *= -1;
						game.ui.lblCall.text = dt.to_s();
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
						if (betMaxToStayInGame <= 0) {
							betMaxToStayInGame = (double)new Random().Next(1, (int)(betMax * Settings.betCurrentMultiplier + 1));
						}
						player.actionFinal = new Raise(player, betMaxToStayInGame);
					} else {
						player.actionFinal = new Call(player, betMaxToStayInGame);
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
