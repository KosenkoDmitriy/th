using System;
using System.Collections.Generic;

public class Pattern
{
	public Pattern ()
	{
		betSubRounds = new List<PatternBetRoundAndAction> ();
	}

	public string name;
	public List<PatternBetRoundAndAction> betSubRounds; // priority 1
	public string actionPriority2;	// priority 2
	public string actionPriority3;	// priority 3
	public string actionDefault;	// priority 4

	public int betMaxCallOrRaise;	// in number of bets
	public double percent;	// of all time
}

public class PatternBetRoundAndAction
{
	public string name_action;
	//	public double costBetDx;	// costBet - costBetTotal
	public double costBetToStayInGame;		// in number of bets (1 credit = 1 bet * multiplier)
	public double costBetAlreadyInvested; // in number of bets (1 credit = 1 bet * multiplier)
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
