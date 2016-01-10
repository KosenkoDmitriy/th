﻿using UnityEngine;
using System.Collections;

public interface IBetRoundState
{
	void SubRound ();
}

public abstract class AbstractBetRound {
	protected int subRoundMaxSize;
	protected int subRoundCurrent;
	protected double betMin;
	protected Game game;
	protected double betToStayInGame, pot;
}

public class BetRound : AbstractBetRound, IBetRoundState {
	public bool isWaiting; // wait for corountine
	
	public BetRound() {
		Init ();
	}
	
	public BetRound(Game game) {
		Init ();
		this.game = game;
	}
	
	private void Init() {
		this.subRoundMaxSize = Settings.betSubRoundMinSize;
		this.betMin = Settings.betPreflopFlop;
	}
	
	#region IBetRoundState implementation
	
	public virtual void SubRound ()
	{
		if (subRoundCurrent == 0) {
			FirstAction ();
		} else if (subRoundCurrent <= subRoundMaxSize && subRoundMaxSize > 0) {
			BetSubRounds ();
		} else {
			LastAction(); //next state (preflop, turn, river)
			subRoundCurrent = 0;
		}
		subRoundCurrent++;
	}
	
	#endregion
	
	public virtual void FirstAction() {
		//		isWaiting = true;
		//		// do something (deal cards, display win info, reinit game, ui animation
		//		isWaiting = false;
	}
	
	public virtual void BetSubRounds() {
		// bet sub rounds
	}
	
	public virtual void LastAction() {
		// bet sub rounds
	}
	
}