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

	public void ClearAll() {
		if (Settings.isDebug) DebugLog("ClearAll()");
		var bg = Resources.Load<Sprite> (Settings.cardBg);
		if (game.players != null)
		foreach (var player in game.players) {
			player.chip.sprite = bg;
			player.isDealer = false;
			player.lblAction.text = "";
			player.lblCredits.text = "";
			foreach (var card in player.hand.getCards()) {
				card.isHidden = true;
			}
			foreach (var card in player.handPreflop.getCards()) {
				card.isHidden = true;
			}
			foreach(var hand in player.hands)
			foreach (var card in hand.getCards()) {
				card.isHidden = true;
			}
		}
		if (game.cards != null)
		foreach (var card in game.cards) {
			card.isHidden = true;
		}
		lblPot.GetComponent<Text>().text = Settings.betNull.to_s();
		lblBet.GetComponent<Text>().text = Settings.betNull.to_s();
		lblRaise.GetComponent<Text>().text = Settings.betNull.to_s();
		lblWin.GetComponent<Text>().text = Settings.betNull.to_s();
	}

	// start win panel
	public void btnWinPanelCloseClick()
	{
		ReInitGame ();

		audio.PlayOneShot(soundBtnClicked);

		game.state.isWaiting = false;
	}


	public void ReInitGame () {
		game.state = new InitGame (game);
	}

	// end win panel
	// start game panel
	public void btnCheckClick()
	{
		audio.PlayOneShot(soundBtnClicked);
		game.player.actionFinal = new Check(game.player, game.player.bet);
		game.player.actionFinal.Do (game);
	}

	public void btnCallClick()
	{
		audio.PlayOneShot(soundBtnClicked);
		game.player.actionFinal = new Call(game.player, game.player.bet);
		game.player.actionFinal.Do (game);
	}

	public void btnRaiseClick()
	{
		audio.PlayOneShot(soundBtnClicked);
		game.betAmount = 0;
		HideDynamicPanels ();
		if (panelInitBet) panelInitBet.SetActive (true);
	}

	public void btnFoldClick()
	{
		audio.PlayOneShot(soundBtnClicked);

		game.state = new EndGame (game);
	}

	public void btnAllInClick()
	{
//		game.MathState.River (game);
		audio.PlayOneShot(soundBtnClicked);
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
	// end game panel

	// start bet panel
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
		Double.TryParse (betAmountString, out game.betAmount);

		if (game.betAmount > 0) {
			game.betAmount /= Settings.betCreditsMultiplier; // * Settings.betCurrentMultiplier;
		}
		var player = game.player;
		if (game.isGameRunning) {
			if (player.betTotal - game.betAmount < 0) {
				game.player.actionFinal = new Check(game.player, game.player.bet);
			} else 
			if (player.betTotal - game.betAmount >= 0) {
				game.player.actionFinal = new Raise(game.player, game.player.bet);
			}
			game.player.actionFinal.Do (game);
		} else if (!game.isGameRunning && game.betAmount > 0 && player.betTotal - game.betAmount >= 0) {
			game.isGameRunning = true;
			game.player.actionFinal = new Raise(game.player, Settings.betAnteMultiplier);
			game.player.actionFinal.Do (game);
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
		
		Settings.betCurrent = Settings.betMaxMath;
		
		string b = Settings.betCurrent.to_s();
		inputBetField.text = b;
		panelInitBet.GetComponentInChildren<InputField>().text = b;
	}
	
	public void btnMinBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnMinBetClick()");
		
		audio.PlayOneShot(soundBtnClicked);
		
		Settings.betCurrent += Settings.betDxMath;
		if (Settings.betCurrent > Settings.betMaxMath)
			Settings.betCurrent = Settings.betNull;

		inputBetField.text = Settings.betCurrent.to_s();
	}
	
	public void btnClearBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnClearBetClick()");
		
		audio.PlayOneShot(soundBtnClicked);
		
		Settings.betCurrent = Settings.betNull;

		inputBetField.text = Settings.betCurrent.to_s();
	}
	
	// end bet panel

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

	#region bonus table
	public void btnBetForBonusTableClick() {
		if (Settings.isDebug) Debug.Log("btnBetForBonusTableClick()");
		if (panelBonus) panelBonus.SetActive (true);
	}
	
	public void btnBonusPanelCloseClick() {
		if (Settings.betBonus > 0) {
			if (payTable != null) {
				lblPot.GetComponent<Text>().text = Settings.betBonus.to_b ();
				game.player.betTotal -= Settings.betBonus;
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
	#endregion


	public void Start ()
	{
		if (Settings.isDebug)
			Debug.Log ("Start()");
		Settings.betCurrent = 0f;
			
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
		lblWinPlayerName = GameObject.Find ("lblWinPlayerName");
		if (panelWin)
			panelWin.SetActive (false);
			
		panelBonus = GameObject.Find ("PanelBonus");
		if (panelBonus)
			panelBonus.SetActive (false);

		// bet panel
		inputBetField = panelInitBet.GetComponentInChildren<InputField> (); //GameObject.Find("InputBetField").GetComponent<InputField>(); // 
		inputBetField.text = Settings.betNull.to_s();
			
		lblPanelBet = GameObject.Find ("lblPanelBet");
			
		// player game panel
		btnCheck = GameObject.Find ("btnCheck");
		btnCall = GameObject.Find ("btnCall");
		btnRaise = GameObject.Find ("btnRaise");
		btnFold = GameObject.Find ("btnFold");
		btnSurrender = GameObject.Find ("btnSurrender");
		playerAllCredits = GameObject.Find ("playerAllCredits");
			
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
		lblPot = GameObject.Find ("lblPot");
		lblRaise = GameObject.Find ("lblRaise");
		lblBet = GameObject.Find ("lblBet");
			
		lblCall = GameObject.Find ("lblCall");
		lblWin = GameObject.Find ("lblWin");
			
		// start sounds
		audio = gameObject.AddComponent<AudioSource> ();
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

//		updatePlayerNames ();
	}

	private IEnumerator DisplayPlayerNames(List<Player> players, float repeatRate) {
		int i = 0;
		foreach(var player in game.players) {
			player.lblName.text = player.name;
			yield return new WaitForSeconds(repeatRate);
			i++;
		}
	}

	private void updatePlayerNames() {
		StartCoroutine(DisplayPlayerNames(game.players, Settings.updateInterval));
//		InvokeRepeating("UpdatePlayerName", Settings.updateInterval, Settings.updateInterval);
	}

	public IEnumerator DealCards() {
//		for(int i = 1; i >= 0; i--)
		for (int i = 0; i < 2; i++) {
		foreach(var player in game.players) {
//			for(var player = game.playerIterator.First (); !game.playerIterator.IsDone; player = game.playerIterator.Next()) {
//			Player player = game.playerIterator.Next();
//			while(!game.playerIterator.IsDone) {
				var card = player.handPreflop.getCard (i);
				if (player.id == Settings.playerRealIndex || Settings.isDebug || player.isFolded) {
					card.FaceUp = true;
				} else {
					card.FaceUp = false;
				}
				audio.PlayOneShot(soundDeal);
				yield return new WaitForSeconds (Settings.updateInterval);
//				player = game.playerIterator.Next();
			}
		}
	
		game.state.isWaiting = false;
	}
	
	public IEnumerator UpdatePlayers() {
		foreach (var player in game.players) {
			UpdatePlayer(player);
			yield return new WaitForSeconds (Settings.updateInterval);
		}
		game.state.isWaiting = false;
	}

	public void UpdatePlayer(Player player) {
		//			player.betTotal -= 10;
		for(int i = 0; i < 2; i++) {
			var card = player.handPreflop.getCard(i);
			//				if (player.id == Settings.playerRealIndex || Settings.isDebug || player.isFolded)
			if (player.isFolded)
				card.FaceUp = true;
			//				else
			//					card.FaceUp = false;
		}
		player.lblCredits.text = player.betTotal.to_s();
		player.lblAction.text = player.actionCurrentString;
	}

	int playerNo = 0;
	private void UpdatePlayerName() {
		if (playerNo < game.players.Count) {
			var player = game.players.ElementAt (playerNo);
			player.lblName.text = player.name;
			playerNo++;
		} else {
			playerNo = 0;
		}
	}

	private void UpdateInterval() {
//		test ();

		if (game.state != null && !game.state.isWaiting) {
			game.state.SubRound ();
		}

	}

	private void test() {
//		int percentRand = GetPercentOfAllTime (20);
//		Debug.Log (percentRand + " 20%/100%");

//		TestPercentOfTime (20);

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
				game.player.betTotal = credits/Settings.betCreditsMultiplier;
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
	public GameObject lblPot, lblRaise, lblBet, lblCall, lblWin, lblPanelBet, lblPanelBetText, lblWinPlayerName, playerAllCredits;
	public AudioSource audio;
	public AudioClip soundBtnClicked, soundDeal, soundRaise, soundVideoWin, soundWin, soundFold;
	public InputField inputBetField;
}

