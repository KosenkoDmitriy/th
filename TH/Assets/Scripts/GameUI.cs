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

		Settings.betCurrent.inCredits = 0;
		game.betAmount.inCredits = 0;
		HideDynamicPanels ();
		if (panelInitBet) {
			panelInitBet.SetActive (true);

			if (btnStartGame) btnStartGame.GetComponentInChildren<Text>().text = "BET";

			inputBetField.text = game.betAmount.inCredits.f();

			game.ui.btnRepeatBetPrepare();

		}
	}

	public void btnFoldClick()
	{
		audio.PlayOneShot(soundBtnClicked);

		game.player.isFolded = true;
		game.state = new EndGame (game);
	}

	public void btnAllInClick()
	{
		audio.PlayOneShot(soundBtnClicked);
		var betToStay = game.state.betMax - game.player.betInvested;
		if (betToStay < 0) {
			betToStay = new Bet(0);
		}
		game.player.actionFinal = new AllIn (game.player, betToStay, new Bet(0));
		game.player.actionFinal.Do (game, game.player);
		game.ui.LoseBalance(game.player.balanceInCredits.ToString());
		
		game.ui.HideDynamicPanels();
		game.ui.panelGame.SetActive(true);
		game.ui.btnCall.GetComponent<Button>().interactable = false; 	//.SetActive(false);
		game.ui.btnCheck.GetComponent<Button>().interactable = false;	//.SetActive(false);
		game.ui.btnRaise.GetComponent<Button>().interactable = false;	//.SetActive(false);
		game.ui.btnFold.GetComponent<Button>().interactable = false;	//.SetActive(false);
		game.ui.btnAllIn.GetComponentInChildren<Text>().text = "CONTINUE";
	}

	public void btnRepeatBetPrepare ()
	{
		if (game.state != null) {
			var betMax = game.state.betMaxLimit - game.state.betMax;
			if (betMax < 0d) {
				Settings.betCurrent.inCredits = Settings.betNull;
			}
			if (Settings.betCurrent <= betMax.inCredits) {
				game.ui.btnRepeatBet.GetComponent<Button> ().interactable = true;
			} else {
				game.ui.btnRepeatBet.GetComponent<Button> ().interactable = false;
			}
		} else {
			game.ui.btnRepeatBet.GetComponent<Button> ().interactable = false;
		}
		game.ui.btnRepeatBet.GetComponentInChildren<Text>().text = string.Format("{0} {1}", Settings.btnBetRepeat, Settings.betRepeat.f());
	}

	public void btnRepeatBetClick() {
		var bet = new Bet(0);
		bet.inCredits = Settings.betRepeat;
		DoFinalActionByCurrentBet(bet);
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

		string betAmountString = "0";
		if (inputBetField)
			betAmountString = inputBetField.text;
		double bet = 0;
		Double.TryParse (betAmountString, out bet);
		game.betAmount.inCredits = bet;

		Settings.betCurrent.inCredits = bet;

		DoFinalActionByCurrentBet(game.betAmount);
	}

	public void DoFinalActionByCurrentBet(Bet bet) {
		// from recommend to optimal
		double betTotalAfterAction = game.player.balanceInCredits - bet.inCredits;
		double betTotalSubRoundAfterA = game.player.betInvested.inCredits + bet.inCredits;
		
		if (game.isGameRunning) {
			
			var betCall = game.state.betMax - game.player.betInvested;
			if (betCall < 0) { // raised already
				betCall.inCredits = 0; // should be positive
			}
			if (betTotalSubRoundAfterA > game.state.betMaxLimit.inCredits) {
				if (betCall > game.state.betMaxLimit) {
					game.player.actionFinal = new Check (game.player, new Bet(0), new Bet(0));
				} else {
					game.player.actionFinal = new Call (game.player, betCall, new Bet(0));
				}
			} else {
				game.player.actionFinal = new Raise (game.player, betCall, bet);
			}
			
			if (lblCall) lblCall.text = game.state.betMax.inCredits.f();
			if (lblRaise) lblRaise.text = bet.inCredits.f();
			
			game.player.actionFinal.Do (game, game.player);
			if (lblBet) lblBet.text = bet.inCredits.f();
		} else if (!game.isGameRunning && bet.inCredits > 0 && betTotalAfterAction >= 0) {
			game.isGameRunning = true;
			game.player.actionFinal = new Raise(game.player, game.state.betMax, Settings.betCurrent);
			game.player.actionFinal.Do (game, game.player);
			if (lblBet) lblBet.text = bet.inCredits.f();
		} else {
			return;
		}
		
		Settings.betRepeat = bet.inCredits;
		
		if (btnStartGame) btnStartGame.GetComponentInChildren<Text>().text = "BET";
		
		if (panelInitBet) panelInitBet.SetActive(false);
		if (panelGame) panelGame.SetActive(true);
	}

	public void btnMaxBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnMaxBetClick()");
		
		audio.PlayOneShot(soundBtnClicked);

		var betMax = game.state.betMaxLimit - game.state.betMax;

		Bet betMin = new Bet(0);
		betMin.inBetMath = Settings.betMinMath;

		while(game.player.balanceInCredits <= betMax.inCredits) {
			betMax -= betMin;
			Settings.betCurrent.inBetMath = betMax.inBetMath;
		}

		if (Settings.betCurrent < 0d) {
			Settings.betCurrent.inCredits = Settings.betNull;
		} else {
			Settings.betCurrent.inCredits = betMax.inCredits;
		}

		string b = Settings.betCurrent.inCredits.f();
		inputBetField.text = b;
		panelInitBet.GetComponentInChildren<InputField>().text = b;
	}
	
	public void btnMinBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnMinBetClick()");
		
		audio.PlayOneShot(soundBtnClicked);

		var betMax = game.state.betMaxLimit - game.state.betMax;

		Bet betMin = new Bet(0);
		betMin.inBetMath = Settings.betMinMath;
		Settings.betCurrent += betMin;

		if (
			(Settings.betCurrent > betMax) 
		    || (Settings.betCurrent > game.player.balanceInCredits)
		   )
			Settings.betCurrent.inBetMath = Settings.betNull;

		inputBetField.text = Settings.betCurrent.inCredits.f();
	}

	public void btnSubBetClick()
	{
		if (Settings.isDebug) Debug.Log("btnSubBetClick()");
		
		audio.PlayOneShot(soundBtnClicked);
		
		var betMax = game.state.betMaxLimit - game.state.betMax;
		
		Bet betMin = new Bet(0);
		betMin.inBetMath = Settings.betMinMath;
		Settings.betCurrent -= betMin;
		if (Settings.betCurrent < 0) Settings.betCurrent = betMax;
		if (
			(Settings.betCurrent > betMax) 
			|| (Settings.betCurrent > game.player.balanceInCredits)
			)
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

		if (panelInstructions)
		if (panelInstructions.activeSelf)
			panelInstructions.SetActive (false);
		else
			panelInstructions.SetActive (true);

		if (panelBonusTable) panelBonusTable.SetActive (false);
		if (panelAddCredits) panelAddCredits.SetActive (false);
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
		//if (panelAddCredits) panelAddCredits.SetActive(true);
		if (panelAddCredits) {
			if (panelInstructions) panelInstructions.SetActive (false);
			if (panelBonusTable) panelBonusTable.SetActive (false);

			if (panelAddCredits.activeSelf)
				panelAddCredits.SetActive (false);
			else
				panelAddCredits.SetActive (true);
		}

		//game.ui.SetBalance(Settings.playerCredits.ToString());
		var lblMyCreditsTitle = GameObject.Find("lblMyCredits");
		if (lblMyCreditsTitle) lblMyCreditsTitle.GetComponent<Text>().text = Settings.playerCredits.f();

	}
	
	private void btnCreditOkClickListener()
	{
		if (Settings.isDebug) Debug.Log("btnCreditOkClickListener()");
		if (panelAddCredits) panelAddCredits.SetActive(false);
	}
	#endregion add credits

	#region bonus pane/table
	public void btnBonusBetSetClick() {

		string textValue = betBonusDropdown.captionText.text;
		double floatValue = 0;
		double.TryParse(textValue, out floatValue);
		if (floatValue > 0) {
			floatValue /= Settings.betCreditsMultiplier;
			Settings.betBonus = floatValue;
		}

		if (Settings.betBonus > 0) {
			if (payTable != null) {
				game.player.balanceInCredits -= Settings.betBonus * Settings.betCreditsMultiplier;
				game.player.lblCredits.text = game.player.balanceInCredits.f();
				if (lblBetBonus) lblBetBonus.text = Settings.betBonus.to_b();
				if (payTable != null) payTable.SetBet(Settings.betBonus);
			}
		}

		// disable bonus buttons
		if (game.ui.btnBonusBetSet) game.ui.btnBonusBetSet.GetComponent<Button>().interactable = false;
		if (game.ui.betBonusDropdown) game.ui.betBonusDropdown.GetComponent<Dropdown>().interactable = false;
		if (panelBonusTable) panelBonusTable.SetActive(false);
	}

	public void btnOpenCloseBonusTableClick() {
		if (panelBonusTable.activeInHierarchy) panelBonusTable.SetActive(false); else panelBonusTable.SetActive(true);
		if (panelBonus) panelBonus.SetActive(false);

		if (panelInstructions) panelInstructions.SetActive (false);
		if (panelAddCredits) panelAddCredits.SetActive (false);
	}

	public void btnBetForBonusTableClick() {
		if (Settings.isDebug) Debug.Log("btnBetForBonusTableClick()");
		if (panelBonus) panelBonus.SetActive (true);
	}

	public void btnBonusBetRepeatClick() {
		if (Settings.isDebug) Debug.Log("btnBonusBetRepeatClick()");
		btnBonusPanelCloseClick();
	}
	
	public void btnBonusPanelCloseClick() {
		GameObject obj = GameObject.Find("InputBonusBetField");
		if (obj) {
			string textValue = obj.GetComponent<InputField>().text;
			double floatValue = 0;
			double.TryParse(textValue, out floatValue);
			if (floatValue > 0) {
				floatValue /= Settings.betCreditsMultiplier;
				Settings.betBonus = floatValue;
			}
		}
		if (Settings.betBonus > 0) {
			if (payTable != null) {
				game.player.balanceInCredits -= Settings.betBonus * Settings.betCreditsMultiplier;
				game.player.lblCredits.text = game.player.balanceInCredits.f();
				if (lblBetBonus) lblBetBonus.text = Settings.betBonus.to_b();
				if (payTable != null) payTable.SetBet(Settings.betBonus);
//				if (panelBonus) panelBonus.GetComponentInChildren<InputField>().text = Settings.betBonus.to_b();

			}
		}
		if (panelBonus) panelBonus.SetActive (false);
		// disable bonus buttons
		if (game.ui.btnBetBonus) game.ui.btnBetBonus.GetComponent<Button>().interactable = false;
		if (game.ui.btnBetBonusRepeat) game.ui.btnBetBonusRepeat.GetComponent<Button>().interactable = false;
		if (panelBonusTable) panelBonusTable.SetActive(false);

	}
	
	public void btnBonusBetMinClick() {
		Settings.betBonus += Settings.betBonusMin;
		if (Settings.betBonus > Settings.betBonusMax)
			Settings.betBonus = 0;
		if (payTable != null) payTable.SetBet(Settings.betBonus);
		if (panelBonus) panelBonus.GetComponentInChildren<InputField>().text = Settings.betBonus.to_b();
	}

	public void btnBonusBetSubClick() {
		Settings.betBonus -= Settings.betBonusMin;
		if (Settings.betBonus < 0)
			Settings.betBonus = Settings.betBonusMax;
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

		if (Settings.isLogined)
			this.GetBalance ();

		IsAutoBonusBet = GameObject.Find("AutoBonusToggle").GetComponent<Toggle>();

		panelAddCredits = GameObject.Find ("PanelAddCredits");
		if (panelAddCredits) {
			btnCreditOk = panelAddCredits.transform.FindChild ("btnOk").gameObject;
			if (btnCreditOk)
				btnCreditOk.GetComponent<Button> ().onClick.AddListener (() => btnCreditOkClickListener ());
		}

			
		panelGame = GameObject.Find ("PanelGame");
		panelInitBet = GameObject.Find ("PanelInitBet"); //GameObject.FindGameObjectWithTag("PanelInitBet");
		//panelBet = GameObject.Find("PanelBet");
		panelSurrender = GameObject.Find ("PanelSurrender");
			
		panelWin = GameObject.Find ("PanelWin");
		lblWinInfo = GameObject.Find ("lblWinInfo").GetComponent<Text>();
		if (panelWin)
			panelWin.SetActive (false);
		lblWinBonusInfo = GameObject.Find ("lblWinBonusInfo").GetComponent<Text>();

		panelBonus = GameObject.Find ("PanelBonus");
		btnBonusBetSet = GameObject.Find("btnBonusBetSet");

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
		btnBetBonus = GameObject.Find ("btnBetBonus");
		btnBetBonusRepeat = GameObject.Find ("btnBetBonusRepeat");

		btnCredit = GameObject.Find ("btnCredit");
		if (btnCredit)
			btnCredit.GetComponent<Button> ().onClick.AddListener (() => btnCreditAddClickListener ());
			
		btnAutoPlay = GameObject.Find ("btnAutoPlay");
		btnNewGame = GameObject.Find ("btnNewGame");
		btnAllIn = GameObject.Find ("btnAllIn");

		var lblPotObj = GameObject.Find ("lblPot");
		var lblRaiseObj = GameObject.Find ("lblRaise");
		var lblBetObj = GameObject.Find ("lblBet");
		var lblBetBonusObj = GameObject.Find ("lblBetBonus");
		var lblCallObj = GameObject.Find ("lblCall");

		if (lblPotObj) lblPot = lblPotObj.GetComponent<Text>();
		if (lblRaiseObj) lblRaise = lblRaiseObj.GetComponent<Text>();
		if (lblBetObj) lblBet = lblBetObj.GetComponent<Text>();
		if (lblBetBonusObj) lblBetBonus = lblBetBonusObj.GetComponent<Text>();
		if (lblCallObj) lblCall = lblCallObj.GetComponent<Text>();

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


		panelBonusTable = GameObject.Find ("BonusTableAndInstructions");

		payTable = new PayTable ();
		if (payTable != null) {
			payTable.Init();
			payTable.SelectColumnByIndex(9);
		}

		GameObject betBonusObj = GameObject.Find("BetBonusDropdown");
		if (betBonusObj)
			betBonusDropdown = betBonusObj.GetComponent<Dropdown>();
		betBonusDropdown.onValueChanged.AddListener(delegate {
			double bet = 0;
			double.TryParse(betBonusDropdown.captionText.text, out bet);
			if (bet > 0) bet /= Settings.betCreditsMultiplier;
			payTable.SetBet(bet);
		});

		if (panelBonusTable)
			panelBonusTable.SetActive (false);

		// avatar for real/live player
		avatar = GameObject.Find("Avatar0");
		if (avatar) {
			if (Settings.isLogined) {
				if (Settings.avatar != null) {
					avatar.GetComponent<Image>().sprite = Settings.avatar;
				} else {
					StartCoroutine("AvatarLoading");
				}
			} else {
				avatar.GetComponent<Image>().sprite = Resources.Load<Sprite>(Settings.avatarDefault);
			}
		}

//		InitCards (); // get dealers from parent object
		HideDynamicPanels ();

		panelInitBet.SetActive (true);

		game = new Game (this);

		ReInitGame ();

		InvokeRepeating ("UpdateInterval", Settings.updateInterval, Settings.updateInterval); // override default frequency of the update()

	}

	private IEnumerator AvatarLoading() {
		string urlFinal = Settings.facebookImageUrl + Settings.facebookMobileImageUrl;
		WWW www = new WWW(urlFinal);
		Debug.Log(urlFinal);
		yield return www;
		Texture2D profilePic = www.texture;
		Rect rect = new Rect(0, 0, profilePic.width, profilePic.height);
		Settings.avatar = Sprite.Create(profilePic, rect, new Vector2(0.5f, 0.5f), 100);
		avatar.GetComponent<Image>().sprite = Settings.avatar;
	}

	public IEnumerator DealCards() {
		DisableButtons(true);

		for (int i = 0; i < Settings.playerHandSizePreflop; i++) {
			for (var player = game.playerIterator.First(); !game.playerIterator.IsDoneFor; player = game.playerIterator.Next()) {
				var card = player.handPreflop.getCard (i);
				if (player.id == Settings.playerRealIndex || Settings.isDebug) {
					card.FaceUp = true; // show cards
				} else {
//					card.FaceUp = false; // hide cards
				}

				// show small cards
				if (player.id > 0) {
					player.VisibleSmallCard(game, true, i);
				}

				audio.PlayOneShot(soundDeal);
				yield return new WaitForSeconds (Settings.updateInterval);
			}
		}
		DisableButtons(false);
		game.state.isWaiting = false;
		game.playerIterator = new PlayerIterator (game.playerCollection);

	}


	public void DisableButtons(bool isDisable) {
		bool WillHide = false;
		if (game.ui.panelGame.activeInHierarchy) {
			
		} else {
			WillHide = true;
			game.ui.panelGame.SetActive(true);
		}

		if (isDisable) {

//			if (game.ui.panelGame)
			game.ui.btnRaise.GetComponent<Button>().interactable = false;
			game.ui.btnCall.GetComponent<Button>().interactable = false;
			game.ui.btnAllIn.GetComponent<Button>().interactable = false;
			game.ui.btnFold.GetComponent<Button>().interactable = false;
			game.ui.btnCheck.GetComponent<Button>().interactable = false;

		} else {

			game.ui.btnRaise.GetComponent<Button>().interactable = true;
			game.ui.btnCall.GetComponent<Button>().interactable = true;
			game.ui.btnAllIn.GetComponent<Button>().interactable = true;
			game.ui.btnFold.GetComponent<Button>().interactable = true;
			game.ui.btnCheck.GetComponent<Button>().interactable = true;

 		}

		if (WillHide) {
			game.ui.panelGame.SetActive(false);
		}
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

		//if (panelAddCredits) panelAddCredits.SetActive(false);

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

	private void PostBalance(string amount, string url) {
		WWWForm form = new WWWForm();
		form.AddField("a", amount);
		form.AddField("k", Settings.key);
		form.AddField("id", Settings.id);
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}

	public void SetBalance(string amount)
	{
		string url = string.Format("{0}/{1}", Settings.host, Settings.actionSetBalance);
		if (Settings.isDebug) Debug.Log(url);
		
		PostBalance(amount, url);
	}

	public void LoseBalance(string amount)
	{
		string url = string.Format("{0}/{1}", Settings.host, Settings.actionLoseBalance);
		if (Settings.isDebug) Debug.Log(url);
		
		PostBalance(amount, url);
	}

	public void WinBalance(string amount)
	{
		string url = string.Format("{0}/{1}", Settings.host, Settings.actionWinBalance);
		if (Settings.isDebug) Debug.Log(url);
		
		PostBalance(amount, url);
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
		var lblMyCreditsTitle = GameObject.Find("lblMyCredits") ;
		if (lblMyCreditsTitle) lblMyCreditsTitle.GetComponent<Text>().text = "syncing ... please wait ...";

		yield return www;
		// check for errors
		if (www.error == null && !string.IsNullOrEmpty(Settings.key))
		{
			double credits = 0;
			Double.TryParse(www.text, out credits);

			Settings.playerCredits = credits;
			game.player.balanceInCredits = credits;
			game.player.lblCredits.text = credits.f();
			if (lblMyCreditsTitle) lblMyCreditsTitle.GetComponent<Text>().text = credits.f();

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
		//Settings.OpenUrl (Settings.urlLogin);
		Settings.OpenUrlInNewTabAsExternalCall(Settings.urlLogin);
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
	public GameObject panelInitBet, panelGame, panelSurrender, panelAddCredits, panelHelp, panelInstructions, panelWin, panelBonus, panelBonusTable;
	public GameObject btnCheck, btnCall, btnRaise, btnFold, btnSurrender, btnStartGame, btnBetBonus, btnCreditOk, 
	btnRepeatBet, btnBetBonusRepeat, btnCredit, btnAutoPlay, btnNewGame, btnAllIn;
	public Text lblPot, lblRaise, lblBet, lblBetBonus, lblCall, lblPanelBet, lblPanelBetText, lblWinInfo, lblWinBonusInfo;
	public AudioSource audio;
	public AudioClip soundBtnClicked, soundDeal, soundRaise, soundVideoWin, soundWin, soundFold;
	public InputField inputBetField;
	public Toggle IsAutoBonusBet;
	public Dropdown betBonusDropdown;
	public GameObject btnBonusBetSet;
	public GameObject avatar;
}

