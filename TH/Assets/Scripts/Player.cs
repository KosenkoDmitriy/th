using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Player {

	public Player() {
		hand = new Hand ();
		alt_patterns = new List<Pattern> ();
		betInvested = new Bet (0);
	}
	#region print
	public override string ToString ()
	{
		string str = string.Format ("{0} {1} {2}({5}%) {3} {4}", id, name, handPreflopString, balanceInCredits, actionCurrentString, winPercent);
		if (Settings.isDev) {
			LogDevInfo(this, false, false);
		}

		return str;
	}

	public void Log(bool isError, bool isWarning, string str ) {
		Log (str, isWarning, isError);
	}

	public void Log(string str, bool isWarning, bool isError) {
		if (isError)
			Debug.LogError (str);
		else if (isWarning)
			Debug.LogWarning (str);
		else
			Debug.Log (str);
	}

	public void LogDevInfo(Player player, bool isWarning, bool isError) {
		if (Settings.isDev) {
			string str = string.Format ("#{0} {1} pos: {6} hand:{2}({5}% win) bets (credits): invested:{7}/total:{3} cur_action: {4}", player.id, player.name, player.handPreflopString, player.balanceInCredits, player.actionCurrentString, player.winPercent, player.position, player.betInvested.inCredits);
			Log(str, isWarning, isError);

			string str2 = "";
			if (player.actionTip != null) {
				str2 = string.Format ("action tip: {0} ({1}% win): bet to stay: call:{2} raise:{3}(credits)", player.actionTip.name, player.winPercent, player.actionTip.betCall.inBetMath, player.actionTip.betRaise.inCredits);
				Log(str2, isWarning, isError);
			}
			if (player.patternCurrent != null) {
				str2 = string.Format (
					"cur pattern: {0} ({8}% of all time): p2:{1} p3:{2} d:{3}\n {4} {5} {6} max:{7} ",
                      player.patternCurrent.name, player.patternCurrent.actionPriority2, player.patternCurrent.actionPriority3, player.patternCurrent.actionDefault, 
                      "", "", "", player.patternCurrent.betMaxCallOrRaise, player.patternCurrent.percent);
				Log(str2, isWarning, isError);
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
	#endregion print

	#region get pattern and final action
	public Pattern GetPatternRandomly() {
		float percentOfTime = UnityEngine.Random.value * 100;
		Pattern patternCurrent = null;
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

		return patternCurrent;
	}

	#endregion

	#region new actions


	public Action GetFinalAction(Game game) {

		if (game.state.betMax > game.state.betMaxLimit && game.state.betMax >= 0d) {
			game.state.betMax = game.state.betMaxLimit;
			if (Settings.isDev) Log(true, false, string.Format("betMax = betMaxLimit because betMax {0} exceed betMaxLimit {1}", game.state.betMax, game.state.betMaxLimit) );
		}

		Bet betMax = game.state.betMax;
		Bet betMaxLimit = game.state.betMaxLimit;

		patternCurrent = GetPatternRandomly ();
		var actionTipTemp = GetActionRecommendInSubrounds (game); // action with priority 1

		if (actionTipTemp == null) {
			// actions with priority 2-4
			var actionNames = new List<String> () {
				patternCurrent.actionPriority2,
				patternCurrent.actionPriority3,
				patternCurrent.actionDefault,
			};
			foreach (var actionName in actionNames) {
				actionTipTemp = GetActionRecommendByName (game, actionName);
				if (actionTipTemp != null) { // optimal action found
					break;
				}
			}
			
//			if (actionTipTemp == null) {
//				actionTipTemp = GetActionRecommendByName (game, patternCurrent.actionDefault);
//			}

		}


		if (betMax >= 0 && betMaxLimit >= 0) {
			if (betMax < betMaxLimit) {	// any action allowed
				if (betMax == betMaxLimit) {	// can't raise
					game.state.isCanToRaise = false;
				}

				if (betInvested < betMax) {	// call (required) or raise (optional)
//					actionFinal = ActionMath(actionTipTemp, 0, isCanToRaise);
					// find action required in math tip/recommend actions
				} else if (betInvested == betMax) {	// check required
//					actionFinal = ActionMath(actionTipTemp, 0, isCanToRaise);
					// find action required in math tip/recommend actions
				} else if (betInvested > betMax) {	// choose another possible action (decrease bet/raise amount)
//					actionFinal = ActionMath(actionTipTemp, 0, isCanToRaise);

					if (Settings.isDev) Log(true, false, string.Format("betInvested {0} > betMax {1}", betInvested, betMax) );
				}
			} else if (betMax > betMaxLimit) { //
				if (Settings.isDev) Log(true, false, string.Format("betMax {0} exceed betMaxLimit {1}", betMax, betMaxLimit) );
			}
		} else {
			if (Settings.isDev) Log(true, false, string.Format("betMax {0} and betMaxLimit {1} can't be negative", betMax, betMaxLimit));
		}

		// final action
		actionFinal = GetActionReal(actionTipTemp, game.state.betMax, game.state.betMaxLimit, game.state.isCanToRaise);
//		actionFinal = GetActionMath(actionTipTemp, game.state.isCanToRaise);

		if (actionFinal == null) {
			Log(true, false, "actionFinal is null > fold");
			actionFinal = new Fold (this, new Bet(0), new Bet(0));
		}

		return actionFinal;
	}

	/*
	 * set call and raise amounts
	*/
	public ActionTip GetOptimalActionTip(Game game, ActionTip actionT) {
		double maxPossibleRaise = game.state.betMaxLimit.inBetMath - game.state.betMax.inBetMath;
		if (maxPossibleRaise < 0) maxPossibleRaise = 0;
		double betToStay = game.state.betMax.inBetMath;
		ActionTip actionIemp = null;
		if (actionT.isUnknown) {
			actionT = null; // will choose another action
		} else if (actionT.isCall) {
			if (betToStay >= 0 && betToStay <= patternCurrent.betMaxCallOrRaise) {
//				double betForRaise = patternCurrent.betMaxCallOrRaise - betToStay;
//				if (betForRaise > 0 && betForRaise <= maxPossibleRaise) {
//				}
				if (betToStay == betInvested.inBetMath) { // check
					actionT.isCheck = true;
				} else { // call
					actionT.betCall.inBetMath = betToStay;
				}
				actionIemp = actionT;
			}
		} else if (actionT.isRaise) { // raise, call or check
			if (game.state.isCanToRaise) {
				if (betToStay >= 0 && betToStay <= patternCurrent.betMaxCallOrRaise) {
					double betForRaise = patternCurrent.betMaxCallOrRaise - betToStay;
					if (betForRaise <= patternCurrent.betMaxCallOrRaise) {
						if (betForRaise > 0 && betForRaise <= maxPossibleRaise) {
							actionT.betRaise.inBetMath = betForRaise;
							actionT.betCall.inBetMath = betToStay;

							actionIemp = actionT;
						}
					}
				}
			}
		} else if (actionT.isCheck) { // check
			if (betToStay == betInvested.inBetMath) {
				actionT.isCheck = true;
				actionIemp = actionT;
			}
		} else if (actionT.isFold) { // fold
			actionT.isFold = true;
			actionIemp = actionT;
		}

		return actionIemp;
	}

	public ActionTip GetActionRecommendInSubrounds(Game game) {
		double betMaxToStayInGameTotal = game.state.betMax.inBetMath;
		double betAlreadyInvestedInMath = betInvested.inBetMath;
		ActionTip actionT = null;

		// is in bet sub rounds?
		if (patternCurrent.betSubRounds != null && patternCurrent.betSubRounds.Count > 0) {
			foreach (var betRound in patternCurrent.betSubRounds) {
				if (betMaxToStayInGameTotal == betRound.costBetToStayInGame && betAlreadyInvestedInMath == betRound.costBetAlreadyInvested) {

					actionT = new ActionTip ();
					actionT.isInBetSubrounds = true;
					actionT.name = betRound.name_action;

//					var dt = betRound.costBetToStayInGame - betRound.costBetAlreadyInvested;
					actionT = GetOptimalActionTip(game, actionT);

					break;
				}
			}
		}
		
		return actionT;
	}

	public ActionTip GetActionRecommendByName(Game game, string name) {

		ActionTip actionT = new ActionTip ();
		actionT.name = name;
		actionT = GetOptimalActionTip(game, actionT);

		return actionT;
	}

	#endregion new

	#region actions

	public Action GetActionMath(ActionTip actionT, bool isCanToRaise) {
		this.actionTip = actionT;
		if (actionTip.isFold) {
			actionFinal = new Fold (this, actionTip.betCall, actionTip.betRaise);
		} else if (actionTip.isCheck) {
			actionFinal = new Check (this, actionTip.betCall, actionTip.betRaise);
		} else if (actionTip.isCall) {
			actionFinal = new Call (this, actionTip.betCall, actionTip.betRaise);
		} else if (actionTip.isRaise) {
			actionFinal = new Raise (this, actionTip.betCall, actionTip.betRaise);
		} else if (actionTip.isAllIn) {
			actionFinal = new AllIn(this, actionTip.betCall, actionTip.betRaise);
		}
		return actionFinal;
	}

	public Action GetActionReal(ActionTip actionT, Bet betMax, Bet betMaxLimit, bool isCanToRaise) {
		var betPossibleMaxRaiseOrCall = betMaxLimit - betMax;		
//		var balanceInCreditsAfterAction = this.balanceInCredits - betMax.inCredits;
		var betInvestedAfterAction = betInvested + betMax;

		if (actionT == null) {
			actionT = new ActionTip();
			actionT.name = patternCurrent.actionDefault; // default action
			actionFinal = new Check (this, actionT.betCall, actionT.betRaise);
		}

		this.actionTip = actionT;
		if (actionTip.isFold) {
			actionFinal = new Fold (this, actionTip.betCall, actionTip.betRaise);
		} else if (actionTip.isCheck) {
			actionFinal = new Check (this, actionTip.betCall, actionTip.betRaise);
		} else if (actionTip.isCall) {
			actionFinal = new Call (this, actionTip.betCall, actionTip.betRaise);
		} else if (actionTip.isRaise) {

			if (actionTip.betToStay.inBetMath > patternCurrent.betMaxCallOrRaise) { // > call
				actionTip.isCall = true;
				actionTip.betRaise = new Bet (0);

				if (actionTip.betToStay.inBetMath > patternCurrent.betMaxCallOrRaise) { // > check
					actionTip.isCheck = true;
					actionTip.betCall = new Bet (0);
					actionFinal = new Check (this, actionTip.betCall, actionTip.betRaise);
				} else { // call
					actionFinal = new Call (this, actionTip.betCall, actionTip.betRaise);
				}
			} else {
				actionFinal = new Raise (this, actionTip.betCall, actionTip.betRaise);
			}
		} else if (actionTip.isAllIn) {
			actionFinal = new AllIn (this, actionTip.betCall, actionTip.betRaise);
		}

			
		// real actions
		if (betInvestedAfterAction == betInvested) { // > check
			actionTip.isCheck = true;
			actionFinal = new Check (this, actionTip.betCall, actionTip.betRaise);
		} else if (betInvestedAfterAction > betInvested && isWinner) { // > call
			if (betInvestedAfterAction <= betMaxLimit && isWinner) {
				actionFinal = new Call (this, betMax, new Bet(0));
			}
		}

//		if (isCanToRaise && isWinner) { // raise if winner
//			var maxRaise = betMaxLimit - betMax;
////			var rand = new System.Random();
////			maxRaise.inBetMath = (double)rand.Next(1, (int)maxRaise.inBetMath + 1);
//			if (maxRaise > 0d) {
//				actionFinal = new Raise (this, betMax, maxRaise);
//			}
//		}

		if (balanceInCredits < 0 || betInvestedAfterAction < 0) { // > fold
			actionTip.isFold = true;
			actionFinal = new Fold (this, actionTip.betCall, actionTip.betRaise);
		}

		this.actionTip = actionT;
		return actionFinal;
	}
	#endregion actions

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

	public Bet betInvested; // already invested in current subround

//	public double credits; // credits/creditMultiplier
	public double balanceInCredits;	// *= creditMulitplier

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
