using System;
using System.Collections;

public class TestData {
	Game game;

	public TestData(Game game) {
		this.game = game;
	}

	private void DisplayCards() {
		if (Settings.isDebug) {
			foreach(var item in game.cards) {
				item.FaceUp = true;
			}
			
			foreach(var player in game.players) {
				foreach (var item in player.handPreflop.getCards()) {
					item.FaceUp = true;
				}
			}
		}
	}

	public void StraightSouldntFold() {
		game.cards[0].setCard(RANK.FOUR, SUIT.CLUBS);
		game.cards[1].setCard(RANK.SEVEN, SUIT.CLUBS);
		game.cards[2].setCard(RANK.QUEEN, SUIT.CLUBS);
		game.cards[3].setCard(RANK.JACK, SUIT.CLUBS);
		game.cards[4].setCard(RANK.ACE, SUIT.HEARTS);
		
		game.players[0].handPreflop.getCard(0).setCard(RANK.SIX, SUIT.CLUBS);
		game.players[0].handPreflop.getCard(1).setCard(RANK.TWO, SUIT.HEARTS);
		
		game.players[1].handPreflop.getCard(0).setCard(RANK.ACE, SUIT.CLUBS);
		game.players[1].handPreflop.getCard(1).setCard(RANK.THREE, SUIT.HEARTS);
		
		game.players[2].handPreflop.getCard(0).setCard(RANK.SIX, SUIT.SPADES);
		game.players[2].handPreflop.getCard(1).setCard(RANK.FIVE, SUIT.DIAMONDS);
		
		game.players[3].handPreflop.getCard(0).setCard(RANK.THREE, SUIT.CLUBS);
		game.players[3].handPreflop.getCard(1).setCard(RANK.NINE, SUIT.HEARTS);
		
		game.players[4].handPreflop.getCard(0).setCard(RANK.QUEEN, SUIT.DIAMONDS);
		game.players[4].handPreflop.getCard(1).setCard(RANK.THREE, SUIT.SPADES);
		
		game.players[5].handPreflop.getCard(0).setCard(RANK.TEN, SUIT.CLUBS);
		game.players[5].handPreflop.getCard(1).setCard(RANK.TEN, SUIT.DIAMONDS);
		
		DisplayCards();
	}

	public void StraightShouldNotFold() {
		game.cards[0].setCard(RANK.JACK, SUIT.DIAMONDS);
		game.cards[1].setCard(RANK.QUEEN, SUIT.DIAMONDS);
		game.cards[2].setCard(RANK.TEN, SUIT.CLUBS);
		game.cards[3].setCard(RANK.THREE, SUIT.DIAMONDS);
		game.cards[4].setCard(RANK.THREE, SUIT.CLUBS);
		
		game.players[0].handPreflop.getCard(0).setCard(RANK.KING, SUIT.CLUBS);
		game.players[0].handPreflop.getCard(1).setCard(RANK.SEVEN, SUIT.DIAMONDS);
		
		game.players[1].handPreflop.getCard(0).setCard(RANK.EIGHT, SUIT.DIAMONDS);
		game.players[1].handPreflop.getCard(1).setCard(RANK.FIVE, SUIT.CLUBS);

		// full house
		game.players[2].handPreflop.getCard(0).setCard(RANK.JACK, SUIT.CLUBS);
		game.players[2].handPreflop.getCard(1).setCard(RANK.JACK, SUIT.SPADES);

		// four of a kind
		game.players[3].handPreflop.getCard(0).setCard(RANK.THREE, SUIT.HEARTS);
		game.players[3].handPreflop.getCard(1).setCard(RANK.THREE, SUIT.SPADES);
		
		game.players[4].handPreflop.getCard(0).setCard(RANK.QUEEN, SUIT.DIAMONDS);
		game.players[4].handPreflop.getCard(1).setCard(RANK.FOUR, SUIT.SPADES);

		// Straight A,K,Q,J,10
		game.players[5].handPreflop.getCard(0).setCard(RANK.ACE, SUIT.CLUBS);
		game.players[5].handPreflop.getCard(1).setCard(RANK.KING, SUIT.SPADES);
		
		DisplayCards();
	}

	public void ThreeOfAKindShouldNotFold() {
		game.cards[0].setCard(RANK.FIVE, SUIT.SPADES);
		game.cards[1].setCard(RANK.FIVE, SUIT.DIAMONDS);
		game.cards[2].setCard(RANK.TEN, SUIT.HEARTS);
		game.cards[3].setCard(RANK.THREE, SUIT.CLUBS);
		game.cards[4].setCard(RANK.TWO, SUIT.HEARTS);
		
		game.players[0].handPreflop.getCard(0).setCard(RANK.NINE, SUIT.HEARTS);
		game.players[0].handPreflop.getCard(1).setCard(RANK.TEN, SUIT.CLUBS);
		
		game.players[1].handPreflop.getCard(0).setCard(RANK.EIGHT, SUIT.CLUBS);
		game.players[1].handPreflop.getCard(1).setCard(RANK.NINE, SUIT.SPADES);

		// three of a kind (5)
		game.players[2].handPreflop.getCard(0).setCard(RANK.FIVE, SUIT.HEARTS);
		game.players[2].handPreflop.getCard(1).setCard(RANK.NINE, SUIT.CLUBS);
		
		game.players[3].handPreflop.getCard(0).setCard(RANK.THREE, SUIT.CLUBS);
		game.players[3].handPreflop.getCard(1).setCard(RANK.KING, SUIT.SPADES);

		// full house
		game.players[4].handPreflop.getCard(0).setCard(RANK.FIVE, SUIT.DIAMONDS);
		game.players[4].handPreflop.getCard(1).setCard(RANK.TEN, SUIT.SPADES);

		// three of a kind (5)
		game.players[5].handPreflop.getCard(0).setCard(RANK.FIVE, SUIT.CLUBS);
		game.players[5].handPreflop.getCard(1).setCard(RANK.SIX, SUIT.DIAMONDS);
		
		DisplayCards();
	}

}
