using System.Collections.Generic;

static class Settings
{
	public static bool isDev = false;	// advanced debug info
	public static bool isDebug = false;	// logs, display cards, etc
	public static bool isTest = false;	// test hands
	public static bool isLog = false;	// reorder players

	public static bool isShowGameHelp = true;
	public static bool isShowBetHelp = true;
	public static bool isNeedToSync = false; // sync balance

	public static double freeCredits = 0;

	public static readonly float audioVolume = 0.2f;
	public static readonly string clickSound = "Sounds/click4";//cardFan1"; //pressed";

	public static double betAmountOfAnteRound = 0;
	public static bool isBtnAnteAmountClicked = false;

	public static UnityEngine.Sprite avatar;
	public static int avatarHeight = 150;
	public static int avatarWidth = 150;
	public static string avatarDefault = "avatars/avatar0";//krest";

	public static string lblWaitAction = "pls select your action >>";
	public static string icWin = "new/starry1";//"winner_start";
	public static string icDealer = "ic_dealer_old";
	public static double betRepeat = 0;
	public static string win = "win";
	public static readonly string aCheck = "CHECK";
	public static readonly string aCall = "CALL";
	public static readonly string aFold = "FOLD";
	public static readonly string aRaise = "RAISE";
	public static readonly string aAllIn = "ALL IN";
	public static readonly string aUnknown = "OPEN";

	public static readonly string btnBetBonusRepeat = "REPEAT BONUS BET OF";
	public static readonly string btnBetRepeat = "REPEAT BET OF";

	public static int levelGame = 1;
	public static int levelMainMenu = 0;
	
	public static readonly int playerSize = 6;
	public static readonly int dealerIndex = playerSize - 1;
	public static readonly int playerRealIndex = 0;

	public static float updateInterval = 0.25f;

	#region bets
	public static readonly int betSubRoundMinSize = 0;
	public static readonly int betAnteSubRoundMinSize = 1;
	public static readonly int betSubRoundMaxSize = 1;//4

	public static readonly double betMinMath = 1;
	public static readonly double betMathLimit = 4;

	public static readonly double bePreflopFlopMultiplier = 2;	//1 bet = 2 credits (preflop, flop)
	public static readonly double betTurnRiverMultiplier = 4;	//1 bet = 4 credits (turn, river)	
	
	public static double betCurrentMultiplier = bePreflopFlopMultiplier; // 2 or 4 credits
	public static readonly double betCreditsMultiplier = 125; // 1 bet = 125 credits
	
	// bonus table
	public static double betBonusMaxMultiplier = 10; // betCreditsMultiplier * 10 hand combination = 250
	public static double betBonus = 0;
	public static double betBonusMin = 1;
	public static double betBonusMax = 5;
	public static bool btnBetBonusIsDone; // if false then auto bonus rebet 

	public static int paytableRowSize = 7; //rows
	public static int paytableColumnSize = 6; //cols
	
	public static readonly int ColsCount = 5;
	public static readonly int RowsCount = 8;
	// end bonus table

	public static readonly double betNull = 0.00;
	public static Bet betCurrent = new Bet(0);

	public static double playerCredits = 2500;
	public static string credits = " credits ";// { get; internal set; }

	public static string chipsRed = "new/chip_S_red";//"chip_red";	//"chips_red";
	public static string chipsBlue = "new/chip_S_blue";//"chip_yellow";	//"chips_blue";
	public static string chipsGreen = "new/chip_S_green";
	public static string chipsWhite = "new/chip_S_white";
	public static string chipsBlack = "new/chip_S_black";

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

//	public static readonly string http = "http://";
//	public static readonly string host = http + "localhost:3001";
//
//	public static readonly string http = "http://";
//	public static readonly string host = http + "localhost:3000";

	public static readonly string http = "https://";
	public static readonly string host = http + "yourplaceforfun.com";

	public static string facebookImageUrl = host + "/f.php?url={0}";
	public static readonly string facebookGraphPictureUrl = "https://graph.facebook.com/{0}/picture&width={1}&height={2}&access_token={3}"; 
	public static readonly string actionFbAvatar = "avatar_url";
	public static string facebookFinalImageUrl = "";

	public static readonly string urlSignUp = host + "/sign_up";
	public static readonly string urlMobileSignUp = host + "/mobile/signup";

	public static readonly string urlRestore = host + "/restore"; //TODO: implement on website
	public static readonly string urlLogin = host + "/sign_in";
    public static readonly string urlBuy = host + "#credits";// "/buy"; //TODO: implement on website
    public static readonly string urlInviteFriend = host + "/invite_friend"; //TODO: implement on website
    public static readonly string urlFortuneWheel = host + "#credits";//"/fortune_wheel"; //TODO: implement on website
    public static readonly string urlCredits = host + "#credits";//"/credits"; //TODO: implement on website

	public static readonly string actionLogin = "login";
	public static readonly string actionFacebookLogin = "flogin";
	public static readonly string actionAdd = "add";
    public static readonly string actionSub = "sub";
    public static string actionGetBalance = "get";
	public static string actionSetBalance = "set";
	public static string actionWinBalance = "add";
	public static string actionLoseBalance = "sub";
	public static readonly string id = "th";
	#endregion api

    // cards
	public static string cardsPrefix = "new/cards/";//"cards_new/";
	public static string cardBackName = cardsPrefix + "card_back";//"card_back_black_with_logo";//"card_back_red";
	public static string cardBg = "transparent";
    public static int cardsSize = 52;
	// end cards


	public static void OpenUrl(string url) {
		#if UNITY_WEBPLAYER || UNITY_WEBGL
		UnityEngine.Application.ExternalEval(string.Format("window.open('{0}', '_blank')", url)); // open url in new tab
		#else
		UnityEngine.Application.OpenURL(url);
		#endif
	}

	public static void OpenUrlAsExternalCall(string url) {
		#if UNITY_WEBPLAYER || UNITY_WEBGL
		UnityEngine.Application.ExternalCall("OpenNormalLink", url);
		#else
		UnityEngine.Application.OpenURL(url);
		#endif
	}

	public static void OpenUrlInNewTabAsExternalCall(string url) {
		#if UNITY_WEBPLAYER || UNITY_WEBGL
		UnityEngine.Application.ExternalCall("OpenTabLink", url);
		#else
		UnityEngine.Application.OpenURL(url);
		#endif
	}
}
