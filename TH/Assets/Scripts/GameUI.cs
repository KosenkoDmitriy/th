using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class GameUI : MonoBehaviour
{
	Game game;

	GameUI() {

	}

	#region clear all deprecated
	public void ClearAll() {
		if (Settings.isDebug) DebugLog("ClearAll()");
		var bg = Resources.Load<Sprite> (Settings.cardBg);
		if (game.playerIterator != null)
		for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			player.chip.sprite = bg;
			player.isDealer = false;
			player.lblAction.text = "";
			player.lblCredits.text = "";

			foreach (var card in player.handPreflop.getCards()) {
				card.isHidden = true;
			}
//			// commented because we don't need clear it
//			foreach (var card in player.hand.getCards()) {
//				card.isHidden = true;
//			}
//			foreach(var hand in player.hands)
//			foreach (var card in hand.getCards()) {
//				card.isHidden = true;
//			}
//			// commented because we don't need clear it
		}
		if (game.cards != null)
		foreach (var card in game.cards) {
			card.isHidden = true;
		}
		lblPot.text = Settings.betNull.f();
		lblBet.text = Settings.betNull.f();
		lblBetBonus.text = Settings.betNull.f();
		lblRaise.text = Settings.betNull.f();
		lblCall.text = Settings.betNull.f();
	}
	#endregion clear all deprecated

	#region win panel
	public void btnWinPanelCloseClick()
	{
		ReInitGame ();

		audio.PlayOneShot(soundBtnClicked);

		game.state.isWaiting = false;
	}


	public void ReInitGame () {
		game.state = new InitGame (game);
	}

	#endregion win panel

	#region game panel
	public void btnCheckClick()
	{
		audio.PlayOneShot(soundBtnClicked);

		game.player.actionFinal = new Check(game.player, new Bet(0),  new Bet(0));
		game.player.actionFinal.Do (game, game.player);
	}

	public void btnCallClick()
	{
		audio.PlayOneShot(soundBtnClicked);
		game.player.actionFinal = new Call(game.player, game.state.betMax - game.player.betInvested, new Bet(0));
		game.player.actionFinal.Do (game, game.player);
	}

	public void btnRaiseClick()
	{
		audio.PlayOneShot(soundBtnClicked);
		Settings.betCurrent.inCredits = game.betAmount.inCredits = 0;
		HideDynamicPanels ();
		if (panelInitBet) {
			panelInitBet.SetActive (true);
			inputBetField.text = Settings.betCurrent.inCredits.f();
		}
	}

	public void btnFoldClick()
	{
		audio.PlayOneShot(soundBtnClicked);

		game.state = new InitGame (game);
	}

	public void btnAllInClick()
	{
		audio.PlayOneShot(soundBtnClicked);

		game.player.actionFinal = new AllIn (game.player, game.state.betMax, new Bet(0));
		game.player.actionFinal.Do (game, game.player);

		
		game.ui.HideDynamicPanels();
		game.ui.panelGame.SetActive(true);
		game.ui.btnCall.GetComponent<Button>().interactable = false; 	//.SetActive(false);
		game.ui.btnCheck.GetComponent<Button>().interactable = false;	//.SetActive(false);
		game.ui.btnRaise.GetComponent<Button>().interactable = false;	//.SetActive(false);
		game.ui.btnFold.GetComponent<Button>().interactable = false;	//.SetActive(false);
		game.ui.btnAllIn.GetComponentInChildren<Text>().text = "CONTINUE";
	}

	public void btnHelpClick()
	{
		audio.PlayOneShot(soundBtnClicked);
		
		if (Settings.isDebug) Debug.Log("btnHelpClick()");
		if (panelHelp) panelHelp.SetActive (true);
	}
	
	public void btnHelpCloseClick() {
		audio.PlayOneShot(soundBtnClicked);
		
		if (Settings.isDebug) Debug.Log("btnHelpCloseClick()");
		if (panelHelp) panelHelp.SetActive (false);
	}
	#endregion game panel

	#region bet panel
	public void btnBetNowClick()
	{
		if (Settings.isDebug) Debug.Log("btnBetNowClick()");
		
		audio.PlayOneShot(soundBtnClicked);
		
//		if (panelInitBet) panelInitBet.SetActive(true);
//		if (btnRepeatBet) btnRepeatBet.SetActive(true);
//		if (btnStartGame) btnStartGame.GetComponentInChildren<Text>().text = "Start Game";
//		//if (btnStartGame) btnStartGame.GetComponent<Button>().onClick.Invoke();
//		if (lblPanelBet) lblPanelBet.GetComponent<Text>().text = "PLACE YOUR BET";
		
//		betAmount = 0;
		string betAmountString = "0";
		if (inputBetField)
			betAmountString = inputBetField.text;
		double bet = 0;
		Double.TryParse (betAmountString, out bet);
		game.betAmount.inCredits = bet;

//		if (game.betAmount.inBet > 0) {
//			game.betAmount /= (Settings.betCreditsMultiplier * Settings.betCurrentMultiplier);
//		}

		// from recommend to optimal
		double betTotalAfterAction = game.player.balanceInCredits - game.betAmount.inCredits;
		double betTotalSubRoundAfterA = game.player.betInvested.inCredits + game.betAmount.inCredits;

//		double betRaise = game.state.betMax - game.betAmount.inCredits;

		if (game.isGameRunning) {

			if (betTotalAfterAction > 0) { //call or raise
				if (betTotalSubRoundAfterA > game.state.betMax.inCredits && betTotalSubRoundAfterA <= game.state.betMaxLimit.inCredits) {
					game.player.actionFinal = new Raise (game.player, game.state.betMax, game.betAmount);
				} else {
					game.player.actionFinal = new Call (game.player, game.state.betMax, new Bet(0));
				}
			} else if (betTotalAfterAction == 0) { //check
				game.player.actionFinal = new Check (game.player, new Bet(0), new Bet(0));
			} else if (betTotalAfterAction < 0) { //fold
				//TODO
			}

			if (lblCall) lblCall.text = game.state.betMax.inCredits.f();
			if (lblRaise) lblRaise.text = game.betAmount.inCredits.f();

			game.player.actionFinal.Do (game, game.player);
			if (lblBet) lblBet.text = game.betAmount.inCredits.f();
		} else if (!game.isGameRunning && game.betAmount.inCredits > 0 && betTotalAfterAction >= 0) {
			game.isGameRunning = true;
			game.player.actionFinal = new Raise(game.player, game.state.betMax, Settings.betCurrent);
			game.player.actionFinal.Do (game, game.player);
			if (lblBet) lblBet.text = game.betAmount.inCredits.f();
		} else {
			return;
		}

		if (panelInitBet) panelInitBet.SetActive(false);
		if (panelGame) panelGame.SetActive(true);
	}
	
	public void btnMaxBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnMaxBetClick()");
		
		audio.PlayOneShot(soundBtnClicked);
//		double betMax = (game.state.betMaxLimit - game.state.betMax);// * Settings.betCurrentMultiplier;
		Settings.betCurrent.inBetMath = game.state.betMaxLimit.inBetMath;
		
		string b = Settings.betCurrent.inCredits.f();
		inputBetField.text = b;
		panelInitBet.GetComponentInChildren<InputField>().text = b;
	}
	
	public void btnMinBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnMinBetClick()");
		
		audio.PlayOneShot(soundBtnClicked);
		
		Settings.betCurrent.inBetMath += Settings.betMinMath;// * Settings.betCurrentMultiplier;
//		double betMax = (game.state.betMaxLimit - game.state.betMax);// * Settings.betCurrentMultiplier;
		if (Settings.betCurrent > game.state.betMaxLimit)
			Settings.betCurrent.inBetMath = Settings.betNull;

		inputBetField.text = Settings.betCurrent.inCredits.f();
	}
	
	public void btnClearBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnClearBetClick()");
		
		audio.PlayOneShot(soundBtnClicked);
		
		Settings.betCurrent.inCredits = Settings.betNull;

		inputBetField.text = Settings.betCurrent.inCredits.f();
	}
	
	#endregion bet panel

	public void btnInstructionClick()
	{
		audio.PlayOneShot(soundBtnClicked);

		if (Settings.isDebug) Debug.Log("btnInstructionClick()");
		if (panelInstructions) panelInstructions.SetActive (true);
	}
	
	public void btnInstructionCloseClick() {
		audio.PlayOneShot(soundBtnClicked);
		if (Settings.isDebug) Debug.Log("btnInstructionCloseClick()");
		if (panelInstructions) panelInstructions.SetActive (false);
	}

	#region add credits
	private void btnCreditAddClickListener()
	{
		if (Settings.isDebug) Debug.Log("btnCreditAddClickListener()");
		if (panelAddCredits) panelAddCredits.SetActive(true);
	}
	
	private void btnCreditOkClickListener()
	{
		if (Settings.isDebug) Debug.Log("btnCreditOkClickListener()");
		if (panelAddCredits) panelAddCredits.SetActive(false);
	}
	#endregion add credits

	#region bonus pane/table
	public void btnBetForBonusTableClick() {
		if (Settings.isDebug) Debug.Log("btnBetForBonusTableClick()");
		if (panelBonus) panelBonus.SetActive (true);
	}
	
	public void btnBonusPanelCloseClick() {
		if (Settings.betBonus > 0) {
			if (payTable != null) {
				lblPot.GetComponent<Text>().text = Settings.betBonus.to_b ();
				game.player.balanceInCredits -= Settings.betBonus;
				game.player.lblCredits.text = game.player.balanceInCredits.f();
				if (lblBetBonus) lblBetBonus.text = Settings.betBonus.to_b();
				payTable.SetBet(Settings.betBonus);
			}
		}
		if (panelBonus) panelBonus.SetActive (false);
	}
	
	public void btnBonusBetMinClick() {
		Settings.betBonus += Settings.betBonusMin;
		if (Settings.betBonus > Settings.betBonusMax)
			Settings.betBonus = 0;
		if (payTable != null) payTable.SetBet(Settings.betBonus);
		if (panelBonus) panelBonus.GetComponentInChildren<InputField>().text = Settings.betBonus.to_b();
	}

	public void btnBonusBetMaxClick() 
	{
		Settings.betBonus = Settings.betBonusMax;
		if (payTable != null) payTable.SetBet(Settings.betBonus);
		if (panelBonus) panelBonus.GetComponentInChildren<InputField>().text = Settings.betBonus.to_b();
	}
	
	public void btnBonusBetClearClick() {
		Settings.betBonus = Settings.betNull;
		if (payTable != null) payTable.SetBet(Settings.betBonus);
		if (panelBonus) panelBonus.GetComponentInChildren<InputField>().text = Settings.betBonus.ToString();
	}
	#endregion bonus pane/table

	public void Start ()
	{
		if (Settings.isDebug)
			Debug.Log ("Start()");
		Settings.betCurrent = new Bet(0);
			
		panelAddCredits = GameObject.Find ("PanelAddCredits");
		if (panelAddCredits) {
			btnCreditOk = panelAddCredits.transform.FindChild ("btnOk").gameObject;
			if (btnCreditOk)
				btnCreditOk.GetComponent<Button> ().onClick.AddListener (() => btnCreditOkClickListener ());
			if (panelAddCredits)
				panelAddCredits.SetActive (false);
		}
			
		panelGame = GameObject.Find ("PanelGame");
		panelInitBet = GameObject.Find ("PanelInitBet"); //GameObject.FindGameObjectWithTag("PanelInitBet");
		//panelBet = GameObject.Find("PanelBet");
		panelSurrender = GameObject.Find ("PanelSurrender");
			
		panelWin = GameObject.Find ("PanelWin");
		lblWinInfo = GameObject.Find ("lblWinInfo").GetComponent<Text>();
		if (panelWin)
			panelWin.SetActive (false);
			
		panelBonus = GameObject.Find ("PanelBonus");
		if (panelBonus)
			panelBonus.SetActive (false);

		// bet panel
		inputBetField = panelInitBet.GetComponentInChildren<InputField> (); //GameObject.Find("InputBetField").GetComponent<InputField>(); // 
		inputBetField.text = Settings.betNull.f();
			
		lblPanelBet = GameObject.Find ("lblPanelBet").GetComponent<Text>();
			
		// player game panel
		btnCheck = GameObject.Find ("btnCheck");
		btnCall = GameObject.Find ("btnCall");
		btnRaise = GameObject.Find ("btnRaise");
		btnFold = GameObject.Find ("btnFold");
		btnSurrender = GameObject.Find ("btnSurrender");
			
		// bet panel
		btnRepeatBet = GameObject.Find ("btnRepeatBet");
		btnStartGame = GameObject.Find ("btnStartGame");
			
		//left panel
		btnBetNow = GameObject.Find ("btnBetNow");
		btnBetBonus = GameObject.Find ("btnBetBonus");
		btnRepeatLastBet = GameObject.Find ("btnRepeatLastBet");
		if (btnRepeatLastBet)
			btnRepeatLastBet.SetActive (false);
			

		btnCredit = GameObject.Find ("btnCredit");
		if (btnCredit)
			btnCredit.GetComponent<Button> ().onClick.AddListener (() => btnCreditAddClickListener ());
			
		btnAutoPlay = GameObject.Find ("btnAutoPlay");
		btnNewGame = GameObject.Find ("btnNewGame");
		btnAllIn = GameObject.Find ("btnAllIn");
		lblPot = GameObject.Find ("lblPot").GetComponent<Text>();
		lblRaise = GameObject.Find ("lblRaise").GetComponent<Text>();
		lblBet = GameObject.Find ("lblBet").GetComponent<Text>();
		lblBetBonus = GameObject.Find ("lblBetBonus").GetComponent<Text>();
		lblCall = GameObject.Find ("lblCall").GetComponent<Text>();
			
		// start sounds
		audio = gameObject.AddComponent<AudioSource> ();
		audio.volume = 0.1f;
		soundBtnClicked = Resources.Load<AudioClip> ("Sounds/cardFan1");//pressed");
		soundDeal = Resources.Load<AudioClip> ("Sounds/cardSlide8");//highlight");
		soundRaise = Resources.Load<AudioClip> ("Sounds/chipsHandle5");//timerbeep");
		soundFold = Resources.Load<AudioClip> ("Sounds/fold");
		soundVideoWin = Resources.Load<AudioClip> ("Sounds/video_poker_long");//VideoWin");
		soundWin = Resources.Load<AudioClip> ("Sounds/VideoWin");

		// end sounds

		panelHelp = GameObject.Find ("PanelHelp");
		if (panelHelp)
			panelHelp.SetActive (false);
			
		panelInstructions = GameObject.Find ("PanelInstructions");
		if (panelInstructions)
			panelInstructions.SetActive (false);
			
		
		payTable = new PayTable ();
		if (payTable != null) {
			payTable.Init();
			payTable.SelectColumnByIndex(9);
		}

//		InitCards (); // get dealers from parent object
		HideDynamicPanels ();

		panelInitBet.SetActive (true);

		game = new Game (this);

		ReInitGame ();

		InvokeRepeating ("UpdateInterval", Settings.updateInterval, Settings.updateInterval); // override default frequency of the update()

	}

	public IEnumerator DealCards() {
		for (int i = 0; i < Settings.playerHandSizePreflop; i++) {
			for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
				var card = player.handPreflop.getCard (i);
				if (player.id == Settings.playerRealIndex || Settings.isDebug) {
					card.FaceUp = true;
				} else {
					card.FaceUp = false;
				}
				audio.PlayOneShot(soundDeal);
				yield return new WaitForSeconds (Settings.updateInterval);
			}
		}
	
		game.state.isWaiting = false;
		game.playerIterator = new PlayerIterator (game.playerCollection);
	}

	public void UpdatePlayerActionAndCredits(Player player) {
		player.lblCredits.text = player.balanceInCredits.f();
		player.lblAction.text = player.actionCurrentString;
	}

	public void DealPreflopCards() { // without any delay
		for (int i = 0; i < Settings.playerHandSizePreflop; i++) {
			for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
				var card = player.handPreflop.getCard (i);
				if (player.id == Settings.playerRealIndex || Settings.isDebug) {
					card.FaceUp = true;
				} else {
					card.FaceUp = false;
				}
				audio.PlayOneShot(soundDeal);
			}
		}
	}

	private void UpdateInterval() {
//		test ();
//		return;
		if (game == null) {
			Debug.LogError ("game is null");
		} else {
			if (game.state != null && !game.state.isWaiting) {
				game.state.SubRound ();
			}
		}
	}

	private void test() {
//		int percentRand = GetPercentOfAllTime (20);
//		Debug.Log (percentRand + " 20%/100%");

//		TestPercentOfTime (20);


		while(!game.playerIterator.IsDone) {
			var player = game.playerIterator.Next();
			player.lblName.text = string.Format("{0} {2} {1} ", player.name, player.GetHandStringFromHandObj(), player.winPercent);
		}

		for (var player = game.playerIterator.First (); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
			player.lblName.text = string.Format("{0} {2} {1} ", player.name, player.GetHandStringFromHandObj(), player.winPercent);
		}
//		if (!game.states.isDone)
//			game.states.Next ();
	}

	public void TestPercentOfTime(int percent) {
		if (percent <= 0)
			return;
		int count1 = 0;
		int count2 = 0;
		int percentOfAllTime = 100;
		for (int i = 0; i < percentOfAllTime; i++) {
			float percentOfTime = UnityEngine.Random.value * 100;
			
			if (percentOfTime < percent) {
				DebugLog(percentOfTime + "%");
				//				DebugLog (string.Format ("20% {0} count: {1}", percentOfTime, count1));
				count1++;
			} else {
				//				DebugLog (string.Format ("80% {0} count: {1}", percentOfTime, count2));
				count2++;
			}
		}
		Debug.Log (string.Format("\n\nTotal: {0} (20%) + {1} (80%) = 100 (100%)", count1, count2));
	}

	public int GetPercentOfAllTime(int percent) {
		float percentOfTime = UnityEngine.Random.value * 100;
		int percentRandom = 0;

		if (percent > 0 && percentOfTime < percent) {
			percentRandom = (int)percentOfTime;
		} else {
			Debug.Log ("Percent can't be negative or null");
		}

		return percentRandom;
	}

	private void InitCards ()
	{
		if (Settings.isDebug) Debug.Log ("InitCards()");
		
		// start dealer icons
		var dealers = new List<GameObject>();
		var transform = GameObject.Find("Dealers").GetComponentsInChildren<Transform>();
		foreach (Transform child in transform)
		{
			if (child != transform[0])
				dealers.Add(child.gameObject);
		}
		// end dealer icons
	}

	public void HideDynamicPanels() {
		if (Settings.isDebug) Debug.Log("HideDynamicPanels()");
		
		if(panelInitBet) panelInitBet.SetActive(false);
		if(panelGame) panelGame.SetActive(false);
		if(panelSurrender) panelSurrender.SetActive(false);
		//panelBet.SetActive(false);
		if (panelAddCredits) panelAddCredits.SetActive(false);

		if (panelHelp)
			panelHelp.SetActive (false);
		if (panelInstructions)
			panelInstructions.SetActive (false);
		if (panelWin)
			panelWin.SetActive (false);
	}

	#region api
	public void Add(string amount)
	{
		string url = string.Format("{0}/{1}", Settings.host, Settings.actionAdd);
		if (Settings.isDebug) Debug.Log(url);
		
		WWWForm form = new WWWForm();
		form.AddField("a", amount);
		form.AddField("k", Settings.key);
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}
	
	public void Sub(string amount)
	{
		string url = string.Format("{0}/{1}", Settings.host, Settings.actionSub);
		if (Settings.isDebug) Debug.Log(url);
		
		WWWForm form = new WWWForm();
		form.AddField("a", amount);
		form.AddField("k", Settings.key);
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}
	
	public void GetBalance()
	{
		string url = string.Format("{0}/{1}", Settings.host, Settings.actionGetBalance);
		if (Settings.isDebug) Debug.Log(url);
		
		WWWForm form = new WWWForm();
		form.AddField("k", Settings.key);
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForGetBalanceRequest(www));
	}
	
	public void SetBalance(string amount)
	{
		string url = string.Format("{0}/{1}", Settings.host, Settings.actionSetBalance);
		if (Settings.isDebug) Debug.Log(url);
		
		WWWForm form = new WWWForm();
		form.AddField("a", amount);
		form.AddField("k", Settings.key);
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		// check for errors
		if (www.error == null)
		{
			if (Settings.isDebug) Debug.Log("api Ok!: " + www.data);
		}
		else
		{
			string msg = "error api: " + www.error;
			if (Settings.isDebug) Debug.Log(msg);
		}
	}
	
	IEnumerator WaitForGetBalanceRequest(WWW www)
	{
		yield return www;
		var lblMyCreditsTitle = GameObject.Find("lblMyCredits") ;
		// check for errors
		if (www.error == null)
		{
			double credits = 0;
			if (lblMyCreditsTitle) lblMyCreditsTitle.GetComponent<Text>().text = www.text;
			Double.TryParse(www.text, out credits);
			if (credits >= 0) {
				game.player.balanceInCredits = credits;///Settings.betCreditsMultiplier;
			}
			if (Settings.isDebug) Debug.Log("api Ok!: " + www.data);
		}
		else
		{
//			game.player.betTotal = Settings.playerCredits/Settings.betCreditsMultiplier; // if we play without login
			if (lblMyCreditsTitle) lblMyCreditsTitle.GetComponent<Text>().text = "pls relogin and try again";
			string msg = "error getting balance: " + www.error;
			if (Settings.isDebug) Debug.Log(msg);
		}
	}
	
	public void urlBuy()
	{
		Settings.OpenUrl (Settings.urlBuy);
	}
	
	public void urlCredits()
	{
		Settings.OpenUrl (Settings.urlCredits);
	}
	
	public void urlLogin()
	{
		Settings.OpenUrl (Settings.urlLogin);
	}
	
	public void urlInviteFriend()
	{
		Settings.OpenUrl (Settings.urlInviteFriend);
	}
	
	public void urlFortuneWheel()
	{
		Settings.OpenUrl (Settings.urlFortuneWheel);
	}
	
	#endregion

	public void DebugLog(string message) {
		if (Settings.isDebug) Debug.Log(message);
	}
	
	public PayTable payTable;
	public GameObject panelInitBet, panelGame, panelSurrender, panelAddCredits, panelHelp, panelInstructions, panelWin, panelBonus;
	public GameObject btnCheck, btnCall, btnRaise, btnFold, btnSurrender, btnStartGame, btnBetBonus, btnCreditOk, 
	btnRepeatBet, btnRepeatLastBet, btnBetNow, btnCredit, btnAutoPlay, btnNewGame, btnAllIn;
	public Text lblPot, lblRaise, lblBet, lblBetBonus, lblCall, lblPanelBet, lblPanelBetText, lblWinInfo;
	public AudioSource audio;
	public AudioClip soundBtnClicked, soundDeal, soundRaise, soundVideoWin, soundWin, soundFold;
	public InputField inputBetField;
}

