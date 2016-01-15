using System;

public interface IAction
{
	void Do (Game game);
}

public class ActionTip: Action {
	public ActionTip (Player player, double betToStayInGame)
	{
		this.p = player;
		this.betToStayInGame = betToStayInGame;
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
		this.betToStayInGame = betToStayInGame;
	}

	#region IAction implementation
	
	public virtual void Do (Game game)
	{
		if (p != null) {
			if (p.isReal) {
				if (p.isFolded) {
					game.state = new InitGame(game);
				}
				game.state.isWaiting = false;
				game.ui.SetBalance(p.betTotal.to_s());
			} else {
				if (!p.isFolded) {
					if (betToStayInGame >= 0) {
						p.bet = betToStayInGame;
						p.betAlreadyInvestedInCurrentSubRound += betToStayInGame;
						p.betTotal -= betToStayInGame;
					} else {
						game.ui.DebugLog("ERROR: betToStayInGame should be positive");
					}
				} else {
					p.ShowCards(game);
				}
			}

			if (p.position == game.playerIterator.LastActive().position) { // last player
				game.state.CheckForNextSubOrRound();
			}
		}
	}
	
	#endregion

	public Player p;
	public double betToStayInGame;
}

public class Call : Action
{
	public Call (Player player, double betToStayInGame)
	{
		this.p = player;
		this.betToStayInGame = betToStayInGame;
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
		this.betToStayInGame = betToStayInGame;
	}
}

public class Fold : Action
{

	public Fold (Player player, double betToStayInGame)
	{
		this.p = player;
		this.betToStayInGame = betToStayInGame;
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
		this.betToStayInGame = betToStayInGame;
	}

	public override void Do(Game game) {
		base.Do (game);
		game.ui.audio.PlayOneShot(game.ui.soundRaise);
	}
}
