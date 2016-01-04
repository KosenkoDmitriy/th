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
//		foreach (var item in list) {
//			DebugLog(item.name);
//		}
	}

	public void ClearAll() {
		ClearPlayerCards ();
		ClearPublicCards ();
	}

	public void ClearPlayerCards() {
		foreach (var item in cardsOfPlayer) {
			item.sprite = cardsAll.LastOrDefault();
		}
	}

	public void ClearPublicCards() {
		foreach (var item in cardsPublic) {
			item.sprite = cardsAll.LastOrDefault();
		}
	}
	// start win panel
	public void btnWinPanelCloseClick()
	{
		ClearAll ();

		HideDynamicPanels ();
		panelInitBet.SetActive (true);

//		game.GameState.InitGame (game);
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
		var player = game.players [0];
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
		lblGameState = GameObject.Find ("lblGameState");
			
		lblSurrender = GameObject.Find ("lblSurrender");
			
		// start sounds
		audio = gameObject.AddComponent<AudioSource> ();
		pressedSound = Resources.Load<AudioClip> ("Sounds/cardFan1");//pressed");
		dealSound = Resources.Load<AudioClip> ("Sounds/cardSlide8");//highlight");
		buttonSound = Resources.Load<AudioClip> ("Sounds/cardsShove4");//push3"); //push2
		raiseSound = Resources.Load<AudioClip> ("Sounds/chipsHandle5");//timerbeep");
		videoWin = Resources.Load<AudioClip> ("Sounds/video_poker_long");//VideoWin");
		// end sounds
			
//		if (btnBetNow)
//			btnBetNow.GetComponent<Button> ().onClick.AddListener (() => btnBetNowClickListener ());
//		if (btnStartGame)
//			btnStartGame.GetComponent<Button> ().onClick.AddListener (() => btnStartGameClickListener ());
//		if (btnRaise)
//			btnRaise.GetComponent<Button> ().onClick.AddListener (() => btnRaiseClickListener ());
//		if (btnCall)
//			btnCall.GetComponent<Button> ().onClick.AddListener (() => btnCallClickListener ());
//		if (btnCheck)
//			btnCheck.GetComponent<Button> ().onClick.AddListener (() => btnCheckClickListener ());
//		if (btnSurrender)
//			btnSurrender.GetComponent<Button> ().onClick.AddListener (() => btnSurrenderClickListener ());
//		if (btnAllIn)
//			btnAllIn.GetComponent<Button> ().onClick.AddListener (() => btnAllInClickListener ());
//		if (btnRepeatBet)
//			btnRepeatBet.GetComponent<Button> ().onClick.AddListener (() => btnRepeatBetOfBetFormClickListener ());
			
		panelHelp = GameObject.Find ("PanelHelp");
		if (panelHelp)
			panelHelp.SetActive (false);
			
		panelInstructions = GameObject.Find ("PanelInstructions");
		if (panelInstructions)
			panelInstructions.SetActive (false);
			
		InitCards ();
		HideDynamicPanels ();

		panelInitBet.SetActive (true);


		game = new Game (this);
		InvokeRepeating ("UpdateInterval", Settings.updateInterval, Settings.updateInterval); // override default frequency of the update()

		updatePlayerNames ();
	}

	private IEnumerator DisplayPlayerNames(List<Player> players, float repeatRate) {
		int i = 0;
		foreach(var player in players) {
			var lbl = playerNamesLabels.ElementAt (i);
			if (lbl)
				lbl.GetComponent<Text> ().text = player.name;

			yield return new WaitForSeconds(repeatRate);

			i++;
		}
	}

	private void updatePlayerNames() {
		StartCoroutine(DisplayPlayerNames(game.players, Settings.updateInterval));
//		InvokeRepeating("UpdatePlayerName", Settings.updateInterval, Settings.updateInterval);
	}

	int playerNo;
	private void UpdatePlayerName() {
		if (playerNo < game.players.Count) {
			var player = game.players.ElementAt (playerNo);
			
			var lbl = playerNamesLabels.ElementAt (playerNo);
			if (lbl)
				lbl.GetComponent<Text> ().text = player.name;
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
			
		//cards
		playerhold1 = GameObject.Find ("player0hold1");
		playerhold2 = GameObject.Find ("player0hold2");
		player1hold1 = GameObject.Find ("player1hold1");
		player1hold2 = GameObject.Find ("player1hold2");
		player2hold1 = GameObject.Find ("player2hold1");
		player2hold2 = GameObject.Find ("player2hold2");
		player3hold1 = GameObject.Find ("player3hold1");
		player3hold2 = GameObject.Find ("player3hold2");
		player4hold1 = GameObject.Find ("player4hold1");
		player4hold2 = GameObject.Find ("player4hold2");
		player5hold1 = GameObject.Find ("player5hold1");
		player5hold2 = GameObject.Find ("player5hold2");
			
		List<GameObject> cardsOfPlayerGameObjects = new List<GameObject> ()
			{
				playerhold1, playerhold2, player1hold1, player1hold2, player2hold1, player2hold2,
				player3hold1, player3hold2, player4hold1, player4hold2, player5hold1, player5hold2
			};
			
		cardsOfPlayer = new List<Image> ();
		foreach (var obj in cardsOfPlayerGameObjects) {
			cardsOfPlayer.Add (obj.GetComponent<Image> ());
		}
			
		// init cards with images/sprites
		cardsAll = new List<Sprite> ();
		List<string> masti = new List<string> () { "spades", "dia", "clubs", "hearts" };
		string separator = "_";
		string path = "";
		Sprite cardSprite;
		List<string> cards = new List<string> () { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
		foreach (string mast in masti)
			foreach (string card in cards) {
				path = Settings.cardsPrefix + card + separator + mast;
				cardSprite = Resources.Load (path, typeof(Sprite)) as Sprite;
				cardsAll.Add (cardSprite);
			}
			
		cardBg = Resources.Load (Settings.cardBg, typeof(Sprite)) as Sprite;
		cardBack = Resources.Load (Settings.cardBackName, typeof(Sprite)) as Sprite;
		cardsAll.Add (cardBack);
		cardsAll.Add (cardBg); //card with the background color
			
		//cards flop, turn, river
		cardsPublic = new List<Image> ()
			{
				GameObject.Find("flop1").GetComponent<Image>(),
				GameObject.Find("flop2").GetComponent<Image>(),
				GameObject.Find("flop3").GetComponent<Image>(),
				GameObject.Find("turn").GetComponent<Image>(),
				GameObject.Find("river").GetComponent<Image>(),
			};
			
		//init labels for credits and bets of each player
		betLabels = new List<GameObject> () {
				GameObject.Find("lblBetPlayer0"),
				GameObject.Find("lblBetPlayer1"),
				GameObject.Find("lblBetPlayer2"),
				GameObject.Find("lblBetPlayer3"),
				GameObject.Find("lblBetPlayer4"),
				GameObject.Find("lblBetPlayer5")
			};
			
		playerNamesLabels = new List<GameObject> () {
				GameObject.Find("lblPlayerName0"),
				GameObject.Find("lblPlayerName1"),
				GameObject.Find("lblPlayerName2"),
				GameObject.Find("lblPlayerName3"),
				GameObject.Find("lblPlayerName4"),
				GameObject.Find("lblPlayerName5")
			};
			
		creditLabels = new List<GameObject> () {
				GameObject.Find("lblCreditPlayer0"),
				GameObject.Find("lblCreditPlayer1"),
				GameObject.Find("lblCreditPlayer2"),
				GameObject.Find("lblCreditPlayer3"),
				GameObject.Find("lblCreditPlayer4"),
				GameObject.Find("lblCreditPlayer5")
			};
		
		payTable = new PayTable ();
		if (payTable != null) {
			payTable.BuildVideoBonusPaytable();
			payTable.SetPaytableSelectedColumn(9);
		}
		
		Image chipBox1 = GameObject.Find("Chip1").GetComponent<Image>();
		Image chipBox2 = GameObject.Find("Chip2").GetComponent<Image>();
		Image chipBox3 = GameObject.Find("Chip3").GetComponent<Image>();
		Image chipBox4 = GameObject.Find("Chip4").GetComponent<Image>();
		Image chipBox5 = GameObject.Find("Chip5").GetComponent<Image>();
		chipBoxes = new List<Image>() { chipBox1, chipBox2, chipBox3, chipBox4, chipBox5 };
		
		// start init chips
		chipSpriteList = new List<Sprite>() {
			Resources.Load("chips_red", typeof(Sprite)) as Sprite,
			Resources.Load("chips_blue", typeof(Sprite)) as Sprite
		};
		// end init chips
		
		// start dealer icons
		dealers = new List<GameObject>();
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
	public GameObject panelInitBet, panelGame, panelSurrender, panelAddCredits, panelHelp, panelInstructions, panelWin; //, bonusPokerPanel;
	public GameObject btnCheck, btnCall, btnRaise, btnFold, btnSurrender, btnStartGame, lblPanelBet, lblPanelBetText; // panelInitBet
	public GameObject btnBetNow, btnRepeatLastBet, playerAllCredits; // left panel (start/restart the game)
	public GameObject btnCredit, btnRepeatBet, btnAutoPlay, btnNewGame, btnAllIn, btnCreditOk;
	public GameObject lblPot, lblRaise, lblBet, lblCall, lblWin, lblGameState;
	public List<GameObject> betLabels, creditLabels, playerNamesLabels; // for each player
	public GameObject txtSurrender, lblSurrender;//panel surrender
	public GameObject playerhold1, playerhold2, player1hold1, player1hold2, player2hold1, player2hold2, player3hold1, player3hold2, player4hold1, player4hold2, player5hold1, player5hold2;
	public double betAmount;
	public double dollarAmount;
	public InputField inputBetField;
	public List<Sprite> cardsAll;
	public List<Image> cardsOfPlayer, cardsPublic;
	public Sprite cardBg; // background of the desk
	public Sprite cardBack; // back card side
	// panel XYZ
		
	bool isFromRaiseBtn = false;
	bool isFromBetOkBtn = false;
	bool isFromFoldBtn = false;
	bool isFromRepeatBetBtn = false;
	List<Image> chipBoxes;
	List<GameObject> dealers;
	List<Sprite> chipSpriteList;
	AudioSource audio;
	AudioClip pressedSound, dealSound, buttonSound, raiseSound, videoWin;
	GameObject lblWinPlayerName;
	GameObject btnBetBonus;
	GameObject panelBonus;
}

