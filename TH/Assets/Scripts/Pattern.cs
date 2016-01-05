﻿using System;
using System.Collections.Generic;

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
