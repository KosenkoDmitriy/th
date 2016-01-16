using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class InitGame : BetRound {

	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new AnteRound (game);
	}

	public InitGame(Game game2) {
		this.game = game2;

		isWaiting = true;

		game.isGameEnd = true;
		game.isGameRunning = false;

		if (game.ui.payTable != null) game.ui.payTable.SelectColumnByIndex(-1);
		Settings.betBonus = 0;
		game.betAmount = 0;

		game.ui.ClearAll ();
		
		game.ui.HideDynamicPanels ();
		game.ui.panelInitBet.SetActive (true);
		
		game.players = game.InitPlayers ();
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
			player.lblCredits.text = player.betTotal.to_s();
			player.lblAction.text = "";
		}
		
		foreach (var player in game.players) {
			for (int i = 1; i <= Settings.playerHandSizePreflop; i++) {
				card = game.deck.Deal ();
				var cardImg = GameObject.Find ("player" + player.id + "hold" + i);
				if (cardImg) {
					card.setImage (cardImg.GetComponent<Image> ());
					//						if (player.id == Settings.playerRealIndex || Settings.isDebug)
					//							card.FaceUp = true;
					//						else
					//							card.FaceUp = false;
				}
				player.hand.getCards().Add (card);
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

		game.winners = game.GetWinnersAndSetWinPercentage (); // calculating the win percentage/hand strength

		if (Settings.isDev)
		foreach(var player in game.players) {
			player.name += string.Format(" {0} {1}", player.winPercent, player.GetHandStringFromHandObj());
			player.lblName.text = player.name;
		}

		game.ui.btnBetBonus.GetComponent<Button> ().interactable = true; // enable "bet bonus" button

		// using in update() of the game loop
		game.playerCollection = new PlayerCollection ();
		foreach (var p in game.players) {
			game.playerCollection[p.position] = p;
		}
		
		game.playerIterator = new PlayerIterator(game.playerCollection);

		isWaiting = false;
	}
	
}
