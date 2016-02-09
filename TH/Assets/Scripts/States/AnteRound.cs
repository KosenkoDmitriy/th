using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class AnteRound : BetRound {

	public AnteRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betAnteSubRoundMinSize;
		Settings.betCurrentMultiplier = Settings.bePreflopFlopMultiplier;
	}

	public override void FirstAction () {
		base.FirstAction ();

		game.ui.btnFold.GetComponent<Button>().interactable = false;
		game.ui.btnAllIn.GetComponent<Button>().interactable = false;
	}
	
	public override void BetSubRounds ()
	{
		if (Settings.isDev) game.player.Log(false, true, string.Format("AnteRound BetSubRounds() {0}/{1}", subRoundCount, subRoundMaxSize));

		//		base.BetSubRounds ();

		if (!game.state.isWaiting) {
			var player = game.playerIterator.Next ();
			if (player == null) {
				subRoundCount++; // or LastAction();
				return;
			} else {
				if (player.isReal) {
					game.ui.DisableButtons(false);

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

					if (player.balanceInCredits <= 0) {
						game.ui.btnCall.GetComponent<Button>().interactable = false;
						game.ui.btnFold.GetComponent<Button>().interactable = false;
						game.ui.btnAllIn.GetComponent<Button>().interactable = false;
					} else {
						game.ui.btnCall.GetComponent<Button>().interactable = true;
						game.ui.btnFold.GetComponent<Button>().interactable = true;
						game.ui.btnAllIn.GetComponent<Button>().interactable = true;
					}
				} else {
					game.ui.DisableButtons(true);

					if (player.position == 0 && !player.isReal) {
						game.isGameRunning = true;

//						betMax.inBetMath = (double)new Random().Next(1, (int)(betMaxLimit.inBetMath + 1));
						List<Bet> betList = new List<Bet>();
						for(int i = 0; i < betMaxLimit.inBetMath; i++) {
							var bet = new Bet(0);
							bet.inBetMath = i;
							if (player.balanceInCredits - bet.inCredits >= 0) {
								betList.Add(bet);
								break;
							}
						}
						System.Random rand = new System.Random();
						betMax.inBetMath = (double)rand.Next(1, betList.Count + 1);

						if (player.balanceInCredits - betMax.inCredits >= 0) {
							player.actionFinal = new Raise(player, new Bet(0), betMax);
						}
					} else {
						if (player.balanceInCredits - betMax.inCredits >= 0) {
							player.actionFinal = new Call(player, betMax, new Bet(0));
						}
					}

					if (player.balanceInCredits <= 0) {
						player.actionFinal = new Check(player, new Bet(0), new Bet(0));
					}
					player.actionFinal.Do (game, player);

				}
			}
		}
	}

	public override void LastAction ()
	{
		base.LastAction ();

		game.ui.btnFold.GetComponent<Button>().interactable = true;
		game.ui.btnAllIn.GetComponent<Button>().interactable = true;

		game.state = new PreflopRound (game);
	}
	
}
