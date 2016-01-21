using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Player {

	public Player() {
		hand = new Hand ();
		alt_patterns = new List<Pattern> ();
	}

	public override string ToString ()
	{
		string str = string.Format ("{0} {1} {2}({5}%) {3} {4}", id, name, handPreflopString, betTotal, actionCurrentString, winPercent);
		if (Settings.isDev) {
			DevInfo(this);
		}

		return str;
	}

	public void DevInfo(Player player) {
		if (Settings.isDev) {
			string str = string.Format ("#{0} {1} pos: {6} hand:{2}({5}% win) bets: in_cur_bet_round:{7}/total:{3} cur_action: {4}", player.id, player.name, player.handPreflopString, player.betTotal, player.actionCurrentString, player.winPercent, player.position, player.betAlreadyInvestedInCurrentSubRound);
			Debug.Log (str);

			string str2 = "";
			if (player.actionTip != null) {
				str2 = string.Format ("action tip: {0} ({1}% win): bet to stay:{2}", player.actionTip.name, player.winPercent, player.actionTip.betCall);
				Debug.Log (str2);
			}
			if (player.patternCurrent != null) {
				str2 = string.Format ("cur pattern: {0} ({8}% of all time): p1:{1} p2:{2} d:{3}\n bet: stay:{4} call:{5} raise:{6} max:{7} ", player.patternCurrent.name, player.patternCurrent.actionPriority1, player.patternCurrent.actionPriority2, player.patternCurrent.actionDefault, player.patternCurrent.betToStayInGame, player.patternCurrent.betCall, player.patternCurrent.betRaise, player.patternCurrent.betMaxCallOrRaise, player.patternCurrent.percent);
				Debug.Log (str2);
			}
		}
	}

	public void UpdateActionCurrentString (string name)
	{
		if (Settings.isDev) {
			string isWinString = this.isWinner ? "(w)" : "";
			this.actionCurrentString = string.Format ("{0} {1} <{2}", name, isWinString, this.actionCurrentString);
//			Debug.Log(this.actionCurrentString);
		} else {
			this.actionCurrentString = string.Format ("{0}", name);
		}
	}
	
	private void UpdateDealerImage ()
	{
		if (isDealer) {
			var sprite = Resources.Load<Sprite> ("ic_dealer_old");
			if (this.dealer != null)
				this.dealer.sprite = sprite;
		} else {
			var sprite = Resources.Load<Sprite> (Settings.cardBg);
			if (this.dealer != null)
				this.dealer.sprite = sprite;
		}
	}

	public void SetChipRandomly() {
		if (chipSpriteList == null) {
			// start init chips
			chipSpriteList = new List<Sprite> ()
			{
				Resources.Load("chips_red", typeof(Sprite)) as Sprite,
				Resources.Load("chips_blue", typeof(Sprite)) as Sprite
			};
			// end init chips
		}

		int index = UnityEngine.Random.Range(0,chipSpriteList.Count);
		if (this.chip != null) this.chip.sprite = chipSpriteList [index];
	}

	public Pattern GetAndSetCurrentPatternRandomly() {
		float percentOfTime = UnityEngine.Random.value * 100;
		if (pattern != null) {
//			if (percentOfTime <= pattern.percent) {
				patternCurrent = pattern;
//			}
		}
		if (alt_patterns.Count > 0) {
			foreach(var item in alt_patterns) {
				if (percentOfTime <= item.percent) {
					patternCurrent = item;
				}
			}
		}

		if (patternCurrent == null) {
			patternCurrent = pattern;
		}
		return patternCurrent;
	}

	public Action GetFinalAction(double betMax, bool isCanToRaise, Game game) {
		// summary:
		// get pattern randomly
		// preferred/recommend action from the pattern
		// final optimal correct actual action

		patternCurrent = GetAndSetCurrentPatternRandomly ();
		actionCurrentString = GetCurrentActionStringFromCurrentPattern (betMax, betAlreadyInvestedInCurrentSubRound); // best actionString from the patternCurrent
		actionCurrentString = GetAndSetActionTipByName (actionCurrentString, patternCurrent.betCall); // set actionTip get actionTipString (recommend action)

		Action actionFinal = new Action();

		double betDt = patternCurrent.betCall;
		if (betDt < 0) {
			Debug.LogWarning("betToStayInGame should be > 0 but:" + betDt);
//			betDt = 0;
		}

		// from recommend to optimal
		double betTotalAfterAction = betTotal - betDt;
		double betTotalSubRoundAfterA = betAlreadyInvestedInCurrentSubRound + betDt;

		if (isCanToRaise) {
			if (actionTip.isRaise) {
				double betRaiseTemp = 0;
				for(int i = 1; i <= patternCurrent.betMaxCallOrRaise; i++) {
					betRaiseTemp = i * Settings.betCurrentMultiplier;
					double betInvestedAfterA = this.betAlreadyInvestedInCurrentSubRound + betRaiseTemp + betDt;
					double balanceAfterA = this.betTotal - betDt - betRaiseTemp;
					if (betInvestedAfterA <= game.state.betMax && balanceAfterA >= 0) {
						patternCurrent.betRaise = betRaiseTemp;
						patternCurrent.betToStayInGame = patternCurrent.betRaise + patternCurrent.betCall;
						betDt = patternCurrent.betCall + patternCurrent.betRaise;
						isCanToRaise = true;
						break;
					} else { //if (betInvestedAfterA > game.state.betMax || balanceAfterA < 0 || (betInvestedAfterA > game.state.betMax && balanceAfterA < 0)) {
						isCanToRaise = false;
					}
				}
				patternCurrent.betRaise = betRaiseTemp;
			}
		}

		if (!isCanToRaise) {
			// choosing between all available actions except raise
			if (actionTip.isRaise)
				actionCurrentString = GetAndSetActionTipByName (patternCurrent.actionPriority1, patternCurrent.betCall);
			if (actionTip.isRaise)
				GetAndSetActionTipByName (patternCurrent.actionPriority2, patternCurrent.betCall);
			if (actionTip.isRaise)
				GetAndSetActionTipByName (patternCurrent.actionDefault, patternCurrent.betCall);
			if (string.IsNullOrEmpty(actionCurrentString))
				GetAndSetActionTipByName ("CALL", patternCurrent.betCall);
			if (string.IsNullOrEmpty(actionCurrentString))
				GetAndSetActionTipByName ("CHECK", patternCurrent.betCall);
			if (string.IsNullOrEmpty(actionCurrentString))
				GetAndSetActionTipByName ("FOLD", patternCurrent.betCall);
		}

		betTotalAfterAction = betTotal - betDt;
		betTotalSubRoundAfterA = betAlreadyInvestedInCurrentSubRound + betDt;
		if (betTotalSubRoundAfterA <= game.state.betMax) {
			if (betTotalSubRoundAfterA > game.state.betMaxToStayInGame) {
				game.state.betMaxToStayInGame = betTotalSubRoundAfterA; // max bet to stay in the game
			}
		}

		if (betTotalSubRoundAfterA > game.state.betMax) { // exceed bet max limit (allow only call or check)
			if (actionTip.isRaise || patternCurrent.betRaise > 0) { // raise action
				if (betAlreadyInvestedInCurrentSubRound + patternCurrent.betCall > game.state.betMax) { // allow check
					actionFinal = new Check(this, 0);
					return actionFinal;
				} else if (betAlreadyInvestedInCurrentSubRound + patternCurrent.betCall <= game.state.betMax) { // allow call
					actionFinal = new Call(this, patternCurrent.betCall);
					return actionFinal;
				}
			}
			if (betTotal < 0) {
				actionFinal = new Fold(this, 0);
			} else {
				if (betAlreadyInvestedInCurrentSubRound < game.state.betMaxToStayInGame) {
					actionFinal = new Call(this, patternCurrent.betCall);
				} else {
					if (game.state.betMaxToStayInGame > 0) {
						actionFinal = new Call(this, patternCurrent.betCall);
					} else {
						actionFinal = new Check(this, 0);
					}
				}
			}
			return actionFinal;
//			if (betTotalSubRoundAfterA < game.state.betMaxToStayInGame) { // call
//				actionFinal = new Call(this, game.state.betMaxToStayInGame);
//			} else if (betTotalSubRoundAfterA == game.state.betMaxToStayInGame) { // check
//				actionFinal = new Check(this, 0);
//			} else if (betTotalSubRoundAfterA > game.state.betMaxToStayInGame) { // raise
//
//			}

		}

//		actionFinal = ActionMath(betDt, betTotalAfterAction, isCanToRaise);
		actionFinal = ActionOptimal (game, betTotalAfterAction, isCanToRaise);

		if (actionFinal == null)
			Debug.LogError ("error: actionFinal is null");

		return actionFinal;
	}

	public Action ActionMath(double betDt, double betTotalAfterAction, bool isCanToRaise) {
		if (actionTip.isFold) {
			actionFinal = new Fold (this, betDt);
		} else if (actionTip.isCheck) {
			actionFinal = new Check (this, betDt);
		} else if (actionTip.isCall) {
			actionFinal = new Call (this, betDt);
		} else if (actionTip.isRaise) {
			actionFinal = new Raise (this, betDt);
		} else if (actionTip.isAllIn) {
			actionFinal = new AllIn(this, betDt);
		}
		return actionFinal;
	}

	public Action ActionOptimal(Game game, double betTotalAfterAction_, bool isCanToRaise) {
		double betStayTotal = game.state.betMaxToStayInGame;
		double betStay2 = patternCurrent.betToStayInGame;

		double betStay = Math.Abs( betAlreadyInvestedInCurrentSubRound - betStayTotal );
		if (patternCurrent.betCall <= 0)
			patternCurrent.betCall = betStay; 

		double betWithRaise = patternCurrent.betCall + patternCurrent.betRaise;
		double betCall = patternCurrent.betCall;

		double betTotalAfterAction = betTotal - betWithRaise;
		double betTotalSubRoundAfterA = betAlreadyInvestedInCurrentSubRound + betWithRaise;

		if (betTotal < betStayTotal) { // call/raise
//			if (betTotalAfterAction > betStay && betTotalAfterAction < bet
//			GetAndSetActionTipByName("CALL", betStayTotal);
		} else if (betTotal == betStayTotal) { // check
//			GetAndSetActionTipByName("CHECK", betStayTotal);
		}

		if (actionTip.isFold) {
			if (betCall == 0) {
				actionFinal = new Check (this, betCall);
			} else {
				actionFinal = new Fold (this, betCall);
			}
		} else if (actionTip.isCheck) {
			if (betCall == 0) {
				actionFinal = new Check (this, betCall);
			} else {
				actionFinal = new Fold (this, betCall);
			}
		} else if (actionTip.isCall) {
			if (betTotalAfterAction < 0) {
				if (betCall == 0) {
					actionFinal = new Check (this, betCall);
				} else {
					actionFinal = new Fold (this, betCall);
				}
			} else {
				actionFinal = new Call (this, betCall);
			}
		} else if (actionTip.isRaise) {
			if (betTotalAfterAction < 0) {
				if (betCall == 0) {
					actionFinal = new Check (this, betCall);
					//				} else if (betDt > 0) {
					//					actionFinal = new Call (this, betDt); //TODO
				} else {
					actionFinal = new Fold (this, betCall);
				}
			} else {
				actionFinal = new Raise (this, betWithRaise);
			}
		} else if (actionTip.isAllIn) {
			actionFinal = new AllIn(this, betCall);
		}
		return actionFinal;
	}

	#region action optimal 2
	public Action ActionOptimal2(double betDt, double betTotalAfterAction, bool isCanToRaise) {
		// evaluate
		if (isWinner) {
			if (betTotal < 0) {
				actionFinal = new Fold (this, betDt);
			} else {
				if (isCanToRaise) {
					actionFinal = new Raise (this, patternCurrent.betCall + patternCurrent.betRaise);
				} else {
					if (betDt == 0) {
						actionFinal = new Check (this, betDt);
					} else {
						actionFinal = new Call (this, betDt);
					}
				}
			}
			
			if (actionFinal == null)
				Debug.LogError ("error: actionFinal is null");
			
			return actionFinal;
		}
		
		if (actionTip.isFold) {
			if (betDt == 0) {
				actionFinal = new Check (this, betDt);
				//			} else if (isWinner) {
				//				actionFinal = new AllIn (this, betDt);
			} else {
				actionFinal = new Fold (this, betDt);
			}
		} else if (actionTip.isCheck) {
			if (betDt == 0) {
				actionFinal = new Check (this, betDt);
			} else {
				actionFinal = new Fold (this, betDt);
			}
		} else if (actionTip.isCall) {
			if (betTotalAfterAction < 0) {
				if (betDt == 0) {
					actionFinal = new Check (this, betDt);
				} else {
					actionFinal = new Fold (this, betDt);
				}
			} else {
				actionFinal = new Call (this, betDt);
			}
		} else if (actionTip.isRaise) {
			if (betTotalAfterAction < 0) {
				if (betDt == 0) {
					actionFinal = new Check (this, betDt);
					//				} else if (betDt > 0) {
					//					actionFinal = new Call (this, betDt); //TODO
				} else {
					actionFinal = new Fold (this, betDt);
				}
			} else {
				actionFinal = new Raise (this, patternCurrent.betCall + patternCurrent.betRaise);
			}
		} else if (actionTip.isAllIn) {
			actionFinal = new AllIn(this, betDt);
		}
		
		/*
		if (betTotalAfterAction < 0) { //fold
			if (betTotal > 0) {
				if (isWinner) {
					actionFinal = new AllIn (this, betDt);
					return actionFinal;
				}
			}
			actionFinal = new Fold (this, betDt);
		} else if (betTotalAfterAction > 0) { //call or raise
			if (betTotalSubRoundAfterA > betMax) {
				actionFinal = new Raise (this, betDt);
			} else {
				actionFinal = new Call (this, betDt);
			}
		} else if (betTotalAfterAction == 0) { //check
			actionFinal = new Check (this, betDt);
		}*/
		
		/*
		if (betTotalAfterAction < 0) {
			if (betTotal >= 0 && isWinner) {
				if (Settings.isDev) actionCurrentString += "> ALL IN (w)"; else actionCurrentString = "ALL IN (w)";
				actionFinal = new AllIn(this, betDt);
//				game.state = new AllInRound(game, this, betDt);
			} else {
				if (Settings.isDev) actionCurrentString += "> FOLD"; else actionCurrentString = "FOLD";
				actionFinal = new Fold (this, betDt);
			}
			return actionFinal;
		} else
		if (actionTip.isRaise) {
			if (betTotalSubRoundAfterA > betMax) {
				actionFinal = new Raise (this, betDt);
			} else if (betTotalSubRoundAfterA == betMax) {
				if (Settings.isDev) actionCurrentString += "> CALL"; else actionCurrentString = "CALL";
				actionFinal = new Call (this, betDt);
			}
		} else if (actionTip.isCall) {
			if (betDt == 0) {
				if (Settings.isDev) actionCurrentString += "> CHECK"; else actionCurrentString = "CHECK";
				actionFinal = new Check (this, betDt);
			} else {
				actionFinal = new Call (this, betDt);
			}
		} else if (actionTip.isCheck) {
			actionFinal = new Check (this, betDt);
		} else if (actionTip.isAllIn) {
			actionFinal = new AllIn (this, betDt);
//			game.state = new AllInRound(game, this, betDt);
		} else if (actionTip.isFold) {
			if (isWinner) {
				if (betTotalSubRoundAfterA == betMax) {
					if (betDt == 0) {
						if (Settings.isDev) actionCurrentString += "> CHECK (w)"; else actionCurrentString = "CHECK (w)";
						actionFinal = new Check (this, betDt);
					} else {
						if (Settings.isDev) actionCurrentString += "> CALL (w)"; else actionCurrentString = "CALL (w)";
						actionFinal = new Call (this, betDt);
					}
				} else if (betTotalSubRoundAfterA > betMax) {
					if (Settings.isDev) actionCurrentString += "> RAISE (w)"; else actionCurrentString = "RAISE (w)";
					actionFinal = new Raise (this, betDt);
				} else if (betTotalSubRoundAfterA < betMax) {
					if (Settings.isDev) actionCurrentString += "> ALL IN (w)"; else actionCurrentString = "ALL IN (w)";
					actionFinal = new AllIn (this, betDt);
//					game.state = new AllInRound(game, this, betDt);
				}
			} else if (betDt == 0) {
				if (Settings.isDev) actionCurrentString += "> CHECK"; else actionCurrentString = "CHECK";
				actionFinal = new Check (this, betDt);
			} else {
				actionFinal = new Fold (this, betDt);
			}
		}
		*/

		return actionFinal;
	}
	#endregion action optimal 2

	public string GetRecommendActionStringFromCurrentPattern(double betToStayInGameTotal, double betTotalInSubRound) {
		if (betToStayInGameTotal != 0) betToStayInGameTotal /= Settings.betCurrentMultiplier;
		if (betTotalInSubRound != 0) betTotalInSubRound /= Settings.betCurrentMultiplier;
		
		string actionString = "";
		
		//TODO
		// is in bet sub rounds?
		
		// is in priority 1 (1,2,3)?
		// is in priority 2 ?
		// is in default?
		
		if (patternCurrent.betCall != 0) patternCurrent.betCall *= Settings.betCurrentMultiplier;
		if (betToStayInGameTotal != 0) betToStayInGameTotal *= Settings.betCurrentMultiplier;
		if (betTotalInSubRound != 0) betTotalInSubRound *= Settings.betCurrentMultiplier;
		
		return actionString;
	}

	public string GetCurrentActionStringFromCurrentPattern(double betToStayInGameTotal, double betTotalInSubRound) {
		if (betToStayInGameTotal != 0) betToStayInGameTotal /= Settings.betCurrentMultiplier;
		if (betTotalInSubRound != 0) betTotalInSubRound /= Settings.betCurrentMultiplier;

		string actionString = "";
		if (patternCurrent != null) {
			if (patternCurrent.betSubRounds != null && patternCurrent.betSubRounds.Count > 0) {
				foreach (var betRound in patternCurrent.betSubRounds) {
					if (betRound.costBet == betToStayInGameTotal && betRound.costBetTotal == betTotalInSubRound) {
						patternCurrent.betCall = betRound.costBet - betRound.costBetTotal;
						actionString = betRound.name_action;
						break;
					}
				}
			}
			if (string.IsNullOrEmpty (actionString)) {
				if (!actionString.Contains("OPEN")) { // unknown action
					actionString = GetAndSetActionTipByName (patternCurrent.actionPriority1, patternCurrent.betCall);
					actionString = EvaluateRaise(actionString);
				}
			}
			if (string.IsNullOrEmpty (actionString)) {
				actionString = GetAndSetActionTipByName (patternCurrent.actionPriority2, patternCurrent.betCall);
				actionString = EvaluateRaise(actionString);
			}
			if (patternCurrent != null) {
				if (string.IsNullOrEmpty (actionString)) {
					actionString = GetAndSetActionTipByName(patternCurrent.actionDefault, patternCurrent.betCall);
					actionString = EvaluateRaise(actionString);
				}
			}
		}

//		if (pattern != null)
//			if (string.IsNullOrEmpty(action)) action = pattern.actionDefault;

		if (patternCurrent.betCall != 0) patternCurrent.betCall *= Settings.betCurrentMultiplier;
		if (betToStayInGameTotal != 0) betToStayInGameTotal *= Settings.betCurrentMultiplier;
		if (betTotalInSubRound != 0) betTotalInSubRound *= Settings.betCurrentMultiplier;

		if (string.IsNullOrEmpty (actionCurrentString)) {
			Debug.LogWarning ("actionCurrentString is empty patternCurrent.name:" + patternCurrent.name);
		}

		return actionString;
	}

	private string EvaluateRaise(string actionString) {
		return actionString;//TODO will remove
		if (actionTip.isRaise) {
			// is raise possible?
			double max = patternCurrent.betMaxCallOrRaise;
			while(max > 0) {
				double betTemp = this.betAlreadyInvestedInCurrentSubRound + patternCurrent.betCall + max;
				if (betTemp <= Settings.betMaxMath) {
					patternCurrent.betCall = betTemp;
					break;
				} else { //can't raise
					actionString = "";
				}
				max--;
			}
			// is call possible?
			if (string.IsNullOrEmpty(actionString)) {
				if (this.betAlreadyInvestedInCurrentSubRound + patternCurrent.betCall > Settings.betMaxMath) { //can't call
					actionString = "";
				} else {
					actionString = GetAndSetActionTipByName ("CALL", patternCurrent.betCall);
				}
			}
			// is check possible?
			if (string.IsNullOrEmpty(actionString)) {
				if (this.betAlreadyInvestedInCurrentSubRound == Settings.betMaxMath || Settings.betMaxMath == 0) {
					actionString = GetAndSetActionTipByName ("CHECK", patternCurrent.betCall);
				}
			}
			
		}

		return actionString;
	}
	
	private string GetAndSetActionTipByName(string action, double betToStayInGame) {
		string actionFinalString = "";

		actionTip = new ActionTip(betToStayInGame);

		if (action == "CALL") {
			actionTip.isCall = true;
			actionFinalString = action;
		} else if (action == "CHECK") {
			actionTip.isCheck = true;
			actionFinalString = action;
		} else if (action == "RAISE") {
			actionTip.isRaise = true;
			actionFinalString = action;
		} else if (action == "FOLD") {
			actionTip.isFold = true;
			actionFinalString = action;
		} 
		/*else if (action == "OPEN") {
			actionTip.isAllIn = true;
			actionFinalString = action;
		}*/
		actionTip.name = action;
		return actionFinalString;
	}

	public string GetHandPreflopString() {
		handPreflopString = "";
		handPreflopStringReversed = "";
		bool isSuited = false;
		if (hand.Count() >= Settings.playerHandSizePreflop) {
			if (hand.getCard(0).getSuit() == hand.getCard(1).getSuit()) {
				isSuited = true;
			}
			handPreflopString += Card.rankToMathString(hand.getCard(0).getRank());
			handPreflopString += Card.rankToMathString(hand.getCard(1).getRank());
			handPreflopStringReversed += Card.rankToMathString(hand.getCard(1).getRank());
			handPreflopStringReversed += Card.rankToMathString(hand.getCard(0).getRank());
			if (hand.getCard(0).getRank() == hand.getCard(1).getRank()) {
			}
			else if (isSuited) {
				handPreflopString += "s";
				handPreflopStringReversed += "s";
			} else {
				handPreflopString += "o";
				handPreflopStringReversed += "o";
			}
		}
		return handPreflopString;
	}

	public Hand GetBestPlayerHand (List<Card> cards)
	{
		var count = 20;
		this.hands = new List<Hand> ();
		
		for (int x = 0; x <= count; x++) {//iterate through all possible 5 card hands
			var hand = new Hand();
			switch (x) {
			case 0:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
			
				break;
			case 1:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
			
				break;
			case 2:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			
				break;
			case 3:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			
				break;
			case 4:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			
				break;
			case 5:
			
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			
				break;
			case 6:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [4]);
			
				break;
			case 7:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			
				break;
			case 8:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			
				break;
			case 9:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			
				break;
			case 10:
			
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			
				break;
			case 11:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			case 12:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [1]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			case 13:

				hand.Add (this.hand.getCard(0));
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			case 14:
			
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			case 15:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			case 16:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			case 17:
			
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			case 18:
			
				hand.Add (this.hand.getCard(0));
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			case 19:
			
				hand.Add (this.hand.getCard(1));
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			case 20:
			
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			
				break;
			}
			if (hand.Count() == 5) {
				hand = HandCombination.getBestHand(hand);
				this.hands.Add (hand);
			} else {
				Debug.LogError("hand must have 5 cards instead of " + hand.Count());
			}
		}
		
		Hand bestHand = this.hands.First();
		foreach (var item in this.hands) {
			if (item > bestHand) {
				bestHand = item;
			}
		}
		return bestHand;
	}

	public void ShowCards (Game game)
	{
		for (int i = 0; i < Settings.playerHandSizePreflop; i++) {
			var card = handPreflop.getCard(i);
			card.FaceUp = true;
		}
	}

	public string GetHandStringFromHandObj() {
		string handWinBestString = HandCombination.GetHandStringByHandObj (this.hand);
		return handWinBestString;
	}

	public double winPercent;
	public ActionTip actionTip;
	public Action actionFinal;

	public bool isReal;
	public bool isFolded;
	public bool isWinner;
	public bool isAllIn;

	public bool isFirstToAct;
	public bool isLastToAct;

	public double betAlreadyInvestedInCurrentSubRound;

//	public double credits; // credits/creditMultiplier
	public double betTotal;	// betTotal * creditMulitplier

	public int id;			// for ui
	public int position;	// first to act (player after dealer)
	public string name;
	
	public string handPreflopString;			// "AKs"
	public string handPreflopStringReversed;	// "KAs"

	public Hand handPreflop;
	public Hand hand;
	public List<Hand> hands;
	
	public string actionCurrentString;
	public Pattern patternCurrent;
	
	public Pattern pattern;
	public List<Pattern> alt_patterns;
	
	//	public List<PreFlop> preflopBets;
	//	public List<Flop> flopBets;
	//	public List<Turn> turnBets;
	//	public List<River> riverBets;
	
	public Image chip;
	List<Sprite> chipSpriteList;
	
	public Image dealer;
	
	private bool is_dealer;
	public bool isDealer {
		get { return is_dealer; }
		set { 
			is_dealer = value;
			UpdateDealerImage ();
		}
	}
	
	public Text lblCredits;
	public Text lblAction;
	public Text lblName;

}
