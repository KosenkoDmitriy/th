using System;
using System.Collections.Generic;

public class Pattern
{
	public string name;
	public string actionDefault;
	public string actionPriority2;
	public string actionPriority1;
	public int betMaxCallOrRaise;	// in number of bets
	
	public List<PatternBetRoundAndAction> betSubRounds;
	public double percent;
	
	public Pattern ()
	{
		betSubRounds = new List<PatternBetRoundAndAction> ();
	}
}

public class PatternBetRoundAndAction
{
	public string name_action;
	public double costBet; // in number of bets (1 credit = 1 bet * multiplier)
	public double costBetTotal; // in number of bets (1 credit = 1 bet * multiplier)
}

public class Oppontents
{
	public int count;
	public List<Position> positions;
	
	public Oppontents ()
	{
		positions = new List<Position> ();
	}
}

public class Position
{
	public List<PatternFTR> items;
	
	public Position ()
	{
		items = new List<PatternFTR> ();
	}
}

public abstract class AbstractPattern
{
	public int position;	// 0 - first to act player
	public Pattern pattern;
	public List<Pattern> alt_patterns;
}

public class PatternPreflop: AbstractPattern
{
	public string hand;

	public PatternPreflop ()
	{
		alt_patterns = new List<Pattern> ();
	}
}

public class PatternFTR: AbstractPattern
{
	public int enemyCount;	// opponents count
	public double winPercentMin;
	public double winPercentMax;
	
	public PatternFTR ()
	{
		alt_patterns = new List<Pattern> ();
	}
}
