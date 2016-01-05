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
		players = new List<PlayerUI> ();
	}

	public void ClearAll() {
		foreach (var card in game.cards) {
			card.isHidden = true;
		}
		foreach (var player in players) {
			player.chip.sprite = Resources.Load<Sprite>(Settings.cardBg);
			player.dealer.sprite = Resources.Load<Sprite>(Settings.cardBg);
			player.lblAction.text = "";
			player.lblCredits.text = "";
			foreach (var card in player.hand.getCards()) {
				card.isHidden = true;
			}
		}
		lblPot.GetComponent<Text>().text = FormatCreditsOrDollars(Settings.betNull);
		lblBet.GetComponent<Text>().text = FormatCreditsOrDollars(Settings.betNull);
		lblRaise.GetComponent<Text>().text = FormatCreditsOrDollars(Settings.betNull);
		lblWin.GetComponent<Text>().text = FormatCreditsOrDollars(Settings.betNull);
	}

	// start win panel
	public void btnWinPanelCloseClick()
	{
		game.GameState.InitGame (game);
	}

	// end win panel
	// start game panel
	public void btnCheckClick()
	{
		game.GameState.Check (game);
	}

	public void btnCallClick()
	{
		game.GameState.Call (game);
	}

	public void btnRaiseClick()
	{
		betAmount = 0;
		HideDynamicPanels ();
		if (panelInitBet) panelInitBet.SetActive (true);
	}

	public void btnFoldClick()
	{
//		game.MathState.BetRound1 ();
//		game.GameState.EndGame (game);
		game.GameState.Fold (game);
	}

	public void btnAllInClick()
	{
//		game.MathState.River (game);
	}

	public void btnHelpClick()
	{
		
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
		Double.TryParse (betAmountString, out betAmount);
		var player = players.First();
		if (game.isGameRunning) {
			if (player.credits - betAmount < 0) {
				game.GameState.Check (game);
			} else if (player.credits - betAmount >= 0) {
				game.GameState.Raise (game);
			} else {
				return;
//				game.GameState.Check (game);
			}
		} else if (!game.isGameRunning && betAmount > 0 && player.credits - betAmount >= 0) {
			game.GameState.Raise(game);
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
		
		Settings.betCurrent = Settings.betMax;
		
		string b = FormatCreditsOrDollars(Settings.betCurrent);
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
		
		Settings.betCurrent += Settings.betDx;
		if (Settings.betCurrent > Settings.betMax)
			Settings.betCurrent = 0f;
		inputBetField.text = FormatCreditsOrDollars(Settings.betCurrent);
	}
	
	public void btnClearBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnClearBetClick()");
		
		audio.PlayOneShot(pressedSound);
		
		Settings.betCurrent = Settings.betNull;
		inputBetField.text = FormatCreditsOrDollars(Settings.betCurrent);
	}
	// end bet panel


	private string FormatCreditsOrDollars(double amount) {
		string creditAmount = String.Format("{0:N2}", amount);// amount.ToString("#,#", System.Globalization.CultureInfo.CurrentCulture);
		return creditAmount;
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
		inputBetField.text = FormatCreditsOrDollars (Settings.betNull);
			
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
		
		// start init chips
		chipSpriteList = new List<Sprite>() {
			Resources.Load("chips_red", typeof(Sprite)) as Sprite,
			Resources.Load("chips_blue", typeof(Sprite)) as Sprite
		};
		// end init chips

//		InitCards (); // get dealers from parent object
		HideDynamicPanels ();

		panelInitBet.SetActive (true);

		game = new Game (this);
		int i = 0;
		foreach (var player1 in game.players) {
//			var player = (PlayerUI)player1.Clone();
			var player = new PlayerUI(player1);
			player.chip = GameObject.Find("Chip"+i).GetComponent<Image>();
			player.dealer = GameObject.Find("Dealer"+i).GetComponent<Image>();
			player.lblAction = GameObject.Find ("lblBetPlayer"+i).GetComponent<Text>();
			player.lblCredits = GameObject.Find ("lblCreditPlayer"+i).GetComponent<Text>();
			player.lblName = GameObject.Find("lblPlayerName"+i).GetComponent<Text>();
			players.Add(player);
			i++;
		}

		InvokeRepeating ("UpdateInterval", Settings.updateInterval, Settings.updateInterval); // override default frequency of the update()

		updatePlayerNames ();
	}

	private IEnumerator DisplayPlayerNames(List<PlayerUI> players, float repeatRate) {
		int i = 0;
		foreach(var player in players) {
			player.lblName.text = player.name;
			yield return new WaitForSeconds(repeatRate);
			i++;
		}
	}

	private void updatePlayerNames() {
		StartCoroutine(DisplayPlayerNames(players, Settings.updateInterval));
//		InvokeRepeating("UpdatePlayerName", Settings.updateInterval, Settings.updateInterval);
	}

	int playerNo = 0;
	private void UpdatePlayerName() {
		if (playerNo < players.Count) {
			var player = players.ElementAt (playerNo);
			player.lblName.text = player.name;
			playerNo++;
		} else {
			playerNo = 0;
		}
	}

	private void UpdateInterval() {
//		int percentRand = GetPercentOfAllTime (20);
//		Debug.Log (percentRand + " 20%/100%");

//		TestPercentOfTime (20);
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
		if (Settings.isDebug)
			Debug.Log ("InitCards()");
		
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
	public List<PlayerUI> players;
	public List<Sprite> chipSpriteList;
	public double betAmount, potAmount;
	public GameObject panelInitBet, panelGame, panelSurrender, panelAddCredits, panelHelp, panelInstructions, panelWin, panelBonus;
	public GameObject btnCheck, btnCall, btnRaise, btnFold, btnSurrender, btnStartGame, btnBetBonus, btnCreditOk, 
	btnRepeatBet, btnRepeatLastBet, btnBetNow, btnCredit, btnAutoPlay, btnNewGame, btnAllIn;
	public GameObject lblPot, lblRaise, lblBet, lblCall, lblWin, lblPanelBet, lblPanelBetText, lblWinPlayerName, playerAllCredits;
	AudioSource audio;
	AudioClip pressedSound, dealSound, buttonSound, raiseSound, videoWin;
	public InputField inputBetField;

}

