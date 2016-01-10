using System;

public interface IAction
{
	void Do ();
}

public class Call : IAction
{
	public Player p;
	double bet;

	public Call (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
	#region IAction implementation

	public void Do ()
	{
		if (bet == 0) {
			p.action = new Check (p, bet);
		}
	}

	#endregion
}

public class Check : IAction
{
	public Player p;
	double bet;

	public Check (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
	#region IAction implementation
	
	public void Do ()
	{

	}
     
 #endregion
}

public class Fold : IAction
{
	public Player p;
	double bet;
	
	public Fold (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
	#region IAction implementation
	
	public void Do ()
	{
		if (bet == 0) {
			p.action = new Check (p, bet);
		}
	}
	
	#endregion
}


public class Raise : IAction
{
	public Player p;
	double bet;
	
	public Raise (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
	#region IAction implementation
	
	public void Do ()
	{
		if (bet == 0) {
			p.action = new Check (p, bet);
		} else if (p.betTotal > bet) {
			// raise
		} else if (p.betTotal == bet) {
			p.action = new Call (p, bet);
		}
	}
	
	#endregion
}
