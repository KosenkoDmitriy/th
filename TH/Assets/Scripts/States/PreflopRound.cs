using System;
using UnityEngine.UI;

public class PreflopRound : BetRound {
	public PreflopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;
	}

	public override void LastAction ()
	{
		//		base.LastAction ();
//		game.potAmount = pot;

		//		game.playerIterator.First ();
		while (!game.playerIterator.IsDone) {
			var player = game.playerIterator.Next();
			pot += player.betAlreadyInvestedBeforeAction;
		}
		game.potAmount = pot;
		game.ui.lblPot.GetComponent<Text>().text = game.potAmount.to_s ();
		game.state = new FlopRound (game);
	}

	public override void FirstAction ()
	{
		//		base.FirstAction ();
		isWaiting = true;
		game.ui.StartCoroutine (game.ui.DealCards ());
		// isWaiting = false (must be in the end line of coroutine)
		game.playerIterator = new PlayerIterator (game.playerCollection);
	}

	public override void BetSubRounds () {
		if (!game.state.isWaiting) {
			var player = game.playerIterator.NextActive();
			if (player.position == game.playerIterator.LastActive().position) { // last player
				isCanToRaise = !isCanToRaise;

				if (player.isReal) {
					game.state.isWaiting = true;
					if (isCanToRaise) {
						game.ui.btnRaise.GetComponent<Button>().interactable = true;
						//					game.ui.btnCheck.GetComponent<Button>().interactable = false;
					} else {
						game.ui.btnRaise.GetComponent<Button>().interactable = false;
						//					game.ui.btnCheck.GetComponent<Button>().interactable = true;
					}
				}
			}

			if (player.isReal) {
				game.state.isWaiting = true;
				if (isCanToRaise) {
					game.ui.btnRaise.GetComponent<Button>().interactable = true;
					//					game.ui.btnCheck.GetComponent<Button>().interactable = false;
				} else {
					game.ui.btnRaise.GetComponent<Button>().interactable = false;
					//					game.ui.btnCheck.GetComponent<Button>().interactable = true;
				}
			} else {
				player.actionFinal = player.GetFinalAction(betMax, isCanToRaise);
				player.actionFinal.Do(game);
				if (!isCanToRaise) {
					//TODO don't allow the raise action to any player at all
				}

				if (player.betAlreadyInvestedBeforeAction >= betMax) {
					betMax = player.betAlreadyInvestedBeforeAction;
					subRoundCurrent++;
//					if (subRoundCurrent >= subRoundMaxSize) {
//						subRoundCurrent = 0;
//						betMax = 0;
//						game.state = new FlopRound(game);
//					}
				} else {

				}
			}

			game.ui.UpdatePlayer(player);


		}
//		isWaiting = true;
//		game.ui.StartCoroutine (game.ui.UpdatePlayer (player));
//		subRoundCurrent++;
	}
}
