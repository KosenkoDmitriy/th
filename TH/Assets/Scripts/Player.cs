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

	List<PreFlop> preflopBets;
	List<PreFlop> flopBets;
	List<Turn> turnBets;
	List<River> riverBets;
	
	public void dealCards() {
	}

}

public class Card {

}

public abstract class ThPFTR {
	int enemyCount;	// opponents count
	int position;	// player no
	Pattern pattern;
	List<Pattern> alt_patterns;
}

public class PreFlop: ThPFTR {
	string hand;
	double winPercent; // hand strength in percents
}

public class Flop: ThPFTR {
	double winPercentMin;
	double winPercentMax;
}

public class Turn: ThPFTR {
	double winPercentMin;
	double winPercentMax;
}

public class River: ThPFTR {
	double winPercentMin;
	double winPercentMax;
}

public class Pattern {
	string name;
	string actionDefault;
	string actionPreffered1;
	string actionPreffered2;

	List<PatternBetRoundAndAction> betRounds;

	double percent;
}

public class PatternBetRoundAndAction {
	string name_action;
	double costBet; // in number of bets (1 credit = 1 bet * multiplier)
	double costBetTotal; // in number of bets (1 credit = 1 bet * multiplier)
}
