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

			if (player.position == game.playerIterator.LastActive().position) { // last player
				isCanToRaise = !isCanToRaise;
				if (!isCanToRaise) {
					
					if (IsNextBetRound()) {
						LastAction(); // next bet round if no any raise
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

				if (player.betAlreadyInvestedBeforeAction >= betMax) {
					game.playerIterator = new PlayerIterator(game.playerCollection); // start from first to act player
					betMax = player.betAlreadyInvestedBeforeAction;
					subRoundCurrent++;	// next bet sub round
					// below code already implemented in the FinalAction()
//					if (subRoundCurrent >= subRoundMaxSize) {
//						subRoundCurrent = 0;
//						betMax = 0;
//						game.state = new FlopRound(game);
//					}
				} else {
					player.actionFinal.Do(game);
					if (player.betAlreadyInvestedBeforeAction > betMax) {
						if (isCanToRaise) {
							betMax = player.betAlreadyInvestedBeforeAction;
//							isRaised = true;
						} else {
							//TODO call
						}
					}
				}
			}
			game.ui.UpdatePlayer(player);

		}
//		game.state.isWaiting = true;
//		isWaiting = true;
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
