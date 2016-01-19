using System;

public interface IAction
{
	void Do (Game game, Player player);
}

public class ActionTip: Action {
	public ActionTip (double betToStayInGame)
	{
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

	public Action (double betDx)
	{
		this.betDx = betDx;
	}

	#region IAction implementation
	
	public virtual void Do (Game game, Player p)
	{
		if (p != null) {
			if (p.isReal) {
				if (p.isFolded) {
					game.state = new InitGame(game);
				} else {
					DoActive(game, p);
					game.player = p;
				}
				game.ui.SetBalance(p.betTotal.to_s());
				game.state.isWaiting = false;
			} else {
				if (p.isFolded) {
					p.ShowCards(game);
				} else {
					DoActive(game, p);
				}
			}
			game.ui.UpdatePlayerActionAndCredits(p);

			if (p.betAlreadyInvestedInCurrentSubRound > game.state.betMax) {
				game.state.betMax = p.betAlreadyInvestedInCurrentSubRound;
//				game.betMax = game.state.betMax;
			}

			if (p.position == game.playerIterator.LastActive().position) { // last player
				game.state.CheckForNextSubOrRound();
			}
		}
	}
	
	#endregion

	private void DoActive(Game game, Player p) {
		p.betAlreadyInvestedInCurrentSubRound += betDx;
		p.betTotal -= betDx;

		double dt = p.betAlreadyInvestedInCurrentSubRound - game.state.betMax;
		if (p.isReal) {
			if (dt > 0) {
				game.ui.lblCall.text = Settings.betNull.to_s();
				game.ui.lblRaise.text = betDx.to_s ();// p.betAlreadyInvestedInCurrentSubRound.to_s();
			} else if (dt == 0) {
				game.ui.lblCall.text = Settings.betNull.to_s();
				game.ui.lblRaise.text = Settings.betNull.to_s();
			} else if (dt < 0) {
				dt *= 1;
				game.ui.lblCall.text = betDx.to_s();
			}
		}
	}

	public double betDx;
	public string name;
}

public class Call : Action
{
	public Call (Player player, double betToStayInGame)
	{
		this.name = "CALL";
		player.UpdateActionCurrentString (this.name);
		this.betDx = betToStayInGame;
	}
	public override void Do(Game game, Player p) {
		base.Do (game, p);
		game.ui.audio.PlayOneShot(game.ui.soundRaise);
	}
}

public class Check : Action
{
	public Check (Player player, double betToStayInGame)
	{
		this.name = "CHECK";
		player.UpdateActionCurrentString (this.name);
		this.betDx = betToStayInGame;
	}
}

public class Fold : Action
{
	public Fold (Player player, double betToStayInGame)
	{
		this.name = "FOLD";
		player.isFolded = true;
		player.UpdateActionCurrentString (this.name);
		this.betDx = betToStayInGame;
	}

	public override void Do(Game game, Player p) {
//		p.isFolded = true;
		base.Do (game, p);
		game.ui.audio.PlayOneShot(game.ui.soundFold);
	}
}

public class Raise : Action
{
	public Raise (Player player, double betToStayInGame)
	{
		this.name = "RAISE";
		player.UpdateActionCurrentString (this.name);
		this.betDx = betToStayInGame;
	}

	public override void Do(Game game, Player p) {
		base.Do (game, p);
		game.ui.audio.PlayOneShot(game.ui.soundRaise);
	}
}


public class AllIn : Action
{
	public AllIn (Player player, double betToStayInGame)
	{
		this.name = "ALL IN";
		player.UpdateActionCurrentString (this.name);
		this.betDx = betToStayInGame;
	}
	
	public override void Do(Game game, Player p) {
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
