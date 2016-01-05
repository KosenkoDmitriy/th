﻿using System;
using System.Linq;
using System.Collections.Generic;

public class Player {

	public Player() {
		hand = new Hand ();
		alt_patterns = new List<Pattern> ();
	}

	public double credits;
	public int no;
	public string name;

	public string handPreflopString;	// "AKs"
	public string handPreflopStringReversed;	// "KAs"

	public Hand handPreflop;
	public Hand hand;
	public List<Hand> hands;

//	bool isActive = true;
	public bool isFolded = false;


	public string actionCurrent;
	public Pattern patternCurrent;

	public Pattern pattern;
	public List<Pattern> alt_patterns;

//	public List<PreFlop> preflopBets;
//	public List<Flop> flopBets;
//	public List<Turn> turnBets;
//	public List<River> riverBets;
	
	public void dealCards() {

	}

	public Pattern GetAndSetPatternRandomly() {
		float percentOfTime = UnityEngine.Random.value * 100;
		if (pattern != null) {
			if (percentOfTime <= pattern.percent) {
				patternCurrent = pattern;
			}
		}
		if (alt_patterns.Count > 0) {
			foreach(var item in alt_patterns) {
				if (percentOfTime <= item.percent) {
					patternCurrent = item;
				}
			}
		}
		return patternCurrent;
	}

	public string GetCurrentAction(double betToStayInGame, double betTotal) {
		string action = "";
		if (patternCurrent != null) {
			action = patternCurrent.actionDefault;
//			action = patternCurrent.actionPreffered1;
//			åaction = patternCurrent.actionPreffered2;
			if (patternCurrent.betRounds != null && patternCurrent.betRounds.Count > 0)
				foreach (var betRound in patternCurrent.betRounds) {
					if (betRound.costBet * Settings.betDx == betToStayInGame && betRound.costBetTotal * Settings.betDx == betTotal) {
						action = betRound.name_action;
						break;
					}
				}
		}
		return action;
	}

	public string GetHandPreflopString() {
		handPreflopString = "";
		handPreflopStringReversed = "";
		bool isSuited = false;
		if (hand.getCards().Count >= 2) {
			if (hand.getCards()[0].getSuit() == hand.getCards()[1].getSuit()) {
				isSuited = true;
			}
			handPreflopString += Card.rankToResString(hand.getCards()[0].getRank());
			handPreflopString += Card.rankToResString(hand.getCards()[1].getRank());
			handPreflopStringReversed += Card.rankToResString(hand.getCards()[1].getRank());
			handPreflopStringReversed += Card.rankToResString(hand.getCards()[0].getRank());
			if (hand.getCards()[0].getRank() == hand.getCards()[1].getRank()) {
			}
			else if (isSuited) {
				handPreflopString += "s";
				handPreflopStringReversed += "s";
			} else {
				handPreflopString += "o";
				handPreflopStringReversed += "o";
			}
		}
		return handPreflopString;
	}

	public Hand GetBestPlayerHand (List<Card> cards)
	{
		var count = 20;
		Hand hand = null;
		this.hands = new List<Hand> ();
		
		for (int x = 0; x <= count; x++) {//iterate through all possible 5 card hands
			hand = new Hand();
			switch (x) {
			case 0:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
			}
				break;
			case 1:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
			}
				break;
			case 2:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			}
				break;
			case 3:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			}
				break;
			case 4:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			}
				break;
			case 5:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			}
				break;
			case 6:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [4]);
			}
				break;
			case 7:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			}
				break;
			case 8:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			}
				break;
			case 9:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			}
				break;
			case 10:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			}
				break;
			case 11:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 12:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 13:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 14:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 15:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 16:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 17:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 18:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 19:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 20:
			{
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			}
			hand = HandCombination.getBestHand(hand); 
			this.hands.Add (hand);
		}
		
		Hand bestHand = this.hands.First();
		foreach (var item in this.hands) {
			if (hand > bestHand) {
				bestHand = hand;
			}
		}
		return bestHand;
	}

}

public class Oppontents {
	public int count;
	public List<Position> positions;

	public Oppontents() {
		positions = new List<Position> ();
	}
}

public class Position {
	public List<ThFTR> items;

	public Position() {
		items = new List<ThFTR> ();
	}
}

public abstract class ThPFTR {
	public int position;	// player no
	public Pattern pattern;
	public List<Pattern> alt_patterns;
}

public class PreFlop: ThPFTR {
	public string hand;
//	public double winPercent; // hand strength in percents

	public PreFlop() {
		alt_patterns = new List<Pattern>();
	}
}

public class ThFTR: ThPFTR {
	public int enemyCount;	// opponents count
	public double winPercentMin;
	public double winPercentMax;

	public ThFTR() {
		alt_patterns = new List<Pattern>();
	}
}

public class Flop: ThFTR {

}

public class Turn: ThFTR {

}

public class River: ThFTR {

}

public class Pattern {

	public string name;
	public string actionDefault;
	public string actionPreffered1;
	public string actionPreffered2;
	public int betMaxCallOrRaise;	// in number of bets

	public List<PatternBetRoundAndAction> betRounds;

	public double percent;

	public Pattern() {
		betRounds = new List<PatternBetRoundAndAction>();
	}
}

public class PatternBetRoundAndAction {
	public string name_action;
	public double costBet; // in number of bets (1 credit = 1 bet * multiplier)
	public double costBetTotal; // in number of bets (1 credit = 1 bet * multiplier)
}
