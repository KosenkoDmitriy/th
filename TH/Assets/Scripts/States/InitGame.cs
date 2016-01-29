using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class InitGame : BetRound {

	public override void LastAction ()
	{
		game.state = new AnteRound (game);
//		game.state = new AllInRound (game, game.players[0], 100); // TODO after test will remove
	}

	public InitGame(Game game2) {
		this.game = game2;

		isWaiting = true;

		game.isGameEnd = true;
		game.isGameRunning = false;

		if (game.ui.payTable != null) game.ui.payTable.SelectColumnByIndex(-1);
		Settings.betBonus = 0;
		Settings.betCurrent = game.betAmount = new Bet(0);

		if (game.ui.inputBetField) game.ui.inputBetField.text = game.betAmount.inCredits.f ();

//		game.ui.ClearAll ();
		game.ui.lblPot.text = Settings.betNull.f();
		game.ui.lblBet.text = Settings.betNull.f();
		game.ui.lblBetBonus.text = Settings.betNull.f();
		game.ui.lblRaise.text = Settings.betNull.f();
		game.ui.lblCall.text = Settings.betNull.f();
		
		game.ui.HideDynamicPanels ();
		game.ui.panelInitBet.SetActive (true);
		
		game.players = game.InitPlayers ();

		// public cards clearing
		if (game.cards != null)
		foreach (var cardPublic in game.cards) {
			cardPublic.isHidden = true;
		}

		game.cards = new List<Card> ();
		
		Card card = null;
		Image image = null;
		
		if (game.source == null)
			game.source = new Constants ();
		game.deck = new Deck ();
		game.deck.Shuffle ();
		
		// chips
		foreach (var player in game.players) {
			player.SetChipRandomly();
			player.lblName.text = player.name;
			player.lblCredits.text = player.balanceInCredits.f();
			player.lblAction.text = "";
		}
		
		foreach (var player in game.players) {
			for (int i = 1; i <= Settings.playerHandSizePreflop; i++) {
				card = game.deck.Deal ();
				var cardImg = GameObject.Find ("player" + player.id + "hold" + i);
				if (cardImg) {
					card.setImage (cardImg.GetComponent<Image> ());
					card.isHidden = true;
//					if (player.id == Settings.playerRealIndex || Settings.isDebug)
//						card.FaceUp = true;
//					else
//						card.FaceUp = false;
				}
				player.hand.Add (card);
			}
			player.handPreflop = player.hand;
			player.handPreflopString = player.GetHandPreflopString();
		}

		// preflop bet rounds
		var preflops = game.source.GetPreflops ();
		SetPatternAndHisAlternativesForPreflop (preflops);
		
		// flop
		for (int i = 1; i <= 3; i++) {
			card = game.deck.Deal ();
			image = GameObject.Find ("flop" + i).GetComponent<Image> ();
			card.setImage (image);
			if (Settings.isDebug)
				card.FaceUp = true;
			game.cards.Add (card);
		}
		// turn
		card = game.deck.Deal ();
		image = GameObject.Find ("turn").GetComponent<Image> ();
		card.setImage (image);
		if (Settings.isDebug)
			card.FaceUp = true;
		game.cards.Add (card);
		
		// river
		card = game.deck.Deal ();
		image = GameObject.Find ("river").GetComponent<Image> ();
		card.setImage (image);
		if (Settings.isDebug)
			card.FaceUp = true;

		game.cards.Add (card);
		
		foreach (var player in game.players) {
			player.hand = player.GetBestPlayerHand (game.cards);
		}

		game.winners = game.GetWinnersAndSetWinPercentage (game.players); // calculating the win percentage/hand strength

		if (Settings.isDev)
		foreach(var player in game.players) {
			if (player.position == 0) {
				player.isFirstToAct = true;
			}
			if (player.position == game.players.Count - 1) {
				player.isLastToAct = true;
			} 
			player.name += string.Format(" {0} {1}", player.winPercent, player.GetHandStringFromHandObj());
			player.lblName.text = player.name;
		}

		game.ui.btnBetBonus.GetComponent<Button> ().interactable = true; // enable "bet bonus" button

		// using in update() of the game loop
		game.playerCollection = new PlayerCollection ();
		foreach (var p in game.players) {
//			p.betAlreadyInvestedInCurrentSubRound = 0;
			game.playerCollection[p.position] = p;
		}
		game.potAmount = 0;
		game.playerIterator = new PlayerIterator(game.playerCollection);
		isWaiting = false;

		if (game.ui.panelInitBet) game.ui.panelInitBet.SetActive(true);
		if (game.ui.btnRepeatBet) game.ui.btnRepeatBet.GetComponent<Button>().interactable = true;
		if (game.ui.btnStartGame) game.ui.btnStartGame.GetComponentInChildren<Text>().text = "START GAME";
		// if (btnStartGame) btnStartGame.GetComponent<Button>().onClick.Invoke();
		// if (lblPanelBet) lblPanelBet.GetComponent<Text>().text = "PLACE YOUR BET";
		if (game.ui.panelInitBet) game.ui.panelInitBet.SetActive(false);

		if (game.players [0].isReal) {
			if (game.ui.panelInitBet) game.ui.panelGame.SetActive(false);
			if (game.ui.panelInitBet) game.ui.panelInitBet.SetActive(true);
		} else {
			if (game.ui.panelInitBet) game.ui.panelGame.SetActive(true);
			if (game.ui.panelInitBet) game.ui.panelInitBet.SetActive(false);
		}
	}
	
}
