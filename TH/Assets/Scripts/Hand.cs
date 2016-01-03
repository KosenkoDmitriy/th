using System;
using System.Collections.Generic;
using System.Linq;

public class Hand {

	private List<Card> myHand;
	private List<int> handValue;
	public Hand()
	{
		myHand = new List<Card>();
		handValue = new List<int>();
	}
	public Hand(Hand otherHand)
	{
		myHand = new List<Card>(otherHand.myHand);
		handValue = new List<int>();
	}
	public Card this[int index]
	{
		get
		{
			return myHand[index];
		}
		set
		{
			myHand[index] = value;
		}
	}
	public void Clear()
	{
		myHand.Clear();
		handValue.Clear();
	}
	public void Add(Card card)
	{
		myHand.Add(card);
	}
	public void Remove(int index)
	{
		myHand.RemoveAt(index);
	}
	public void Remove(Card card)
	{
		myHand.Remove(card);
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
		return myHand.Count;
	}
	public Card getCard(int index)
	{
		if (index >= myHand.Count)
			throw new ArgumentOutOfRangeException();
		return myHand[index];
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
		myHand = QuickSortRank(myHand);
	}
	public void sortBySuit()
	{
		myHand = QuickSortSuit(myHand);
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
			if (a[i] != myHand[i] || a[i].getSuit() != myHand[i].getSuit())
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

	private void shuffleDeck()
	{
//		if (Settings.isDebug) DebugLog("shuffleDeck()");
		int i = 0;
		deck = new List<int> ();
		var rand = new Random ();
		for (i = 0; i < Settings.cardsSize; i++) {
			deck.Add(0xFF);
		}

		for (i = 0; i < Settings.cardsSize; i++)
		{
			int a = 0;
			int temp = rand.Next(Settings.cardsSize);
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


	
	const int ROYAL_FLUSH = 21;
	const int STRAIGHT_FLUSH = 20;
	const int HIGH_FOUR_OF_A_KIND = 19;
	const int MID_FOUR_OF_A_KIND = 18;
	const int FOUR_OF_A_KIND = 17;
	const int FULL_HOUSE = 16;
	const int FLUSH = 15;
	const int STRAIGHT = 14;
	const int HIGH_THREE_OF_A_KIND = 13;
	const int MID_THREE_OF_A_KIND = 12;
	const int THREE_OF_A_KIND = 11;
	const int TWO_PAIR = 10;
	const int HIGH_PAIR = 9;
	const int MID_PAIR = 8;
	const int PAIR = 7;
	const int FOUR_TO_A_FLUSH = 6;
	const int THREE_TO_A_FLUSH = 5;
	const int FOUR_TO_A_STRAIGHT_INSIDE = 4;
	const int THREE_TO_A_STRAIGHT_INSIDE = 3;
	const int FOUR_TO_A_STRAIGHT_OUTSIDE = 2;
	const int THREE_TO_A_STRAIGHT_OUTSIDE = 1;
	
	public enum cardValues
	{
		US = 0, // off suit/ unsuit?
		S = 1,  // suit
		_2 = 2,
		_3 = 3,
		_4 = 4,
		_5 = 5,
		_6 = 6,
		_7 = 7,
		_8 = 8,
		_9 = 9,
		T = 10,
		J = 11,
		Q = 12,
		K = 13,
		A = 14,
		G1 = 1,
		G2 = 2,
		G3 = 3,
		G4 = 4,
		G5 = 5,
		G6 = 6,
		G7 = 7,
		G8 = 8,
		ANY = 0xFF
	}

	const int S2 = 0;
	const int S3 = 1;
	const int S4 = 2;
	const int S5 = 3;
	const int S6 = 4;
	const int S7 = 5;
	const int S8 = 6;
	const int S9 = 7;
	const int ST = 8;
	const int SJ = 9;
	const int SQ = 10;
	const int SK = 11;
	const int SA = 12;
	const int D2 = 13;
	const int D3 = 14;
	const int D4 = 15;
	const int D5 = 16;
	const int D6 = 17;
	const int D7 = 18;
	const int D8 = 19;
	const int D9 = 20;
	const int DT = 21;
	const int DJ = 22;
	const int DQ = 23;
	const int DK = 24;
	const int DA = 25;
	const int C2 = 26;
	const int C3 = 27;
	const int C4 = 28;
	const int C5 = 29;
	const int C6 = 30;
	const int C7 = 31;
	const int C8 = 32;
	const int C9 = 33;
	const int CT = 34;
	const int CJ = 35;
	const int CQ = 36;
	const int CK = 37;
	const int CA = 38;
	const int H2 = 39;
	const int H3 = 40;
	const int H4 = 41;
	const int H5 = 42;
	const int H6 = 43;
	const int H7 = 44;
	const int H8 = 45;
	const int H9 = 46;
	const int HT = 47;
	const int HJ = 48;
	const int HQ = 49;
	const int HK = 50;
	const int HA = 51;

//	List<Image> cards = new List<Image>(); // Image[52];
	List<int> deck = new List<int>();// int[52];
	int[] communityCards = new int[5];
}
