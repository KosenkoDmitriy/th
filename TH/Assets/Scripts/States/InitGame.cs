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
		
		game.isGameRunning = true;
		game.isGameEnd = false;
		
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
			//					else
			//						card.FaceUp = false;
			game.cards.Add (card);
		}
		// turn
		card = game.deck.Deal ();
		image = GameObject.Find ("turn").GetComponent<Image> ();
		card.setImage (image);
		if (Settings.isDebug)
			card.FaceUp = true;
		//				else
		//					card.FaceUp = false;
		game.cards.Add (card);
		
		// river
		card = game.deck.Deal ();
		image = GameObject.Find ("river").GetComponent<Image> ();
		card.setImage (image);
		if (Settings.isDebug)
			card.FaceUp = true;
		//				else
		//					card.FaceUp = false;
		game.cards.Add (card);
		
		foreach (var player in game.players) {
			player.hand = player.GetBestPlayerHand (game.cards);
		}

		// calculating the win percentage/hand strength
		foreach (var player in game.players) {
			if (HandCombination.isRoyalFlush(player.hand)) {
				player.winPercent = 100;
			} else if (HandCombination.isStraightFlush(player.hand)) {
				player.winPercent = 90;
			} else if (HandCombination.isFourOfAKind(player.hand)) {
				player.winPercent = 80;
			} else if (HandCombination.isFullHouse(player.hand)) {
				player.winPercent = 70;
			} else if (HandCombination.isFlush(player.hand)) {
				player.winPercent = 60;
			} else if (HandCombination.isStraight(player.hand)) {
				player.winPercent = 50;
			} else if (HandCombination.isThreeOfAKind(player.hand)) {
				player.winPercent = 40;
			} else if (HandCombination.isTwoPair(player.hand)) {
				player.winPercent = 30;
			} else if (HandCombination.isOnePair(player.hand)) {
				player.winPercent = 20;
			} else if (HandCombination.isHighCard(player.hand)) {
				player.winPercent = 10;
			} else {
				player.winPercent = 0;
			}
		}
		// TODO: split the pot between win players

		
		// using in update() of the game loop
		game.playerCollection = new PlayerCollection ();
		foreach (var p in game.players) {
			game.playerCollection[p.position] = p;
		}
		
		game.playerIterator = new PlayerIterator(game.playerCollection);
	}
	
}
