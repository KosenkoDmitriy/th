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
		game.playerIterator = new PlayerIterator (game.playerCollection);
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

			if (player.isReal) {
				game.state.isWaiting = true;
				if (isCanToRaise) {
					game.ui.btnRaise.GetComponent<Button>().interactable = true;
				} else {
					game.ui.btnRaise.GetComponent<Button>().interactable = false;
				}
			} else {
				player.actionFinal = player.GetFinalAction(betMax, isCanToRaise);
				player.actionTip.Do(game);
			}

			if (player.position == game.playerIterator.LastActive().position) { // last player
				isCanToRaise = false;
				if (!isCanToRaise) {
					if (subRoundCount < subRoundMaxSize) {
						subRoundCount++;
					} else if (subRoundCount == subRoundMaxSize) {	// last subround
						if (IsNextBetRound()) {						// no any raise
							subRoundCount++; // LastAction();		// next bet round
						} else {			
							isCanToRaise = false; // repeat last subround with disabled raise action
						}
					}
					if (IsNextBetRound()) {
						LastAction(); // next bet round if no any raise in any subround
					}
				}
			}
			game.ui.UpdatePlayer(player);
		}
//		game.state.isWaiting = true;
//		game.ui.StartCoroutine (game.ui.UpdatePlayer (player));
	}

	private bool IsNextBetRound() {
		var iterator = new PlayerIterator (game.playerCollection);
		bool isNextBetRound = false;
		while (!iterator.IsDone) {
			var player = iterator.NextActive();
			if (player.betAlreadyInvestedBeforeAction != betMax) {
				isNextBetRound = false;
			} else {
				isNextBetRound = true;
				break;
			}
		}
		return isNextBetRound;
	}
}
