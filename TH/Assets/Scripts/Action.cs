using System;
using UnityEngine;
using System.Collections.Generic;

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

			if (!p.isFolded && (p.betInvested + betToStay) > 0f) {
				p.lblCurBet.text = (p.betInvested + betToStay).inCredits.f();
				p.isChipHidden = false;
			} else {
				p.lblCurBet.text = "";
				p.isChipHidden = true;
			}

			if (p.isReal) {
				if (p.isFolded) {
					game.state = new InitGame(game);
				} else {
					DoActive(game, p);
					game.player = p;
				}
				game.ui.LoseBalance(betToStay.inCredits.ToString());
				Settings.playerCredits = p.balanceInCredits;
				game.state.isWaiting = false;
			} else {
				if (p.isFolded) {
					p.ShowCards(game);
					p.VisibleSmallCards(game, false); // hide small cards
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
		// skip check or any other action if no any raise 
		if (p.actionFinal.isRaise) {
			string a = p.lblAction.text;
			if (Settings.isLog) Debug.Log("isRaise=="+a);
			p.isLastToRaise = true;
			var playersActive = ReorderPlayers(game, p);
			game.playerIterator = new PlayerIterator(playersActive);
			var playerTemp = game.playerIterator.Next();
		}

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

	public PlayerCollection ReorderPlayers(Game game, Player player) {

		int id = player.position;
		if (Settings.isLog) Debug.LogWarning(string.Format("reorder players: #{0} | {1}",id, game.players.Count));
		var items = game.players;
		Player temp = null;
		var playersActive = new PlayerCollection();
		if (game != null && game.players.Count > 0) {
			if (Settings.isLog) Debug.Log ("==ReorderPlayers start===");
				
			for (int k = 0; k < items.Count; k++) {
				for (int l = 0; l < items.Count - 1; l++) {
					if (items[l].position > items[l+1].position) {
						temp = items[l];
						items[l] = items[l+1];
						items[l + 1] = temp;
					}
				}
			}
			if (Settings.isLog) {
				foreach (var player2 in items)
					Debug.Log (player2.ToString () + " pos: " + player2.position);
				Debug.Log ("==end==");
			}
		}
		Player p;
		foreach (var p1 in items) {
			p = p1;
			if (p.position == id) {
				p.isLastToRaise = true; 
				playersActive [0] = p;
			} else {
				p.isLastToRaise = false;
			}
		}
		int i = 1;
		foreach(var p2 in items) {
			p = p2;
			if (!p.isFolded) {
				if (p.position > id) {
					playersActive[i] = p;
					i++;
				} 
			}
		}
		i = 1;
		foreach(var p3 in items) {
			p = p3;
			if (!p.isFolded) {
				if (p.position < id) {
					playersActive[i] = p;
					i++;
				}
			}
		}
		if (Settings.isLog) {
			var iterator = new PlayerIterator(playersActive);
			for(var item = iterator.First(); !iterator.IsDoneFor; item = iterator.Next()) {
				Debug.Log(string.Format("#{0} isLastToRaise: {1} isDealer: {2} pos: {3}",item.id,item.isLastToRaise, item.isDealer, item.position));
			}
		}
		return playersActive;
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

//		p.lblCurBet.text = betToStay.inCredits.f();
//		p.isChipHidden = false;

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
		foreach (var player in game.players) {
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

//		p.lblCurBet.text = betToStay.inCredits.f();
//		p.isChipHidden = false;

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
