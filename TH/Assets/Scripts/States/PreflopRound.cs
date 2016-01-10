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
		game.state = new FlopRound (game);
	}

	public override void FirstAction ()
	{
		//		base.FirstAction ();
		isWaiting = true;
		game.ui.StartCoroutine (game.ui.DealCards ());
		// isWaiting = false (must be in the end line of coroutine)
	}
		
	public override void BetSubRounds ()
	{
		//		base.BetSubRounds ();

		game.ui.DebugLog ("Preflop()");
		
		
		// the same for all preflop bet rounds
		foreach (var player in game.players) {
			
			foreach (var pcard in player.handPreflop.getCards()) {
				if (player.isReal || player.isFolded || Settings.isDebug) {
					pcard.FaceUp = true;
				} else {
					pcard.FaceUp = false;
				}
			}
			
			if (!player.isFolded) // active virtual players only
			{
				player.patternCurrent = player.GetAndSetPatternRandomly ();
				player.actionCurrentString = player.GetCurrentActionString (betToStayInGame, player.bet);	// betTotalInThisRound);
				//TODO: handle player's current action
				
				if (player.actionCurrentString == "FOLD") {
					player.lblAction.text = player.actionCurrentString;
					player.lblCredits.text = player.betTotal.to_s();
					player.isFolded = true;
					foreach (var pcard in player.handPreflop.getCards()) {
						pcard.FaceUp = true;
					}
				} else if (player.actionCurrentString == "CHECK") {
					Update (game, player);
					
				} else if (player.actionCurrentString == "CALL") {
					Update (game, player);
					
				} else if (player.actionCurrentString == "RAISE") {
					int multiplier = player.patternCurrent.betMaxCallOrRaise; //TODO
					player.betTotal -= game.betAmount * multiplier;
					betToStayInGame += game.betAmount * multiplier;
					betToStayInGame += betToStayInGame;
					Update (game, player);
				}
			}
		}
		
		//TODO: tips for real player as enable/disable buttons
		int index = 0;
		var playerReal = game.players[index]; //real player
		if (playerReal.betTotal <= 0 || game.betAmount <= 0) {
			game.ui.btnCheck.GetComponent<Button> ().interactable = true;
			game.ui.btnCall.GetComponent<Button> ().interactable = false;
		} else {
			game.ui.btnCheck.GetComponent<Button> ().interactable = false;
			game.ui.btnCall.GetComponent<Button> ().interactable = true;
			playerReal.lblCredits.text = playerReal.betTotal.ToString ();
			game.ui.lblPot.GetComponent<Text> ().text = game.potAmount.ToString ();
		}
	}
	
	private void Update(Game game, Player player) {
		int multiplier = 1;
		//			int multiplier = player.patternCurrent.betMaxCallOrRaise; //TODO
		player.betTotal -= game.betAmount * multiplier;
		betToStayInGame += game.betAmount * multiplier;
		pot += game.betAmount * multiplier;
		game.potAmount += game.betAmount * multiplier;
		
		// TODO: will refactor (credit label)
		player.lblCredits.text = player.betTotal.to_s();
		player.lblAction.text = player.actionCurrentString;
		
		game.ui.lblPot.GetComponent<Text> ().text = game.potAmount.to_s();
		game.ui.lblBet.GetComponent<Text> ().text = game.betAmount.to_s();
		game.ui.lblRaise.GetComponent<Text> ().text = betToStayInGame.to_s(); // TODO:
		game.ui.lblCall.GetComponent<Text> ().text = betToStayInGame.to_s();
	}
	
}