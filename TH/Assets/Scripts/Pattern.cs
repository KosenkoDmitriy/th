using System;
using System.Collections.Generic;

public class Pattern
{
	public Pattern ()
	{
		betSubRounds = new List<PatternBetRoundAndAction> ();
	}

	public string name;
	public string actionDefault;
	public string actionPriority2;
	public string actionPriority1;
	
	public int betMaxCallOrRaise;	// in number of bets
	public double betDt;
	
	public List<PatternBetRoundAndAction> betSubRounds;
	public double percent;
}

public class PatternBetRoundAndAction
{
	public string name_action;
	//	public double costBetDx;	// costBet - costBetTotal
	public double costBet;		// in number of bets (1 credit = 1 bet * multiplier)
	public double costBetTotal; // in number of bets (1 credit = 1 bet * multiplier)
}

public class Oppontents
{
	public Oppontents ()
	{
		positions = new List<Position> ();
	}

	public int count;
	public List<Position> positions;
}

public class Position
{
	public Position ()
	{
		items = new List<PatternFTR> ();
	}

	public List<PatternFTR> items;
}

public abstract class AbstractPattern
{
	public int position;	// 0 - first to act player
	public Pattern pattern;
	public List<Pattern> alt_patterns;
}

public class PatternPreflop: AbstractPattern
{
	public PatternPreflop ()
	{
		alt_patterns = new List<Pattern> ();
	}

	public string hand;
}

public class PatternFTR: AbstractPattern
{
	public PatternFTR ()
	{
		alt_patterns = new List<Pattern> ();
	}

	public int enemyCount;	// opponents count
	public double winPercentMin;
	public double winPercentMax;
}
