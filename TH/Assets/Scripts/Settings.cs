using System.Collections.Generic;

static class Settings
{
	public static bool isDev = true;
	public static bool isDebug = false;
	public static int levelGame = 2;
	public static int levelMainMenu = 0;
	
	public static readonly int playerSize = 6;
	public static readonly int dealerIndex = playerSize - 1;
	public static readonly int playerRealIndex = 0;

	public static float updateInterval = 0.05f;

	#region bets
	public static readonly int betSubRoundMinSize = 0;
	public static readonly int betAnteSubRoundMinSize = 1;
	public static readonly int betSubRoundMaxSize = 4;
	
	public static readonly double betAnteMultiplier = 1;
	public static readonly double betPreflopFlopMultiplier = betAnteMultiplier;
	public static readonly double betTurnRiverMultiplier = 2;

	public static readonly double betMinMath = 1;	// .25;	
	public static readonly double betMaxMath = 4;	// 1.5;

	public static double betCurrentMultiplier = betAnteMultiplier; // 2 or 4 bets
	public static readonly double betCreditsMultiplier = 25; // 1 bet = 25 credits

	// bonus table
	public static double betBonusMaxMultiplier = 10;
	public static double betBonus = 0;
	public static double betBonusMin = 1;
	public static double betBonusMax = 5;

	public static int paytableRowSize = 9; //rows
	public static int paytableColumnSize = 6; //cols
	
	public static readonly int ColsCount = 5;
	public static readonly int RowsCount = 8;
	// end bonus table

	public static readonly double betNull = 0.00;
	public static double betCurrent = 0.00;

	public static double playerCredits = 500;
	public static string credits = " credits ";// { get; internal set; }

	public static double playerCreditsInNumberOfBets = playerCredits / betCreditsMultiplier;
	#endregion

	// subrounds
	public static int playerHandMaxSize = 5;
	public static readonly int playerHandSizePreflop = 2;
	public static readonly int maxSubRoundCount = 1;
	public static readonly int maxRoundCount = maxSubRoundCount * 4;
	// end subrounds
	public static readonly string defaultPreflopPattern = "CHECK/FOLD"; // default pattern if the preflop hand is not found in the math model
	    
	#region api
    public static bool isLogined = false;

	public static string key = "";
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
	#endregion api

    // cards
    public static string cardsPrefix = "cards_new/";
	public static string cardBackName = cardsPrefix + "card_back_black_with_logo";
	public static string cardBg = "transparent";
    public static int cardsSize = 52;
	// end cards


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
