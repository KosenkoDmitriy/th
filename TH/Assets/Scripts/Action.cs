﻿using System;
//using UnityEngine.UI;

public interface IAction
{
	void Do (Game game);
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
	
	public virtual void Do (Game game)
	{
		if (p != null) {
			if (p.isReal) {
				game.state.isWaiting = false;
			} else {
				if (!p.isFolded) {
					p.bet = bet;
					p.betAlreadyInvestedBeforeAction += bet;
					p.betTotal -= bet;
				}
			}
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

	public Check (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
}

public class Fold : Action
{

	public Fold (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}
}


public class Raise : Action
{

	public Raise (Player player, double betToStayInGame)
	{
		this.p = player;
		this.bet = betToStayInGame;
	}

	public override void Do(Game game) {
//		if (p.isReal) {
//			if (game.state.isCanToRaise) {
//				game.ui.btnRaise.GetComponent<Button>().interactable = true;
//			} else {
//				game.ui.btnRaise.GetComponent<Button>().interactable = false;
//			}
//		}
//		if (!p.isFolded) {
//			if (!game.state.isCanToRaise) {
//				p.actionFinal = new Call (p, p.bet);
//				p.actionFinal.Do (game);
//			} else {
//				base.Do (game);
//			}
//		}
	}
}
