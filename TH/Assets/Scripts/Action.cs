using System;

public interface IAction
{
	void Do (Game game, Player player);
}

public class ActionTip: Action {

	public bool isInBetSubrounds;
	public bool isInPriority1;
	public bool isInPriority2;
	public bool isInDefault;
}

public class Action : IAction {
	public Action() {
		Init ();
	}

	private void Init() {
		betRaise = new Bet(0);
		betCall = new Bet(0);
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
				game.ui.SetBalance(p.balanceInCredits.ToString());
				Settings.playerCredits = p.balanceInCredits;
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
		/*
		if (game.state.betMax <= game.state.betMaxLimit) {
			if (betToStay > game.state.betMax) {
				if (betToStay >= 0) {
					game.state.betMax = betToStay;
				} else {
					p.Log(true, false, "betToStay < 0 in DoActive()");
				}
			}
		}
		*/

		if (betToStay > 0 && p.betInvested <= game.state.betMaxLimit) {
			p.betInvested += betToStay;
			p.balanceInCredits -= betToStay.inCredits;
			if (p.betInvested > game.state.betMax) {
				game.state.betMax = p.betInvested;
			}
		}

//		double dtRaise = p.betInvested.inBet - game.state.betMax.inBet;
		if (p.isReal) {
			game.ui.lblCall.text = betCall.inCredits.f ();
			game.ui.lblRaise.text = betRaise.inCredits.f();
			/*
			if (betRaise.inCredits > 0) {
				if (game.state.betMax > p.betInvested)
					game.ui.lblCall.text = betCall.inCredits.f ();
				else
					game.ui.lblCall.text = Settings.betNull.f ();
				game.ui.lblRaise.text = betRaise.inCredits.f();
			} else if (betRaise.inCredits == 0) {
				game.ui.lblCall.text = betCall.inCredits.f ();
				game.ui.lblRaise.text = betRaise.inCredits.f();
			} else if (betRaise.inCredits < 0) {
				game.ui.lblCall.text = betCall.inCredits.f ();
				game.ui.lblRaise.text = Settings.betNull.f ();
			}
		*/
		}

		if (Settings.isDev) {
			p.Log(false, false, string.Format("bet: p_invested:{0}/{1} stay:{2}/max:{3}", p.betInvested, p.balanceInCredits, game.state.betMax, game.state.betMaxLimit));
//			p.ToString();
			p.LogDevInfo(p, false, false);
		}
	}

	public string name = "";
	public Bet betCall;
	public Bet betRaise;
	public Bet betToStay {
		get {
			var sum = betCall + betRaise;
//			if (sum < 0)
//				return new Bet(0); 
			return sum;
		}
	}

	public bool isRaise {
		get { return name.isRaise (); }
		set {
			if (value) name = Settings.aRaise;
		}
	}
	public bool isCall {
		get { return name.isCall (); }
		set {
			if (value) {
				name = Settings.aCall;
				betRaise = new Bet(0);
			}
		}
	}
	public bool isCheck {
		get { return name.isCheck (); }
		set {
			if (value) {
				name = Settings.aCheck;
				betCall = new Bet(0);
				betRaise = new Bet(0);
			}
		}
	}
	public bool isFold {
		get { return name.isFold (); }
		set {
			if (value) {
				name = Settings.aFold;
				betCall = new Bet(0);
				betRaise = new Bet(0);
			}
		}
	}
	public bool isAllIn {
		get { return name.isAllIn (); }
		set {
			if (value) name = Settings.aAllIn;
		}
	}
	public bool isUnknown {
		get { return name.isUnknown(); }
		set {
			if (value) name = Settings.aUnknown;
		}
	}
}

public class Call : Action
{
	public Call (Player player, Bet betCall, Bet betRaise)
	{
		this.name = Settings.aCall;
		player.UpdateActionCurrentString (this.name);
//		this.betCall = betToStayInGame;
//		this.betRaise.inCredits = 0;

		this.betCall = betCall;
		this.betRaise = betRaise;
	}
	public override void Do(Game game, Player p) {
		base.Do (game, p);
		game.ui.audio.PlayOneShot(game.ui.soundRaise);
	}
}

public class Check : Action
{
	public Check (Player player, Bet betCall, Bet betRaise)
	{
		this.name = Settings.aCheck;
		player.UpdateActionCurrentString (this.name);

		this.betCall = betCall;
		this.betRaise = betRaise;
	}
}

public class Fold : Action
{
	public Fold (Player player, Bet betCall, Bet betRaise)
	{
		this.name = Settings.aFold;
		player.UpdateActionCurrentString (this.name);

		this.betCall = betCall;
		this.betRaise = betRaise;
	}

	public override void Do(Game game, Player p) {
		if (Settings.isDev) {
			p.Log (false, true, string.Format ("{0} folded", p.name));
			p.LogDevInfo(p, true, false);
		}
		p.isFolded = true;

		//start | if folded then reinit player's position, patterns and it alternatives
		int i = 0;
		for(var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; p = game.playerIterator.Next()) {
			if (!player.isFolded) {
				player.position = i;
				i++;
			}
		}
		game.state.UpdatePattern();
		//end | if folded then reinit player's position, patterns and it alternatives

		base.Do (game, p);
		game.ui.audio.PlayOneShot(game.ui.soundFold);
	}
}

public class Raise : Action
{
	public Raise (Player player, Bet betCall, Bet betRaise)
	{
		this.name = Settings.aRaise;
		player.UpdateActionCurrentString (this.name);

		this.betCall = betCall;
		this.betRaise = betRaise;
	}

	public override void Do(Game game, Player p) {
		base.Do (game, p);
		game.ui.audio.PlayOneShot(game.ui.soundRaise);
	}
}


public class AllIn : Action
{
	public AllIn (Player player, Bet betCall, Bet betRaise)
	{
		this.name = Settings.aAllIn;
		player.UpdateActionCurrentString (this.name);

		this.betCall = betCall;
		this.betRaise = betRaise;
	}
	
	public override void Do(Game game, Player p) {
		game.ui.audio.PlayOneShot(game.ui.soundRaise);

		p.isAllIn = true;
		if (game.state.playerFirstToAllIn == null) {
			game.state = new AllInRound (game, p, game.state.betMax.inCredits);
		} else {

		}

		if (p.isAllIn) {
			game.state.playersAllIn.Add (p);
			if (p.balanceInCredits > game.state.betMax.inCredits) {
				game.state.betMax.inCredits = p.balanceInCredits;
			}
			p.lblCredits.text = Settings.betNull.f ();
		}
		game.state.isWaiting = false;
	}
}
