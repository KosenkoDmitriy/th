using System;

public class AnteRound : BetRound {

	public AnteRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betAnteSubRoundMinSize;
		this.betMin = Settings.betDxMath;
		Settings.betCurrentMultiplier = Settings.betAnteMultiplier;
	}

	public override void FirstAction ()
	{
//		base.FirstAction ();
		this.game.playerIterator = new PlayerIterator(this.game.playerCollection);
	}

	public override void BetSubRounds ()
	{
		//		base.BetSubRounds ();
		if (!game.state.isWaiting) {
			var player = game.player;
			player = this.game.playerIterator.Next ();
			if (player == null) {
				subRoundCount++; // or LastAction();
			} else {
				player.actionFinal = player.GetFinalAction (betMin, isCanToRaise);
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
					if (player.actionTip.isFold) { // always call in ante round
						player.actionFinal = new Call(player, betMin);
					}
					player.actionFinal.Do (game);
				}
				game.ui.UpdatePlayer(player);
			}
		}

//		game.state.isWaiting = true;
//		game.ui.StartCoroutine (game.ui.UpdatePlayers());
	}

	public override void LastAction ()
	{
		base.LastAction ();
		game.state = new PreflopRound (game);
	}
	
}
