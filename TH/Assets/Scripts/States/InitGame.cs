using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class InitGame : BetRound {

	public override void LastAction ()
	{
		game.state = new AnteRound (game);
//		game.state = new AllInRound (game, game.players[0], 100); // TODO after test will remove
	}

	public InitGame(Game game2) {
		this.game = game2;

		isWaiting = true;

		game.isGameEnd = false;
		game.isGameRunning = false;

		if (game.ui.payTable != null) game.ui.payTable.SelectColumnByIndex(-1);
		game.betAmount = new Bet (0);

		if (game.ui.inputBetField) game.ui.inputBetField.text = game.betAmount.inCredits.f ();

//		game.ui.ClearAll ();
		if (game.ui.lblPot) game.ui.lblPot.text = Settings.betNull.f();
		if (game.ui.lblBet) game.ui.lblBet.text = Settings.betNull.f();
		if (game.ui.lblBetBonus) game.ui.lblBetBonus.text = Settings.betNull.f();
		if (game.ui.lblRaise) game.ui.lblRaise.text = Settings.betNull.f();
		if (game.ui.lblCall) game.ui.lblCall.text = Settings.betNull.f();
		
		game.ui.HideDynamicPanels ();
		game.ui.panelInitBet.SetActive (true);
		
		game.players = game.InitPlayers ();

		// public cards clearing
		if (game.cards != null)
		foreach (var cardPublic in game.cards) {
			cardPublic.isHidden = true;
		}

		game.cards = new List<Card> ();
		
		Card card = null;
		Image image = null;
		
		if (game.source == null)
			game.source = new Constants ();
		game.deck = new Deck ();
		game.deck.Shuffle ();
		
		// chips
		foreach (var player in game.players) {
//			player.SetChipRandomly();
			player.lblName.text = player.name;
			player.lblCredits.text = player.balanceInCredits.f();
			player.lblAction.text = "";
		}
		
		foreach (var player in game.players) {
			player.handPreflop = new Hand();
			for (int i = 1; i <= Settings.playerHandSizePreflop; i++) {
				card = game.deck.Deal ();
				var cardImg = GameObject.Find ("player" + player.id + "hold" + i);
				if (cardImg) {
					card.setImage (cardImg.GetComponent<Image> ());
					card.isHidden = true;
//					if (player.id == Settings.playerRealIndex || Settings.isDebug)
//						card.FaceUp = true;
//					else
//						card.FaceUp = false;
				}
				player.handPreflop.Add (card);
//				player.hand = player.handPreflop;
			}
			player.handPreflopString = player.GetStringByHand(player.handPreflop);

			// hide small cards
			player.VisibleSmallCards(game, false);
		}


		// flop
		for (int i = 1; i <= 3; i++) {
			card = game.deck.Deal ();
			image = GameObject.Find ("flop" + i).GetComponent<Image> ();
			card.setImage (image);
			if (Settings.isDebug)
				card.FaceUp = true;
			card.isHidden = true;
			game.cards.Add (card);
		}
		// turn
		card = game.deck.Deal ();
		image = GameObject.Find ("turn").GetComponent<Image> ();
		card.setImage (image);
		if (Settings.isDebug)
			card.FaceUp = true;
		card.isHidden = true;
		game.cards.Add (card);
		
		// river
		card = game.deck.Deal ();
		image = GameObject.Find ("river").GetComponent<Image> ();
		card.setImage (image);
		if (Settings.isDebug)
			card.FaceUp = true;
		card.isHidden = true;
		game.cards.Add (card);
		
		// start using of test data (straight)
		if (Settings.isTest) {
			var test = new TestData(game);
//			test.StraightSouldntFold();
//			test.ThreeOfAKindShouldNotFold();
//			test.StraightShouldNotFold(); // should not fold when raise and bet with 40 credits (flop)
//			test.LivePlayerWin();
			test.LivePlayerWinThreeOfAKind();

//			test.SplitPotRoyalFlush();
//			test.SplitPotThreeOfAKind();
		}
		// end using of test data (straight)

		foreach (var player in game.players) {
			player.hand = player.GetBestPlayerHand (game.cards);
			player.handPreflopString = player.GetStringByHand(player.handPreflop);
		}

		game.winners = game.GetWinners (game.players); // calculating the win percentage/hand strength
		game.players = game.GetPlayersAndSetWinPercentage (game.players); // calculating the win percentage/hand strength

		if (Settings.isDev)
		foreach(var player in game.players) {
			if (player.position == 0) {
				player.isFirstToAct = true;
			}
			if (player.position == game.players.Count - 1) {
				player.isLastToAct = true;
			}
			player.name = string.Format("#{0} {1} {2}", player.id, player.winPercent, player.GetHandStringFromHandObj());
			player.lblName.text = player.name;
//			player.LogDevInfo(player, false, false);
		}

		// using in update() of the game loop
		game.playerCollection = new PlayerCollection ();
		foreach (var p in game.players) {
//			p.betAlreadyInvestedInCurrentSubRound = 0;
			game.playerCollection[p.position] = p;
		}
		game.potAmount = 0;
		game.playerIterator = new PlayerIterator(game.playerCollection);
		isWaiting = false;

		if (game.ui.panelInitBet) game.ui.showPanelBet();//panelInitBet.SetActive(true);

		game.ui.btnRepeatBetPrepare();

		if (game.ui.btnStartGame) game.ui.btnStartGame.GetComponentInChildren<Text>().text = "START GAME";
		// if (btnStartGame) btnStartGame.GetComponent<Button>().onClick.Invoke();
		// if (lblPanelBet) lblPanelBet.GetComponent<Text>().text = "PLACE YOUR BET";
		if (game.ui.panelInitBet) game.ui.panelInitBet.SetActive(false);

		if (game.players [0].isReal) {
			if (game.ui.panelGame) game.ui.panelGame.SetActive(false);
			if (game.ui.panelInitBet) game.ui.showPanelBet();//panelInitBet.SetActive(true);
		} else {
			if (game.ui.panelGame) game.ui.showPanelGame();//panelGame.SetActive(true);
			if (game.ui.panelInitBet) game.ui.panelInitBet.SetActive(false);
		}

		// enable "bet bonus" button
		if (game.ui.panelBonusTable) {
			game.ui.panelBonusTable.SetActive (true);
		foreach (var player in game.players) {
			if (player.isReal) {
				if (player.balanceInCredits > 0) {
					if (game.ui.btnBetBonus) game.ui.btnBetBonus.GetComponent<Button>().interactable = true;
				} else {
					if (game.ui.btnBetBonus) game.ui.btnBetBonus.GetComponent<Button>().interactable = false;
				}

				// on/off auto bonus
				if (!game.ui.IsAutoBonusBet.isOn) {
					Settings.betBonus = 0;
				}
				
				// enable bonus bet feature
				if (game.ui.btnBonusBetSet) game.ui.btnBonusBetSet.GetComponent<Button>().interactable = true;
				if (game.ui.betBonusDropdown) game.ui.betBonusDropdown.GetComponent<Dropdown>().interactable = true;

				if (Settings.betBonus > 0) {
					if (game.ui.btnBetBonusRepeat) game.ui.btnBetBonusRepeat.GetComponent<Button>().interactable = true;
				} else {
					if (game.ui.btnBetBonusRepeat) game.ui.btnBetBonusRepeat.GetComponent<Button>().interactable = false;
				}
				if (game.ui.btnBetBonusRepeat) game.ui.btnBetBonusRepeat.GetComponentInChildren<Text>().text = string.Format("{0} {1}", Settings.btnBetBonusRepeat, Settings.betBonus.to_b());

					if (player.balanceInCredits < Settings.betCreditsMultiplier * Settings.bePreflopFlopMultiplier ) {
					if (game.ui.btnBetBonusRepeat) game.ui.btnBetBonusRepeat.GetComponent<Button>().interactable = false;

					game.ui.HideDynamicPanels();
					game.ui.panelWin.SetActive(true);
					game.ui.lblWinInfo.text = "Please add some credits and then click on OK button";

					if (game.ui.panelAddCredits) game.ui.panelAddCredits.SetActive(true); // open "add credits" panel
//					game.state.isWaiting = true;
				}

				break;
			}
		}
			Settings.btnBetBonusIsDone = false;
			game.ui.panelBonusTable.SetActive (false);
		}
		game.ui.lblWinBonusInfo.text = "";
	}
	
}
