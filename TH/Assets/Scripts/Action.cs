using System;

public interface IAction
{
	void Do ();
}
public class Action : IAction {
	public Player p;
	public double bet;

	public Action() {}

	public Action (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
	#region IAction implementation
	
	public void Do ()
	{
		
//		if (bet == 0) {
//			p.actionFinal = new Check (p, bet);
//		}
	}
	
	#endregion
}

public class Call : Action
{
	public Call (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
//	#region IAction implementation

//	public void Do ()
//	{
//		if (bet == 0) {
//			p.actionFinal = new Check (p, bet);
//		}
//	}
//
//	#endregion
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
//	#region IAction implementation
//	
//	public void Do ()
//	{
//
//	}
//     
// #endregion
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
//	#region IAction implementation
//	
//	public void Do ()
//	{
//		if (bet == 0) {
//			p.actionFinal = new Check (p, bet);
//		}
//	}
//	
//	#endregion
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
//	#region IAction implementation
//	
//	public void Do ()
//	{
//		if (bet == 0) {
//			p.actionFinal = new Check (p, bet);
//		} else if (p.betTotal > bet) {
//			// raise
//		} else if (p.betTotal == bet) {
//			p.actionFinal = new Call (p, bet);
//		}
//	}
//	
//	#endregion
}
