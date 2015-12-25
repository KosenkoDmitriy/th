using System.Collections.Generic;

static class Settings
{
	public static double betBonusAmount = 0;
	public static double betMaxBonusAmount = 125;

	public static bool isPlayerWithNo = true;
    public static string key = "";
    public static int levelGame = 1;
    public static int levelMainMenu = 0;

    public static bool isLogined = false;
    public static double playerCredits = 500;

    public static readonly string http = "http://";
    public static readonly string host = http + "th.shopomob.ru";

    public static readonly string urlSignUp = host + "#sign_in_up";
    public static readonly string urlRestore = host + "/restore"; //TODO: implement on website
    public static readonly string urlLogin = urlSignUp;
    public static readonly string urlBuy = host + "#credits";// "/buy"; //TODO: implement on website
    public static readonly string urlInviteFriend = host + "/invite_friend"; //TODO: implement on website
    public static readonly string urlFortuneWheel = host + "#credits";//"/fortune_wheel"; //TODO: implement on website
    public static readonly string urlCredits = host + "#credits";//"/credits"; //TODO: implement on website

    public static readonly string actionLogin = "login";
    public static readonly string actionAdd = "add";
    public static readonly string actionSub = "sub";
    public static readonly string actionGetBalance = "get";
    public static readonly string actionSetBalance = "set";

    // cards
    public static string cardsPrefix = "cards_new/"; //"cards";
    //public static string cardBackName = cardsPrefix + "logo_back_cards"; // image name of the card back side
    public static string cardBackName = cardsPrefix + "card_back_black_with_logo";
    //"card_back_black_with_logo", "card_back_black_with_logo1", "card_back_black", "card_back_red" "card_back_red_with_logo"
    public static string cardBg = "card_background"; // the same color as in table
    // end cards

    public static int videoPokerLowRank = 15;
    public static int selectedColumn;

    public static bool isDebug = true;
    public static bool isIgnoreIniFile = true;
    public static bool testGame = false;
    public static bool logging = false;
    public static float updateInterval = 0.5f;

    public static int bonusTableMaxTitleSize = 10;  //unused
    public static int bonusTableWidth = 220;
    public static int bonusTableHeight = 170;
    public static int bonusTableFontSize = 10;

    public static int cardsSize = 52; 
    public static readonly int pockerHandPossibleSize = 5;
    public static readonly int playerSize = 6;
    public static readonly int playerVirtualSize = 5;
    public static readonly int kickerSize = 6;
    public static int year = 2010;
    public static int playerNameSize = 20;
    public static int playerCreditsLimit = 100;
    //public static int playerAutoPlayCredits = 1000;
    public static int intervalGameOver = 1000;

    public static readonly double betDx = 25; // .25;
    public static readonly double betMax = betDx * 6; // 1.5;

    public static double gameDenomination = betDx;
    public static readonly int GameDenominationDivider = 100;
    public static readonly int videoBonusMaxMultiplier = 5;
    public static readonly int betAmountAutoplay = 5;

    public static string dollar = " credits ";// { get; internal set; }
    public static double betNull = 0.00;
    public static double betCurrent = 0.00;

    // ini settings
    public static string pathToCurDir = "";// Directory.GetCurrentDirectory();
    public static string pathToAssetRes = "";// Directory.GetCurrentDirectory() + "\\Assets\\Resources\\";
    public static string iniFile = "TexasHoldem.idni";
    public static string logFile = "TexasHoldem.log";
    public static string datFile = "TexasHoldem.dat";

    public static bool videoBonusWinOnly = false;
    public static int surrenderReturnRank = 43; //100
    public static double PlayerRaiseFoldThreshold = 11; // 3.6
    public static int surrenderMinimumPair = 2; //4
    public static int highCardThreshhold = 11; //4
    public static int tempDelay = 1; //250;
    public static int nextPlayerDelay = 1;// 100;
    public static int dealDelay = tempDelay;
    public static int virtualPlayerRaiseLimit = 2; //1
    public static bool gameEnable = true; //false
    public static int gameDenomMultiplier = 6; //5
    public static int raiseLimitMultiplier = 2; //5
    public static int holeMinThreshold = 53;

    // dynamic help
    public static string foldString = "Ends the game and all bets are forfeit";
    public static string checkString = "Is a pass, no bet and the bet moves to the next player";
    public static string callString = "Matches, but doesn't raise the previous bets";
    public static string raiseString = "Bet more money then the previous bettor, this bet also bets the call amount";
    public static string allInString = "Bets all credits and the game continues on, but no more money can be added by you";
    public static string surrenderString = "Ends the game and 1/2 of your bet is returned to you. You then become the first to act on the next game";
    public static string continueString = "You have used the ALL IN and the game is pausing for you to see the betting action";
    public static string surrenderBoxString = "If you would like to SURRENDER your hand, click this surrender box. See instruction 8 for details.";
    public static string realPlayerName = "YOU"; // "PLAYER"
//	public static string yourName = realPlayerName;

    public static double jurisdictionalLimit = 0; // TODO: jurisdictionalLimit == jurisdictionalBETLimit in ini file
    public static int jurisdictionalBetLimit = 1000;
    public static readonly int gameMaxDenomMultiplier = 9999;
    public static readonly int foldLevelsSize = 8;
    public static readonly int raiseLevelsSize = 6;

    public static double currentIniVersion = 2.5;
    public static double iniVersion = 0.0;

    public static int gameNumber = 1;
    public static double creditsPlayed = 0;
    public static double creditsWon = 0;
    public static readonly int cardsOnDeckSize = 17;

    public static List<VirtualPlayer> GetVirtualPlayers()
    {
        List<VirtualPlayer> virtualPlayerList = new List<VirtualPlayer>() {
			GetVirtualPlayer0(),
			GetVirtualPlayer1(),
            GetVirtualPlayer2(),
            GetVirtualPlayer3(),
            GetVirtualPlayer4(),
            GetVirtualPlayer5(),
//            GetVirtualPlayer6(),
//			GetVirtualPlayer7(),
//			GetVirtualPlayer8(),
        };
        return virtualPlayerList;
    }

	public static void OpenUrl(string url) {
		#if UNITY_WEBPLAYER
		UnityEngine.Application.ExternalEval(string.Format("window.open('{0}', '_blank')", url));
		#elif UNITY_WEBGL
		UnityEngine.Application.ExternalEval(string.Format("window.open('{0}', '_blank')", url)); // open url in new tab
		#else
		UnityEngine.Application.OpenURL(url);
		#endif
	}
	
	private static int[] GetFlopNoRaiseBetPercentages() {
		return new int[] { 100, 100, 100, 100, 100, 75, 100, 100, 50, 50, 50, 40, 25, 40, 30, 20, 30, 0, 40, 0, 0, 0 };
	}
	
	private static int[] GetTurnNoRaiseBetPercentages() {
		return new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 60, 50, 40, 30, 00, 20, 10, 0, 10, 0, 0, 0, 0, 0 };
	}
	
	private static int[] GetRiverNoRaiseBetPercentages() {
		return new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 75, 60, 50, 20, 00, 10, 0, 0, -1, -1, -1, -1, -1, -1 };
	}

	private static VirtualPlayer GetVirtualPlayer0()
	{
		VirtualPlayer vp = new VirtualPlayer();
		vp.Name = "Otto the Auto Player";
		vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
		vp.HoleMinThreshold = 53;  //the lowest rank for play anything lower folds
		vp.RaiseLevels = new List<RaiseLevel>() {
			new RaiseLevel() { //1
				RaiseHands = IntRange(12, 20), // the hands that are raised on initial round or called in susequent
				Range = new double[] { 0, 0 },                   // the percentate range the real player must raise to use these hands
				RaisePercentage = 34,                           // the percentage of the pot  to raise with the above hands.
				ReRaiseRange = new double[] { 0, 0 },             // the percentage of the pot any of the players must raise after after this player has raised
				ReRaisePercentage = 0                           // the percentage of the pot we re raise if reraise level is met
			},
			new RaiseLevel() { //2
				RaiseHands = IntRange(5, 10),
				Range = new double[] { 0, 0 },
				RaisePercentage = 65,
				ReRaiseRange = new double[] { 50, gameMaxDenomMultiplier },
				ReRaisePercentage = 100
			},
			new RaiseLevel() { //3
				RaiseHands = new int[] { 1, 2, 3, 4, 11 },
				Range = new double[] { 0, 0 },
				RaisePercentage = 75,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //4
				RaiseHands = new int[] { 1, 2, 3 },
				Range = new double[] { 0, gameMaxDenomMultiplier },
				RaisePercentage = 60,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //5
				RaiseHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
				RaisePercentage = 0,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //6
				RaiseHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
				RaisePercentage = 0,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
		};
		vp.FoldLevels = new List<FoldLevel>() {
			new FoldLevel() { //1
				FoldHands = new int[] { 39,40,42,46,48,50,51,52 },      // the hands that are folded if in the prescribed range
				Range = new double[] { 61, gameMaxDenomMultiplier },    // the range all the players raised the pot to fold these hands
			},
			new FoldLevel() { //2
				FoldHands = IntRange(21, 36),
				Range = new double[] { 101, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //3
				FoldHands = IntRange(12, 20),
				Range = new double[] { 151, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //4
				FoldHands = IntRange(5, 10),
				Range = new double[] { 201, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //5
				FoldHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
			},
			new FoldLevel() { //6
				FoldHands = new int[] { 0 },
				Range = new double[] { 0, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //7
				FoldHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
			},
			new FoldLevel() { //8
				FoldHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
			},
		};
		// ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;| RF| SF|H4K|M4K| 4K| FH| FL| ST|H3K|M3K| 3K| 2P| PP| TP| MP| BP| 4F| 3F|4SI|3SI|4SO|3SO 
		// RF - royal flash, etc
		vp.FlopNoRaiseBetPercentages = GetFlopNoRaiseBetPercentages ();
		vp.TurnNoRaiseBetPercentages = GetTurnNoRaiseBetPercentages ();
		vp.RiverNoRaiseBetPercentages = GetRiverNoRaiseBetPercentages ();
		
		vp.BluffHands = new int[] { 3,5,6,8,10,12,16,18,27 };
		vp.BluffPercentage = 25;
		vp.BluffCallRaisePercentage = 0;

		vp.SlowPlayHands = new int[] { 0 };
		vp.AllInHands = new int[] { 0 };
		vp.Folded = false;

		return vp;
	}

    private static VirtualPlayer GetVirtualPlayer1()
    {
        VirtualPlayer vp = new VirtualPlayer();
        vp.Name = "Mr Tight Ted";
        vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
        vp.HoleMinThreshold = 53;  //the lowest rank for play anything lower folds
        vp.RaiseLevels = new List<RaiseLevel>() {
                new RaiseLevel() { //1
                    RaiseHands = new int[] { 7, 8, 9, 10, 12 }, // the hands that are raised on initial round or called in susequent
                    Range = new double[] { 0, 0 },                   // the percentate range the real player must raise to use these hands
                    RaisePercentage = 34,                           // the percentage of the pot  to raise with the above hands.
                    ReRaiseRange = new double[] { 0, 0 },             // the percentage of the pot any of the players must raise after after this player has raised
                    ReRaisePercentage = 0                           // the percentage of the pot we re raise if reraise level is met
                },
                new RaiseLevel() { //2
                    RaiseHands = new int[] { 3, 5, 6, 11 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 63,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 25
                },
                new RaiseLevel() { //3
                    RaiseHands = new int[] { 1, 2, 3 },
                    Range = new double[] { 0, gameMaxDenomMultiplier},
                    RaisePercentage = 60,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //4
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //5
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //6
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
            };
        vp.FoldLevels = new List<FoldLevel>() {
                new FoldLevel() { //1
                    FoldHands = new int[] { 45, 47 },                       // the hands that are folded if in the prescribed range
                    Range = new double[] { 11, gameMaxDenomMultiplier },    // the range all the players raised the pot to fold these hands
                },
                new FoldLevel() { //2
                    FoldHands = new int[] { 35,36,39,40,42,46,48,50,51,52 },
                    Range = new double[] { 42, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //3
                    FoldHands = IntRange(21, 34),
                    Range = new double[] { 52, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //4
                    FoldHands = IntRange(13, 20),
                    Range = new double[] { 76, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //5
                    FoldHands = new int[] { 3, 5, 6, 11 },
                    Range = new double[] { 120, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //6
                    FoldHands = new int[] { 7, 8, 9, 10, 12 },
                    Range = new double[] { 76, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //7
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
                new FoldLevel() { //8
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
            };

		vp.FlopNoRaiseBetPercentages = GetFlopNoRaiseBetPercentages ();
		vp.TurnNoRaiseBetPercentages = GetTurnNoRaiseBetPercentages ();
		vp.RiverNoRaiseBetPercentages = GetRiverNoRaiseBetPercentages ();

        vp.BluffHands = new int[] { 6, 7, 12, 13, 16, 18 };
		vp.BluffPercentage = 15;
		vp.BluffCallRaisePercentage = 0;

		vp.SlowPlayHands = new int[] { 0 };
        vp.AllInHands = new int[] { 0 };
        vp.Folded = false;
        return vp;
    }

    private static VirtualPlayer GetVirtualPlayer2()
    {
        VirtualPlayer vp = new VirtualPlayer();
        vp.Name = "Mr Flop";
        vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
        vp.HoleMinThreshold = 104;  //the lowest rank for play anything lower folds
        vp.RaiseLevels = new List<RaiseLevel>() {
                new RaiseLevel() { //1
                    RaiseHands = new int[] { 1, 2, 3 }, // the hands that are raised on initial round or called in susequent
                    Range = new double[] { 0, gameMaxDenomMultiplier },                   // the percentate range the real player must raise to use these hands
                    RaisePercentage = 60,                           // the percentage of the pot  to raise with the above hands.
                    ReRaiseRange = new double[] { 0, 0 },             // the percentage of the pot any of the players must raise after after this player has raised
                    ReRaisePercentage = 0                           // the percentage of the pot we re raise if reraise level is met
                },
                new RaiseLevel() { //2
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //3
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //4
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //5
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //6
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
            };
        vp.FoldLevels = new List<FoldLevel>() {
                new FoldLevel() { //1
                    FoldHands = new int[] { 81, 84, 91, 101, 102, 104 },                       // the hands that are folded if in the prescribed range
                    Range = new double[] { 24, gameMaxDenomMultiplier },    // the range all the players raised the pot to fold these hands
                },
                new FoldLevel() { //2
                    FoldHands = IntRange(53, 76),
                    Range = new double[] { 49, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //3
                    FoldHands = IntRange(28, 52),
                    Range = new double[] { 69, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //4
                    FoldHands = IntRange(13, 27),
                    Range = new double[] { 79, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //5
                    FoldHands = new int[] { 7, 8, 9, 10, 12 },
                    Range = new double[] { 89, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //6
                    FoldHands = new int[] { 5, 6, 11 },
                    Range = new double[] { 111, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //7
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
                new FoldLevel() { //8
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
            };
        vp.FlopNoRaiseBetPercentages = GetFlopNoRaiseBetPercentages ();
        vp.TurnNoRaiseBetPercentages = GetTurnNoRaiseBetPercentages ();
        vp.RiverNoRaiseBetPercentages = GetRiverNoRaiseBetPercentages ();

        vp.BluffHands = new int[] { 0 };
		vp.BluffPercentage = 0;
		vp.BluffCallRaisePercentage = 0;

		vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
        vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
        vp.Folded = false;

        return vp;
    }

    private static VirtualPlayer GetVirtualPlayer3()
    {
        VirtualPlayer vp = new VirtualPlayer();
        vp.Name = "Solid Sam";
        vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
        vp.HoleMinThreshold = 53;  //the lowest rank for play anything lower folds
        vp.RaiseLevels = new List<RaiseLevel>() {
                new RaiseLevel() { //1
                    RaiseHands = IntRange(12, 20), // the hands that are raised on initial round or called in susequent
                    Range = new double[] { 0, 0 },                   // the percentate range the real player must raise to use these hands
                    RaisePercentage = 34,                           // the percentage of the pot  to raise with the above hands.
                    ReRaiseRange = new double[] { 0, 0 },             // the percentage of the pot any of the players must raise after after this player has raised
                    ReRaisePercentage = 0                           // the percentage of the pot we re raise if reraise level is met
                },
                new RaiseLevel() { //2
                    RaiseHands = IntRange(5, 10),
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 65,
                    ReRaiseRange = new double[] { 50, 9999 },
                    ReRaisePercentage = 100
                },
                new RaiseLevel() { //3
                    RaiseHands = IntRange(1, 4, new int[] { 11 }),
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 75,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //4
                    RaiseHands = new int[] { 1, 2, 3 },
                    Range = new double[] { 0, gameMaxDenomMultiplier },
                    RaisePercentage = 60,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //5
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //6
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
            };
        vp.FoldLevels = new List<FoldLevel>() {
                new FoldLevel() { //1
                    FoldHands = new int[] { 39, 40, 42, 46, 48, 50, 51, 52 },                       // the hands that are folded if in the prescribed range
                    Range = new double[] { 61, gameMaxDenomMultiplier },    // the range all the players raised the pot to fold these hands
                },
                new FoldLevel() { //2
                    FoldHands = IntRange(21, 36),
                    Range = new double[] { 101, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //3
                    FoldHands = IntRange(12, 20),
                    Range = new double[] { 151, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //4
                    FoldHands = IntRange(5, 10),
                    Range = new double[] { 201, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //5
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
                new FoldLevel() { //6
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
                new FoldLevel() { //7
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
                new FoldLevel() { //8
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
            };
        vp.FlopNoRaiseBetPercentages = GetFlopNoRaiseBetPercentages ();
        vp.TurnNoRaiseBetPercentages = GetTurnNoRaiseBetPercentages ();
        vp.RiverNoRaiseBetPercentages = GetRiverNoRaiseBetPercentages ();

        vp.BluffHands = new int[] { 3, 5, 6, 8, 10, 12, 16, 18, 27 };
		vp.BluffPercentage = 25;
		vp.BluffCallRaisePercentage = 0;

		vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
        vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
        vp.Folded = false;
        
		return vp;
    }

    private static VirtualPlayer GetVirtualPlayer4()
    {
        VirtualPlayer vp = new VirtualPlayer();
        vp.Name = "Loose Lou";
        vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
        vp.HoleMinThreshold = 105;  //the lowest rank for play anything lower folds
        vp.RaiseLevels = new List<RaiseLevel>() {
                new RaiseLevel() { //1
                    RaiseHands = IntRange(37, 52), // the hands that are raised on initial round or called in susequent
                    Range = new double[] { 0, 0 },                   // the percentate range the real player must raise to use these hands
                    RaisePercentage = 25,                           // the percentage of the pot  to raise with the above hands.
                    ReRaiseRange = new double[] { 0, 0 },             // the percentage of the pot any of the players must raise after after this player has raised
                    ReRaisePercentage = 0                           // the percentage of the pot we re raise if reraise level is met
                },
                new RaiseLevel() { //2
                    RaiseHands = IntRange(21, 36),
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 72,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //3
                    RaiseHands = IntRange(12, 20),
                    Range = new double[] { 86, gameMaxDenomMultiplier },
                    RaisePercentage = 74,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //4
                    RaiseHands = IntRange(5, 10),
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 65,
                    ReRaiseRange = new double[] { 111, gameMaxDenomMultiplier },
                    ReRaisePercentage = 100
                },
                new RaiseLevel() { //5
                    RaiseHands = new int[] { 1, 2, 3, 4, 11 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 100,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //6
                    RaiseHands = new int[] { 1, 2, 3 },
                    Range = new double[] { 0, gameMaxDenomMultiplier },
                    RaisePercentage = 60,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
            };
        vp.FoldLevels = new List<FoldLevel>() {
                new FoldLevel() { //1
                    FoldHands = new int[] { 54, 57, 62, 70, 74, 77, 88, 89, 90, 94, 95, 96, 97, 98, 100, 103 },                       // the hands that are folded if in the prescribed range
                    Range = new double[] { 11, gameMaxDenomMultiplier },    // the range all the players raised the pot to fold these hands
                },
                new FoldLevel() { //2
                    FoldHands = new int[] { 77, 79, 80, 81, 82, 83, 84, 85, 86, 91, 92, 93, 99, 101, 102, 104, 105 },
                    Range = new double[] { 49, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //3
                    FoldHands = new int[] { 53, 55, 56, 58, 59, 60, 61, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 75, 76},
                    Range = new double[] { 74, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //4
                    FoldHands = IntRange(37, 52),
                    Range = new double[] { 45, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //5
                    FoldHands = IntRange(21, 36),
                    Range = new double[] { 74, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //6
                    FoldHands = IntRange(12, 20),
                    Range = new double[] { 100, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //7
                    FoldHands = IntRange(5, 10),
                    Range = new double[] { 111, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //8
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
            };
        vp.FlopNoRaiseBetPercentages = GetFlopNoRaiseBetPercentages ();
        vp.TurnNoRaiseBetPercentages = GetTurnNoRaiseBetPercentages ();
        vp.RiverNoRaiseBetPercentages = GetRiverNoRaiseBetPercentages ();

        vp.BluffHands = new int[] { 3, 5, 6, 7, 8, 10, 16, 17, 21, 27, 29, 36, 46, 50 };
        vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
        vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
        vp.BluffPercentage = 20;
        vp.BluffCallRaisePercentage = 0;

        vp.Folded = false;
        return vp;
    }

    private static VirtualPlayer GetVirtualPlayer5()
    {
        VirtualPlayer vp = new VirtualPlayer();
        vp.Name = "Mr Ace";
        vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
        vp.HoleMinThreshold = 24;  //the lowest rank for play anything lower folds
        vp.RaiseLevels = new List<RaiseLevel>() {
                new RaiseLevel() { //1
                    RaiseHands = new int[] { 7, 8, 9, 10, 12 }, // the hands that are raised on initial round or called in susequent
                    Range = new double[] { 0, 0 },                   // the percentate range the real player must raise to use these hands
                    RaisePercentage = 34,                           // the percentage of the pot  to raise with the above hands.
                    ReRaiseRange = new double[] { 0, 0 },             // the percentage of the pot any of the players must raise after after this player has raised
                    ReRaisePercentage = 0                           // the percentage of the pot we re raise if reraise level is met
                },
                new RaiseLevel() { //2
                    RaiseHands = new int[] { 3, 5, 6, 11 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 53,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //3
                    RaiseHands = new int[] { 1, 2, 4 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 66,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //4
                    RaiseHands = new int[] { 1, 2, 3 },
                    Range = new double[] { 0, gameMaxDenomMultiplier },
                    RaisePercentage = 60,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //5
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //6
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
            };
        vp.FoldLevels = new List<FoldLevel>() {
                new FoldLevel() { //1
                    FoldHands = new int[] { 45, 47 },                       // the hands that are folded if in the prescribed range
                    Range = new double[] { 11, gameMaxDenomMultiplier },    // the range all the players raised the pot to fold these hands
                },
                new FoldLevel() { //2
                    FoldHands = new int[] { 35, 36, 39, 40, 46, 48, 50, 51, 52 },
                    Range = new double[] { 42, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //3
                    FoldHands = IntRange(21, 34),
                    Range = new double[] { 52, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //4
                    FoldHands = new int[] { 30,32,33,39,42,43,76,91,101,102,104 },
                    Range = new double[] { 66, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //5
                    FoldHands = IntRange(13, 20),
                    Range = new double[] { 76, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //6
                    FoldHands = new int[] { 3,5,6,11 },
                    Range = new double[] { 121, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //7
                    FoldHands = new int[] { 7,8,9,10,12 },
                    Range = new double[] { 76, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //8
                    FoldHands = new int[] { 3,5,6,11 },
                    Range = new double[] { 121, gameMaxDenomMultiplier },
                },
            };
        vp.FlopNoRaiseBetPercentages = GetFlopNoRaiseBetPercentages ();
        vp.TurnNoRaiseBetPercentages = GetTurnNoRaiseBetPercentages ();
        vp.RiverNoRaiseBetPercentages = GetRiverNoRaiseBetPercentages ();

        vp.BluffHands = new int[] { 12, 18, 19, 24, 27, 28, 30, 32, 33, 34, 39, 42 };
		vp.BluffPercentage = 30;
		vp.BluffCallRaisePercentage = 0;

		vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
        vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
        vp.Folded = false;
        
		return vp;
    }

    private static VirtualPlayer GetVirtualPlayer6()
    {
        VirtualPlayer vp = new VirtualPlayer();
        vp.Name = "Mr Super Tight";
        vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
        vp.HoleMinThreshold = 53;  //the lowest rank for play anything lower folds
        vp.RaiseLevels = new List<RaiseLevel>() {
                new RaiseLevel() { //1
                    RaiseHands = IntRange(7, 10, new int[] { 12 }), // the hands that are raised on initial round or called in susequent
                    Range = new double[] { 0, 0 },                   // the percentate range the real player must raise to use these hands
                    RaisePercentage = 34,                           // the percentage of the pot  to raise with the above hands.
                    ReRaiseRange = new double[] { 0, 0 },             // the percentage of the pot any of the players must raise after after this player has raised
                    ReRaisePercentage = 0                           // the percentage of the pot we re raise if reraise level is met
                },
                new RaiseLevel() { //2
                    RaiseHands = new int[] { 3, 5, 6, 11 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 63,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 25
                },
                new RaiseLevel() { //3
                    RaiseHands = new int[] { 1, 2, 3 },
                    Range = new double[] { 0, gameMaxDenomMultiplier },
                    RaisePercentage = 60,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //4
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //5
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
                new RaiseLevel() { //6
                    RaiseHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                    RaisePercentage = 0,
                    ReRaiseRange = new double[] { 0, 0 },
                    ReRaisePercentage = 0
                },
            };
        vp.FoldLevels = new List<FoldLevel>() {
                new FoldLevel() { //1
                    FoldHands = new int[] { 45, 47 },                       // the hands that are folded if in the prescribed range
                    Range = new double[] { 11, gameMaxDenomMultiplier },    // the range all the players raised the pot to fold these hands
                },
                new FoldLevel() { //2
                    FoldHands = new int[] { 35, 36, 39, 40, 46, 48, 50, 51, 52 },
                    Range = new double[] { 42, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //3
                    FoldHands = IntRange(21, 34),
                    Range = new double[] { 52, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //4
                    FoldHands = IntRange(13, 20),
                    Range = new double[] { 76, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //5
                    FoldHands = new int[] { 3, 5, 6, 11 },
                    Range = new double[] { 120, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //6
                    FoldHands = IntRange(7, 10, new int[] { 12 }),
                    Range = new double[] { 76, gameMaxDenomMultiplier },
                },
                new FoldLevel() { //7
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
                new FoldLevel() { //8
                    FoldHands = new int[] { 0 },
                    Range = new double[] { 0, 0 },
                },
            };
        vp.FlopNoRaiseBetPercentages = GetFlopNoRaiseBetPercentages ();
        vp.TurnNoRaiseBetPercentages = GetTurnNoRaiseBetPercentages ();
        vp.RiverNoRaiseBetPercentages = GetRiverNoRaiseBetPercentages ();

        vp.BluffHands = new int[] { 0 };
        vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
        vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
        vp.BluffPercentage = 0;
        vp.BluffCallRaisePercentage = 0;

        vp.Folded = false;
        return vp;
    }

	private static VirtualPlayer GetVirtualPlayer7()
	{
		VirtualPlayer vp = new VirtualPlayer();
		vp.Name = "Mr Telephone";
		vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
		vp.HoleMinThreshold = 105;  //the lowest rank for play anything lower folds
		vp.RaiseLevels = new List<RaiseLevel>() {
			new RaiseLevel() { //1
				RaiseHands = new int[] { 1, 2, 3 }, // the hands that are raised on initial round or called in susequent
				Range = new double[] { 0, gameMaxDenomMultiplier },                   // the percentate range the real player must raise to use these hands
				RaisePercentage = 60,                           // the percentage of the pot  to raise with the above hands.
				ReRaiseRange = new double[] { 0, 0 },             // the percentage of the pot any of the players must raise after after this player has raised
				ReRaisePercentage = 0                           // the percentage of the pot we re raise if reraise level is met
			},
			new RaiseLevel() { //2
				RaiseHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
				RaisePercentage = 0,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //3
				RaiseHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
				RaisePercentage = 0,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //4
				RaiseHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
				RaisePercentage = 0,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //5
				RaiseHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
				RaisePercentage = 0,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //6
				RaiseHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
				RaisePercentage = 0,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
		};
		vp.FoldLevels = new List<FoldLevel>() {
			new FoldLevel() { //1
				FoldHands = IntRange(81, 105),                       // the hands that are folded if in the prescribed range
				Range = new double[] { 12, gameMaxDenomMultiplier },    // the range all the players raised the pot to fold these hands
			},
			new FoldLevel() { //2
				FoldHands = IntRange(50, 80),
				Range = new double[] { 24, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //3
				FoldHands = IntRange(36, 49),
				Range = new double[] { 49, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //4
				FoldHands = IntRange(26, 35),
				Range = new double[] { 69, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //5
				FoldHands = IntRange(13, 25),
				Range = new double[] { 79, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //6
				FoldHands = IntRange(7, 10, new int[] { 12 }),
				Range = new double[] { 89, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //7
				FoldHands = new int[] { 5, 6, 11 },
				Range = new double[] { 111, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //8
				FoldHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
			},
		};
		vp.FlopNoRaiseBetPercentages = GetFlopNoRaiseBetPercentages ();
		vp.TurnNoRaiseBetPercentages = GetTurnNoRaiseBetPercentages ();
		vp.RiverNoRaiseBetPercentages = GetRiverNoRaiseBetPercentages ();
		
		vp.BluffHands = new int[] { 0 };
		vp.BluffPercentage = 0;
		vp.BluffCallRaisePercentage = 0;

		vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
		vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
		vp.Folded = false;
	
		return vp;
	}
    
	private static VirtualPlayer GetVirtualPlayer8()
	{
		VirtualPlayer vp = new VirtualPlayer();
		vp.Name = "Mrs. Loose Lou";
		vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
		vp.HoleMinThreshold = 105;  //the lowest rank for play anything lower folds

		vp.RaiseLevels = new List<RaiseLevel>() {
			new RaiseLevel() { //1
				RaiseHands = IntRange(37, 52), // the hands that are raised on initial round or called in susequent
				Range = new double[] { 0, 0 },                   // the percentate range the real player must raise to use these hands
				RaisePercentage = 25,                           // the percentage of the pot  to raise with the above hands.
				ReRaiseRange = new double[] { 0, 0 },             // the percentage of the pot any of the players must raise after after this player has raised
				ReRaisePercentage = 0                           // the percentage of the pot we re raise if reraise level is met
			},
			new RaiseLevel() { //2
				RaiseHands = IntRange(21, 36),
				Range = new double[] { 0, 0 },
				RaisePercentage = 71,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //3
				RaiseHands = IntRange(12, 20),
				Range = new double[] { 86, gameMaxDenomMultiplier },
				RaisePercentage = 74,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //4
				RaiseHands = IntRange(5, 10),
				Range = new double[] { 0, 0 },
				RaisePercentage = 65,
				ReRaiseRange = new double[] { 111, gameMaxDenomMultiplier },
				ReRaisePercentage = 100
			},
			new RaiseLevel() { //5
				RaiseHands = new int[] { 1, 2, 3, 4, 11 },
				Range = new double[] { 0, 0 },
				RaisePercentage = 100,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
			new RaiseLevel() { //6
				RaiseHands = new int[] { 1, 2, 3 },
				Range = new double[] { 0, gameMaxDenomMultiplier },
				RaisePercentage = 60,
				ReRaiseRange = new double[] { 0, 0 },
				ReRaisePercentage = 0
			},
		};
		vp.FoldLevels = new List<FoldLevel>() {
			new FoldLevel() { //1
				FoldHands = IntRange(94, 98, new int[] { 54,57,62,70,74,77,88,89,90,100,103 }),                       // the hands that are folded if in the prescribed range
				Range = new double[] { 11, gameMaxDenomMultiplier },    // the range all the players raised the pot to fold these hands
			},
			new FoldLevel() { //2
				FoldHands = IntRange(80, 86, new int[] { 77,79,91,92,93,99,101,102,104,105 }),                       // the hands that are folded if in the prescribed range
				Range = new double[] { 49, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //3
				FoldHands = IntRange(63, 71, new int[] { 53, 55, 56, 58, 59, 60, 61, 72, 73, 75, 76 }),
				Range = new double[] { 74, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //4
				FoldHands = IntRange(37, 52),
				Range = new double[] { 45, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //5
				FoldHands = IntRange(21, 36),
				Range = new double[] { 74, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //6
				FoldHands = IntRange(12, 20),
				Range = new double[] { 100, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //7
				FoldHands = IntRange(5, 10),
				Range = new double[] { 111, gameMaxDenomMultiplier },
			},
			new FoldLevel() { //8
				FoldHands = new int[] { 0 },
				Range = new double[] { 0, 0 },
			},
		};
		vp.FlopNoRaiseBetPercentages = GetFlopNoRaiseBetPercentages ();
		vp.TurnNoRaiseBetPercentages = GetTurnNoRaiseBetPercentages ();
		vp.RiverNoRaiseBetPercentages = GetRiverNoRaiseBetPercentages ();
		
		vp.BluffHands = new int[] { 3,5,6,7,8,10,16,17,21,27,29,36,46,50 };
		vp.BluffPercentage = 40;
		vp.BluffCallRaisePercentage = 0;
		
		vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
		vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
		vp.Folded = false;
		
		return vp;
	}

	private static int[] IntRange(int min, int max)
    {
        List<int> intList = new List<int>();
        for (int i = min; i <= max; i++)
            intList.Add(i);
        return intList.ToArray();
    }

    private static int[] IntRange(int min, int max, int[] arr)
    {
        List<int> intList = new List<int>();
        for (int i = min; i <= max; i++)
            intList.Add(i);
        foreach (int v in arr)
            intList.Add(v);
        return intList.ToArray();
    }

	public enum cardValues
	{
		US = 0, // off suit/ unsuit?
		S = 1,  // suit
		_2 = 2,
		_3 = 3,
		_4 = 4,
		_5 = 5,
		_6 = 6,
		_7 = 7,
		_8 = 8,
		_9 = 9,
		T = 10,
		J = 11,
		Q = 12,
		K = 13,
		A = 14,
		G1 = 1,
		G2 = 2,
		G3 = 3,
		G4 = 4,
		G5 = 5,
		G6 = 6,
		G7 = 7,
		G8 = 8,
		ANY = 0xFF
	}

	/*public static readonly cardValues[,] Group = new cardValues[,] {
		{ cardValues._2, cardValues._2, cardValues.ANY },
		{ cardValues._3, cardValues._2, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.S },
		{ cardValues._3, cardValues._2, cardValues.US },
		{ cardValues._3, cardValues._3, cardValues.ANY },
		{ cardValues._4, cardValues._3, cardValues.S },
		{ cardValues._5, cardValues._3, cardValues.S },
		{ cardValues._6, cardValues._3, cardValues.S },
		{ cardValues._7, cardValues._3, cardValues.S },
		{ cardValues._8, cardValues._3, cardValues.S },
		{ cardValues._9, cardValues._3, cardValues.S },
		{ cardValues.T, cardValues._3, cardValues.S },
		{ cardValues.J, cardValues._3, cardValues.S },
		{ cardValues.Q, cardValues._3, cardValues.S },
		{ cardValues.K, cardValues._3, cardValues.S },
		{ cardValues.A, cardValues._3, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.US },
		{ cardValues._4, cardValues._3, cardValues.US },
		{ cardValues._4, cardValues._4, cardValues.ANY },
		{ cardValues._5, cardValues._4, cardValues.S },
		{ cardValues._6, cardValues._4, cardValues.S },
		{ cardValues._7, cardValues._4, cardValues.S },
		{ cardValues._8, cardValues._4, cardValues.S },
		{ cardValues._9, cardValues._4, cardValues.S },
		{ cardValues.T, cardValues._4, cardValues.S },
		{ cardValues.J, cardValues._4, cardValues.S },
		{ cardValues.Q, cardValues._4, cardValues.S },
		{ cardValues.K, cardValues._4, cardValues.S },
		{ cardValues.A, cardValues._4, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.US },
		{ cardValues._5, cardValues._3, cardValues.US },
		{ cardValues._5, cardValues._4, cardValues.US },
		{ cardValues._5, cardValues._5, cardValues.ANY },
		{ cardValues._6, cardValues._5, cardValues.S },
		{ cardValues._7, cardValues._5, cardValues.S },
		{ cardValues._8, cardValues._5, cardValues.S },
		{ cardValues._9, cardValues._5, cardValues.S },
		{ cardValues.T, cardValues._5, cardValues.S },
		{ cardValues.J, cardValues._5, cardValues.S },
		{ cardValues.Q, cardValues._5, cardValues.S },
		{ cardValues.K, cardValues._5, cardValues.S },
		{ cardValues.A, cardValues._5, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.US },
		{ cardValues._6, cardValues._3, cardValues.US },
		{ cardValues._6, cardValues._4, cardValues.US },
		{ cardValues._6, cardValues._5, cardValues.US },
		{ cardValues._6, cardValues._6, cardValues.ANY },
		{ cardValues._7, cardValues._6, cardValues.S },
		{ cardValues._8, cardValues._6, cardValues.S },
		{ cardValues._9, cardValues._6, cardValues.S },
		{ cardValues.T, cardValues._6, cardValues.S },
		{ cardValues.J, cardValues._6, cardValues.S },
		{ cardValues.Q, cardValues._6, cardValues.S },
		{ cardValues.K, cardValues._6, cardValues.S },
		{ cardValues.A, cardValues._6, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.US },
		{ cardValues._7, cardValues._3, cardValues.US },
		{ cardValues._7, cardValues._4, cardValues.US },
		{ cardValues._7, cardValues._5, cardValues.US },
		{ cardValues._7, cardValues._6, cardValues.US },
		{ cardValues._7, cardValues._7, cardValues.ANY },
		{ cardValues._8, cardValues._7, cardValues.S },
		{ cardValues._9, cardValues._7, cardValues.S },
		{ cardValues.T, cardValues._7, cardValues.S },
		{ cardValues.J, cardValues._7, cardValues.S },
		{ cardValues.Q, cardValues._7, cardValues.S },
		{ cardValues.K, cardValues._7, cardValues.S },
		{ cardValues.A, cardValues._7, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.US },
		{ cardValues._8, cardValues._3, cardValues.US },
		{ cardValues._8, cardValues._4, cardValues.US },
		{ cardValues._8, cardValues._5, cardValues.US },
		{ cardValues._8, cardValues._6, cardValues.US },
		{ cardValues._8, cardValues._7, cardValues.US },
		{ cardValues._8, cardValues._8, cardValues.ANY },
		{ cardValues._9, cardValues._8, cardValues.S },
		{ cardValues.T, cardValues._8, cardValues.S },
		{ cardValues.J, cardValues._8, cardValues.S },
		{ cardValues.Q, cardValues._8, cardValues.S },
		{ cardValues.K, cardValues._8, cardValues.S },
		{ cardValues.A, cardValues._8, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.US },
		{ cardValues._9, cardValues._3, cardValues.US },
		{ cardValues._9, cardValues._4, cardValues.US },
		{ cardValues._9, cardValues._5, cardValues.US },
		{ cardValues._9, cardValues._6, cardValues.US },
		{ cardValues._9, cardValues._7, cardValues.US },
		{ cardValues._9, cardValues._8, cardValues.US },
		{ cardValues._9, cardValues._9, cardValues.ANY },
		{ cardValues.T, cardValues._9, cardValues.S },
		{ cardValues.J, cardValues._9, cardValues.S },
		{ cardValues.Q, cardValues._9, cardValues.S },
		{ cardValues.K, cardValues._9, cardValues.S },
		{ cardValues.A, cardValues._9, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.US },
		{ cardValues.T, cardValues._3, cardValues.US },
		{ cardValues.T, cardValues._4, cardValues.US },
		{ cardValues.T, cardValues._5, cardValues.US },
		{ cardValues.T, cardValues._6, cardValues.US },
		{ cardValues.T, cardValues._7, cardValues.US },
		{ cardValues.T, cardValues._8, cardValues.US },
		{ cardValues.T, cardValues._9, cardValues.US },
		{ cardValues.T, cardValues.T, cardValues.ANY },
		{ cardValues.J, cardValues.T, cardValues.S },
		{ cardValues.Q, cardValues.T, cardValues.S },
		{ cardValues.K, cardValues.T, cardValues.S },
		{ cardValues.A, cardValues.T, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.US },
		{ cardValues.J, cardValues._3, cardValues.US },
		{ cardValues.J, cardValues._4, cardValues.US },
		{ cardValues.J, cardValues._5, cardValues.US },
		{ cardValues.J, cardValues._6, cardValues.US },
		{ cardValues.J, cardValues._7, cardValues.US },
		{ cardValues.J, cardValues._8, cardValues.US },
		{ cardValues.J, cardValues._9, cardValues.US },
		{ cardValues.J, cardValues.T, cardValues.US },
		{ cardValues.J, cardValues.J, cardValues.ANY },
		{ cardValues.Q, cardValues.J, cardValues.S },
		{ cardValues.K, cardValues.J, cardValues.S },
		{ cardValues.A, cardValues.J, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.US },
		{ cardValues.Q, cardValues._3, cardValues.US },
		{ cardValues.Q, cardValues._4, cardValues.US },
		{ cardValues.Q, cardValues._5, cardValues.US },
		{ cardValues.Q, cardValues._6, cardValues.US },
		{ cardValues.Q, cardValues._7, cardValues.US },
		{ cardValues.Q, cardValues._8, cardValues.US },
		{ cardValues.Q, cardValues._9, cardValues.US },
		{ cardValues.Q, cardValues.T, cardValues.US },
		{ cardValues.Q, cardValues.J, cardValues.US },
		{ cardValues.Q, cardValues.Q, cardValues.ANY },
		{ cardValues.K, cardValues.Q, cardValues.S },
		{ cardValues.A, cardValues.Q, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.US },
		{ cardValues.K, cardValues._3, cardValues.US },
		{ cardValues.K, cardValues._4, cardValues.US },
		{ cardValues.K, cardValues._5, cardValues.US },
		{ cardValues.K, cardValues._6, cardValues.US },
		{ cardValues.K, cardValues._7, cardValues.US },
		{ cardValues.K, cardValues._8, cardValues.US },
		{ cardValues.K, cardValues._9, cardValues.US },
		{ cardValues.K, cardValues.T, cardValues.US },
		{ cardValues.K, cardValues.J, cardValues.US },
		{ cardValues.K, cardValues.Q, cardValues.US },
		{ cardValues.K, cardValues.K, cardValues.ANY },
		{ cardValues.A, cardValues.K, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.US },
		{ cardValues.A, cardValues._3, cardValues.US },
		{ cardValues.A, cardValues._4, cardValues.US },
		{ cardValues.A, cardValues._5, cardValues.US },
		{ cardValues.A, cardValues._6, cardValues.US },
		{ cardValues.A, cardValues._7, cardValues.US },
		{ cardValues.A, cardValues._8, cardValues.US },
		{ cardValues.A, cardValues._9, cardValues.US },
		{ cardValues.A, cardValues.T, cardValues.US },
		{ cardValues.A, cardValues.J, cardValues.US },
		{ cardValues.A, cardValues.Q, cardValues.US },
		{ cardValues.A, cardValues.K, cardValues.US },
		{ cardValues.A, cardValues.A, cardValues.ANY },
		{ cardValues._2, cardValues._2, cardValues.ANY },
		{ cardValues._3, cardValues._2, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.S },
		{ cardValues._3, cardValues._2, cardValues.US },
		{ cardValues._3, cardValues._3, cardValues.ANY },
		{ cardValues._4, cardValues._3, cardValues.S },
		{ cardValues._5, cardValues._3, cardValues.S },
		{ cardValues._6, cardValues._3, cardValues.S },
		{ cardValues._7, cardValues._3, cardValues.S },
		{ cardValues._8, cardValues._3, cardValues.S },
		{ cardValues._9, cardValues._3, cardValues.S },
		{ cardValues.T, cardValues._3, cardValues.S },
		{ cardValues.J, cardValues._3, cardValues.S },
		{ cardValues.Q, cardValues._3, cardValues.S },
		{ cardValues.K, cardValues._3, cardValues.S },
		{ cardValues.A, cardValues._3, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.US },
		{ cardValues._4, cardValues._3, cardValues.US },
		{ cardValues._4, cardValues._4, cardValues.ANY },
		{ cardValues._5, cardValues._4, cardValues.S },
		{ cardValues._6, cardValues._4, cardValues.S },
		{ cardValues._7, cardValues._4, cardValues.S },
		{ cardValues._8, cardValues._4, cardValues.S },
		{ cardValues._9, cardValues._4, cardValues.S },
		{ cardValues.T, cardValues._4, cardValues.S },
		{ cardValues.J, cardValues._4, cardValues.S },
		{ cardValues.Q, cardValues._4, cardValues.S },
		{ cardValues.K, cardValues._4, cardValues.S },
		{ cardValues.A, cardValues._4, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.US },
		{ cardValues._5, cardValues._3, cardValues.US },
		{ cardValues._5, cardValues._4, cardValues.US },
		{ cardValues._5, cardValues._5, cardValues.ANY },
		{ cardValues._6, cardValues._5, cardValues.S },
		{ cardValues._7, cardValues._5, cardValues.S },
		{ cardValues._8, cardValues._5, cardValues.S },
		{ cardValues._9, cardValues._5, cardValues.S },
		{ cardValues.T, cardValues._5, cardValues.S },
		{ cardValues.J, cardValues._5, cardValues.S },
		{ cardValues.Q, cardValues._5, cardValues.S },
		{ cardValues.K, cardValues._5, cardValues.S },
		{ cardValues.A, cardValues._5, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.US },
		{ cardValues._6, cardValues._3, cardValues.US },
		{ cardValues._6, cardValues._4, cardValues.US },
		{ cardValues._6, cardValues._5, cardValues.US },
		{ cardValues._6, cardValues._6, cardValues.ANY },
		{ cardValues._7, cardValues._6, cardValues.S },
		{ cardValues._8, cardValues._6, cardValues.S },
		{ cardValues._9, cardValues._6, cardValues.S },
		{ cardValues.T, cardValues._6, cardValues.S },
		{ cardValues.J, cardValues._6, cardValues.S },
		{ cardValues.Q, cardValues._6, cardValues.S },
		{ cardValues.K, cardValues._6, cardValues.S },
		{ cardValues.A, cardValues._6, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.US },
		{ cardValues._7, cardValues._3, cardValues.US },
		{ cardValues._7, cardValues._4, cardValues.US },
		{ cardValues._7, cardValues._5, cardValues.US },
		{ cardValues._7, cardValues._6, cardValues.US },
		{ cardValues._7, cardValues._7, cardValues.ANY },
		{ cardValues._8, cardValues._7, cardValues.S },
		{ cardValues._9, cardValues._7, cardValues.S },
		{ cardValues.T, cardValues._7, cardValues.S },
		{ cardValues.J, cardValues._7, cardValues.S },
		{ cardValues.Q, cardValues._7, cardValues.S },
		{ cardValues.K, cardValues._7, cardValues.S },
		{ cardValues.A, cardValues._7, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.US },
		{ cardValues._8, cardValues._3, cardValues.US },
		{ cardValues._8, cardValues._4, cardValues.US },
		{ cardValues._8, cardValues._5, cardValues.US },
		{ cardValues._8, cardValues._6, cardValues.US },
		{ cardValues._8, cardValues._7, cardValues.US },
		{ cardValues._8, cardValues._8, cardValues.ANY },
		{ cardValues._9, cardValues._8, cardValues.S },
		{ cardValues.T, cardValues._8, cardValues.S },
		{ cardValues.J, cardValues._8, cardValues.S },
		{ cardValues.Q, cardValues._8, cardValues.S },
		{ cardValues.K, cardValues._8, cardValues.S },
		{ cardValues.A, cardValues._8, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.US },
		{ cardValues._9, cardValues._3, cardValues.US },
		{ cardValues._9, cardValues._4, cardValues.US },
		{ cardValues._9, cardValues._5, cardValues.US },
		{ cardValues._9, cardValues._6, cardValues.US },
		{ cardValues._9, cardValues._7, cardValues.US },
		{ cardValues._9, cardValues._8, cardValues.US },
		{ cardValues._9, cardValues._9, cardValues.ANY },
		{ cardValues.T, cardValues._9, cardValues.S },
		{ cardValues.J, cardValues._9, cardValues.S },
		{ cardValues.Q, cardValues._9, cardValues.S },
		{ cardValues.K, cardValues._9, cardValues.S },
		{ cardValues.A, cardValues._9, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.US },
		{ cardValues.T, cardValues._3, cardValues.US },
		{ cardValues.T, cardValues._4, cardValues.US },
		{ cardValues.T, cardValues._5, cardValues.US },
		{ cardValues.T, cardValues._6, cardValues.US },
		{ cardValues.T, cardValues._7, cardValues.US },
		{ cardValues.T, cardValues._8, cardValues.US },
		{ cardValues.T, cardValues._9, cardValues.US },
		{ cardValues.T, cardValues.T, cardValues.ANY },
		{ cardValues.J, cardValues.T, cardValues.S },
		{ cardValues.Q, cardValues.T, cardValues.S },
		{ cardValues.K, cardValues.T, cardValues.S },
		{ cardValues.A, cardValues.T, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.US },
		{ cardValues.J, cardValues._3, cardValues.US },
		{ cardValues.J, cardValues._4, cardValues.US },
		{ cardValues.J, cardValues._5, cardValues.US },
		{ cardValues.J, cardValues._6, cardValues.US },
		{ cardValues.J, cardValues._7, cardValues.US },
		{ cardValues.J, cardValues._8, cardValues.US },
		{ cardValues.J, cardValues._9, cardValues.US },
		{ cardValues.J, cardValues.T, cardValues.US },
		{ cardValues.J, cardValues.J, cardValues.ANY },
		{ cardValues.Q, cardValues.J, cardValues.S },
		{ cardValues.K, cardValues.J, cardValues.S },
		{ cardValues.A, cardValues.J, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.US },
		{ cardValues.Q, cardValues._3, cardValues.US },
		{ cardValues.Q, cardValues._4, cardValues.US },
		{ cardValues.Q, cardValues._5, cardValues.US },
		{ cardValues.Q, cardValues._6, cardValues.US },
		{ cardValues.Q, cardValues._7, cardValues.US },
		{ cardValues.Q, cardValues._8, cardValues.US },
		{ cardValues.Q, cardValues._9, cardValues.US },
		{ cardValues.Q, cardValues.T, cardValues.US },
		{ cardValues.Q, cardValues.J, cardValues.US },
		{ cardValues.Q, cardValues.Q, cardValues.ANY },
		{ cardValues.K, cardValues.Q, cardValues.S },
		{ cardValues.A, cardValues.Q, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.US },
		{ cardValues.K, cardValues._3, cardValues.US },
		{ cardValues.K, cardValues._4, cardValues.US },
		{ cardValues.K, cardValues._5, cardValues.US },
		{ cardValues.K, cardValues._6, cardValues.US },
		{ cardValues.K, cardValues._7, cardValues.US },
		{ cardValues.K, cardValues._8, cardValues.US },
		{ cardValues.K, cardValues._9, cardValues.US },
		{ cardValues.K, cardValues.T, cardValues.US },
		{ cardValues.K, cardValues.J, cardValues.US },
		{ cardValues.K, cardValues.Q, cardValues.US },
		{ cardValues.K, cardValues.K, cardValues.ANY },
		{ cardValues.A, cardValues.K, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.US },
		{ cardValues.A, cardValues._3, cardValues.US },
		{ cardValues.A, cardValues._4, cardValues.US },
		{ cardValues.A, cardValues._5, cardValues.US },
		{ cardValues.A, cardValues._6, cardValues.US },
		{ cardValues.A, cardValues._7, cardValues.US },
		{ cardValues.A, cardValues._8, cardValues.US },
		{ cardValues.A, cardValues._9, cardValues.US },
		{ cardValues.A, cardValues.T, cardValues.US },
		{ cardValues.A, cardValues.J, cardValues.US },
		{ cardValues.A, cardValues.Q, cardValues.US },
		{ cardValues.A, cardValues.K, cardValues.US },
		{ cardValues.A, cardValues.A, cardValues.ANY },
		{ cardValues._2, cardValues._2, cardValues.ANY },
		{ cardValues._3, cardValues._2, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.S },
		{ cardValues._3, cardValues._2, cardValues.US },
		{ cardValues._3, cardValues._3, cardValues.ANY },
		{ cardValues._4, cardValues._3, cardValues.S },
		{ cardValues._5, cardValues._3, cardValues.S },
		{ cardValues._6, cardValues._3, cardValues.S },
		{ cardValues._7, cardValues._3, cardValues.S },
		{ cardValues._8, cardValues._3, cardValues.S },
		{ cardValues._9, cardValues._3, cardValues.S },
		{ cardValues.T, cardValues._3, cardValues.S },
		{ cardValues.J, cardValues._3, cardValues.S },
		{ cardValues.Q, cardValues._3, cardValues.S },
		{ cardValues.K, cardValues._3, cardValues.S },
		{ cardValues.A, cardValues._3, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.US },
		{ cardValues._4, cardValues._3, cardValues.US },
		{ cardValues._4, cardValues._4, cardValues.ANY },
		{ cardValues._5, cardValues._4, cardValues.S },
		{ cardValues._6, cardValues._4, cardValues.S },
		{ cardValues._7, cardValues._4, cardValues.S },
		{ cardValues._8, cardValues._4, cardValues.S },
		{ cardValues._9, cardValues._4, cardValues.S },
		{ cardValues.T, cardValues._4, cardValues.S },
		{ cardValues.J, cardValues._4, cardValues.S },
		{ cardValues.Q, cardValues._4, cardValues.S },
		{ cardValues.K, cardValues._4, cardValues.S },
		{ cardValues.A, cardValues._4, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.US },
		{ cardValues._5, cardValues._3, cardValues.US },
		{ cardValues._5, cardValues._4, cardValues.US },
		{ cardValues._5, cardValues._5, cardValues.ANY },
		{ cardValues._6, cardValues._5, cardValues.S },
		{ cardValues._7, cardValues._5, cardValues.S },
		{ cardValues._8, cardValues._5, cardValues.S },
		{ cardValues._9, cardValues._5, cardValues.S },
		{ cardValues.T, cardValues._5, cardValues.S },
		{ cardValues.J, cardValues._5, cardValues.S },
		{ cardValues.Q, cardValues._5, cardValues.S },
		{ cardValues.K, cardValues._5, cardValues.S },
		{ cardValues.A, cardValues._5, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.US },
		{ cardValues._6, cardValues._3, cardValues.US },
		{ cardValues._6, cardValues._4, cardValues.US },
		{ cardValues._6, cardValues._5, cardValues.US },
		{ cardValues._6, cardValues._6, cardValues.ANY },
		{ cardValues._7, cardValues._6, cardValues.S },
		{ cardValues._8, cardValues._6, cardValues.S },
		{ cardValues._9, cardValues._6, cardValues.S },
		{ cardValues.T, cardValues._6, cardValues.S },
		{ cardValues.J, cardValues._6, cardValues.S },
		{ cardValues.Q, cardValues._6, cardValues.S },
		{ cardValues.K, cardValues._6, cardValues.S },
		{ cardValues.A, cardValues._6, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.US },
		{ cardValues._7, cardValues._3, cardValues.US },
		{ cardValues._7, cardValues._4, cardValues.US },
		{ cardValues._7, cardValues._5, cardValues.US },
		{ cardValues._7, cardValues._6, cardValues.US },
		{ cardValues._7, cardValues._7, cardValues.ANY },
		{ cardValues._8, cardValues._7, cardValues.S },
		{ cardValues._9, cardValues._7, cardValues.S },
		{ cardValues.T, cardValues._7, cardValues.S },
		{ cardValues.J, cardValues._7, cardValues.S },
		{ cardValues.Q, cardValues._7, cardValues.S },
		{ cardValues.K, cardValues._7, cardValues.S },
		{ cardValues.A, cardValues._7, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.US },
		{ cardValues._8, cardValues._3, cardValues.US },
		{ cardValues._8, cardValues._4, cardValues.US },
		{ cardValues._8, cardValues._5, cardValues.US },
		{ cardValues._8, cardValues._6, cardValues.US },
		{ cardValues._8, cardValues._7, cardValues.US },
		{ cardValues._8, cardValues._8, cardValues.ANY },
		{ cardValues._9, cardValues._8, cardValues.S },
		{ cardValues.T, cardValues._8, cardValues.S },
		{ cardValues.J, cardValues._8, cardValues.S },
		{ cardValues.Q, cardValues._8, cardValues.S },
		{ cardValues.K, cardValues._8, cardValues.S },
		{ cardValues.A, cardValues._8, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.US },
		{ cardValues._9, cardValues._3, cardValues.US },
		{ cardValues._9, cardValues._4, cardValues.US },
		{ cardValues._9, cardValues._5, cardValues.US },
		{ cardValues._9, cardValues._6, cardValues.US },
		{ cardValues._9, cardValues._7, cardValues.US },
		{ cardValues._9, cardValues._8, cardValues.US },
		{ cardValues._9, cardValues._9, cardValues.ANY },
		{ cardValues.T, cardValues._9, cardValues.S },
		{ cardValues.J, cardValues._9, cardValues.S },
		{ cardValues.Q, cardValues._9, cardValues.S },
		{ cardValues.K, cardValues._9, cardValues.S },
		{ cardValues.A, cardValues._9, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.US },
		{ cardValues.T, cardValues._3, cardValues.US },
		{ cardValues.T, cardValues._4, cardValues.US },
		{ cardValues.T, cardValues._5, cardValues.US },
		{ cardValues.T, cardValues._6, cardValues.US },
		{ cardValues.T, cardValues._7, cardValues.US },
		{ cardValues.T, cardValues._8, cardValues.US },
		{ cardValues.T, cardValues._9, cardValues.US },
		{ cardValues.T, cardValues.T, cardValues.ANY },
		{ cardValues.J, cardValues.T, cardValues.S },
		{ cardValues.Q, cardValues.T, cardValues.S },
		{ cardValues.K, cardValues.T, cardValues.S },
		{ cardValues.A, cardValues.T, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.US },
		{ cardValues.J, cardValues._3, cardValues.US },
		{ cardValues.J, cardValues._4, cardValues.US },
		{ cardValues.J, cardValues._5, cardValues.US },
		{ cardValues.J, cardValues._6, cardValues.US },
		{ cardValues.J, cardValues._7, cardValues.US },
		{ cardValues.J, cardValues._8, cardValues.US },
		{ cardValues.J, cardValues._9, cardValues.US },
		{ cardValues.J, cardValues.T, cardValues.US },
		{ cardValues.J, cardValues.J, cardValues.ANY },
		{ cardValues.Q, cardValues.J, cardValues.S },
		{ cardValues.K, cardValues.J, cardValues.S },
		{ cardValues.A, cardValues.J, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.US },
		{ cardValues.Q, cardValues._3, cardValues.US },
		{ cardValues.Q, cardValues._4, cardValues.US },
		{ cardValues.Q, cardValues._5, cardValues.US },
		{ cardValues.Q, cardValues._6, cardValues.US },
		{ cardValues.Q, cardValues._7, cardValues.US },
		{ cardValues.Q, cardValues._8, cardValues.US },
		{ cardValues.Q, cardValues._9, cardValues.US },
		{ cardValues.Q, cardValues.T, cardValues.US },
		{ cardValues.Q, cardValues.J, cardValues.US },
		{ cardValues.Q, cardValues.Q, cardValues.ANY },
		{ cardValues.K, cardValues.Q, cardValues.S },
		{ cardValues.A, cardValues.Q, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.US },
		{ cardValues.K, cardValues._3, cardValues.US },
		{ cardValues.K, cardValues._4, cardValues.US },
		{ cardValues.K, cardValues._5, cardValues.US },
		{ cardValues.K, cardValues._6, cardValues.US },
		{ cardValues.K, cardValues._7, cardValues.US },
		{ cardValues.K, cardValues._8, cardValues.US },
		{ cardValues.K, cardValues._9, cardValues.US },
		{ cardValues.K, cardValues.T, cardValues.US },
		{ cardValues.K, cardValues.J, cardValues.US },
		{ cardValues.K, cardValues.Q, cardValues.US },
		{ cardValues.K, cardValues.K, cardValues.ANY },
		{ cardValues.A, cardValues.K, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.US },
		{ cardValues.A, cardValues._3, cardValues.US },
		{ cardValues.A, cardValues._4, cardValues.US },
		{ cardValues.A, cardValues._5, cardValues.US },
		{ cardValues.A, cardValues._6, cardValues.US },
		{ cardValues.A, cardValues._7, cardValues.US },
		{ cardValues.A, cardValues._8, cardValues.US },
		{ cardValues.A, cardValues._9, cardValues.US },
		{ cardValues.A, cardValues.T, cardValues.US },
		{ cardValues.A, cardValues.J, cardValues.US },
		{ cardValues.A, cardValues.Q, cardValues.US },
		{ cardValues.A, cardValues.K, cardValues.US },
		{ cardValues.A, cardValues.A, cardValues.ANY },
		{ cardValues._2, cardValues._2, cardValues.ANY },
		{ cardValues._3, cardValues._2, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.S },
		{ cardValues._3, cardValues._2, cardValues.US },
		{ cardValues._3, cardValues._3, cardValues.ANY },
		{ cardValues._4, cardValues._3, cardValues.S },
		{ cardValues._5, cardValues._3, cardValues.S },
		{ cardValues._6, cardValues._3, cardValues.S },
		{ cardValues._7, cardValues._3, cardValues.S },
		{ cardValues._8, cardValues._3, cardValues.S },
		{ cardValues._9, cardValues._3, cardValues.S },
		{ cardValues.T, cardValues._3, cardValues.S },
		{ cardValues.J, cardValues._3, cardValues.S },
		{ cardValues.Q, cardValues._3, cardValues.S },
		{ cardValues.K, cardValues._3, cardValues.S },
		{ cardValues.A, cardValues._3, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.US },
		{ cardValues._4, cardValues._3, cardValues.US },
		{ cardValues._4, cardValues._4, cardValues.ANY },
		{ cardValues._5, cardValues._4, cardValues.S },
		{ cardValues._6, cardValues._4, cardValues.S },
		{ cardValues._7, cardValues._4, cardValues.S },
		{ cardValues._8, cardValues._4, cardValues.S },
		{ cardValues._9, cardValues._4, cardValues.S },
		{ cardValues.T, cardValues._4, cardValues.S },
		{ cardValues.J, cardValues._4, cardValues.S },
		{ cardValues.Q, cardValues._4, cardValues.S },
		{ cardValues.K, cardValues._4, cardValues.S },
		{ cardValues.A, cardValues._4, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.US },
		{ cardValues._5, cardValues._3, cardValues.US },
		{ cardValues._5, cardValues._4, cardValues.US },
		{ cardValues._5, cardValues._5, cardValues.ANY },
		{ cardValues._6, cardValues._5, cardValues.S },
		{ cardValues._7, cardValues._5, cardValues.S },
		{ cardValues._8, cardValues._5, cardValues.S },
		{ cardValues._9, cardValues._5, cardValues.S },
		{ cardValues.T, cardValues._5, cardValues.S },
		{ cardValues.J, cardValues._5, cardValues.S },
		{ cardValues.Q, cardValues._5, cardValues.S },
		{ cardValues.K, cardValues._5, cardValues.S },
		{ cardValues.A, cardValues._5, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.US },
		{ cardValues._6, cardValues._3, cardValues.US },
		{ cardValues._6, cardValues._4, cardValues.US },
		{ cardValues._6, cardValues._5, cardValues.US },
		{ cardValues._6, cardValues._6, cardValues.ANY },
		{ cardValues._7, cardValues._6, cardValues.S },
		{ cardValues._8, cardValues._6, cardValues.S },
		{ cardValues._9, cardValues._6, cardValues.S },
		{ cardValues.T, cardValues._6, cardValues.S },
		{ cardValues.J, cardValues._6, cardValues.S },
		{ cardValues.Q, cardValues._6, cardValues.S },
		{ cardValues.K, cardValues._6, cardValues.S },
		{ cardValues.A, cardValues._6, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.US },
		{ cardValues._7, cardValues._3, cardValues.US },
		{ cardValues._7, cardValues._4, cardValues.US },
		{ cardValues._7, cardValues._5, cardValues.US },
		{ cardValues._7, cardValues._6, cardValues.US },
		{ cardValues._7, cardValues._7, cardValues.ANY },
		{ cardValues._8, cardValues._7, cardValues.S },
		{ cardValues._9, cardValues._7, cardValues.S },
		{ cardValues.T, cardValues._7, cardValues.S },
		{ cardValues.J, cardValues._7, cardValues.S },
		{ cardValues.Q, cardValues._7, cardValues.S },
		{ cardValues.K, cardValues._7, cardValues.S },
		{ cardValues.A, cardValues._7, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.US },
		{ cardValues._8, cardValues._3, cardValues.US },
		{ cardValues._8, cardValues._4, cardValues.US },
		{ cardValues._8, cardValues._5, cardValues.US },
		{ cardValues._8, cardValues._6, cardValues.US },
		{ cardValues._8, cardValues._7, cardValues.US },
		{ cardValues._8, cardValues._8, cardValues.ANY },
		{ cardValues._9, cardValues._8, cardValues.S },
		{ cardValues.T, cardValues._8, cardValues.S },
		{ cardValues.J, cardValues._8, cardValues.S },
		{ cardValues.Q, cardValues._8, cardValues.S },
		{ cardValues.K, cardValues._8, cardValues.S },
		{ cardValues.A, cardValues._8, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.US },
		{ cardValues._9, cardValues._3, cardValues.US },
		{ cardValues._9, cardValues._4, cardValues.US },
		{ cardValues._9, cardValues._5, cardValues.US },
		{ cardValues._9, cardValues._6, cardValues.US },
		{ cardValues._9, cardValues._7, cardValues.US },
		{ cardValues._9, cardValues._8, cardValues.US },
		{ cardValues._9, cardValues._9, cardValues.ANY },
		{ cardValues.T, cardValues._9, cardValues.S },
		{ cardValues.J, cardValues._9, cardValues.S },
		{ cardValues.Q, cardValues._9, cardValues.S },
		{ cardValues.K, cardValues._9, cardValues.S },
		{ cardValues.A, cardValues._9, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.US },
		{ cardValues.T, cardValues._3, cardValues.US },
		{ cardValues.T, cardValues._4, cardValues.US },
		{ cardValues.T, cardValues._5, cardValues.US },
		{ cardValues.T, cardValues._6, cardValues.US },
		{ cardValues.T, cardValues._7, cardValues.US },
		{ cardValues.T, cardValues._8, cardValues.US },
		{ cardValues.T, cardValues._9, cardValues.US },
		{ cardValues.T, cardValues.T, cardValues.ANY },
		{ cardValues.J, cardValues.T, cardValues.S },
		{ cardValues.Q, cardValues.T, cardValues.S },
		{ cardValues.K, cardValues.T, cardValues.S },
		{ cardValues.A, cardValues.T, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.US },
		{ cardValues.J, cardValues._3, cardValues.US },
		{ cardValues.J, cardValues._4, cardValues.US },
		{ cardValues.J, cardValues._5, cardValues.US },
		{ cardValues.J, cardValues._6, cardValues.US },
		{ cardValues.J, cardValues._7, cardValues.US },
		{ cardValues.J, cardValues._8, cardValues.US },
		{ cardValues.J, cardValues._9, cardValues.US },
		{ cardValues.J, cardValues.T, cardValues.US },
		{ cardValues.J, cardValues.J, cardValues.ANY },
		{ cardValues.Q, cardValues.J, cardValues.S },
		{ cardValues.K, cardValues.J, cardValues.S },
		{ cardValues.A, cardValues.J, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.US },
		{ cardValues.Q, cardValues._3, cardValues.US },
		{ cardValues.Q, cardValues._4, cardValues.US },
		{ cardValues.Q, cardValues._5, cardValues.US },
		{ cardValues.Q, cardValues._6, cardValues.US },
		{ cardValues.Q, cardValues._7, cardValues.US },
		{ cardValues.Q, cardValues._8, cardValues.US },
		{ cardValues.Q, cardValues._9, cardValues.US },
		{ cardValues.Q, cardValues.T, cardValues.US },
		{ cardValues.Q, cardValues.J, cardValues.US },
		{ cardValues.Q, cardValues.Q, cardValues.ANY },
		{ cardValues.K, cardValues.Q, cardValues.S },
		{ cardValues.A, cardValues.Q, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.US },
		{ cardValues.K, cardValues._3, cardValues.US },
		{ cardValues.K, cardValues._4, cardValues.US },
		{ cardValues.K, cardValues._5, cardValues.US },
		{ cardValues.K, cardValues._6, cardValues.US },
		{ cardValues.K, cardValues._7, cardValues.US },
		{ cardValues.K, cardValues._8, cardValues.US },
		{ cardValues.K, cardValues._9, cardValues.US },
		{ cardValues.K, cardValues.T, cardValues.US },
		{ cardValues.K, cardValues.J, cardValues.US },
		{ cardValues.K, cardValues.Q, cardValues.US },
		{ cardValues.K, cardValues.K, cardValues.ANY },
		{ cardValues.A, cardValues.K, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.US },
		{ cardValues.A, cardValues._3, cardValues.US },
		{ cardValues.A, cardValues._4, cardValues.US },
		{ cardValues.A, cardValues._5, cardValues.US },
		{ cardValues.A, cardValues._6, cardValues.US },
		{ cardValues.A, cardValues._7, cardValues.US },
		{ cardValues.A, cardValues._8, cardValues.US },
		{ cardValues.A, cardValues._9, cardValues.US },
		{ cardValues.A, cardValues.T, cardValues.US },
		{ cardValues.A, cardValues.J, cardValues.US },
		{ cardValues.A, cardValues.Q, cardValues.US },
		{ cardValues.A, cardValues.K, cardValues.US },
		{ cardValues.A, cardValues.A, cardValues.ANY },
		{ cardValues._2, cardValues._2, cardValues.ANY },
		{ cardValues._3, cardValues._2, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.S },
		{ cardValues._3, cardValues._2, cardValues.US },
		{ cardValues._3, cardValues._3, cardValues.ANY },
		{ cardValues._4, cardValues._3, cardValues.S },
		{ cardValues._5, cardValues._3, cardValues.S },
		{ cardValues._6, cardValues._3, cardValues.S },
		{ cardValues._7, cardValues._3, cardValues.S },
		{ cardValues._8, cardValues._3, cardValues.S },
		{ cardValues._9, cardValues._3, cardValues.S },
		{ cardValues.T, cardValues._3, cardValues.S },
		{ cardValues.J, cardValues._3, cardValues.S },
		{ cardValues.Q, cardValues._3, cardValues.S },
		{ cardValues.K, cardValues._3, cardValues.S },
		{ cardValues.A, cardValues._3, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.US },
		{ cardValues._4, cardValues._3, cardValues.US },
		{ cardValues._4, cardValues._4, cardValues.ANY },
		{ cardValues._5, cardValues._4, cardValues.S },
		{ cardValues._6, cardValues._4, cardValues.S },
		{ cardValues._7, cardValues._4, cardValues.S },
		{ cardValues._8, cardValues._4, cardValues.S },
		{ cardValues._9, cardValues._4, cardValues.S },
		{ cardValues.T, cardValues._4, cardValues.S },
		{ cardValues.J, cardValues._4, cardValues.S },
		{ cardValues.Q, cardValues._4, cardValues.S },
		{ cardValues.K, cardValues._4, cardValues.S },
		{ cardValues.A, cardValues._4, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.US },
		{ cardValues._5, cardValues._3, cardValues.US },
		{ cardValues._5, cardValues._4, cardValues.US },
		{ cardValues._5, cardValues._5, cardValues.ANY },
		{ cardValues._6, cardValues._5, cardValues.S },
		{ cardValues._7, cardValues._5, cardValues.S },
		{ cardValues._8, cardValues._5, cardValues.S },
		{ cardValues._9, cardValues._5, cardValues.S },
		{ cardValues.T, cardValues._5, cardValues.S },
		{ cardValues.J, cardValues._5, cardValues.S },
		{ cardValues.Q, cardValues._5, cardValues.S },
		{ cardValues.K, cardValues._5, cardValues.S },
		{ cardValues.A, cardValues._5, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.US },
		{ cardValues._6, cardValues._3, cardValues.US },
		{ cardValues._6, cardValues._4, cardValues.US },
		{ cardValues._6, cardValues._5, cardValues.US },
		{ cardValues._6, cardValues._6, cardValues.ANY },
		{ cardValues._7, cardValues._6, cardValues.S },
		{ cardValues._8, cardValues._6, cardValues.S },
		{ cardValues._9, cardValues._6, cardValues.S },
		{ cardValues.T, cardValues._6, cardValues.S },
		{ cardValues.J, cardValues._6, cardValues.S },
		{ cardValues.Q, cardValues._6, cardValues.S },
		{ cardValues.K, cardValues._6, cardValues.S },
		{ cardValues.A, cardValues._6, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.US },
		{ cardValues._7, cardValues._3, cardValues.US },
		{ cardValues._7, cardValues._4, cardValues.US },
		{ cardValues._7, cardValues._5, cardValues.US },
		{ cardValues._7, cardValues._6, cardValues.US },
		{ cardValues._7, cardValues._7, cardValues.ANY },
		{ cardValues._8, cardValues._7, cardValues.S },
		{ cardValues._9, cardValues._7, cardValues.S },
		{ cardValues.T, cardValues._7, cardValues.S },
		{ cardValues.J, cardValues._7, cardValues.S },
		{ cardValues.Q, cardValues._7, cardValues.S },
		{ cardValues.K, cardValues._7, cardValues.S },
		{ cardValues.A, cardValues._7, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.US },
		{ cardValues._8, cardValues._3, cardValues.US },
		{ cardValues._8, cardValues._4, cardValues.US },
		{ cardValues._8, cardValues._5, cardValues.US },
		{ cardValues._8, cardValues._6, cardValues.US },
		{ cardValues._8, cardValues._7, cardValues.US },
		{ cardValues._8, cardValues._8, cardValues.ANY },
		{ cardValues._9, cardValues._8, cardValues.S },
		{ cardValues.T, cardValues._8, cardValues.S },
		{ cardValues.J, cardValues._8, cardValues.S },
		{ cardValues.Q, cardValues._8, cardValues.S },
		{ cardValues.K, cardValues._8, cardValues.S },
		{ cardValues.A, cardValues._8, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.US },
		{ cardValues._9, cardValues._3, cardValues.US },
		{ cardValues._9, cardValues._4, cardValues.US },
		{ cardValues._9, cardValues._5, cardValues.US },
		{ cardValues._9, cardValues._6, cardValues.US },
		{ cardValues._9, cardValues._7, cardValues.US },
		{ cardValues._9, cardValues._8, cardValues.US },
		{ cardValues._9, cardValues._9, cardValues.ANY },
		{ cardValues.T, cardValues._9, cardValues.S },
		{ cardValues.J, cardValues._9, cardValues.S },
		{ cardValues.Q, cardValues._9, cardValues.S },
		{ cardValues.K, cardValues._9, cardValues.S },
		{ cardValues.A, cardValues._9, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.US },
		{ cardValues.T, cardValues._3, cardValues.US },
		{ cardValues.T, cardValues._4, cardValues.US },
		{ cardValues.T, cardValues._5, cardValues.US },
		{ cardValues.T, cardValues._6, cardValues.US },
		{ cardValues.T, cardValues._7, cardValues.US },
		{ cardValues.T, cardValues._8, cardValues.US },
		{ cardValues.T, cardValues._9, cardValues.US },
		{ cardValues.T, cardValues.T, cardValues.ANY },
		{ cardValues.J, cardValues.T, cardValues.S },
		{ cardValues.Q, cardValues.T, cardValues.S },
		{ cardValues.K, cardValues.T, cardValues.S },
		{ cardValues.A, cardValues.T, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.US },
		{ cardValues.J, cardValues._3, cardValues.US },
		{ cardValues.J, cardValues._4, cardValues.US },
		{ cardValues.J, cardValues._5, cardValues.US },
		{ cardValues.J, cardValues._6, cardValues.US },
		{ cardValues.J, cardValues._7, cardValues.US },
		{ cardValues.J, cardValues._8, cardValues.US },
		{ cardValues.J, cardValues._9, cardValues.US },
		{ cardValues.J, cardValues.T, cardValues.US },
		{ cardValues.J, cardValues.J, cardValues.ANY },
		{ cardValues.Q, cardValues.J, cardValues.S },
		{ cardValues.K, cardValues.J, cardValues.S },
		{ cardValues.A, cardValues.J, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.US },
		{ cardValues.Q, cardValues._3, cardValues.US },
		{ cardValues.Q, cardValues._4, cardValues.US },
		{ cardValues.Q, cardValues._5, cardValues.US },
		{ cardValues.Q, cardValues._6, cardValues.US },
		{ cardValues.Q, cardValues._7, cardValues.US },
		{ cardValues.Q, cardValues._8, cardValues.US },
		{ cardValues.Q, cardValues._9, cardValues.US },
		{ cardValues.Q, cardValues.T, cardValues.US },
		{ cardValues.Q, cardValues.J, cardValues.US },
		{ cardValues.Q, cardValues.Q, cardValues.ANY },
		{ cardValues.K, cardValues.Q, cardValues.S },
		{ cardValues.A, cardValues.Q, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.US },
		{ cardValues.K, cardValues._3, cardValues.US },
		{ cardValues.K, cardValues._4, cardValues.US },
		{ cardValues.K, cardValues._5, cardValues.US },
		{ cardValues.K, cardValues._6, cardValues.US },
		{ cardValues.K, cardValues._7, cardValues.US },
		{ cardValues.K, cardValues._8, cardValues.US },
		{ cardValues.K, cardValues._9, cardValues.US },
		{ cardValues.K, cardValues.T, cardValues.US },
		{ cardValues.K, cardValues.J, cardValues.US },
		{ cardValues.K, cardValues.Q, cardValues.US },
		{ cardValues.K, cardValues.K, cardValues.ANY },
		{ cardValues.A, cardValues.K, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.US },
		{ cardValues.A, cardValues._3, cardValues.US },
		{ cardValues.A, cardValues._4, cardValues.US },
		{ cardValues.A, cardValues._5, cardValues.US },
		{ cardValues.A, cardValues._6, cardValues.US },
		{ cardValues.A, cardValues._7, cardValues.US },
		{ cardValues.A, cardValues._8, cardValues.US },
		{ cardValues.A, cardValues._9, cardValues.US },
		{ cardValues.A, cardValues.T, cardValues.US },
		{ cardValues.A, cardValues.J, cardValues.US },
		{ cardValues.A, cardValues.Q, cardValues.US },
		{ cardValues.A, cardValues.K, cardValues.US },
		{ cardValues.A, cardValues.A, cardValues.ANY },
		{ cardValues._2, cardValues._2, cardValues.ANY },
		{ cardValues._3, cardValues._2, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.S },
		{ cardValues._3, cardValues._2, cardValues.US },
		{ cardValues._3, cardValues._3, cardValues.ANY },
		{ cardValues._4, cardValues._3, cardValues.S },
		{ cardValues._5, cardValues._3, cardValues.S },
		{ cardValues._6, cardValues._3, cardValues.S },
		{ cardValues._7, cardValues._3, cardValues.S },
		{ cardValues._8, cardValues._3, cardValues.S },
		{ cardValues._9, cardValues._3, cardValues.S },
		{ cardValues.T, cardValues._3, cardValues.S },
		{ cardValues.J, cardValues._3, cardValues.S },
		{ cardValues.Q, cardValues._3, cardValues.S },
		{ cardValues.K, cardValues._3, cardValues.S },
		{ cardValues.A, cardValues._3, cardValues.S },
		{ cardValues._4, cardValues._2, cardValues.US },
		{ cardValues._4, cardValues._3, cardValues.US },
		{ cardValues._4, cardValues._4, cardValues.ANY },
		{ cardValues._5, cardValues._4, cardValues.S },
		{ cardValues._6, cardValues._4, cardValues.S },
		{ cardValues._7, cardValues._4, cardValues.S },
		{ cardValues._8, cardValues._4, cardValues.S },
		{ cardValues._9, cardValues._4, cardValues.S },
		{ cardValues.T, cardValues._4, cardValues.S },
		{ cardValues.J, cardValues._4, cardValues.S },
		{ cardValues.Q, cardValues._4, cardValues.S },
		{ cardValues.K, cardValues._4, cardValues.S },
		{ cardValues.A, cardValues._4, cardValues.S },
		{ cardValues._5, cardValues._2, cardValues.US },
		{ cardValues._5, cardValues._3, cardValues.US },
		{ cardValues._5, cardValues._4, cardValues.US },
		{ cardValues._5, cardValues._5, cardValues.ANY },
		{ cardValues._6, cardValues._5, cardValues.S },
		{ cardValues._7, cardValues._5, cardValues.S },
		{ cardValues._8, cardValues._5, cardValues.S },
		{ cardValues._9, cardValues._5, cardValues.S },
		{ cardValues.T, cardValues._5, cardValues.S },
		{ cardValues.J, cardValues._5, cardValues.S },
		{ cardValues.Q, cardValues._5, cardValues.S },
		{ cardValues.K, cardValues._5, cardValues.S },
		{ cardValues.A, cardValues._5, cardValues.S },
		{ cardValues._6, cardValues._2, cardValues.US },
		{ cardValues._6, cardValues._3, cardValues.US },
		{ cardValues._6, cardValues._4, cardValues.US },
		{ cardValues._6, cardValues._5, cardValues.US },
		{ cardValues._6, cardValues._6, cardValues.ANY },
		{ cardValues._7, cardValues._6, cardValues.S },
		{ cardValues._8, cardValues._6, cardValues.S },
		{ cardValues._9, cardValues._6, cardValues.S },
		{ cardValues.T, cardValues._6, cardValues.S },
		{ cardValues.J, cardValues._6, cardValues.S },
		{ cardValues.Q, cardValues._6, cardValues.S },
		{ cardValues.K, cardValues._6, cardValues.S },
		{ cardValues.A, cardValues._6, cardValues.S },
		{ cardValues._7, cardValues._2, cardValues.US },
		{ cardValues._7, cardValues._3, cardValues.US },
		{ cardValues._7, cardValues._4, cardValues.US },
		{ cardValues._7, cardValues._5, cardValues.US },
		{ cardValues._7, cardValues._6, cardValues.US },
		{ cardValues._7, cardValues._7, cardValues.ANY },
		{ cardValues._8, cardValues._7, cardValues.S },
		{ cardValues._9, cardValues._7, cardValues.S },
		{ cardValues.T, cardValues._7, cardValues.S },
		{ cardValues.J, cardValues._7, cardValues.S },
		{ cardValues.Q, cardValues._7, cardValues.S },
		{ cardValues.K, cardValues._7, cardValues.S },
		{ cardValues.A, cardValues._7, cardValues.S },
		{ cardValues._8, cardValues._2, cardValues.US },
		{ cardValues._8, cardValues._3, cardValues.US },
		{ cardValues._8, cardValues._4, cardValues.US },
		{ cardValues._8, cardValues._5, cardValues.US },
		{ cardValues._8, cardValues._6, cardValues.US },
		{ cardValues._8, cardValues._7, cardValues.US },
		{ cardValues._8, cardValues._8, cardValues.ANY },
		{ cardValues._9, cardValues._8, cardValues.S },
		{ cardValues.T, cardValues._8, cardValues.S },
		{ cardValues.J, cardValues._8, cardValues.S },
		{ cardValues.Q, cardValues._8, cardValues.S },
		{ cardValues.K, cardValues._8, cardValues.S },
		{ cardValues.A, cardValues._8, cardValues.S },
		{ cardValues._9, cardValues._2, cardValues.US },
		{ cardValues._9, cardValues._3, cardValues.US },
		{ cardValues._9, cardValues._4, cardValues.US },
		{ cardValues._9, cardValues._5, cardValues.US },
		{ cardValues._9, cardValues._6, cardValues.US },
		{ cardValues._9, cardValues._7, cardValues.US },
		{ cardValues._9, cardValues._8, cardValues.US },
		{ cardValues._9, cardValues._9, cardValues.ANY },
		{ cardValues.T, cardValues._9, cardValues.S },
		{ cardValues.J, cardValues._9, cardValues.S },
		{ cardValues.Q, cardValues._9, cardValues.S },
		{ cardValues.K, cardValues._9, cardValues.S },
		{ cardValues.A, cardValues._9, cardValues.S },
		{ cardValues.T, cardValues._2, cardValues.US },
		{ cardValues.T, cardValues._3, cardValues.US },
		{ cardValues.T, cardValues._4, cardValues.US },
		{ cardValues.T, cardValues._5, cardValues.US },
		{ cardValues.T, cardValues._6, cardValues.US },
		{ cardValues.T, cardValues._7, cardValues.US },
		{ cardValues.T, cardValues._8, cardValues.US },
		{ cardValues.T, cardValues._9, cardValues.US },
		{ cardValues.T, cardValues.T, cardValues.ANY },
		{ cardValues.J, cardValues.T, cardValues.S },
		{ cardValues.Q, cardValues.T, cardValues.S },
		{ cardValues.K, cardValues.T, cardValues.S },
		{ cardValues.A, cardValues.T, cardValues.S },
		{ cardValues.J, cardValues._2, cardValues.US },
		{ cardValues.J, cardValues._3, cardValues.US },
		{ cardValues.J, cardValues._4, cardValues.US },
		{ cardValues.J, cardValues._5, cardValues.US },
		{ cardValues.J, cardValues._6, cardValues.US },
		{ cardValues.J, cardValues._7, cardValues.US },
		{ cardValues.J, cardValues._8, cardValues.US },
		{ cardValues.J, cardValues._9, cardValues.US },
		{ cardValues.J, cardValues.T, cardValues.US },
		{ cardValues.J, cardValues.J, cardValues.ANY },
		{ cardValues.Q, cardValues.J, cardValues.S },
		{ cardValues.K, cardValues.J, cardValues.S },
		{ cardValues.A, cardValues.J, cardValues.S },
		{ cardValues.Q, cardValues._2, cardValues.US },
		{ cardValues.Q, cardValues._3, cardValues.US },
		{ cardValues.Q, cardValues._4, cardValues.US },
		{ cardValues.Q, cardValues._5, cardValues.US },
		{ cardValues.Q, cardValues._6, cardValues.US },
		{ cardValues.Q, cardValues._7, cardValues.US },
		{ cardValues.Q, cardValues._8, cardValues.US },
		{ cardValues.Q, cardValues._9, cardValues.US },
		{ cardValues.Q, cardValues.T, cardValues.US },
		{ cardValues.Q, cardValues.J, cardValues.US },
		{ cardValues.Q, cardValues.Q, cardValues.ANY },
		{ cardValues.K, cardValues.Q, cardValues.S },
		{ cardValues.A, cardValues.Q, cardValues.S },
		{ cardValues.K, cardValues._2, cardValues.US },
		{ cardValues.K, cardValues._3, cardValues.US },
		{ cardValues.K, cardValues._4, cardValues.US },
		{ cardValues.K, cardValues._5, cardValues.US },
		{ cardValues.K, cardValues._6, cardValues.US },
		{ cardValues.K, cardValues._7, cardValues.US },
		{ cardValues.K, cardValues._8, cardValues.US },
		{ cardValues.K, cardValues._9, cardValues.US },
		{ cardValues.K, cardValues.T, cardValues.US },
		{ cardValues.K, cardValues.J, cardValues.US },
		{ cardValues.K, cardValues.Q, cardValues.US },
		{ cardValues.K, cardValues.K, cardValues.ANY },
		{ cardValues.A, cardValues.K, cardValues.S },
		{ cardValues.A, cardValues._2, cardValues.US },
		{ cardValues.A, cardValues._3, cardValues.US },
		{ cardValues.A, cardValues._4, cardValues.US },
		{ cardValues.A, cardValues._5, cardValues.US },
		{ cardValues.A, cardValues._6, cardValues.US },
		{ cardValues.A, cardValues._7, cardValues.US },
		{ cardValues.A, cardValues._8, cardValues.US },
		{ cardValues.A, cardValues._9, cardValues.US },
		{ cardValues.A, cardValues.T, cardValues.US },
		{ cardValues.A, cardValues.J, cardValues.US },
		{ cardValues.A, cardValues.Q, cardValues.US },
		{ cardValues.A, cardValues.K, cardValues.US },
		{ cardValues.A, cardValues.A, cardValues.ANY },

	};
*/

	//orig/old 
	public static readonly cardValues[,] Group = new cardValues[,] {
		{cardValues.A,  cardValues.A,  cardValues.US},//1
		{cardValues.K,  cardValues.K,  cardValues.US},//2
		{cardValues.Q,  cardValues.Q,  cardValues.US},//3
		{cardValues.A,  cardValues.K,  cardValues.S },//4
		{cardValues.J,  cardValues.J,  cardValues.US},//5
		{cardValues.A,  cardValues.Q,  cardValues.S },//6
		
		
		{cardValues.K,  cardValues.Q,  cardValues.S },//7
		{cardValues.A,  cardValues.J,  cardValues.S },//8
		{cardValues.K,  cardValues.J,  cardValues.S },//9
		{cardValues.T,  cardValues.T,  cardValues.US},//10
		{cardValues.A,  cardValues.K,  cardValues.US},//11
		
		{cardValues.A,  cardValues.T,  cardValues.S },//12
		{cardValues.Q,  cardValues.J,  cardValues.S },//13
		{cardValues.K,  cardValues.T,  cardValues.S },//14
		{cardValues.Q,  cardValues.T,  cardValues.S },//15
		{cardValues.J,  cardValues.T,  cardValues.S },//16
		{cardValues._9, cardValues._9, cardValues.US},//17
		
		{cardValues.A,  cardValues.Q,  cardValues.US},//18
		{cardValues.A,  cardValues._9, cardValues.S },//19
		{cardValues.K,  cardValues.Q,  cardValues.US},//20
		{cardValues._8, cardValues._8, cardValues.US},//21
		{cardValues.K,  cardValues._9, cardValues.S },//22
		{cardValues.T,  cardValues._9, cardValues.S },//23
		{cardValues.A,  cardValues._8, cardValues.S },//24
		{cardValues.Q,  cardValues._9, cardValues.S },//25
		
		{cardValues.J,  cardValues._9, cardValues.S },//26
		{cardValues.A,  cardValues.J,  cardValues.US},//27
		{cardValues.A,  cardValues._5, cardValues.S },//28
		{cardValues._7, cardValues._7, cardValues.US},//29
		{cardValues.A,  cardValues._7, cardValues.S },//30
		{cardValues.K,  cardValues.J,  cardValues.US},//31
		{cardValues.A,  cardValues._4, cardValues.S },//32
		{cardValues.A,  cardValues._3, cardValues.S },//33
		{cardValues.A,  cardValues._6, cardValues.S },//34
		{cardValues.Q,  cardValues.J,  cardValues.US},//35
		{cardValues._6, cardValues._6, cardValues.US},//36
		
		{cardValues.K,  cardValues._8, cardValues.US},//37
		{cardValues.T,  cardValues._8, cardValues.S },//38
		{cardValues.A,  cardValues._2, cardValues.S },//39
		{cardValues._9, cardValues._8, cardValues.S },//40
		{cardValues.J,  cardValues._8, cardValues.S },//41
		{cardValues.A,  cardValues.T,  cardValues.US},//42
		{cardValues.Q,  cardValues._8, cardValues.S },//43
		{cardValues.K,  cardValues._7, cardValues.S },//44
		{cardValues.K,  cardValues.T,  cardValues.US},//45
		{cardValues._5, cardValues._5, cardValues.US},//46
		
		
		{cardValues.J,  cardValues.T,  cardValues.US},//47
		{cardValues._8, cardValues._7, cardValues.S },//48
		{cardValues.Q,  cardValues.T,  cardValues.US},//49
		{cardValues._4, cardValues._4, cardValues.US},//50
		{cardValues._3, cardValues._3, cardValues.US},//51
		{cardValues._2, cardValues._2, cardValues.US},//52
		{cardValues.K,  cardValues._6, cardValues.S },//53
		{cardValues._9, cardValues._7, cardValues.S },//54
		{cardValues.K,  cardValues._5, cardValues.S },//55
		{cardValues._7, cardValues._6, cardValues.S },//56
		{cardValues.T,  cardValues._7, cardValues.S },//57
		{cardValues.K,  cardValues._4, cardValues.S },//58
		
		
		{cardValues.K,  cardValues._2, cardValues.S },//59
		{cardValues.K,  cardValues._3, cardValues.S },//60
		{cardValues.Q,  cardValues._7, cardValues.S },//61
		{cardValues._8, cardValues._6, cardValues.S },//62
		{cardValues._6, cardValues._5, cardValues.S },//63
		{cardValues.J,  cardValues._7, cardValues.S },//64
		{cardValues._5, cardValues._4, cardValues.S },//65
		{cardValues.Q,  cardValues._6, cardValues.S },//66
		{cardValues._7, cardValues._5, cardValues.S },//67
		{cardValues._9, cardValues._6, cardValues.S },//68
		{cardValues.Q,  cardValues._5, cardValues.S },//69
		{cardValues._6, cardValues._4, cardValues.S },//70
		{cardValues.Q,  cardValues._4, cardValues.S },//71
		{cardValues.Q,  cardValues._3, cardValues.S },//72
		{cardValues.T,  cardValues._9, cardValues.US},//73
		
		{cardValues.T,  cardValues._6, cardValues.S },//74
		{cardValues.Q,  cardValues._2, cardValues.S },//75
		{cardValues.A,  cardValues._9, cardValues.US},//76
		{cardValues._5, cardValues._3, cardValues.S },//77
		{cardValues._8, cardValues._5, cardValues.S },//78
		{cardValues.J,  cardValues._6, cardValues.S },//79
		{cardValues.J,  cardValues._9, cardValues.US},//80
		{cardValues.K,  cardValues._9, cardValues.US},//81
		{cardValues.J,  cardValues._5, cardValues.S },//82
		{cardValues.Q,  cardValues._9, cardValues.US},//83
		{cardValues._4, cardValues._3, cardValues.S },//84
		{cardValues._7, cardValues._4, cardValues.S },//85
		
		{cardValues.J,  cardValues._4, cardValues.S },//86
		{cardValues.J,  cardValues._3, cardValues.S },//87
		{cardValues._9, cardValues._5, cardValues.S },//88
		{cardValues.J,  cardValues._2, cardValues.S },//89
		{cardValues._6, cardValues._3, cardValues.S },//90
		{cardValues.A,  cardValues._8, cardValues.US},//91
		{cardValues._5, cardValues._2, cardValues.S },//92
		{cardValues.T,  cardValues._5, cardValues.S },//93
		{cardValues._8, cardValues._4, cardValues.S },//94
		{cardValues.T,  cardValues._4, cardValues.S },//95
		{cardValues.T,  cardValues._3, cardValues.S },//96
		{cardValues._4, cardValues._2, cardValues.S },//97
		{cardValues.T,  cardValues._2, cardValues.S },//98
		{cardValues._9, cardValues._8, cardValues.US},//99
		{cardValues.T,  cardValues._8, cardValues.US},//100
		{cardValues.A,  cardValues._5, cardValues.US},//101
		{cardValues.A,  cardValues._7, cardValues.US},//102
		{cardValues._7, cardValues._3, cardValues.S },//103 
		{cardValues.A,  cardValues._4, cardValues.US},//104
		{cardValues._3, cardValues._2, cardValues.S}};//105

}

