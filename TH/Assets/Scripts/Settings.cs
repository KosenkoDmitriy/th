using System.IO;
using UnityEngine;

static class Settings
{
    public static bool isDebug = true;
    public static int cardsSize = 52;

    public static readonly int pockerHandPossibleSize = 5;
    public static readonly int playerSize = 6;
    public static readonly int kickerSize = 6;
    public static int playerCredits = 1000;
    public static int year = 2010;
    public static int playerNameSize = 20;
    public static int playerCreditsLimit = 100;
    //public static int playerAutoPlayCredits = 1000;

    public static int jurisdictionalBetLimit = 1000;

    public static readonly int gameDenomMultiplier = 5;
    public static readonly int raiseLimitMultiplier = 5;

    public static int intervalGameOver = 1000;

    public static readonly double betDx = 2.50; // .25;
    public static double gameDenomination = betDx;
    public static readonly int GameDenominationDivider = 100;
        
    public static readonly double betMax = betDx*6; // 1.5;
    public static string dollar = "$";// { get; internal set; }
    public static double betNull = 0.00;
    public static double betCurrent = 0.00;

    // ini settings
    public static string pathToAppData = Application.dataPath;
    public static string pathToCurDir = Directory.GetCurrentDirectory();
    public static string pathToAssetRes = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\";

    public static double currentIniVersion = 2.5;
    public static double iniVersion = 0.0;
    public static bool testGame = false;
    public static bool logging = false;

    public static int gameNumber = 1;
    public static double creditsPlayed = 0;
    public static double creditsWon = 0;
}

