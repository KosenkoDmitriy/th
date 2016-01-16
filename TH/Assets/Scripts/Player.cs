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
		return string.Format ("{0} {1} {2} {3} {4}", id, name, handPreflopString, betTotal, actionCurrentString);
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

	public Action GetFinalAction(double betMax, bool isCanToRaise) {
		// summary:
		// get pattern randomly
		// preferred/recommend action from the pattern
		// final optimal correct actual action

		patternCurrent = GetAndSetCurrentPatternRandomly ();
		actionCurrentString = GetCurrentActionStringFromCurrentPattern (betMax, betAlreadyInvestedInCurrentSubRound); // best actionString from the patternCurrent
		actionCurrentString = GetAndSetActionTipByName (actionCurrentString, patternCurrent.betDt); // set actionTip get actionTipString (recommend action)

		Action actionFinal = new Action();
		double betDt = patternCurrent.betDt; // betMax - betAlreadyInvestedInCurrentSubRound;
		if (betDt < 0) {
			Debug.LogWarning("betToStayInGame should be > 0 but:" + betDt);
			betDt = 0;
		}

		double creditsAfterAction = betTotal - betDt;

		if (actionTip.isRaise) {
			actionFinal = new Raise (this, betDt);
		} else if (actionTip.isCall) {
			actionFinal = new Call (this, betDt);
		} else if (actionTip.isCheck) {
			actionFinal = new Check (this, betDt);
		} else if (actionTip.isFold) {
			if (isWinner) {
				actionCurrentString += "> CHECK";
				actionFinal = new Check (this, betDt);
			} else {
				isFolded = true;
				actionFinal = new Fold (this, betDt);
			}
		}
		/*
		if (!isCanToRaise) {
			// choosing between all available actions except raise
			if (actionTip.isRaise)
				GetAndSetActionTipByName (patternCurrent.actionPriority1, patternCurrent.betDx);
			if (actionTip.isRaise)
				GetAndSetActionTipByName (patternCurrent.actionPriority2, patternCurrent.betDx);
			if (actionTip.isRaise)
				GetAndSetActionTipByName (patternCurrent.actionDefault, patternCurrent.betDx);
		}

		Action actionFinal = new Action();

		int betMaxCallOrRaiseInMathBets = patternCurrent.betMaxCallOrRaise;

		double betDt = betToStayInGame - betAlreadyInvestedInCurrentSubRound; //patternCurrent.betMaxCallOrRaise;

		if (betDt < 0) betDt = 0;
		double creditsAfterAction = betTotal - betDt;
		if (actionTip.isRaise) {
			actionFinal = new Raise(this, betDt);
		} else if (actionTip.isCall) {
			if (creditsAfterAction < 0) {
				actionFinal = new Fold(this, betDt);
			} else if (creditsAfterAction >= 0) {
				if (betAlreadyInvestedInCurrentSubRound == betToStayInGame) {
					actionFinal = new Check(this, betDt);
				} else {
					actionFinal = new Call(this, betDt);
				}
			}
		} else if (actionTip.isCheck) {
			if (creditsAfterAction < 0) {
				actionFinal = new Fold(this, betDt);
			} else if (creditsAfterAction >= 0) {
				if (betAlreadyInvestedInCurrentSubRound == betToStayInGame) {
					actionFinal = new Check(this, betDt);
				} else {
					actionFinal = new Call(this, betDt);
				}
			}
		} else if (actionTip.isFold) {
			if (creditsAfterAction < 0) {
				actionFinal = new Fold(this, betDt);
			} else if (creditsAfterAction >= 0) {
				if (betAlreadyInvestedInCurrentSubRound == betDt) {
					actionFinal = new Check(this, betDt);
				} else {
					actionFinal = new Call(this, betDt);
				}
			}
		} else if (actionTip.isRaise) {
			if (creditsAfterAction < 0) {
				actionFinal = new Fold(this, betDt);
			} else if (creditsAfterAction >= 0) {
				if (betMaxCallOrRaiseInMathBets <= betDt) {
					actionFinal = new Raise(this, betMaxCallOrRaiseInMathBets);
				} else if (betAlreadyInvestedInCurrentSubRound == betDt) {
					actionFinal = new Check(this, betDt);
				} else {
					actionFinal = new Call(this, betDt);
				}
			}
		}
		*/
		if (actionFinal == null)
			Debug.LogError ("error: actionFinal is null");

		return actionFinal;
	}

	public string GetCurrentActionStringFromCurrentPattern(double betToStayInGameTotal, double betTotalInSubRound) {
		if (betToStayInGameTotal > 0) betToStayInGameTotal /= Settings.betCurrentMultiplier;
		if (betTotalInSubRound > 0) betTotalInSubRound /= Settings.betCurrentMultiplier;

		string actionString = "";
		if (patternCurrent != null) {
			if (patternCurrent.betSubRounds != null && patternCurrent.betSubRounds.Count > 0) {
				foreach (var betRound in patternCurrent.betSubRounds) {
					if (betRound.costBet == betToStayInGameTotal && betRound.costBetTotal == betTotalInSubRound) {
						patternCurrent.betDt = betRound.costBet - betRound.costBetTotal;
						actionString = GetAndSetActionTipByName (patternCurrent.actionPriority1, patternCurrent.betDt);
						actionString = betRound.name_action;
						break;
					}
				}
			}
			if (string.IsNullOrEmpty (actionString)) {
				patternCurrent.betDt = patternCurrent.betMaxCallOrRaise;
				actionString = GetAndSetActionTipByName (patternCurrent.actionPriority1, patternCurrent.betDt);
			}
			if (string.IsNullOrEmpty (actionString)) {
				if (patternCurrent.actionPriority2 != "OPEN") // unknown action
					actionString = GetAndSetActionTipByName (patternCurrent.actionPriority2, patternCurrent.betDt);
			}
			if (patternCurrent != null)
				if (string.IsNullOrEmpty (actionString))
					actionString = GetAndSetActionTipByName(patternCurrent.actionDefault, patternCurrent.betDt);
		}
//		if (pattern != null)
//			if (string.IsNullOrEmpty(action)) action = pattern.actionDefault;
		if (patternCurrent.betDt > 0) patternCurrent.betDt *= Settings.betCurrentMultiplier;
		if (betToStayInGameTotal > 0) betToStayInGameTotal *= Settings.betCurrentMultiplier;
		if (betTotalInSubRound > 0) betTotalInSubRound *= Settings.betCurrentMultiplier;

		actionCurrentString = actionString;

		if (string.IsNullOrEmpty (actionCurrentString)) {
			Debug.LogWarning ("actionCurrentString is empty patternCurrent.name:" + patternCurrent.name);
		}

		return actionString;
	}
	
	private string GetAndSetActionTipByName(string action, double betToStayInGame) {
		string actionFinalString = "";

		actionTip = new ActionTip(this, betToStayInGame);

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
		return actionFinalString;
	}

	public string GetHandPreflopString() {
		handPreflopString = "";
		handPreflopStringReversed = "";
		bool isSuited = false;
		if (hand.getCards().Count >= 2) {
			if (hand.getCards()[0].getSuit() == hand.getCards()[1].getSuit()) {
				isSuited = true;
			}
			handPreflopString += Card.rankToMathString(hand.getCards()[0].getRank());
			handPreflopString += Card.rankToMathString(hand.getCards()[1].getRank());
			handPreflopStringReversed += Card.rankToMathString(hand.getCards()[1].getRank());
			handPreflopStringReversed += Card.rankToMathString(hand.getCards()[0].getRank());
			if (hand.getCards()[0].getRank() == hand.getCards()[1].getRank()) {
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
		for (int i = 0; i < 2; i++) {
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
