using System;
using UnityEngine.UI;

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

					if (game.isGameRunning) {
						game.ui.panelGame.SetActive (true);
						game.ui.btnRaise.GetComponent<Button>().interactable = false;
						game.ui.panelInitBet.SetActive (false);
					} else {
						game.ui.btnRaise.GetComponent<Button>().interactable = true;
						game.ui.panelGame.SetActive (false);
						game.ui.panelInitBet.SetActive (true);
					}

					Bet dt = player.betInvested - game.state.betMax;

					if (dt > 0) {
						game.ui.lblCall.text = Settings.betNull.f();
						game.ui.lblRaise.text = dt.inCredits.f();
						
						game.ui.btnCall.GetComponent<Button>().interactable = true;
						game.ui.btnCheck.GetComponent<Button>().interactable = false;
					} else if (dt == 0) {
						game.ui.lblCall.text = Settings.betNull.f();
						game.ui.lblRaise.text = Settings.betNull.f();
						
						game.ui.btnCall.GetComponent<Button>().interactable = false;
						game.ui.btnCheck.GetComponent<Button>().interactable = true;
					} else if (dt < 0) {
						dt *= -1;
						game.ui.lblCall.text = dt.inCredits.f();
						
						game.ui.btnCall.GetComponent<Button>().interactable = true;
						game.ui.btnCheck.GetComponent<Button>().interactable = false;
					}

				} else {
					if (player.position == 0 && !player.isReal) {
						game.isGameRunning = true;
						if (betMax <= 0) {
							betMax.inCredits = (double)new Random().Next(1, (int)(betMaxLimit.inCredits + 1));
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
