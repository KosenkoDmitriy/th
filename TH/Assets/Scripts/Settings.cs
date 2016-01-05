using System.Collections.Generic;

static class Settings
{
	public static bool isDebug = false;
	public static int levelGame = 2;

	public static int playerHandMaxSize = 5;
	public static int playerHandSizePreflop = 2;
	public static readonly int maxSubRoundCount = 1;
	public static readonly int maxRoundCount = maxSubRoundCount * 4;

	public static readonly string defaultPreflopPattern = "CHECK/FOLD"; // default pattern if the preflop hand is not found in the math model

	public static double betBonusAmount = 0;
	public static double betMaxBonusAmount = 125;

	public static bool isPlayerWithNo = true;
    public static string key = "";
    
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

	public static void OpenUrl(string url) {
		#if UNITY_WEBPLAYER
		UnityEngine.Application.ExternalEval(string.Format("window.open('{0}', '_blank')", url));
		#elif UNITY_WEBGL
		UnityEngine.Application.ExternalEval(string.Format("window.open('{0}', '_blank')", url)); // open url in new tab
		#else
		UnityEngine.Application.OpenURL(url);
		#endif
	}
}
