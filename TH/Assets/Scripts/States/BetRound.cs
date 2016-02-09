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
	protected Bet pot;
}

public class BetRound : AbstractBetRound, IBetRoundState {
	private bool _isWaiting;
	public bool isWaiting; // wait for corountine
	/*{ 
		get {
			return _isWaiting;
		}
		set {
			_isWaiting = value;
			if (this.game != null)
			if (_isWaiting)
				this.game.ui.DisableButtons(true);
			else
				this.game.ui.DisableButtons(false);
		}
	}*/
	public bool isCanToRaise;
	public Player playerFirstToAllIn;
	public List<Player> playersAllIn;
	public List<PatternFTR> items; // preflop, flop, turn, river

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
		this.betMaxLimit = new Bet(0);
		this.betMaxLimit.inBetMath = Settings.betMathLimit;
		this.pot = new Bet (0);
		this.betMax = new Bet (0);
	}
	
	#region IBetRoundState implementation
	
	public virtual void SubRound ()
	{
		if (subRoundCount == 0) {
			FirstAction ();
			subRoundCount++;
		} else if (subRoundCount <= subRoundMaxSize && subRoundMaxSize > 0) {
			BetSubRounds ();
		} else if (subRoundCount == subRoundMaxSize + 1) {
			LastAction(); // next state (preflop, turn, river)
//			subRoundCount = 0;
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
		game.playerIterator = new PlayerIterator (game.playerCollection);
	}

	public virtual void LastAction() {
		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			pot += player.betInvested;
			player.betInvested.inCredits = 0;
		}
	
		game.potAmount += pot.inCredits;
		game.ui.lblPot.GetComponent<Text>().text = game.potAmount.f();

		game.state.betMax = new Bet(0);
//		game.playerIterator = new PlayerIterator (game.playerCollection);
	}
	
	public virtual void BetSubRounds() {
		// bet sub rounds
		if (!game.state.isWaiting) {
			var player = game.playerIterator.Next();
			
			if (player == null) {
				if (Settings.isDev) Debug.Log(string.Format("end player iterator CUR SUB ROUND:{0}/{1} isCanToRaise:{2} POT: cur:{3}/main:{4}", subRoundCount, subRoundMaxSize, isCanToRaise, pot, game.potAmount));
				if (IsOneActivePlayer()) { // if one active player then he is winner
					if (Settings.isDev) Debug.Log ("one active player > EndGame()");
					game.winners = new List<Player>();
					game.winners.Add(player);
					game.state = new EndGame(game);
					return;
				}

				game.state.CheckForNextSubOrRound();
				
				var playersActive = new PlayerCollection();
				
				int i = 0;
				for(var p = game.playerIterator.First(); !game.playerIterator.IsDoneFor; p = game.playerIterator.Next()) {
					if (!p.isFolded) {
						playersActive[i] = p;
						i++;
					}
				}
				game.playerIterator = new PlayerIterator(playersActive);
//				player = game.playerIterator.Next();
				return;
			}
//			if (Settings.isDev) player.Log(false, false, "BetRound>SubRounds()");
//			if (Settings.isDev) player.LogDevInfo(player, false, false);

			if (player.isReal) {
				game.ui.DisableButtons(false);

				if (player.betInvested >= betMaxLimit) { // skip action
					return;
				}
				game.state.isWaiting = true;
				var bet = new Bet(player.betInvested.inCredits - game.state.betMax.inCredits);
				if (bet > 0) { // raise amount ?
					game.ui.lblRaise.text = bet.inCredits.f ();
					game.ui.lblCall.text = Settings.betNull.f();
				} else 
				if (bet == 0) { // check
					game.ui.lblRaise.text = Settings.betNull.f();
					game.ui.lblCall.text = Settings.betNull.f();

					game.ui.btnCall.GetComponent<Button>().interactable = false;
					game.ui.btnCheck.GetComponent<Button>().interactable = true;
				} else { // call amount
					bet *= -1;
					game.ui.lblCall.text = bet.inCredits.f ();
					game.ui.lblRaise.text = Settings.betNull.f();

					game.ui.btnCall.GetComponent<Button>().interactable = true;
					game.ui.btnCheck.GetComponent<Button>().interactable = false;
				}

				if (isCanToRaise) {
					game.ui.btnRaise.GetComponent<Button>().interactable = true;
				} else {
					game.ui.btnRaise.GetComponent<Button>().interactable = false;
				}

				Bet betMin = new Bet(0);
				betMin.inBetMath = Settings.betMinMath;
				if (game.player.balanceInCredits < bet.inCredits || game.player.balanceInCredits < betMin.inCredits || (player.betInvested >= betMaxLimit) ) { // don't allow raise
					game.ui.btnRaise.GetComponent<Button>().interactable = false;
				} else {
					game.ui.btnRaise.GetComponent<Button>().interactable = true;
				}
			} else {
				game.ui.DisableButtons(true);

				if (!player.isFolded) {
					player.actionFinal = player.GetFinalAction(game);//(betMax, isCanToRaise, game);
					player.actionFinal.Do(game, player);
				}
			}

		}
	}

	private bool IsNextBetRound() {
		var iterator = new PlayerIterator (game.playerCollection);
		bool isNextBetRound = false;
		for(var player = iterator.First(); !iterator.IsDoneFor; player = iterator.Next()) {
			if (!player.isFolded) {
				if (player.betInvested == game.state.betMax) { // && game.state.betMax > 0) {// || game.state.betMax > game.state.betMaxLimit) {
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
		foreach (var player in game.players) {
			player.pattern = null;
			foreach (var item in items) {
				if (item.position == player.position) {
					if (item.winPercentMin <= player.winPercent && player.winPercent <= item.winPercentMax) {
					
						player.pattern = item.pattern;
						player.alt_patterns = item.alt_patterns;
						break;
					}
				}
			}
			if (player.pattern == null) {
				player.Log(true, false, "player.pattern == null Flop/Turn/River");
				player.pattern = game.source.GetPatternByName (Settings.defaultPreflopPattern);
			}
		}
	}

	public void SetPatternAndHisAlternativesForPreflop(List<PatternPreflop> items) {
		foreach (var player in game.players) {
			player.pattern = null;
			foreach (var preflop in items) {
				if (preflop.position == player.position) {
					if (preflop.hand == player.handPreflopString || preflop.hand == player.handPreflopStringReversed) {
					
						player.pattern = preflop.pattern;
						player.alt_patterns = preflop.alt_patterns;
					
						break;
					}
				}
			}
			if (player.pattern == null) {
				player.Log(true, false, "player.pattern == null Preflop");
				player.pattern = game.source.GetPatternByName (Settings.defaultPreflopPattern);
			}
		}
	}

	public void CheckForNextSubOrRound() {
		isCanToRaise = false;
		if (!isCanToRaise) {
			if (subRoundCount < subRoundMaxSize) { // || game.state.betMax < game.state.betMaxLimit) {
				isCanToRaise = true;
				subRoundCount++;
			} else if (subRoundCount == subRoundMaxSize) { // last subround
				if (IsNextBetRound()) {						// no any raise
					subRoundCount++; // LastAction();		// next bet round
				}
				isCanToRaise = false; // repeat last subround with disabled raise action
//			} else if (game.state.betMax > 0 && (game.state.betMax == game.state.betMaxLimit || game.state.betMax > game.state.betMaxLimit)) {
//				isCanToRaise = false;
			}
			if (IsNextBetRound()) { // no any raise
				LastAction(); // next bet round if no any raise in any subround
			}
		}
	}

	public virtual void UpdatePattern() {
		SetPatternAndHisAlternatives (items); // for all bet rounds except preflop
	}
}
