﻿using System;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

public enum RANK
{
	TWO=2,
	THREE,
	FOUR,
	FIVE,
	SIX,
	SEVEN,
	EIGHT,
	NINE,
	TEN,
	JACK,
	QUEEN,
	KING,
	ACE
}

public enum SUIT
{
	DIAMONDS = 1,
	CLUBS,
	HEARTS,
	SPADES
}
/// <summary>
/// THIS IS THE CLASS THAT STARTED EVERYTHING AND MADE THIS GAME POSSIBLE
/// </summary>
public class Card
{
	private int rank, suit;
	private Image Image;
	private Sprite sprite;
	private bool faceUp;
	private bool is_hidden;
	private bool highlight;

	public bool FaceUp {
		get { return faceUp; }
		set { 
			faceUp = value;
			getImageFromFile ();
		}
	}
	public bool isHidden {
		get { return is_hidden; }
		set { 
			is_hidden = value;
			getHiddenImageFromFile ();
		}
	}
	//default two of diamonds
	public Card ()
	{
		rank = (int)RANK.TWO;
		suit = (int)SUIT.DIAMONDS;
		faceUp = false;
		highlight = false;
	}

	public Card (RANK rank, SUIT suit)
	{
		this.rank = (int)rank;
		this.suit = (int)suit;
		faceUp = false;
		highlight = false;
	}

	public Card (int rank, int suit)
	{
		if (rank < 1 || rank > 14 || suit < 1 || suit > 4)
			throw new ArgumentOutOfRangeException ();
		this.rank = rank;
		this.suit = suit;
		faceUp = false;
		highlight = false;
	}

	public Card (RANK rank, SUIT suit, bool faceUp)
	{
		this.rank = (int)rank;
		this.suit = (int)suit;
		this.faceUp = faceUp;
		highlight = false;
	}

	public Card (int rank, int suit, bool faceUp)
	{
		if (rank < 1 || rank > 14 || suit < 1 || suit > 4)
			throw new ArgumentOutOfRangeException ();
		this.rank = rank;
		this.suit = suit;
		this.faceUp = faceUp;
		highlight = false;
	}

	public Card (Card card)
	{
		this.rank = card.rank;
		this.suit = card.suit;
		this.faceUp = card.faceUp;
		highlight = false;
	}

	public static string rankToString (int rank)
	{
		switch (rank) {
		case 11:
			return "Jack";
		case 12:
			return "Queen";
		case 13:
			return "King";
		case 14:
			return "Ace";
		default:
			return rank.ToString ();
		}
	}

	public static string suitToString (int suit)
	{
		switch (suit) {
		case 1:
			return "Diamonds";
		case 2:
			return "Clubs";
		case 3:
			return "Hearts";
		default:
			return "Spades";
		}
	}

	public static string rankToResString (int rank)
	{
		switch (rank) {
		case 11:
			return "J";
		case 12:
			return "Q";
		case 13:
			return "K";
		case 14:
			return "A";
		default:
			return rank.ToString ();
		}
	}

	public static string rankToMathString (int rank)
	{
		switch (rank) {
		case 10:
			return "T";
		case 11:
			return "J";
		case 12:
			return "Q";
		case 13:
			return "K";
		case 14:
			return "A";
		default:
			return rank.ToString ();
		}
	}
	
	public static string suitToResString (int suit)
	{
		switch (suit) {
		case 1:
			return "dia";
		case 2:
			return "clubs";
		case 3:
			return "hearts";
		default:
			return "spades";
		}
	}

	public int getRank ()
	{
		return rank;
	}

	public int getSuit ()
	{
		return suit;
	}
	//get image from depending on if the card is faceup or down
	private void getImageFromFile ()
	{
		if (faceUp) {
			string path = Settings.cardsPrefix + rankToResString (rank) + "_" + suitToResString (suit);
			this.sprite = Resources.Load<Sprite> (path);
			if (this.Image != null)
				this.Image.sprite = sprite;
		} else {
			this.sprite = Resources.Load<Sprite> (Settings.cardBackName);
			if (this.Image != null)
				this.Image.sprite = sprite;
		}
	}
	private void getHiddenImageFromFile ()
	{
		if (isHidden) {
			this.sprite = Resources.Load<Sprite> (Settings.cardBg);
			if (this.Image != null)
				this.Image.sprite = sprite;
		}
	}
	//get the current image
	public Image getImage ()
	{
		if (Image == null)
			getImageFromFile ();
		return this.Image;
	}

	public void setImage (Image img)
	{
		this.Image = img;
	}

	public Sprite getSprite ()
	{
		if (sprite == null)
			getImageFromFile ();
		return this.sprite;
	}

	public void setRank (RANK rank)
	{
		this.rank = (int)rank;
	}

	public void setCard (RANK rank, SUIT suit)
	{
		this.rank = (int)rank;
		this.suit = (int)suit;
	}

	public void setCard (int rank, int suit)
	{
		if (rank < 1 || rank > 14 || suit < 1 || suit > 4)
			throw new ArgumentOutOfRangeException ();
		this.rank = rank;
		this.suit = suit;
	}

	public override string ToString ()
	{
		if (faceUp == true)
			return rankToString (rank) + " of " + suitToString (suit);
		return "The card is facedown, you cannot see it!";
	}
		
	//extract green channel from image to highlight image
	public void Highlight ()
	{
		if (faceUp == false)
			return;
			
		this.highlight = true;
			
		if (this.Image == null)
			getImageFromFile ();
//		Bitmap HighlightedBitmap = new Bitmap (Image.Width, Image.Height);
//		for (int i = 0; i < Image.Width; i++) {
//			for (int j = 0; j < Image.Height; j++) {
//				int green = Image.GetPixel (i, j).G;
//				HighlightedBitmap.SetPixel (i, j, Color.FromArgb (255, 0, green, 0));
//			}
//		}
//		Image = new Bitmap (HighlightedBitmap);
	}
	//reload original image to unhighlight
	public void UnHighlight ()
	{
		if (faceUp == false)
			return;
		this.highlight = false;
		getImageFromFile ();
	}

	public bool isHighlighted ()
	{
		return this.highlight;
	}
		
	//compare rank of cards
	public static bool operator == (Card a, Card b)
	{
		if (a.rank == b.rank)
			return true;
		else
			return false;
	}

	public static bool operator != (Card a, Card b)
	{
		if (a.rank != b.rank)
			return true;
		else
			return false;
	}

	public static bool operator < (Card a, Card b)
	{
		if (a.rank < b.rank)
			return true;
		else
			return false;
	}

	public static bool operator > (Card a, Card b)
	{
		if (a.rank > b.rank)
			return true;
		else
			return false;
	}

	public static bool operator <= (Card a, Card b)
	{
		if (a.rank <= b.rank)
			return true;
		else
			return false;
	}

	public static bool operator >= (Card a, Card b)
	{
		if (a.rank >= b.rank)
			return true;
		else
			return false;
	}
}
