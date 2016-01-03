using System;
//using UnityEngine;
using System.Collections.Generic;

public class Player {

	public Player() {
		cards = new List<Card>();
	}

	public double credits;
	public int no;
	public string name;

	public string handString;
	public Hand gand;

	public List<Card> cards;
//	public List<Card> cardsTwo;

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
		string action = patternCurrent.actionDefault;
//		action = patternCurrent.actionPreffered1;
//		action = patternCurrent.actionPreffered2;
		if (patternCurrent.betRounds != null && patternCurrent.betRounds.Count > 0)
		foreach (var betRound in patternCurrent.betRounds) {
			if (betRound.costBet * Settings.betDx == betToStayInGame && betRound.costBetTotal * Settings.betDx == betTotal) {
				action = betRound.name_action;
				break;
			}
		}
		return action;
	}

}

public class Oppontents {
	public int count;
	public List<Position> positions;
}

public class Position {
	public List<ThFTR> items;
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
	public string actionPreffered1;
	public string actionPreffered2;

	public List<PatternBetRoundAndAction> betRounds;

	public double percent;
}

public class PatternBetRoundAndAction {
	public string name_action;
	public double costBet; // in number of bets (1 credit = 1 bet * multiplier)
	public double costBetTotal; // in number of bets (1 credit = 1 bet * multiplier)
}
