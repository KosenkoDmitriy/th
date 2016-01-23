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
	protected Game game;
	public Bet betMax, betMaxLimit;
	protected Bet betToStayInGame, pot;
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
		Settings.betCurrentMultiplier = Settings.bePreflopFlopMultiplier;
		this.betMaxLimit = new Bet(Settings.betCurrentMultiplier);
		this.betToStayInGame = this.pot = this.betMax = new Bet (0);
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
//		game.state.betMax = betMax = 0;
		
		// clear betAlreadyInvestedInNumberOfBets for new bet round
//		if (game != null && game.playerIterator != null)
//		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
//			player.betAlreadyInvestedInCurrentSubRound = 0;
//		}
//		game.playerIterator = new PlayerIterator (game.playerCollection);
	}

	public virtual void LastAction() {
		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			pot += player.betInvested;
			player.betInvested.inBet = 0;
		}
		game.state.betMax.inBet = betMax.inBet = 0;
	
		game.potAmount += pot.inCredits;
		game.ui.lblPot.GetComponent<Text>().text = game.potAmount.f();
	}
	
	public virtual void BetSubRounds() {
		// bet sub rounds
		if (!game.state.isWaiting) {
			var player = game.playerIterator.Next();
			
			if (player == null) {
				Debug.Log(string.Format("CUR SUB ROUND:{0}/{1} isCanToRaise:{2} POT: cur:{3}/main:{4}", subRoundCount, subRoundMaxSize, isCanToRaise, pot, game.potAmount));
				if (IsOneActivePlayer()) { // if one active player then he is winner
					game.winners = new List<Player>();
					game.winners.Add(player);
					game.state = new EndGame(game);
					return;
				}

				CheckForNextSubOrRound(); // game.state.CheckForNextSubOrRound();
				
				var playersActive = new PlayerCollection();
				
				int i = 0;
				for(var p = game.playerIterator.First(); !game.playerIterator.IsDoneFor; p = game.playerIterator.Next()) {
					if (!p.isFolded) {
						playersActive[i] = p;
						i++;
					}
				}
				game.playerIterator = new PlayerIterator(playersActive);
				player = game.playerIterator.Next();
				//				return;
			}

			if (player.isReal) {
				game.state.isWaiting = true;

				Bet dt = player.betInvested - game.state.betMax;

				if (dt > 0) {
					game.ui.lblCall.text = Settings.betNull.f();
					game.ui.lblRaise.text = dt.inCredits.f();
				} else if (dt == 0) {
					game.ui.lblCall.text = Settings.betNull.f();
					game.ui.lblRaise.text = Settings.betNull.f();
				} else if (dt < 0) {
					dt *= -1;
					game.ui.lblCall.text = dt.inCredits.f();
				}

				if (dt > 0) {
					game.ui.btnCall.GetComponent<Button>().interactable = true;
					game.ui.btnCheck.GetComponent<Button>().interactable = false;
				} else if (dt == 0) {
					game.ui.btnCall.GetComponent<Button>().interactable = false;
					game.ui.btnCheck.GetComponent<Button>().interactable = true;
				} else if (dt < 0) {
					game.ui.btnCall.GetComponent<Button>().interactable = true;
					game.ui.btnCheck.GetComponent<Button>().interactable = true;
				}

				game.ui.lblCall.text = dt.inCredits.f();
				game.ui.lblRaise.text = Settings.betNull.f();


				if (isCanToRaise) {
					game.ui.btnRaise.GetComponent<Button>().interactable = true;
				} else {
					game.ui.btnRaise.GetComponent<Button>().interactable = false;
				}

			} else {
				player.actionFinal = player.GetFinalActionNew(game);//(betMax, isCanToRaise, game);
				player.actionFinal.Do(game, player);
			}

		}
	}

	private bool IsNextBetRound() {
		var iterator = new PlayerIterator (game.playerCollection);
		bool isNextBetRound = false;
		while (!iterator.IsDone) {
			var player = iterator.Next();
			if (!player.isFolded) {
				if (player.betInvested  == betMax || game.state.betMax > game.state.betMaxLimit) {
					isNextBetRound = true;
				} else {
					isNextBetRound = false;
					break;
				}
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
			if (subRoundCount < subRoundMaxSize || game.state.betMax < game.state.betMaxLimit) {
				isCanToRaise = true;
				subRoundCount++;
			} else if (subRoundCount == subRoundMaxSize || game.state.betMax == game.state.betMaxLimit) {	// last subround
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
