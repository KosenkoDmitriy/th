using System.Collections.Generic;

static class Settings
{
    public static string key = "";
    public static int levelGame = 1;
    public static int levelMainMenu = 0;

    public static bool isLogined = false;
    public static double playerCredits = 100;

    public static readonly string http = "http://";
    public static readonly string host = http + "th.shopomob.ru";
    public static readonly string actionLogin = "login";
    public static readonly string urlSignUp = host + "#sign_in_up";
    public static readonly string urlRestore = host + "/restore"; //TODO: implement on website

    public static readonly string actionAdd = "add";
    public static readonly string actionSub = "sub";
    public static readonly string actionGetBalance = "get";
    public static readonly string actionSetBalance = "set";

    public static int videoPokerLowRank = 15;
    public static int selectedColumn;

    public static bool isDebug = true;
    public static bool isIgnoreIniFile = true;
    public static bool testGame = false;
    public static bool logging = false;
    public static float updateInterval = 0.1f;

    public static string cardBackName = "logo_back_cards"; // image name of the card back side

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

    public static readonly double betDx = 2.50; // .25;
    public static double gameDenomination = betDx;
    public static readonly int GameDenominationDivider = 100;
    public static readonly int videoBonusMaxMultiplier = 5;
    public static readonly int betAmountAutoplay = 5;

    public static readonly double betMax = betDx * 6; // 1.5;
    public static string dollar = "$";// { get; internal set; }
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
            GetVirtualPlayer1(),
            GetVirtualPlayer2(),
            GetVirtualPlayer3(),
            GetVirtualPlayer4(),
            GetVirtualPlayer5(),
            GetVirtualPlayer6()
        };
        return virtualPlayerList;
    }

    private static VirtualPlayer GetVirtualPlayer1()
    {
        VirtualPlayer vp = new VirtualPlayer();
        vp.Name = "Mr Tight Ted";
        vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
        vp.HoleMinThreshold = 53;  //the lowest rank for play anything lower folds
        vp.RaiseLevels = new List<RaiseLevel>() {
                new RaiseLevel() { //1
                    RaiseHands = new int[] { 7, 8, 9, 10, 12 }, // the hands that are raised on initial round or called in susiquent
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
        vp.FlopNoRaiseBetPercentages = new int[] { 100, 100, 100, 100, 100, 75, 100, 100, 50, 50, 50, 40, 25, 40, 30, 20, 30, 0, 40, 0, 0, 0 };
        vp.TurnNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 60, 50, 40, 30, 00, 20, 10, 0, 10, 0, 0, 0, 0, 0 };
        vp.RiverNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 75, 60, 50, 20, 00, 10, 0, 0, -1, -1, -1, -1, -1, -1 };

        vp.BluffHands = new int[] { 6, 7, 12, 13, 16, 18 };
        vp.SlowPlayHands = new int[] { 0 };
        vp.AllInHands = new int[] { 0 };
        vp.BluffPercentage = 15;
        vp.BluffCallRaisePercentage = 0;
        vp.Folded = false;
        return vp;
    }

    private static VirtualPlayer GetVirtualPlayer2()
    {
        VirtualPlayer vp = new VirtualPlayer();
        vp.Name = "Mr Tight Ted";
        vp.FoldOnAnyRaise = false; //true = fold on raise false = use rules;
        vp.HoleMinThreshold = 104;  //the lowest rank for play anything lower folds
        vp.RaiseLevels = new List<RaiseLevel>() {
                new RaiseLevel() { //1
                    RaiseHands = new int[] { 1, 2, 3 }, // the hands that are raised on initial round or called in susiquent
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
        vp.FlopNoRaiseBetPercentages = new int[] { 100, 100, 100, 100, 100, 75, 100, 100, 50, 50, 50, 40, 25, 40, 30, 20, 30, 0, 40, 0, 0, 0 };
        vp.TurnNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 60, 50, 40, 30, 00, 20, 10, 0, 10, 0, 0, 0, 0, 0 };
        vp.RiverNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 75, 60, 50, 20, 00, 10, 0, 0, -1, -1, -1, -1, -1, -1 };

        vp.BluffHands = new int[] { 0 };
        vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
        vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
        vp.BluffPercentage = 0;
        vp.BluffCallRaisePercentage = 0;

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
                    RaiseHands = IntRange(12, 20), // the hands that are raised on initial round or called in susiquent
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
        vp.FlopNoRaiseBetPercentages = new int[] { 100, 100, 100, 100, 100, 75, 100, 100, 50, 50, 50, 40, 25, 40, 30, 20, 30, 0, 40, 0, 0, 0 };
        vp.TurnNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 60, 50, 40, 30, 00, 20, 10, 0, 10, 0, 0, 0, 0, 0 };
        vp.RiverNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 75, 60, 50, 20, 00, 10, 0, 0, -1, -1, -1, -1, -1, -1 };

        vp.BluffHands = new int[] { 3, 5, 6, 8, 10, 12, 16, 18, 27 };
        vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
        vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
        vp.BluffPercentage = 25;
        vp.BluffCallRaisePercentage = 0;

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
                    RaiseHands = IntRange(37, 52), // the hands that are raised on initial round or called in susiquent
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
        vp.FlopNoRaiseBetPercentages = new int[] { 100, 100, 100, 100, 100, 75, 100, 100, 50, 50, 50, 40, 25, 40, 30, 20, 30, 0, 40, 0, 0, 0 };
        vp.TurnNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 60, 50, 40, 30, 00, 20, 10, 0, 10, 0, 0, 0, 0, 0 };
        vp.RiverNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 75, 60, 50, 20, 00, 10, 0, 0, -1, -1, -1, -1, -1, -1 };

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
                    RaiseHands = new int[] { 7, 8, 9, 10, 12 }, // the hands that are raised on initial round or called in susiquent
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
        vp.FlopNoRaiseBetPercentages = new int[] { 100, 100, 100, 100, 100, 75, 100, 100, 50, 50, 50, 40, 25, 40, 30, 20, 30, 0, 40, 0, 0, 0 };
        vp.TurnNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 60, 50, 40, 30, 00, 20, 10, 0, 10, 0, 0, 0, 0, 0 };
        vp.RiverNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 75, 60, 50, 20, 00, 10, 0, 0, -1, -1, -1, -1, -1, -1 };

        vp.BluffHands = new int[] { 12, 18, 19, 24, 27, 28, 30, 32, 33, 34, 39, 42 };
        vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
        vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
        vp.BluffPercentage = 30;
        vp.BluffCallRaisePercentage = 0;

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
                    RaiseHands = IntRange(7, 10, new int[] { 12 }), // the hands that are raised on initial round or called in susiquent
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
        vp.FlopNoRaiseBetPercentages = new int[] { 100, 100, 100, 100, 100, 75, 100, 100, 50, 50, 50, 40, 25, 40, 30, 20, 30, 0, 40, 0, 0, 0 };
        vp.TurnNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 60, 50, 40, 30, 00, 20, 10, 0, 10, 0, 0, 0, 0, 0 };
        vp.RiverNoRaiseBetPercentages = new int[] { 999, 999, 200, 175, 150, 125, 100, 100, 75, 60, 50, 20, 00, 10, 0, 0, -1, -1, -1, -1, -1, -1 };

        vp.BluffHands = new int[] { 0 };
        vp.SlowPlayHands = new int[] { 0 }; // hands we slow play
        vp.AllInHands = new int[] { 0 };    // we go all in after any raise.
        vp.BluffPercentage = 0;
        vp.BluffCallRaisePercentage = 0;

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
}

