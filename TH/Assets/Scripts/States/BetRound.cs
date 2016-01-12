using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
	
	public virtual void FirstAction() {}

	public virtual void LastAction() {
		game.playerIterator = new PlayerIterator (game.playerCollection);
		while (!game.playerIterator.IsDone) {
			var player = game.playerIterator.Next();
			pot += player.betAlreadyInvestedBeforeAction;
		}
		game.potAmount = pot;
		game.ui.lblPot.GetComponent<Text>().text = game.potAmount.to_s ();
		game.state = new FlopRound (game);
	}
	
	public virtual void BetSubRounds() {
		// bet sub rounds
		if (!game.state.isWaiting) {
			var player = game.playerIterator.NextActive();
			
			if (player.isReal) {
				game.state.isWaiting = true;
				if (isCanToRaise) {
					game.ui.btnRaise.GetComponent<Button>().interactable = true;
				} else {
					game.ui.btnRaise.GetComponent<Button>().interactable = false;
				}
			} else {
				player.actionFinal = player.GetFinalAction(betMax, isCanToRaise);
				player.actionTip.Do(game);
			}
			
			if (player.position == game.playerIterator.LastActive().position) { // last player
				isCanToRaise = false;
				if (!isCanToRaise) {
					if (subRoundCount < subRoundMaxSize) {
						subRoundCount++;
					} else if (subRoundCount == subRoundMaxSize) {	// last subround
						if (IsNextBetRound()) {						// no any raise
							subRoundCount++; // LastAction();		// next bet round
						} else {			
							isCanToRaise = false; // repeat last subround with disabled raise action
						}
					}
					if (IsNextBetRound()) {
						LastAction(); // next bet round if no any raise in any subround
					}
				}
			}
			game.ui.UpdatePlayer(player);
		}
		//		game.state.isWaiting = true;
		//		game.ui.StartCoroutine (game.ui.UpdatePlayer (player));
	}

	private bool IsNextBetRound() {
		var iterator = new PlayerIterator (game.playerCollection);
		bool isNextBetRound = false;
		while (!iterator.IsDone) {
			var player = iterator.NextActive();
			if (player.betAlreadyInvestedBeforeAction != betMax) {
				isNextBetRound = false;
			} else {
				isNextBetRound = true;
				break;
			}
		}
		return isNextBetRound;
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
