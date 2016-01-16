using System;
using System.Linq;
using System.Collections.Generic;

public class Hand {

	private List<Card> cards;
	private List<int> handValue;
	public Hand()
	{
		cards = new List<Card>();
		handValue = new List<int>();
	}
	public Hand(Hand otherHand)
	{
		cards = new List<Card>(otherHand.cards);
		handValue = new List<int>();
	}
	public Card this[int index]
	{
		get
		{
			return cards[index];
		}
		set
		{
			cards[index] = value;
		}
	}
	public void Clear()
	{
		cards.Clear();
		handValue.Clear();
	}
	public void Add(Card card)
	{
		cards.Add(card);
	}
	public void Remove(int index)
	{
		cards.RemoveAt(index);
	}
	public void Remove(Card card)
	{
		cards.Remove(card);
	}
	public List<int> getValue()
	{
		return this.handValue;
	}
	public void setValue(int value)
	{
		handValue.Add(value);
	}
	public int Count()
	{
		return cards.Count;
	}
	public Card getCard(int index)
	{
		if (index >= cards.Count)
			index = cards.Count - 1; //TODO: throw new ArgumentOutOfRangeException();
		return cards[index];
	}
	List<Card> QuickSortRank(List<Card> myCards)
	{
		Card pivot;
		Random ran = new Random();
		
		if (myCards.Count() <= 1)
			return myCards;
		pivot = myCards[ran.Next(myCards.Count())];
		myCards.Remove(pivot);
		
		var less = new List<Card>();
		var greater = new List<Card>();
		// Assign values to less or greater list
		foreach (Card i in myCards)
		{
			if (i > pivot)
			{
				greater.Add(i);
			}
			else if (i <= pivot)
			{
				less.Add(i);
			}
		}
		// Recurse for less and greaterlists
		var list = new List<Card>();
		list.AddRange(QuickSortRank(greater));
		list.Add(pivot);
		list.AddRange(QuickSortRank(less));
		return list;
	}
	List<Card> QuickSortSuit(List<Card> myCards)
	{
		Card pivot;
		Random ran = new Random();
		
		if (myCards.Count() <= 1)
			return myCards;
		pivot = myCards[ran.Next(myCards.Count())];
		myCards.Remove(pivot);
		
		var less = new List<Card>();
		var greater = new List<Card>();
		// Assign values to less or greater list
		for (int i = 0; i < myCards.Count(); i++)
		{
			if (myCards[i].getSuit() > pivot.getSuit())
			{
				greater.Add(myCards[i]);
			}
			else if (myCards[i].getSuit() <= pivot.getSuit())
			{
				less.Add(myCards[i]);
			}
		}
		// Recurse for less and greaterlists
		var list = new List<Card>();
		list.AddRange(QuickSortSuit(less));
		list.Add(pivot);
		list.AddRange(QuickSortSuit(greater));
		return list;
	}
	public void sortByRank()
	{
		cards = QuickSortRank(cards);
	}
	public void sortBySuit()
	{
		cards = QuickSortSuit(cards);
	}
	public List<Card> getCards()
	{
		return cards;
	}
	public override string ToString()
	{
		if (this.handValue.Count() == 0)
			return "No Poker Hand is Found";
		switch (this.handValue[0])
		{
		case 1:
			return Card.rankToString(handValue[1]) + " High";
		case 2:
			return "Pair of " + Card.rankToString(handValue[1]) + "s";
		case 3:
			return "Two Pair: "+Card.rankToString(handValue[1]) + "s over " + Card.rankToString(handValue[2])+"s";
		case 4:
			return "Three " + Card.rankToString(handValue[1]) + "s";
		case 5:
			return Card.rankToString(handValue[1]) + " High Straight";
		case 6:
			return Card.rankToString(handValue[1]) + " High Flush";
		case 7:
			return Card.rankToString(handValue[1]) + "s Full of " + Card.rankToString(handValue[2]) + "s";
		case 8:
			return "Quad " + Card.rankToString(handValue[1]) + "s";
		case 9:
			return Card.rankToString(handValue[1]) + " High Straight Flush";
		default:
			return "Royal Flush";
		}
	}
	//check is the hands are equal, NOT their value
	public bool isEqual(Hand a)
	{
		for (int i = 0; i < a.Count(); i++)
		{
			if (a[i] != cards[i] || a[i].getSuit() != cards[i].getSuit())
				return false;
		}
		return true;
	}
	//operator overloads for hand comparison, check if the hand values are equal
	public static bool operator ==(Hand a, Hand b)
	{
		if (a.getValue().Count == 0 || b.getValue().Count == 0)
			throw new NullReferenceException();
		for (int i = 0; i < a.getValue().Count(); i++)
		{
			if (a.getValue()[i] != b.getValue()[i])
			{
				return false;
			}
		}
		return true;
	}
	
	public static bool operator !=(Hand a, Hand b)
	{
		if (a.getValue().Count == 0 || b.getValue().Count == 0)
			throw new NullReferenceException();
		for (int i = 0; i < a.getValue().Count(); i++)
		{
			if (a.getValue()[i] != b.getValue()[i])
			{
				return true;
			}
		}
		return false;
	}
	public static bool operator <(Hand a, Hand b)
	{
		if (a.getValue().Count == 0 || b.getValue().Count == 0)
			throw new NullReferenceException();
		for (int i = 0; i < a.getValue().Count(); i++)
		{
			if (a.getValue()[i] < b.getValue()[i])
			{
				return true;
			}
			if (a.getValue()[i] > b.getValue()[i])
			{
				return false;
			}
		}
		return false;
	}
	public static bool operator >(Hand a, Hand b)
	{
		if (a.getValue().Count == 0 || b.getValue().Count == 0)
			throw new NullReferenceException();
		for (int i = 0; i < a.getValue().Count(); i++)
		{
			if (a.getValue()[i] > b.getValue()[i])
			{
				return true;
			}
			if (a.getValue()[i] < b.getValue()[i])
			{
				return false;
			}
			
		}
		return false;
	}
	public static bool operator <=(Hand a, Hand b)
	{
		if (a.getValue().Count == 0 || b.getValue().Count == 0)
			throw new NullReferenceException();
		for (int i = 0; i < a.getValue().Count(); i++)
		{
			if (a.getValue()[i] < b.getValue()[i])
			{
				return true;
			}
			if (a.getValue()[i] > b.getValue()[i])
			{
				return false;
			}
			
		}
		return true;
	}
	public static bool operator >=(Hand a, Hand b)
	{
		if (a.getValue().Count == 0 || b.getValue().Count == 0)
			throw new NullReferenceException();
		for (int i = 0; i < a.getValue().Count(); i++)
		{
			if (a.getValue()[i] > b.getValue()[i])
			{
				return true;
			}
			if (a.getValue()[i] < b.getValue()[i])
			{
				return false;
			}
			
		}
		return true;
	}
	public static Hand operator +(Hand a, Hand b)
	{
		for (int i = 0; i < b.Count(); i++)
		{
			a.Add(b[i]);
		}
		return a;
	}
}
