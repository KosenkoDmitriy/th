using System;
//using UnityEngine;
using System.Collections.Generic;

public class Player {
	public double credits;
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

public class Oppontents {
	public int count;
	public List<Position> positions;
}

public class Position {
	public List<ThFTR> items;
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

public class ThFTR: ThPFTR {
	public int enemyCount;	// opponents count
	public double winPercentMin;
	public double winPercentMax;
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
