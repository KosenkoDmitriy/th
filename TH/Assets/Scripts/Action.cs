using System;

public interface IAction
{
	void Do (Game game);
}

public class ActionTip: Action {
	public ActionTip (Player player, double betToStayInGame)
	{
		this.p = player;
		this.betDx = betToStayInGame;
	}
	public bool isCall;
	public bool isFold;
	public bool isCheck;
	public bool isRaise;
	//	public bool isAllIn;
}

public class Action : IAction {
	public Action() {}

	public Action (Player player, double betDx)
	{
		this.p = player;
		this.betDx = betDx;
	}

	#region IAction implementation
	
	public virtual void Do (Game game)
	{
		if (p != null) {
			if (p.isReal) {
				if (!p.isFolded) {
					DoActive(game);
				} else {
					game.state = new InitGame(game);
				}
				game.ui.lblCall.text = Settings.betNull.to_s();
				game.state.isWaiting = false;
				game.ui.SetBalance(p.betTotal.to_s());
			} else {
				if (!p.isFolded) {
					DoActive(game);
				} else {
					p.ShowCards(game);
				}
			}

			if (p.betAlreadyInvestedInCurrentSubRound > game.state.betMax) {
				game.state.betMax = p.betAlreadyInvestedInCurrentSubRound;
			}

			if (p.position == game.playerIterator.LastActive().position) { // last player
				game.state.CheckForNextSubOrRound();
			}
		}
	}
	
	#endregion

	private void DoActive(Game game) {
		p.betAlreadyInvestedInCurrentSubRound += betDx;
		p.betTotal -= betDx;

		if (p.isReal) {
			game.ui.lblCall.text = betDx.to_s();

			if (p.betAlreadyInvestedInCurrentSubRound > game.state.betMax) {
				game.ui.lblRaise.text = p.betAlreadyInvestedInCurrentSubRound.to_s();
			} else {
				game.ui.lblRaise.text = Settings.betNull.to_s();
			}
		}
	}

	public Player p;
	public double betDx;
}

public class Call : Action
{
	public Call (Player player, double betToStayInGame)
	{
		this.p = player;
		this.betDx = betToStayInGame;
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
		this.betDx = betToStayInGame;
	}
}

public class Fold : Action
{

	public Fold (Player player, double betToStayInGame)
	{
		this.p = player;
		this.betDx = betToStayInGame;
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
		this.betDx = betToStayInGame;
	}

	public override void Do(Game game) {
		base.Do (game);
		game.ui.audio.PlayOneShot(game.ui.soundRaise);
	}
}
