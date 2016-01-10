using System;

public interface IAction
{
	void Do ();
}
public class ActionTip: Action {
	public ActionTip (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
	public bool isCall;
	public bool isFold;
	public bool isCheck;
	public bool isRaise;
	//	public bool isAllIn;
}
public class Action : IAction {
	public Action() {}

	public Action (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}

	#region IAction implementation
	
	public void Do ()
	{
		if (p.isReal) {
			//game.state.isWaiting = true //TODO:
		}
	}
	
	#endregion

	public Player p;
	public double bet;
}

public class Call : Action
{
	public Call (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
}

public class Check : Action
{
	public Player p;
	double bet;

	public Check (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
}

public class Fold : Action
{
	public Player p;
	double bet;
	
	public Fold (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
}


public class Raise : Action
{
	public Player p;
	double bet;
	
	public Raise (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
}
