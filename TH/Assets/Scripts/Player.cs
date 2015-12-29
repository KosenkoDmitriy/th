using System;
//using UnityEngine;
using System.Collections.Generic;

public class Player {
	public string name;
	public List<Card> hand;

	List<string> winCards;
//	bool isActive = true;
	bool isFolded = false;

	List<Pattern> patterns;
	List<Pattern> alt_patterns;

	public List<PreFlop> preflopBets;
	public List<Flop> flopBets;
	public List<Turn> turnBets;
	public List<River> riverBets;
	
	public void dealCards() {
	}

}

public class Card {

}

public abstract class ThPFTR {
	public int position;	// player no
	public Pattern pattern;
	public List<Pattern> alt_patterns;
}

public class PreFlop: ThPFTR {
	public string hand;
//	public double winPercent; // hand strength in percents
}

public class Flop: ThPFTR {
	public int enemyCount;	// opponents count
	double winPercentMin;
	double winPercentMax;
}

public class Turn: ThPFTR {
	public int enemyCount;	// opponents count
	double winPercentMin;
	double winPercentMax;
}

public class River: ThPFTR {
	public int enemyCount;	// opponents count
	double winPercentMin;
	double winPercentMax;
}

public class Pattern {
	public string name;
	public string actionDefault;
	string actionPreffered1;
	string actionPreffered2;

	public List<PatternBetRoundAndAction> betRounds;

	public double percent;
}

public class PatternBetRoundAndAction {
	public string name_action;
	public double costBet; // in number of bets (1 credit = 1 bet * multiplier)
	public double costBetTotal; // in number of bets (1 credit = 1 bet * multiplier)
}
