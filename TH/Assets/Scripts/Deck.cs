﻿using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// standard fair deck of 52 cards
/// </summary>
public class Deck
{
	private List<Card> deck = new List<Card>();
	public Deck()
	{
		for (int i = 2; i <= 14; i++)
		{
			for (int j = 1; j <= 4; j++)
			{
				deck.Add(new Card(i, j, false));
			}
		}
	}
	public Deck(bool faceUp)
	{
		for (int i = 2; i <= 14; i++)
		{
			for (int j = 1; j <= 4; j++)
			{
				deck.Add(new Card(i, j, faceUp));
			}
		}
	}
	public Deck(Deck otherDeck)
	{
		foreach (Card card in otherDeck.deck)
		{
			this.deck.Add(new Card(card));
		}
	}
	public void Add(Card card)
	{
		deck.Add(card);
	}
	//using an online algorithm for shuffling
	public void Shuffle()
	{
		var rand = new Random();
		for (int i = CardsLeft()-1 ; i > 0; i--)
		{
			int n = rand.Next(i + 1);
			Card temp = deck[i];
			deck[i] = deck[n];
			deck[n] = temp;
		}
	}
	public void ShuffleBuggable()
	{
//		int deckPtr = 0;
		var rand = new Random ();
//		for (deckPtr = 0; deckPtr < Settings.cardsSize; deckPtr++)
//		{
//			deck.Add(0xFF);
//		}
//		deckPtr = 0;
		int i;
		for (i = 0; i < deck.Count; i++)
		{
			int a = 0;
			var temp = deck[rand.Next(deck.Count)];
			for (a = 0; a <= i; a++)
			{
				if (temp == deck[a])//dupe?
				{
					i--;
					break;
				}
				else
				{
					if (a == i)
					{
						deck[i] = temp;//we have our card
					}
				}
			}
		}
	}
	public string Print()
	{
		string output = "";
		foreach (Card card in deck)
		{
			output += card.ToString() + " ";
		}
		return output;
	}
	public int CardsLeft()
	{
		return deck.Count;
	}
	public Card Deal()
	{
		Card dealCard = deck.ElementAt(deck.Count - 1);
		deck.RemoveAt(deck.Count - 1);
		dealCard.FaceUp = true;
		return dealCard;
	}
	public Card Deal(bool faceUp)
	{
		Card dealCard = deck.ElementAt(deck.Count - 1);
		dealCard.FaceUp = faceUp;
		deck.RemoveAt(deck.Count - 1);
		return dealCard;
	}
	public void Remove(int index)
	{
		if (index < 0 || index >= deck.Count)
			throw new ArgumentOutOfRangeException();
		deck.RemoveAt(index);
	}
	public void Remove(Card card)
	{
		for(int i=0;i<deck.Count;i++)
		{
			if (deck[i] == card && deck[i].getSuit() == card.getSuit())
			{
				deck.RemoveAt(i);
			}
		}
	}
	public Card[] ToArray()
	{
		return deck.ToArray();
	}
	public List<Card> ToList()
	{
		return deck;
	}
}
