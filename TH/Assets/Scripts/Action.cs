using System;

public interface IAction
{
	void Do (Game game, Player player);
}

public class ActionTip: Action {
	public ActionTip (double betToStayInGame)
	{
		this.betCall = betToStayInGame;
	}
	public bool isInBetSubrounds = true;
	public bool isInPriority1 = true;
	public bool isInPriority2 = true;
	public bool isInDefault = true;
}

public class Action : IAction {
	public Action() {}

	public Action (double betDx)
	{
		this.betCall = betDx;
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
		}
	}
	
	#endregion

	private void DoActive(Game game, Player p) {

		if (game.state.betMax <= game.state.betMaxLimit) {
			if (betToStay > game.state.betMax) {
				game.state.betMax = betToStay;
			}
		}

		p.betInvested += betToStay;
		p.betTotal -= betToStay;

		double dtRaise = p.betInvested  - game.state.betMax;
		if (p.isReal) {
			if (dtRaise > 0) {
				if (game.state.betMax > p.betInvested )
					game.ui.lblCall.text = betCall.to_s ();
				else
					game.ui.lblCall.text = Settings.betNull.to_s ();
				game.ui.lblRaise.text = dtRaise.to_s();
			} else if (dtRaise == 0) {
				game.ui.lblCall.text = betCall.to_s ();
				game.ui.lblRaise.text = dtRaise.to_s();
			} else if (dtRaise < 0){
				game.ui.lblCall.text = betCall.to_s ();
				game.ui.lblRaise.text = Settings.betNull.to_s ();
			}
		}

		if (Settings.isDev) {
			game.ui.DebugLog(string.Format("bet: p_invested:{0}/{1} stay:{2}/max:{3}", p.betInvested , p.betTotal, game.state.betMax, game.state.betMaxLimit));
//			p.ToString();
			p.DevInfo(p);
		}
	}

	public string name;
	public double betCall;
	public double betRaise;
	public double betToStay {
		get { return betCall + betRaise; } 
	}

	public bool isRaise {
		get { return name.Contains ("RAISE"); }
	}
	public bool isCall {
		get { return name.Contains ("CALL"); }
	}
	public bool isCheck {
		get { return name.Contains ("CHECK"); }
	}
	public bool isFold {
		get { return name.Contains ("FOLD"); }
	}
	public bool isAllIn {
		get { return name.Contains ("ALL IN"); }
	}
	public bool isUnknown {
		get { return name.Contains ("OPEN"); }
	}
}

public class Call : Action
{
	public Call (Player player, double betToStayInGame)
	{
		this.name = "CALL";
		player.UpdateActionCurrentString (this.name);
		this.betCall = betToStayInGame;
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
		this.betCall = betToStayInGame;
	}
}

public class Fold : Action
{
	public Fold (Player player, double betToStayInGame)
	{
		this.name = "FOLD";
		player.isFolded = true;
		player.UpdateActionCurrentString (this.name);
		this.betCall = betToStayInGame;
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
//		if (player.patternCurrent != null)
//			betToStayInGame += player.patternCurrent.betMaxCallOrRaise * Settings.betCurrentMultiplier;//TODO
		this.betCall = betToStayInGame;
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
		this.betCall = betToStayInGame;
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
