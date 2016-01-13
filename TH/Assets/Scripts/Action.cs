using System;

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
				if (p.isFolded) {
					game.state = new InitGame(game);
//					game.state = new EndGame(game);
				}
				game.state.isWaiting = false;
				game.ui.SetBalance(p.betTotal.to_s());
			} else {
				if (!p.isFolded) {
					p.bet = bet;
					p.betAlreadyInvestedBeforeAction += bet;
					p.betTotal -= bet;
				} else {
					p.ShowCards(game);
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
	public override void Do(Game game) {
		base.Do (game);
		game.ui.audio.PlayOneShot(game.ui.soundRaise);
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

	public override void Do(Game game) {
		base.Do (game);
		game.ui.audio.PlayOneShot(game.ui.soundFold);
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
		base.Do (game);
		game.ui.audio.PlayOneShot(game.ui.soundRaise);
	}
}
