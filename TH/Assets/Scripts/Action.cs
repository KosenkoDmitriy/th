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
	public bool isAllIn;
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
				if (p.isFolded) {
					game.state = new InitGame(game);
				} else {
					DoActive(game);
					game.player = p;
				}
				game.ui.lblCall.text = Settings.betNull.to_s();
				game.ui.SetBalance(p.betTotal.to_s());
				game.state.isWaiting = false;
			} else {
				if (p.isFolded) {
					p.ShowCards(game);
				} else {
					DoActive(game);
				}
			}
			game.ui.UpdatePlayerActionAndCredits(p);

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
				game.ui.lblRaise.text = betDx.to_s ();// p.betAlreadyInvestedInCurrentSubRound.to_s();
			} else {
				game.ui.lblRaise.text = Settings.betNull.to_s();
			}
		}
	}

	public Player p;
	public double betDx;
	public string name;
}

public class Call : Action
{
	public Call (Player player, double betToStayInGame)
	{
		this.name = "CALL";
		player.UpdateActionCurrentString (this.name);
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
		this.name = "CHECK";
		player.UpdateActionCurrentString (this.name);
		this.p = player;
		this.betDx = betToStayInGame;
	}
}

public class Fold : Action
{
	public Fold (Player player, double betToStayInGame)
	{
		this.name = "FOLD";
		player.UpdateActionCurrentString (this.name);
		this.p = player;
		this.betDx = betToStayInGame;
	}

	public override void Do(Game game) {
		p.isFolded = true;
		base.Do (game);
		game.ui.audio.PlayOneShot(game.ui.soundFold);
	}
}

public class Raise : Action
{
	public Raise (Player player, double betToStayInGame)
	{
		this.name = "RAISE";
		player.UpdateActionCurrentString (this.name);
		this.p = player;
		this.betDx = betToStayInGame;
	}

	public override void Do(Game game) {
		base.Do (game);
		game.ui.audio.PlayOneShot(game.ui.soundRaise);
	}
}


public class AllIn : Action
{
	public AllIn (Player player, double betToStayInGame)
	{
		this.name = "ALL IN";
		player.UpdateActionCurrentString (this.name);
		this.p = player;
		this.betDx = betToStayInGame;
	}
	
	public override void Do(Game game) {
		game.ui.audio.PlayOneShot(game.ui.soundRaise);

		p.isAllIn = true;
		if (game.state.playerFirstToAllIn == null) {
			game.state = new AllInRound (game, p, game.state.betMax);
		} else {
			if (p.isReal) {

			} else {

			}
		}

		if (p.isAllIn) {
			game.state.playersAllIn.Add (p);
			if (p.betTotal > game.state.betMax) {
				game.state.betMax = p.betTotal;
			}
			p.lblCredits.text = Settings.betNull.to_s ();
//			game.potAmount += p.betTotal;
//			game.ui.lblPot.text = game.potAmount.to_s ();
		}


//		game.ui.UpdatePlayerActionAndCredits(p);

		game.state.isWaiting = false;
	}
}
