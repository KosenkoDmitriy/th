using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public interface IBetRoundState
{
	void SubRound ();
	void NextPlayer ();
}

public abstract class AbstractBetRound {
	protected int subRoundMaxSize;
	protected int subRoundCount;
	public double betMax;
	protected Game game;
	protected double betToStayInGame, pot;
}

public class BetRound : AbstractBetRound, IBetRoundState {
	public bool isWaiting; // wait for corountine
	public bool isCanToRaise;
//	public bool isRaised;
	public Player playerFirstToAllIn;
	public List<Player> playersAllIn;

	public BetRound() {
		Init ();
	}
	
	public BetRound(Game game) {
		Init ();
		this.game = game;
	}
	
	private void Init() {
		this.subRoundCount = 0;
		this.subRoundMaxSize = Settings.betSubRoundMinSize;
		this.isCanToRaise = true;
		Settings.betCurrentMultiplier = Settings.betPreflopFlopMultiplier;

		// clear betAlreadyInvestedInNumberOfBets for new bet round
		if (game != null && game.playerIterator != null)
		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			player.betAlreadyInvestedInCurrentSubRound = 0;
		}
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
		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			pot += player.betAlreadyInvestedInCurrentSubRound;
		}
		game.potAmount = pot;
		game.ui.lblPot.GetComponent<Text>().text = game.potAmount.to_s ();
	}
	
	public virtual void BetSubRounds() {
		// bet sub rounds
		if (!game.state.isWaiting) {
			var player = game.playerIterator.NextActive();

			if (IsOneActivePlayer()) { // if one active player then he is winner
				game.winners = new List<Player>();
				game.winners.Add(player);
				game.state = new EndGame(game);
				return;
			}

			if (player.isReal) {
				game.state.isWaiting = true;
				game.player = player;

				double dt = betMax - player.betAlreadyInvestedInCurrentSubRound;

				if (dt > 0) {
					game.ui.btnCall.GetComponent<Button>().interactable = true;
					game.ui.btnCheck.GetComponent<Button>().interactable = false;
				} else if (dt == 0) {
					game.ui.btnCall.GetComponent<Button>().interactable = false;
					game.ui.btnCheck.GetComponent<Button>().interactable = true;
				} else {
					game.ui.btnCall.GetComponent<Button>().interactable = true;
					game.ui.btnCheck.GetComponent<Button>().interactable = true;
				}

				game.ui.lblCall.text = dt.to_s();
				game.ui.lblRaise.text = Settings.betNull.to_s();

				if (isCanToRaise) {
					game.ui.btnRaise.GetComponent<Button>().interactable = true;
				} else {
					game.ui.btnRaise.GetComponent<Button>().interactable = false;
				}
			} else {
				player.actionFinal = player.GetFinalAction(betMax, isCanToRaise, game);
				player.actionFinal.Do(game);
			}

		}
	}

	private bool IsNextBetRound() {
		var iterator = new PlayerIterator (game.playerCollection);
		bool isNextBetRound = false;
		while (!iterator.IsDone) {
			var player = iterator.NextActive();
			if (player.betAlreadyInvestedInCurrentSubRound != betMax) {
				isNextBetRound = false;
			} else {
				isNextBetRound = true;
				break;
			}
		}
		return isNextBetRound;
	}

	private bool IsOneActivePlayer() {
		bool isOneActivePlayer = false;
		var list = new List<Player> ();

		var iterator = new PlayerIterator (game.playerCollection);

//		while (!iterator.IsDone) {
//			var player = iterator.Next();
//			if (!player.isFolded)
//				list.Add(player);
//		}

		for (Player player = iterator.First(); !iterator.IsDoneFor; player = iterator.Next()) {
			if (!player.isFolded)
				list.Add(player);
		}

		if (list.Count <= 1)
			isOneActivePlayer = true;

		return isOneActivePlayer;
	}

	public void SetPatternAndHisAlternatives(List<PatternFTR> items) {
		foreach (var player in game.players)
			foreach (var item in items) {
				if (item.position == player.position) {
					if (item.winPercentMin > player.winPercent && player.winPercent <= item.winPercentMax) {
					
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

	public void CheckForNextSubOrRound() {
		isCanToRaise = false;
		if (!isCanToRaise) {
			if (subRoundCount < subRoundMaxSize) {
				isCanToRaise = true;
				subRoundCount++;
			} else if (subRoundCount == subRoundMaxSize) {	// last subround
				if (IsNextBetRound()) {						// no any raise
					subRoundCount++; // LastAction();		// next bet round
				}			
				isCanToRaise = false; // repeat last subround with disabled raise action
			}
			if (IsNextBetRound()) { // no any raise
				LastAction(); // next bet round if no any raise in any subround
			}
		}
	}
	
}
