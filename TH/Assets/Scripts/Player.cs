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

	public Pattern GetAndSetPatternRandomly() {
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

	public Action GetFinalAction(double betToStayInGame, double betAlreadyInvested) {
		// get pattern randomly
		// preferred/recommend action from the pattern
		// final optimal correct actual action
		patternCurrent = GetAndSetPatternRandomly ();
		actionCurrentString = GetCurrentActionString (betToStayInGame, betAlreadyInvested); // best actionString from the patternCurrent
		GetAndSetActionTip (actionCurrentString, betToStayInGame); // set actionTip get actionTipString (recommend action)

		Action actionFinal = null;
		double creditsAfterAction = betTotal - betAlreadyInvested - betToStayInGame; // betAlreadyInvested == betTotal
		int betMaxCallOrRaiseInMathBets = patternCurrent.betMaxCallOrRaise;
		//2
		double betDt = betToStayInGame - betAlreadyInvested;
//		if (betMaxCallOrRaiseInMathBets <= betDt) {
//			// raise
//		} else {
//
//		}
		if (actionTip.isRaise) {
			actionFinal = new Raise(this, betToStayInGame);
		} else if (actionTip.isCall) {
			if (creditsAfterAction < 0) {
				actionFinal = new Fold(this, betToStayInGame);
			} else if (creditsAfterAction >= 0) {
				if (betAlreadyInvested == betToStayInGame) {
					actionFinal = new Check(this, betToStayInGame);
				} else {
					actionFinal = new Call(this, betToStayInGame);
				}
			}
		} else if (actionTip.isCheck) {
			if (creditsAfterAction < 0) {
				actionFinal = new Fold(this, betToStayInGame);
			} else if (creditsAfterAction >= 0) {
				if (betAlreadyInvested == betToStayInGame) {
					actionFinal = new Check(this, betToStayInGame);
				} else {
					actionFinal = new Call(this, betToStayInGame);
				}
			}
		} else if (actionTip.isFold) {
			if (creditsAfterAction < 0) {
				actionFinal = new Fold(this, betToStayInGame);
			} else if (creditsAfterAction >= 0) {
				if (betAlreadyInvested == betToStayInGame) {
					actionFinal = new Check(this, betToStayInGame);
				} else {
					actionFinal = new Call(this, betToStayInGame);
				}
			}
		} else if (actionTip.isRaise) {
			if (creditsAfterAction < 0) {
				actionFinal = new Fold(this, betToStayInGame);
			} else if (creditsAfterAction >= 0) {
				if (betMaxCallOrRaiseInMathBets <= betDt) {
					actionFinal = new Raise(this, betToStayInGame);
				} else if (betAlreadyInvested == betToStayInGame) {
					actionFinal = new Check(this, betToStayInGame);
				} else {
					actionFinal = new Call(this, betToStayInGame);
				}
			}
		}
		if (actionFinal == null)
			Debug.LogError ("error: actionFinal is null");

		return actionFinal;
	}

	public string GetCurrentActionString(double betToStayInGame, double betTotal) {
		string actionString = "";
		if (patternCurrent != null) {
			if (patternCurrent.betSubRounds != null && patternCurrent.betSubRounds.Count > 0)
				foreach (var betRound in patternCurrent.betSubRounds) {
					if (betRound.costBet * Settings.betDxInCredits == betToStayInGame && betRound.costBetTotal * Settings.betDxInCredits == betTotal) {
						actionString = betRound.name_action;
					break;
					}
				}
			if (string.IsNullOrEmpty (actionString))
				actionString = GetAndSetActionTip (patternCurrent.actionPreffered2, betToStayInGame);
			if (string.IsNullOrEmpty (actionString))
				actionString = GetAndSetActionTip (patternCurrent.actionPreffered1, betToStayInGame);
			if (patternCurrent != null)
			if (string.IsNullOrEmpty (actionString))
				actionString = patternCurrent.actionDefault;
		}
//		if (pattern != null)
//			if (string.IsNullOrEmpty(action)) action = pattern.actionDefault;

		// final action
//		actionFinal = GetFinalAction(betToStayInGame, betTotal);
		return actionString;
	}
	
	private string GetAndSetActionTip(string action, double betToStayInGame) {
		// TODO: maxBet for call and raise
		string res = "";
		actionTip = new ActionTip(this, betToStayInGame);
		double amount = betTotal - betToStayInGame;
		if (action == "CALL") {
			if (amount >= 0) {
				actionTip.isCall = true;
				res = action;
			}
		} else if (action == "CHECK") {
			if (amount >= 0) {
				actionTip.isCheck = true;
				res = action;
			}
		} else if (action == "RAISE") {
			if (amount >= 0) {
				actionTip.isRaise = true;
				res = action;
			}
		} else if (action == "FOLD") {
			if (amount < 0) {
				actionTip.isFold = true;
				res = action;
			}
		}
		return res;
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
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
			}
				break;
			case 1:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
			}
				break;
			case 2:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			}
				break;
			case 3:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			}
				break;
			case 4:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			}
				break;
			case 5:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
			}
				break;
			case 6:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [4]);
			}
				break;
			case 7:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			}
				break;
			case 8:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			}
				break;
			case 9:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			}
				break;
			case 10:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [4]);
			}
				break;
			case 11:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 12:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 13:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 14:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 15:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 16:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 17:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [0]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 18:
			{
				hand.Add (this.hand.getCards() [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 19:
			{
				hand.Add (this.hand.getCards() [1]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			case 20:
			{
				hand.Add (cards [0]);
				hand.Add (cards [1]);
				hand.Add (cards [2]);
				hand.Add (cards [3]);
				hand.Add (cards [4]);
			}
				break;
			}
			hand = HandCombination.getBestHand(hand); 
			this.hands.Add (hand);
		}
		
		Hand bestHand = this.hands.First();
		foreach (var item in this.hands) {
			if (item > bestHand) {
				bestHand = item;
			}
		}
		return bestHand;
	}


	public ActionTip actionTip;
	public Action actionFinal;
	//	public int currentBetRoundNo;
	public bool isReal;
	
	public double bet;
	public double betAlreadyInvestedBeforeAction;

	public double credits; // credits/creditMultiplier
	public double betTotal;	// betTotal * creditMulitplier

	public int id;			// for ui
	public int position;	// first to act (player after dealer)
	public string name;
	
	public string handPreflopString;			// "AKs"
	public string handPreflopStringReversed;	// "KAs"
	
	public Hand handPreflop;
	public Hand hand;
	public List<Hand> hands;
	
	//	bool isActive = true;
	public bool isFolded;
	
	
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
