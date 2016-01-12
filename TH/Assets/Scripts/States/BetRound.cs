using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBetRoundState
{
	void SubRound ();
	void NextPlayer ();
}

public abstract class AbstractBetRound {
	protected int subRoundMaxSize;
	protected int subRoundCount;
	protected double betMin;
	protected double betMax;
	protected Game game;
	protected double betToStayInGame, pot;
}

public class BetRound : AbstractBetRound, IBetRoundState {
	public bool isWaiting; // wait for corountine
	public bool isCanToRaise;
//	public bool isRaised;

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
		this.isCanToRaise = true;
	}
	
	#region IBetRoundState implementation
	
	public virtual void SubRound ()
	{
		if (subRoundCount == 0) {
			FirstAction ();
			subRoundCount++;
		} else if (subRoundCount <= subRoundMaxSize && subRoundMaxSize > 0) {
			BetSubRounds ();
		} else {
			LastAction(); //next state (preflop, turn, river)
			subRoundCount = 0;
		}
	}

	public virtual void NextPlayer() {

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

	public void SetPatternAndHisAlternatives(List<PatternFTR> items) {
		foreach (var player in game.players)
			foreach (var item in items) {
				if (item.position == player.position) {
					if (item.winPercentMin >= player.winPercent && player.winPercent <= item.winPercentMax) {
					
						player.pattern = item.pattern;
						player.alt_patterns = item.alt_patterns;
					
						break;
					} else {
						player.pattern = game.source.GetPatternByName (Settings.defaultPreflopPattern);
					}
				}
			}
	}

	public void SetPatternAndHisAlternativesForPreflop(List<PatternPreflop> items) {
		foreach (var player in game.players)
		foreach (var preflop in items) {
			if (preflop.position == player.position) {
				if (preflop.hand == player.handPreflopString || preflop.hand == player.handPreflopStringReversed) {
					
					player.pattern = preflop.pattern;
					player.alt_patterns = preflop.alt_patterns;
					
					break;
				} else {
					player.pattern = game.source.GetPatternByName(Settings.defaultPreflopPattern);
				}
			}
		}
	}
	
}
