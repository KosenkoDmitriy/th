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
//		game.GameState.InitGame (game);
		ReInitGame ();

		audio.PlayOneShot(pressedSound);
		isWaiting = false;
	}


	public void ReInitGame () {

		// TODO: clear all/reset all

		//		game.GameState.InitGame (game);
//		game.states = new States(game);
		game.state = new InitGame (game);
//		game.states.Next ();
//		game.state = game.states.state;


	}

	// end win panel
	// start game panel
	public void btnCheckClick()
	{
		audio.PlayOneShot(pressedSound);
		isWaiting = false;

		game.GameState.Check (game);
	}

	public void btnCallClick()
	{
		audio.PlayOneShot(pressedSound);
		isWaiting = false;

		game.GameState.Call (game);
	}

	public void btnRaiseClick()
	{
		audio.PlayOneShot(pressedSound);
		isWaiting = false;

		game.betAmount = 0;
		HideDynamicPanels ();
		if (panelInitBet) panelInitBet.SetActive (true);
	}

	public void btnFoldClick()
	{
//		game.MathState.BetRound1 ();
//		game.GameState.EndGame (game);
		game.GameState.Fold (game);
		audio.PlayOneShot(pressedSound);
	}

	public void btnAllInClick()
	{
//		game.MathState.River (game);
		audio.PlayOneShot(pressedSound);
	}

	public void btnHelpClick()
	{
		audio.PlayOneShot(pressedSound);
		
		if (Settings.isDebug) Debug.Log("btnHelpClick()");
		if (panelHelp) panelHelp.SetActive (true);
	}
	
	public void btnHelpCloseClick() {
		audio.PlayOneShot(pressedSound);
		
		if (Settings.isDebug) Debug.Log("btnHelpCloseClick()");
		if (panelHelp) panelHelp.SetActive (false);
	}
	// end game panel

	// start bet panel
	public void btnBetNowClick()
	{
		if (Settings.isDebug) Debug.Log("btnBetNowClick()");
		
		audio.PlayOneShot(pressedSound);
		
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

		var player = game.player;
		if (game.isGameRunning) {
			if (player.betTotal - game.betAmount < 0) {
				game.GameState.Check (game);
			} else 
			if (player.betTotal - game.betAmount >= 0) {
				game.GameState.Raise (game);
			}
			isWaiting = false;
		} else if (!game.isGameRunning && game.betAmount > 0 && player.betTotal - game.betAmount >= 0) {
			game.GameState.Raise(game);
			isWaiting = false;
		} else {
			return;
		}
		if (panelInitBet) panelInitBet.SetActive(false);
		if (panelGame) panelGame.SetActive(true);
	}
	
	public void btnMaxBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnMaxBetClick()");
		
		audio.PlayOneShot(pressedSound);
		
		Settings.betCurrent = Settings.betMaxInCredits;
		
		string b = Settings.betCurrent.to_s();
		inputBetField.text = b;
		panelInitBet.GetComponentInChildren<InputField>().text = b;
		//GameObject.Find("InputField").GetComponent<InputField>().text = b;
		
		//var obj = GameObject.Find("BetInputField");
		//obj.GetComponent<InputField>().text = b;
	}
	
	public void btnMinBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnMinBetClick()");
		
		audio.PlayOneShot(pressedSound);
		
		Settings.betCurrent += Settings.betDxInCredits;
		if (Settings.betCurrent > Settings.betMaxInCredits)
			Settings.betCurrent = 0f;
		inputBetField.text = Settings.betCurrent.to_s();
	}
	
	public void btnClearBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnClearBetClick()");
		
		audio.PlayOneShot(pressedSound);
		
		Settings.betCurrent = Settings.betNull;
		inputBetField.text = Settings.betCurrent.to_s();
	}
	
	// end bet panel

	public void btnInstructionClick()
	{
		audio.PlayOneShot(pressedSound);

		if (Settings.isDebug) Debug.Log("btnInstructionClick()");
		if (panelInstructions) panelInstructions.SetActive (true);
	}
	
	public void btnInstructionCloseClick() {
		audio.PlayOneShot(pressedSound);
		if (Settings.isDebug) Debug.Log("btnInstructionCloseClick()");
		if (panelInstructions) panelInstructions.SetActive (false);
	}
	
	public void Start ()
	{
		if (Settings.isDebug)
			Debug.Log ("Start()");
		Settings.betCurrent = 0f;
			
		panelAddCredits = GameObject.Find ("PanelAddCredits");
		if (panelAddCredits) {
			btnCreditOk = panelAddCredits.transform.FindChild ("btnOk").gameObject;
			if (btnCreditOk)
//				btnCreditOk.GetComponent<Button> ().onClick.AddListener (() => btnCreditOkClickListener ());
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
//		if (btnCredit)
//			btnCredit.GetComponent<Button> ().onClick.AddListener (() => btnCreditAddClickListener ());
			
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
		pressedSound = Resources.Load<AudioClip> ("Sounds/cardFan1");//pressed");
		dealSound = Resources.Load<AudioClip> ("Sounds/cardSlide8");//highlight");
		buttonSound = Resources.Load<AudioClip> ("Sounds/cardsShove4");//push3"); //push2
		raiseSound = Resources.Load<AudioClip> ("Sounds/chipsHandle5");//timerbeep");
		videoWin = Resources.Load<AudioClip> ("Sounds/video_poker_long");//VideoWin");
		// end sounds

		panelHelp = GameObject.Find ("PanelHelp");
		if (panelHelp)
			panelHelp.SetActive (false);
			
		panelInstructions = GameObject.Find ("PanelInstructions");
		if (panelInstructions)
			panelInstructions.SetActive (false);
			
		
		payTable = new PayTable ();
		if (payTable != null) {
			payTable.BuildVideoBonusPaytable();
			payTable.SetPaytableSelectedColumn(9);
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
		for(int i = 0; i < 2; i++)
		foreach(var player in game.players) {
			var card = player.handPreflop.getCard(i);
			if (player.id == Settings.playerRealIndex || Settings.isDebug)
				card.FaceUp = true;
			else
				card.FaceUp = false;
			yield return new WaitForSeconds(Settings.updateInterval);
		}
		game.state.isWaiting = false;
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

	bool isWaiting;
	private void UpdateInterval() {
//		int percentRand = GetPercentOfAllTime (20);
//		Debug.Log (percentRand + " 20%/100%");

//		TestPercentOfTime (20);

//		if (!game.states.isDone)
//			game.states.Next ();

//		game.state = new AnteRound ();
		if (game.state != null && !game.state.isWaiting) {
			game.state.SubRound ();
		}
		return;

		if (!isWaiting && !game.states.isDone) {
			var playerPrev = game.playerIterator.PrevActive();
			var player = game.playerIterator.NextActive();
			var playerLastActive = game.playerIterator.LastActive();
			if (player.isReal) {
				isWaiting = true;
				StartCoroutine(DealCards());
				player.lblAction.text = "waiting";
			} else {
				player.lblAction.text = "auto";
			}
		}

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

	public void DebugLog(string message) {
		if (Settings.isDebug) Debug.Log(message);
	}

	public PayTable payTable;
	public GameObject panelInitBet, panelGame, panelSurrender, panelAddCredits, panelHelp, panelInstructions, panelWin, panelBonus;
	public GameObject btnCheck, btnCall, btnRaise, btnFold, btnSurrender, btnStartGame, btnBetBonus, btnCreditOk, 
	btnRepeatBet, btnRepeatLastBet, btnBetNow, btnCredit, btnAutoPlay, btnNewGame, btnAllIn;
	public GameObject lblPot, lblRaise, lblBet, lblCall, lblWin, lblPanelBet, lblPanelBetText, lblWinPlayerName, playerAllCredits;
	AudioSource audio;
	AudioClip pressedSound, dealSound, buttonSound, raiseSound, videoWin;
	public InputField inputBetField;
}

