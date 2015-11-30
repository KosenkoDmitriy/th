using UnityEngine;
using UnityEngine.UI;

using System;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

public class LoadOnClick : MonoBehaviour
{
    #region init vars

    Assets.Scripts.PayTable payTable;
    bool AutoPlay = false;

    int offsetX = 25;//40;
    int offsetY = 20;//20;

    bool showdown = false;
    int videoMultiplier = 0;
    public double betLimit = 9999;
    public double raiseLimit = 9999;

    int virtualPlayerCount = 0;
    int denomUnits;

    //System.Windows.Forms.Timer gameStartTimer = new System.Windows.Forms.Timer();
    //Timer gameOverTimer = new Timer();
    //Timer nextPlayerTimer = new Timer();
    int[] loop = new int[] { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5 };
    public enum cardValues
    {
        US = 0,
        S = 1,
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

    const int S2 = 0;
    const int S3 = 1;
    const int S4 = 2;
    const int S5 = 3;
    const int S6 = 4;
    const int S7 = 5;
    const int S8 = 6;
    const int S9 = 7;
    const int ST = 8;
    const int SJ = 9;
    const int SQ = 10;
    const int SK = 11;
    const int SA = 12;
    const int D2 = 13;
    const int D3 = 14;
    const int D4 = 15;
    const int D5 = 16;
    const int D6 = 17;
    const int D7 = 18;
    const int D8 = 19;
    const int D9 = 20;
    const int DT = 21;
    const int DJ = 22;
    const int DQ = 23;
    const int DK = 24;
    const int DA = 25;
    const int C2 = 26;
    const int C3 = 27;
    const int C4 = 28;
    const int C5 = 29;
    const int C6 = 30;
    const int C7 = 31;
    const int C8 = 32;
    const int C9 = 33;
    const int CT = 34;
    const int CJ = 35;
    const int CQ = 36;
    const int CK = 37;
    const int CA = 38;
    const int H2 = 39;
    const int H3 = 40;
    const int H4 = 41;
    const int H5 = 42;
    const int H6 = 43;
    const int H7 = 44;
    const int H8 = 45;
    const int H9 = 46;
    const int HT = 47;
    const int HJ = 48;
    const int HQ = 49;
    const int HK = 50;
    const int HA = 51;

    public enum BetTypes
    {
        checking,
        calling,
        raising,
        allIn,
        folding
    }

    public enum GameStates
    {
        Ante,
        HoldCardBet,
        FlopBet,
        TurnBet,
        RiverBet,
        Idle,
        EndGame,
        PlayerWin,
        PlayerLose,
        GameOver
    }

    public enum PairTypes
    {
        Bottom,
        Middle,
        Top,
        Pocket
    }

    int _ante = 0;
    //int _bet = 1;
    int _raise = 2;
    //int _call = 3;
    //int _check = 4;

    Image[] cards = new Image[52];
    int[] deck = new int[52];

    VirtualPlayer[] virtualPlayers = new VirtualPlayer[20];
    VirtualPlayer[] virtualTempPlayers = new VirtualPlayer[20];

    int[,] playerHands = new int[,]    {    {0xFF,0xFF},
                                                {0xFF,0xFF},
                                                {0xFF,0xFF},
                                                {0xFF,0xFF},
                                                {0xFF,0xFF},
                                                {0xFF,0xFF}
                                            };
    int[] playerHoleCardsRankings = new int[6];
    int[] playerFiveCardRankings = new int[6];
    string[] PayTableStrings = new string[10]{   "ROYAL FLUSH",
                                                    "STRAIGHT FLUSH",
                                                    "FOUR OF A KIND",
                                                    "FULL HOUSE",
                                                    "FLUSH",
                                                    "STRAIGHT",
                                                    "THREE OF A KIND",
                                                    "TWO PAIR",
                                                    "PAIR",
                                                    "HIGH CARD"};





    public int betStringPtr;
    public string[] betWindowTitles = new string[]{    "PLACE YOUR BET",
                                                            "BET AMOUNT",
                                                            "RAISE AMOUNT"};

    public string[] betButtonTitles = new string[] {    "BET THE HOLE CARDS",
                                                            "BET THE FLOP",
                                                            "BET THE TURN",
                                                            "BET THE RIVER",
                                                            "BET LAST ROUND",
                                                            "PLAYER WIN !!!"};

    public string[] gameOverStrings = new string[] {    "GAME OVER",
                                                            "PLAYER WIN                 ",
                                                            "1/2 BET RETURNED           ",
                                                            "POT WAS SPLIT              ",
                                                            "TEXAS HOLD'EM FOLDUP BONUS ",
                                                            ""};

    public string[] instrucionStrings = new string[11];

    public string realPlayerName = "                                  ";

    public int[] gaffHand = new int[] { D6, CJ, HA, SA, C7, S4, DQ, S2, H7, DK, DJ, H9, S2, H2, SA, CJ, H8 };

    public int[,] gaffHands = new int[,] {  {D6, CJ, H3, C8, C7, S4, DQ, S2, H7, DK, DJ, H9, S2, H2, SA, CJ, H8},
                                                {D6, CJ, H6, C8, C7, S4, DQ, S2, H7, DK, DJ, H9, S2, H2, SA, CJ, H4},
                                                {C5, C6, D5, S9, CQ, HA, S5, D9, HJ, S4, SK, SJ, CK, DA, D2, H6, C3},
                                                {CK, DK, CA, DA, C3, H9, S3, D9, HA, HK, SQ, SJ, C7, D2, C4, H8, S5},
                                                {47, 39, 30, 51, 1, 42, 7, 34, 3, 19, 31, 43, 9, 17, 29, 37, 5},
                                            };

    public int[] adjustedRanks = new int[] {ROYAL_FLUSH,
                                                STRAIGHT_FLUSH,
                                                FOUR_OF_A_KIND,
                                                FULL_HOUSE,
                                                FLUSH,
                                                STRAIGHT,
                                                THREE_OF_A_KIND,
                                                TWO_PAIR,
                                                PAIR
                                                };

    const int ROYAL_FLUSH = 21;
    const int STRAIGHT_FLUSH = 20;
    const int HIGH_FOUR_OF_A_KIND = 19;
    const int MID_FOUR_OF_A_KIND = 18;
    const int FOUR_OF_A_KIND = 17;
    const int FULL_HOUSE = 16;
    const int FLUSH = 15;
    const int STRAIGHT = 14;
    const int HIGH_THREE_OF_A_KIND = 13;
    const int MID_THREE_OF_A_KIND = 12;
    const int THREE_OF_A_KIND = 11;
    const int TWO_PAIR = 10;
    const int HIGH_PAIR = 9;
    const int MID_PAIR = 8;
    const int PAIR = 7;
    const int FOUR_TO_A_FLUSH = 6;
    const int THREE_TO_A_FLUSH = 5;
    const int FOUR_TO_A_STRAIGHT_INSIDE = 4;
    const int THREE_TO_A_STRAIGHT_INSIDE = 3;
    const int FOUR_TO_A_STRAIGHT_OUTSIDE = 2;
    const int THREE_TO_A_STRAIGHT_OUTSIDE = 1;

    cardValues[,] Group = new cardValues[,] {
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

    int[] communityCards = new int[5];

    int x = 0;

    int deckPtr;

    int buttonPosition = 0;

    int selectedColumn;

    double playerCurrentBet = 0;//the real players current bet
    double playerCurrentRaise = 0; //the real players last raise amount

    bool[] ties = new bool[Settings.playerSize];

    public double anteBet = 0;

    public double potAmount = 0;
    public double PotAmount
    {
        get
        {
            return potAmount;
        }
        set
        {
            potAmount = value;
            string dollarAmount = String.Format("{0:C}", potAmount);
            lblPot.GetComponent<Text>().text = dollarAmount;
        }
    }

    public double playerBet = 0;
    public double PlayerBet
    {
        get
        {
            return playerBet;
        }
        set
        {
            playerBet = value;
            string dollarAmount = String.Format("{0:C}", playerBet);
            lblBet.GetComponent<Text>().text = dollarAmount;
        }
    }

    public double playerRaise = 0;
    public double PlayerRaise
    {
        get
        {
            return playerRaise;
        }
        set
        {
            playerRaise = value;
            string dollarAmount = String.Format("{0:C}", playerRaise);
            lblRaise.GetComponent<Text>().text = dollarAmount;
        }
    }

    public double callAmount = 0;//amount required to call
    public double CallAmount
    {
        get
        {
            return callAmount;
        }
        set
        {
            callAmount = value;
            string dollarAmount = String.Format("{0:C}", callAmount);
            lblCall.GetComponent<Text>().text = dollarAmount;
        }
    }

    double playerCredits = 0;
    public double RealPlayerCredits
    {
        set
        {
            playerCredits = value;
            string dollarAmount = String.Format("{0:C}", playerCredits);

            if (playerAllCredits != null) playerAllCredits.GetComponent<Text>().text = dollarAmount;
        }
        get
        {
            return playerCredits;
        }
    }

    public double PlayerCredits
    {
        set
        {
            if (jurisdictionalLimit == 0)
            {
                RealPlayerCredits = value;
                return;
            }
            double newValue = value;
            if (newValue >= jurisdictionalLimit)//we have more than the limit allows
            {
                dollarAmount = jurisdictionalLimit;//make it the limit
                RealPlayerCredits += newValue - jurisdictionalLimit;//give the rest to the real credit meter
            }
            else//the new value is equal or less than the limit
            {
                double difference = jurisdictionalLimit - value;
                dollarAmount = value;
            }
        }
        get
        {
            if (jurisdictionalLimit == 0)
            {
                return RealPlayerCredits;
            }
            return dollarAmount;
        }
    }

    protected double win = 0;
    public double WinAmount
    {
        set
        {
            win = value;
            string dollarAmount = String.Format("{0:C}", win);
            lblWin.GetComponent<Text>().text = dollarAmount;
            if (win > 0)
            {
                //videoWin.Play();
            }
        }
        get
        {
            return win;
        }
    }

    int currentBetPosition = 0;
    public int CurrentBetPosition
    {
        set
        {
            currentBetPosition = value;
            if (currentBetPosition > 5)
            {
                currentBetPosition = 0;
            }
        }
        get
        {
            return currentBetPosition;
        }
    }

    public int VideoMultiplier
    {
        set
        {
            //TODO:
            //if (value > paytableGrid.ColumnCount - 1)
            //{
            //    videoMultiplier = paytableGrid.ColumnCount - 1;
            //}
            //else
            //{
            videoMultiplier = value;
            //}
        }
        get
        {
            return videoMultiplier;
        }
    }


    System.Random rand = new System.Random();

    int cardsDealt = 0;

    public GameStates GameState;
    public BetTypes BetType;

    GamePlayer[] GamePlayers = new GamePlayer[Settings.playerSize];

    public bool[] GameWinners = new bool[Settings.playerSize];
    public int PotSplit = 1;

    bool PlayerSurrender = false;

    int gameOverPtr = 0;

    string foldString;
    string checkString;
    string callString;
    string raiseString;
    string allInString;
    public string surrenderString;
    string continueString;
    public string surrenderBoxString;

    bool winnerDeclared = false;
    double videoPokerWin = 0;
    int videoPokerLowRank = 15;

    bool isNextPlayer = false;

    Color pixelColor;

    //TODO: Add Audio/Music Support
    /*
    System.Media.SoundPlayer dealSound = new System.Media.SoundPlayer(SoundResource.highlight);
    public System.Media.SoundPlayer buttonSound = new System.Media.SoundPlayer(SoundResource.push3);
    System.Media.SoundPlayer raiseSound = new System.Media.SoundPlayer(SoundResource.timerbeep);
    System.Media.SoundPlayer callSound = new System.Media.SoundPlayer(SoundResource.s_bang);
    System.Media.SoundPlayer videoWin = new System.Media.SoundPlayer(SoundResource.VideoWin);
    */

    double AntePotAmount = 0;
    double FlopPotAmount = 0;
    double TurnPotAmount = 0;
    double RiverPotAmount = 0;

    int virtualPlayerRaised = 0;
    bool flopTurnRiverRaised = false;

    bool testDataRetrieved = false;

    int ThisRoundRaisePercentage = 0;
    bool DealButtonPassed = false;


    double gamePercentage = 0;
    int playerWithBestHand = 0;
    public double lastBet = 0;
    public bool autoStart = false;

    //System.Drawing.Color winColor = System.Drawing.Color.Red;
    // AmountWindow creditLimitWindow; //TODO: seems unused - need check it
    public double jurisdictionalLimit;

    Image[] chipBoxes;
    DateTime now;
    int year;

    #endregion

    public LoadOnClick()
    {
        if (Settings.isDebug) Debug.Log("LoadOnClick()");
        payTable = new Assets.Scripts.PayTable();
    }


    //void gameOverTimer_Tick(object sender, EventArgs e)
    void gameOberTimer()
    {
        if (year == Settings.year)
        {
            if (Settings.gameEnable == false)
            {
                //gameOverTimer.Stop();
                isNextPlayer = false;

                //MessageBox.Show("Correct the INI file error", "INI File Error");
                //Close();
            }
        }

        // bonusPokerPanel.SetActive(true);

        if (gameOverPtr == 1 && winnerDeclared == false)
            gameOverPtr++;
        if (gameOverPtr == 2 && PlayerSurrender == false)
            gameOverPtr++;
        if (gameOverPtr == 3 && PotSplit < 2)
            gameOverPtr++;
        if (gameOverPtr == 4 && videoPokerWin == 0)
            gameOverPtr++;

        int size = (int)(18 - lblWin.GetComponent<Font>().fontSize);

        lblWin.SetActive(true);
        if (gameOverPtr == 2)
        {

            if (WinAmount > 0)//the player won something
            {
                string dollarAmount = String.Format("{0:C}", WinAmount);
                lblWin.GetComponent<Text>().text = gameOverStrings[gameOverPtr] + dollarAmount;
            }
            else
            {
                lblWin.GetComponent<Text>().text = gameOverStrings[gameOverPtr];
            }
        }
        if (gameOverPtr == 3)
        {
            gameOverStrings[3] = "THE POT WAS SPLIT ";
            gameOverStrings[3] += PotSplit.ToString() + " WAYS";
            //lblWin.Text = gameOverStrings[gameOverPtr];
        }
        if (gameOverPtr == 4)
        {
            gameOverStrings[4] = "TEXAS HOLD'EM FOLDUP BONUS ";
            string dollarAmount = String.Format("{0:C}", videoPokerWin);
            gameOverStrings[4] += dollarAmount;
        }
        else//2 or 3
        {
            lblWin.GetComponent<Text>().text = gameOverStrings[gameOverPtr];
        }
        gameOverPtr++;
        if (gameOverPtr > 5)
            gameOverPtr = 0;

        if (jurisdictionalLimit == 0)
        {
            if (PlayerCredits < Settings.playerCreditsLimit)
            {
                btnCredit.SetActive(true);
            }
            else
            {
                btnCredit.SetActive(false);
            }
        }

        if (AutoPlay == true)
        {
            if (PlayerCredits < Settings.playerCreditsLimit)
            {
                PlayerCredits = Settings.jurisdictionalBetLimit;
            }
            btnNewGame_Click();// sender, e);
        }
    }

    void nextPlayerTimer_Tick()//object sender, EventArgs e)
    {
        if (isNextPlayer)
        {
            NextPlayer();
        }
        else
        {
            isNextPlayer = false;
            // nextPlayerTimer.Stop();
            //if (MessageBox.Show("Next Player Wait", "Waiting", MessageBoxButtons.OK) == DialogResult.OK)
            //{
            //    NextPlayer();
            //}
        }
    }

    public void BuildVirtualPlayerProfilesFromIniFile()
    {
        int i = 0;
        bool done = false;
        int test;
        int test1;
        virtualPlayerCount--;//we don't want to count the auto player
        string Player = "                                                        ";
        //string name = new string(' ',20);
        string[] stringArray = new string[20];
        //virtualPlayers[0] = new VirtualPlayer();//create a virtual player for the actual player
        string fileName = Settings.pathToAssetRes + Settings.iniFile;
        do
        {
            Player = "Player" + i.ToString();
            //test to see if there is anything in the player if not we are done. 
            int charsTransferred;// = Win32Support.GetPrivateProfileString(Player, "Hole Min Threshold", null, temp, 5, currentDirectory + "\\TexasHoldem.ini");
            string iniTest = IniFileHandler.GetIniString(Player, "Hole Min Threshold", null, out charsTransferred, Settings.pathToAssetRes + Settings.iniFile);
            if (charsTransferred == 0)
            {
                done = true;
            }
            else
            {
                virtualPlayerCount++;
                try
                {
                    virtualTempPlayers[i] = new VirtualPlayer();
                    virtualTempPlayers[i].playerNumber = i;
                    for (int x = 0; x < Settings.playerSize; x++)//lets get the raise parameters
                    {
                        virtualTempPlayers[i].RaiseLevels[x] = new RaiseLevel();
                    }
                    for (int x = 0; x < 8; x++)//get the fold stuff
                    {
                        virtualTempPlayers[i].FoldLevels[x] = new FoldLevel();
                    }
                    int testchars;
                    virtualTempPlayers[i].Name = IniFileHandler.GetIniString(Player, "Player Name", "Player " + i.ToString(), out testchars, fileName);
                    virtualTempPlayers[i].FoldOnAnyRaise = IniFileHandler.GetIniBool(Player, "Fold On Any Raise", false, Settings.pathToAssetRes + Settings.iniFile);
                    //string value;
                    virtualTempPlayers[i].HoleMinThreshold = IniFileHandler.GetIniInt(Player, "Hole Min Threshold", 72, fileName);
                    for (int x = 0; x < Settings.playerSize; x++)//lets get the raise parameters
                    {
                        test1 = x;
                        string raiseHand = "Hole Raise " + (x + 1).ToString() + " Hand Array";
                        virtualTempPlayers[i].RaiseLevels[x].RaiseHands = IniFileHandler.GetINIIntArray(Player, raiseHand, 1, fileName);

                        string holeRaiseRange = "Hole Raise " + (x + 1).ToString() + " Range";
                        virtualTempPlayers[i].RaiseLevels[x].Range = IniFileHandler.GetINIDoubleArray(Player, holeRaiseRange, 2, fileName);
                        virtualTempPlayers[i].RaiseLevels[x].RaisePercentage = IniFileHandler.GetIniInt(Player, "Hole Raise " + (x + 1).ToString() + " Percentage", 50, fileName);

                        string holeReRaiseRange = "Hole Raise " + (x + 1).ToString() + " ReRaise Range";
                        virtualTempPlayers[i].RaiseLevels[x].ReRaiseRange = IniFileHandler.GetINIDoubleArray(Player, holeReRaiseRange, 2, fileName);

                        virtualTempPlayers[i].RaiseLevels[x].ReRaisePercentage = IniFileHandler.GetIniInt(Player, "Hole Raise " + (x + 1).ToString() + " ReRaise Percentage", 50, fileName);

                    }

                    for (int x = 0; x < 8; x++)//get the fold stuff
                    {
                        test = x;
                        string holeFoldHands = "Hole Fold " + (x + 1).ToString() + " Hand Array";
                        virtualTempPlayers[i].FoldLevels[x].FoldHands = IniFileHandler.GetINIIntArray(Player, holeFoldHands, 1, fileName);
                        virtualTempPlayers[i].FoldLevels[x].Range = IniFileHandler.GetINIDoubleArray(Player, "Hole Fold " + (x + 1).ToString() + " Range", 2, fileName);
                    }

                    virtualTempPlayers[i].BluffHands = IniFileHandler.GetINIIntArray(Player, "Bluff Hands", 1, fileName);
                    virtualTempPlayers[i].SlowPlayHands = IniFileHandler.GetINIIntArray(Player, "Slow Play Hands", 1, fileName);
                    virtualTempPlayers[i].AllInHands = IniFileHandler.GetINIIntArray(Player, "Hole All In Hands", 1, fileName);
                    virtualTempPlayers[i].BluffPercentage = IniFileHandler.GetIniInt(Player, "Bluff Percentage", 0, fileName);
                    virtualTempPlayers[i].BluffCallRaisePercentage = IniFileHandler.GetIniInt(Player, "Bluff Call Raise Percentage", 50, fileName);
                    virtualTempPlayers[i].Folded = false;

                    virtualTempPlayers[i].FlopNoRaiseBetPercentages = IniFileHandler.GetINIIntArray(Player, "Flop No Raise Bet Percentages", 21, fileName);
                    virtualTempPlayers[i].TurnNoRaiseBetPercentages = IniFileHandler.GetINIIntArray(Player, "Turn No Raise Bet Percentages", 21, fileName);
                    virtualTempPlayers[i].RiverNoRaiseBetPercentages = IniFileHandler.GetINIIntArray(Player, "River No Raise Bet Percentages", 21, fileName);

                }
                catch (FormatException e)
                {
                    //TODO: MessageBox.Show(e.Message, "INI FILE Error");
                    string ex = e.Message;
                }
                i++;
            }
        } while (done == false);

        virtualPlayers[0] = new VirtualPlayer();//create a virtual player for the actual player
        virtualPlayers[0] = virtualTempPlayers[0];//use Otto for autoplay
    }

    public void BuildVirtualPlayerProfiles()
    {
        virtualPlayers = Settings.GetVirtualPlayers().ToArray();
        //virtualPlayers[0] = new VirtualPlayer();//create a virtual player for the actual player
        //virtualPlayers[0] = virtualTempPlayers[0];//use Otto for autoplay
    }


    public void ShuffleVirtualPlayers() //TODO: fix read/write from ini file
    {
        if (virtualPlayerCount <= 0)
        {
            if (Settings.isDebug) Debug.Log("Can't ShuffleVirtualPlayers() because VirtualPlayerCount should be > 0 - NOW is: " + virtualPlayerCount.ToString());
            return;
        }

        int i;
        int[] players = new int[5];
        for (i = 0; i < 5; i++)
        {
            int a = 0;
            int temp = rand.Next(1, virtualPlayerCount + 1);
            for (a = 0; a <= i; a++)
            {
                if (temp == players[a])//dupe?
                {
                    i--;
                    break;
                }
                else
                {
                    if (a == i)
                    {
                        players[i] = temp;//we have our player
                    }
                }
            }
        }
        for (int x = 1; x < virtualPlayerCount; x++)
        {
            virtualPlayers[x] = null;
        }
        for (int x = 1; x < Settings.playerSize; x++)
        {
            //int r = rand.Next(1,virtualPlayerCount+1);
            if (virtualPlayers[x] == null)
            {
                virtualPlayers[x] = new VirtualPlayer();
                virtualPlayers[x] = virtualTempPlayers[players[x - 1]];
            }
        }
    }

    public void updateBettingButtonTitle()
    {
        switch (GameState)
        {
            case GameStates.HoldCardBet:
                {
                    lblBettingGroup.GetComponent<Text>().text = betButtonTitles[0];
                }
                break;
            case GameStates.FlopBet:
                {
                    lblBettingGroup.GetComponent<Text>().text = betButtonTitles[1];
                }
                break;
            case GameStates.TurnBet:
                {
                    lblBettingGroup.GetComponent<Text>().text = betButtonTitles[2];
                }
                break;
            case GameStates.RiverBet:
                {
                    lblBettingGroup.GetComponent<Text>().text = betButtonTitles[3];
                }
                break;
            case GameStates.EndGame:
                {
                    lblBettingGroup.GetComponent<Text>().text = betButtonTitles[4];
                }
                break;
            case GameStates.PlayerWin:
                {
                    lblBettingGroup.GetComponent<Text>().text = betButtonTitles[5];
                }
                break;
        }

    }

    public void updateFoldedPlayersImages(bool visible)
    {
        for (int x = 1; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x].Folded == false)
            {
                continue;
            }
            switch (x)
            {
                case 1:
                    player1hold1.SetActive(visible);
                    player1hold2.SetActive(visible);
                    break;
                case 2:
                    player2hold1.SetActive(visible);
                    player2hold2.SetActive(visible);
                    break;
                case 3:
                    player3hold1.SetActive(visible);
                    player3hold2.SetActive(visible);
                    break;
                case 4:
                    player4hold1.SetActive(visible);
                    player4hold2.SetActive(visible);
                    break;
                case 5:
                    player5hold1.SetActive(visible);
                    player5hold2.SetActive(visible);
                    break;
            }
        }
    }

    public bool getWeightedResult(int value)
    {
        int temp = rand.Next(100);
        if (temp <= value)
        {
            return true;
        }
        return false;
    }

    public int getWeightedIntResult(int value)
    {
        int temp = rand.Next(100);
        if (temp <= value)
        {
            return 1;
        }
        return 0;
    }

    public void DealNextRound()
    {
        if (GameState == GameStates.HoldCardBet)
        {
            dealPlayerCards(); // give 2 cards to each plaer
            for (int x = 1; x < Settings.playerSize; x++) 
            {
                UpdateCreditLabel(x);
                //chipImageList.Draw(formHwnd, chipBoxes[x - 1].Location, getWeightedIntResult(50)); TODO: // draw chip, counter, shtick, schtick, specialty, check
            }
        }
        if (GameState == GameStates.FlopBet)
        {
            dealFlop();
        }
        if (GameState == GameStates.TurnBet)
        {
            dealTurn();
        }
        if (GameState == GameStates.RiverBet)
        {
            dealRiver();
        }
    }

    public int IncrementButtonPosition(bool getposition)
    {
        //TODO: fix button position ??
        /*
        if (getposition == false)
        {
            buttonImage0.SetActive(false);
            buttonImage1.SetActive(false);
            buttonImage2.SetActive(false);
            buttonImage3.SetActive(false);
            buttonImage4.SetActive(false);
            buttonImage5.SetActive(false);

            buttonPosition--;
            if (buttonPosition < 0)
            {
                buttonPosition = 5;
            }
            switch (buttonPosition)
            {
                case 0: buttonImage0.SetActive(true); buttonImage0.Image = dealerButtonImageList.Images[0]; break;
                case 1: buttonImage1.SetActive(true); buttonImage1.Image = dealerButtonImageList.Images[0]; break;
                case 2: buttonImage2.SetActive(true); buttonImage2.Image = dealerButtonImageList.Images[0]; break;
                case 3: buttonImage3.SetActive(true); buttonImage3.Image = dealerButtonImageList.Images[0]; break;
                case 4: buttonImage4.SetActive(true); buttonImage4.Image = dealerButtonImageList.Images[0]; break;
                case 5: buttonImage5.SetActive(true); buttonImage5.Image = dealerButtonImageList.Images[0]; break;
            }

        }*/
        return buttonPosition;
    }

    public void UpdateBetLabel(string text, int player, bool yellow)
    {
        if (yellow)
        {
            //betLabels[player].ForeColor = System.Drawing.Color.Yellow;
        }
        else
        {
            //betLabels[player].ForeColor = System.Drawing.Color.White;
        }
        betLabels[player].GetComponent<Text>().text = text;
        betLabels[player].SetActive(true);
    }

    public void UpdateCreditLabel(int player)
    {
        if (player == 0) return;
        creditLabels[player].GetComponent<Text>().text = "";
        double amount = virtualPlayers[player].Credits;
        string dollarAmount = String.Format("{0:C}", amount);
        creditLabels[player].GetComponent<Text>().text = dollarAmount;
        creditLabels[player].SetActive(true);
    }

    public void clearBetLabel(int player)
    {
        betLabels[player].GetComponent<Text>().text = "";
    }

    public void clearBetLabels()
    {
        for (int x = 0; x < Settings.playerSize; x++)
        {
            betLabels[x].GetComponent<Text>().text = "";
            betLabels[x].SetActive(false);
        }
    }

    public void clearCreditLabels()
    {
        for (int x = 1; x < Settings.playerSize; x++)
        {
            creditLabels[x].GetComponent<Text>().text = "";
            creditLabels[x].SetActive(false);
        }
        clearChips();
    }
    public void clearChips()
    {
        //TODO:
        /*
        chipBox1.Image = null;
        chipBox2.Image = null;
        chipBox3.Image = null;
        chipBox4.Image = null;
        chipBox5.Image = null;
        */
    }

    public void restoreCardDefaults(bool firstPass)
    {
        for (int x = 0; x < Settings.playerSize; x++)
        {
            if (betLabels[x] != null)
            {
                betLabels[x].GetComponent<Text>().text = "";
                betLabels[x].SetActive(false);
            }
        }

        for (int x = 1; x < Settings.playerSize; x++)
        {
            if (creditLabels[x] != null)
            {
                creditLabels[x].GetComponent<Text>().text = "";
                creditLabels[x].SetActive(true); // GetComponentInParent<GameObject>().SetActive(true);
            }
        }
    }


    private void TestShuffleDeck(int[,] gaffHand, int element)
    {
        deck[0] = gaffHand[element, 0];// 0 1
        deck[6] = gaffHand[element, 1];// 0 2

        deck[1] = gaffHand[element, 2];// 1
        deck[7] = gaffHand[element, 3];// 1 

        deck[2] = gaffHand[element, 4];// 2
        deck[8] = gaffHand[element, 5];// 2

        deck[3] = gaffHand[element, 6];// 3
        deck[9] = gaffHand[element, 7];// 3

        deck[4] = gaffHand[element, 8];// 4
        deck[10] = gaffHand[element, 9];//4

        deck[5] = gaffHand[element, 10];// 5
        deck[11] = gaffHand[element, 11];//5

        deck[12] = gaffHand[element, 12];//flop 1
        deck[13] = gaffHand[element, 13];//flop 2//17
        deck[14] = gaffHand[element, 14];//flop 3

        deck[15] = gaffHand[element, 15];//turn
        deck[16] = gaffHand[element, 16];//river
    }

    private void shuffleDeck()
    {

        for (deckPtr = 0; deckPtr < Settings.cardsSize; deckPtr++)
        {
            deck[deckPtr] = 0xFF;
        }
        deckPtr = 0;
        int i;
        for (i = 0; i < Settings.cardsSize; i++)
        {
            int a = 0;
            int temp = rand.Next(Settings.cardsSize);
            for (a = 0; a <= i; a++)
            {
                if (temp == deck[a])//dupe?
                {
                    i--;
                    break;
                }
                else
                {
                    if (a == i)
                    {
                        deck[i] = temp;//we have our card
                    }
                }
            }
        }
    }

    private void dealCard(int target, bool firstCard, bool cardback)
    {
        switch (target)
        {
            case 0:
                {
                    if (firstCard)
                    {
                        playerHands[target, 0] = deck[deckPtr];
                        cardsOfPlayer[0].sprite = cardsAll[playerHands[target, 0]];
                        GamePlayers[target].hand.cardHand[0] = playerHands[target, 0];
                    }
                    else
                    {
                        playerHands[target, 1] = deck[deckPtr];
                        cardsOfPlayer[1].sprite = cardsAll[playerHands[target, 1]];
                        GamePlayers[target].hand.cardHand[1] = playerHands[target, 1];
                    }
                }
                break;
            case 1:
                {
                    if (firstCard)
                    {
                        playerHands[target, 0] = deck[deckPtr];
                        if (cardback && Settings.testGame == false)
                        {
                            cardsOfPlayer[2].sprite = cardsAll[52];
                        }
                        else
                        {
                            cardsOfPlayer[2].sprite = cardsAll[playerHands[target, 0]];
                        }
                        GamePlayers[target].hand.cardHand[0] = playerHands[target, 0];
                        // player1hold1.GetComponent<Image>().sprite = cardsAll[playerHands[target, 0]];
                    }
                    else
                    {
                        playerHands[target, 1] = deck[deckPtr];
                        if (cardback)
                        {
                            if (Settings.testGame == true)
                            {
                                cardsOfPlayer[3].sprite = cardsAll[playerHands[target, 1]];
                            }
                            else
                            {
                                cardsOfPlayer[3].sprite = cardsAll[52];
                            }
                        }
                        else
                        {
                            cardsOfPlayer[3].sprite = cardsAll[playerHands[target, 1]];
                        }
                        GamePlayers[target].hand.cardHand[1] = playerHands[target, 1];
                        // player1hold2.GetComponent<Image>().sprite = cardsAll[playerHands[target, 1]];
                    }
                }
                break;
            case 2:
                {
                    if (firstCard)
                    {
                        playerHands[target, 0] = deck[deckPtr];
                        if (cardback && Settings.testGame == false)
                        {
                            cardsOfPlayer[4].sprite = cardsAll[52];
                        }
                        else
                        {
                            cardsOfPlayer[4].sprite = cardsAll[playerHands[target, 0]];
                        }
                        GamePlayers[target].hand.cardHand[0] = playerHands[target, 0];
                        // player2hold1.GetComponent<Image>().sprite = cardsAll[playerHands[target, 0]];
                    }
                    else
                    {
                        playerHands[target, 1] = deck[deckPtr];
                        if (cardback)
                        {
                            if (Settings.testGame == true)
                            {
                                cardsOfPlayer[5].sprite = cardsAll[playerHands[target, 1]];
                            }
                            else
                            {
                                cardsOfPlayer[5].sprite = cardsAll[52];
                            }
                        }
                        else
                        {
                            cardsOfPlayer[5].sprite = cardsAll[playerHands[target, 1]];
                        }
                        GamePlayers[target].hand.cardHand[1] = playerHands[target, 1];
                        // player2hold2.GetComponent<Image>().sprite = cardsAll[playerHands[target, 1]];
                    }
                }
                break;
            case 3:
                {
                    if (firstCard)
                    {
                        playerHands[target, 0] = deck[deckPtr];
                        if (cardback && Settings.testGame == false)
                        {
                            cardsOfPlayer[6].sprite = cardsAll[52];
                        }
                        else
                        {
                            cardsOfPlayer[6].sprite = cardsAll[playerHands[target, 0]];
                        }
                        GamePlayers[target].hand.cardHand[0] = playerHands[target, 0];
                        // player3hold1.GetComponent<Image>().sprite = cardsAll[playerHands[target, 0]];
                    }
                    else
                    {
                        playerHands[target, 1] = deck[deckPtr];
                        if (cardback)
                        {
                            if (Settings.testGame == true)
                            {
                                cardsOfPlayer[7].sprite = cardsAll[playerHands[target, 1]];
                            }
                            else
                            {
                                cardsOfPlayer[7].sprite = cardsAll[52];
                            }
                        }
                        else
                        {
                            cardsOfPlayer[7].sprite = cardsAll[playerHands[target, 1]];
                        }
                        GamePlayers[target].hand.cardHand[1] = playerHands[target, 1];
                        // player3hold2.GetComponent<Image>().sprite = cardsAll[playerHands[target, 1]];
                    }
                }
                break;
            case 4:
                {
                    if (firstCard)
                    {
                        playerHands[target, 0] = deck[deckPtr];
                        if (cardback && Settings.testGame == false)
                        {
                            cardsOfPlayer[8].sprite = cardsAll[52];
                        }
                        else
                        {
                            cardsOfPlayer[8].sprite = cardsAll[playerHands[target, 0]];
                        }
                        GamePlayers[target].hand.cardHand[0] = playerHands[target, 0];
                        // player4hold1.GetComponent<Image>().sprite = cardsAll[playerHands[target, 0]];
                    }
                    else
                    {
                        playerHands[target, 1] = deck[deckPtr];
                        if (cardback)
                        {
                            if (Settings.testGame == true)
                            {
                                cardsOfPlayer[9].sprite = cardsAll[playerHands[target, 1]];
                            }
                            else
                            {
                                cardsOfPlayer[9].sprite = cardsAll[52];
                            }
                        }
                        else
                        {
                            cardsOfPlayer[9].sprite = cardsAll[playerHands[target, 1]];
                        }
                        GamePlayers[target].hand.cardHand[1] = playerHands[target, 1];
                        // player4hold2.GetComponent<Image>().sprite = cardsAll[playerHands[target, 1]];
                    }
                }
                break;
            case 5:
                {
                    if (firstCard)
                    {
                        playerHands[target, 0] = deck[deckPtr];
                        if (cardback && Settings.testGame == false)
                        {
                            cardsOfPlayer[10].sprite = cardsAll[52];
                        }
                        else
                        {
                            cardsOfPlayer[10].sprite = cardsAll[playerHands[target, 0]];
                        }
                        GamePlayers[target].hand.cardHand[0] = playerHands[target, 0];
                        // player5hold1.GetComponent<Image>().sprite = cardsAll[playerHands[target, 0]];
                    }
                    else
                    {
                        playerHands[target, 1] = deck[deckPtr];
                        if (cardback)
                        {
                            if (Settings.testGame == true)
                            {
                                cardsOfPlayer[11].sprite = cardsAll[playerHands[target, 1]];
                            }
                            else
                            {
                                cardsOfPlayer[11].sprite = cardsAll[52];
                            }
                        }
                        else
                        {
                            cardsOfPlayer[11].sprite = cardsAll[playerHands[target, 1]];
                        }
                        GamePlayers[target].hand.cardHand[1] = playerHands[target, 1];
                        // player5hold2.GetComponent<Image>().sprite = cardsAll[playerHands[target, 1]];
                    }
                }
                break;
        }
        deckPtr++;
        //dealSound.Play();
    }

    public void InitializeNewGame()
    {
        shuffleDeck();


        //TestShuffleDeck(gaffHands, 3);

        if (testDataRetrieved == true)
        {
            UseTestData();
            //testDataRetrieved = false;
        }
        //36,25,12,34,31,47,49,17,11,35,40,5,41,24,38,9,18,2,7,8,6,4,5

        testDataRetrieved = false;

        int player = 0;
        for (player = 0; player < Settings.playerSize; player++)
        {
            GamePlayers[player] = null;
            GamePlayers[player] = new GamePlayer();
        }
        restoreCardDefaults(false);
        DisableBettingButtons();
    }

    public void dealPlayerCards()
    {
        if (Settings.isDebug) Debug.Log("dealPlayerCards()");
        InitializeNewGame();
        bool firstcard = true;
        for (int card = 0; card < 2; card++)
        {
            if (card != 0)
            {
                firstcard = false;
            }

            for (int player = 0; player < Settings.playerSize; player++)
            {
                dealCard(loop[buttonPosition + player + 1], firstcard, true);
                if (player != 5)
                {
                    //TODO: Thread.Sleep(dealDelay);
                }
            }
        }
        x++;
        if (x > 51)
        {
            x = 0;
        }
        GameState = GameStates.HoldCardBet;
        playerWithBestHand = GetPlayerWithBestHand();
    }


    private void dealFlop()
    {
        communityCards[0] = deck[deckPtr++];
        cardsPublic[0].sprite = cardsAll[communityCards[0]];

        Thread.Sleep(Settings.dealDelay);
        //dealSound.Play();

        communityCards[1] = deck[deckPtr++];
        cardsPublic[1].sprite = cardsAll[communityCards[1]];

        Thread.Sleep(Settings.dealDelay);
        //dealSound.Play();

        communityCards[2] = deck[deckPtr++];
        cardsPublic[2].sprite = cardsAll[communityCards[2]];

        //dealSound.Play();
        cardsDealt = 5;
        for (int x = 0; x < Settings.playerSize; x++)
        {
            GamePlayers[x].hand.cardHand[2] = communityCards[0];
            GamePlayers[x].hand.cardHand[3] = communityCards[1];
            GamePlayers[x].hand.cardHand[4] = communityCards[2];
            if (x > 0)
            {
                if (virtualPlayers[x].Folded == false)
                {
                    virtualPlayers[x].FiveCardHandRank = GetFiveCardRanking(x);
                }
            }
        }
        playerWithBestHand = GetPlayerWithBestHand();
    }
    private void dealTurn()
    {
        this.communityCards[3] = deck[deckPtr++];
        cardsPublic[3].sprite = cardsAll[communityCards[3]];

        //dealSound.Play();

        for (int x = 0; x < Settings.playerSize; x++)
        {
            GamePlayers[x].hand.cardHand[5] = communityCards[3];
            if (x > 0)
            {
                if (virtualPlayers[x].Folded == false)
                {
                    virtualPlayers[x].FiveCardHandRank = GetFiveCardRanking(x);
                }
            }
        }
        cardsDealt = 6;
        playerWithBestHand = GetPlayerWithBestHand();
    }

    private void dealRiver()
    {
        this.communityCards[4] = deck[deckPtr++];
        cardsPublic[4].sprite = cardsAll[communityCards[4]];

        //dealSound.Play();
        for (int x = 0; x < Settings.playerSize; x++)
        {
            GamePlayers[x].hand.cardHand[Settings.playerSize] = communityCards[4];
            if (x > 0)
            {
                if (virtualPlayers[x].Folded == false)
                {
                    virtualPlayers[x].FiveCardHandRank = GetFiveCardRanking(x);
                }
            }
        }
        cardsDealt = 7;
        playerWithBestHand = GetPlayerWithBestHand();
    }

    private void AllInPlayer(int player)
    {
        string dollarAmount = String.Format("{0:C}", virtualPlayers[player].Credits);
        if (virtualPlayers[player].Credits > 0)
        {
            UpdateBetLabel("ALL IN " + dollarAmount, player, true); //, System.Drawing.Color.Yellow);
        }
        else
        {
            UpdateBetLabel("ALL IN ", player, true); // System.Drawing.Color.Yellow);
        }
        virtualPlayers[player].RoundRaiseAmount += virtualPlayers[player].Credits;
        //virtualPlayers[player].Credits = 0;
        virtualPlayers[player].AllIn = true;
        UpdateCreditLabel(player);
    }

    private void RaisePlayer(int player, double amount)
    {
        string dollarAmount = String.Format("{0:C}", amount);
        UpdateBetLabel("RAISE " + dollarAmount, player, true);
        UpdateCreditLabel(player);
        virtualPlayers[player].RoundRaiseAmount += amount;
        //raiseSound.Play(); //TODO
    }

    private void CallPlayer(int player)
    {
        UpdateBetLabel("CALL", player, false);
        UpdateCreditLabel(player);
    }

    private void CheckPlayer(int player)
    {
        UpdateBetLabel("CHECK", player, false);
        virtualPlayers[player].RoundChecked = true;
    }

    private void FoldPlayer(int player)
    {
        if (player == 0)
        {
            betLabels[player].GetComponent<Text>().text = "FOLD";
            virtualPlayers[player].Folded = true;

            int rank = playerHoleCardsRankings[0] + 1;
            if ((GameState == GameStates.HoldCardBet) && (rank > Settings.surrenderReturnRank && virtualPlayers[0].AllIn == false && GetPlayerPairValue(0) < Settings.surrenderMinimumPair))
            {
                btnSurrenderClick();
            }
            else
            {
                btnFoldClick();
            }
            return;
        }
        cardsOfPlayer[(player * 2) + 1].sprite = cardsAll[53];
        ClearPlayerCards(player);

        cardsOfPlayer[player * 2].sprite = cardsAll[playerHands[player, 0]];
        cardsOfPlayer[(player * 2) + 1].sprite = cardsAll[playerHands[player, 1]];

        virtualPlayers[player].Folded = true;
    }

    private void ShowPlayerCards(int player, bool fold)
    {
        ClearPlayerCards(player);
        if (fold == true)
        {
            //TODO: cardsOfPlayer[(player * 2) + 1].X += offsetX;
        }
        cardsOfPlayer[(player * 2)].sprite = cardsAll[playerHands[player, 0]];
        cardsOfPlayer[(player * 2) + 1].sprite = cardsAll[playerHands[player, 1]];
    }

    private void ClearPlayerCards(int player)
    {
        cardsOfPlayer[(player * 2)].sprite = cardBg;
        cardsOfPlayer[(player * 2) + 1].sprite = cardBg;
    }

    public void EvalPlayerHands(bool hole, bool flop, bool turn, bool river)
    {
        bool suited;
        for (int x = 0; x < Settings.playerSize; x++)
        {
            if (hole == true)
            {
                suited = false;
                if (GetCardSuit(playerHands[x, 0]) == GetCardSuit(playerHands[x, 1]))
                {
                    suited = true;
                }
                playerHoleCardsRankings[x] = GetDealRanking(getCard(playerHands[x, 0]), getCard(playerHands[x, 1]), suited);
                if (GetCardValue(playerHands[x, 0]) == GetCardValue(playerHands[x, 1]))
                {
                    virtualPlayers[x].PocketPair = GetCardValue(playerHands[x, 0]);
                }
            }
            if (flop == true)
            {
                suited = false;
            }
            if (x < Settings.playerSize)
            {
                int firstCard = GetCardValue(playerHands[x, 0]);
                int secondCard = GetCardValue(playerHands[x, 1]);
                if (firstCard == 1) firstCard = 14;
                if (secondCard == 1) secondCard = 14;

                if (firstCard > secondCard)
                {
                    virtualPlayers[x].HighCard = firstCard;
                }
                else
                {
                    virtualPlayers[x].HighCard = secondCard;
                }
                try {
                    GamePlayers[x].hand.HighCard = virtualPlayers[x].HighCard;
                } catch(Exception e) {
                    if (Settings.isDebug)
                    {
                        Debug.LogError("GamePlayers.count " + GamePlayers.Count());
                        Debug.LogError("virtualPlayers.Count " + virtualPlayers.Count());
                        Debug.LogError("index " + x);

                        Debug.LogError("EvalPlayerHands() last line" + e.Message);
                    }
                }
            }
        }
    }



    public int EvaluateFlop(int player, int[] FlopHand)
    {
        int[] Hand = new int[5];
        int[] suitCounts = new int[4];
        int lastHighCard = 52;
        int flushCount = 0;
        int highFlush = 0;
        int[] foldedCards = new int[12];
        int foldedPtr = 0;
        int FlopRank = 0;

        //Lets see if there are any completed hands 
        FlopRank = EvaluatePokerHand(player, FlopHand);

        //since we are a fold up game the virtual players also get to see the folded cards
        for (int x = 0; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x].Folded == true)
            {
                foldedCards[foldedPtr++] = playerHands[x, 0];
                foldedCards[foldedPtr++] = playerHands[x, 1];
            }
        }
        //now we can figure out how many outs we have


        for (int x = 0; x < 5; x++)//lets put the cards in order
        {
            int highcard = 0;
            for (int c = 0; c < 5; c++)
            {
                if (FlopHand[c] > highcard)
                {
                    if (FlopHand[c] < lastHighCard)
                    {
                        highcard = FlopHand[c];
                    }
                }
            }
            lastHighCard = highcard;
            Hand[x] = highcard;
        }
        //lets figure out how many members of a flush we have

        for (int x = 0; x < 5; x++)
        {
            suitCounts[GetCardSuit(Hand[x]) - 1]++;
        }
        for (int x = 0; x < 4; x++)
        {
            if (suitCounts[x] >= flushCount)
            {
                flushCount = suitCounts[x];
                if (flushCount > 2)
                {
                    highFlush = x + 1;//the actual suit
                }
            }
        }

        return FlopRank;//////////////
    }

    public int EvaluateTurn(int[] TurnHand)
    {
        return 0;//////////////
    }

    public int GetFlushCount(int[] FinalSuit)
    {
        int FlushCount = 0;
        int[] suitCounts = new int[4];
        for (int x = 0; x < 5; x++)
        {
            suitCounts[FinalSuit[x] - 1]++;
        }
        for (int x = 0; x < 4; x++)
        {
            if (suitCounts[x] > FlushCount)
            {
                FlushCount = suitCounts[x];
            }
        }
        return FlushCount;
    }

    public int GetConnectedStraightRank(int[] FinalValue, int SaveValue)
    {
        int rank = 0;
        int[] connectCards = new int[5];
        for (int i = 0; i < 5; i++)
        {
            int subtractSaves = 0;
            for (int x = 0; x < 5; x++)
            {
                if (x != i)//don't compare the exact same card
                {
                    if (FinalValue[i] >= FinalValue[x])
                    {
                        if (FinalValue[i] - FinalValue[x] < 5)
                        {
                            if (FinalValue[x] == SaveValue)
                            {
                                subtractSaves++;
                            }
                            connectCards[i]++;
                        }
                    }
                    else
                    {
                        if (FinalValue[x] - FinalValue[i] < 5)
                        {
                            if (FinalValue[x] == SaveValue)
                            {
                                subtractSaves++;
                            }
                            connectCards[i]++;
                        }
                    }


                }
            }
            if (subtractSaves > 1)
                connectCards[i]--;

        }
        for (int x = 0; x < 5; x++)
        {
            if (connectCards[x] > rank)
            {
                rank = connectCards[x];
            }
        }
        return (rank);
    }

    public int GetInsideStraightRank(int[] FinalValue)
    {
        int MembersOfStraight = 0;
        int HighCard = 0;
        int ReturnValue = 0;

        for (int i = 15; i >= 1; i--)
        {
            HighCard = i;
            for (int x = 1; x <= 5; x++)
            {
                if (FinalValue[x - 1] == (HighCard - 1))
                {
                    HighCard = FinalValue[x - 1];
                    MembersOfStraight++;
                    x = 0;
                }
            }
            if (MembersOfStraight > ReturnValue)
            {
                ReturnValue = MembersOfStraight;

            }
            MembersOfStraight = 0;
        }
        return ReturnValue;
    }

    public int GetFiveCardTotal(int[] Hand)
    {
        int temp = 0;
        for (int x = 0; x < 5; x++)
        {
            temp += GetCardValue(Hand[x]);
        }
        return temp;
    }


    public int EvaluatePokerHand(int player, int[] FinalHand)
    {
        int XofaKind = 1, SaveValue = 0, CardValue, PayType = 0, HighCard = 0, MembersOfStraight = 1;
        int highValue = 0;
        bool oneOfaKind = false;
        bool twoOfaKind = false;
        bool threeOfaKind = false;
        bool flush = false;
        bool AceFound = false;
        bool KingFound = false;
        int[] FinalSuit = new int[5];
        int[] FinalValue = new int[5];

        int FlushCount = 0;//hold em interim hands 

        for (int x = 0; x < 5; x++)
        {
            FinalSuit[x] = GetCardSuit(FinalHand[x]);
            FinalValue[x] = GetCardValue(FinalHand[x]);
            if (FinalValue[x] == 13)
            {
                KingFound = true;
            }
            if (FinalValue[x] == 14)
            {
                AceFound = true;
                FinalValue[x] = 1;
            }
        }
        //lets get the flush count
        FlushCount = GetFlushCount(FinalSuit);

        if (FlushCount == 5)
        {
            flush = true;
            PayType = FLUSH;
        }

        for (int x = 0; x < 5; x++)//do preliminary check
        {
            XofaKind = 1;
            CardValue = FinalValue[x];
            for (int a = x + 1; a < 5; a++)
            {
                if (FinalValue[a] == CardValue)
                {
                    XofaKind++;
                }
            }
            if (XofaKind > highValue)
            {
                highValue = XofaKind;//save highest number of cards
                SaveValue = CardValue;//save that card value
            }
        }
        switch (highValue)
        {
            case 4:
                {
                    GamePlayers[player].hand.XofaKindValue = SaveValue;
                    if (SaveValue > 10 || SaveValue == 1)
                    {
                        return (HIGH_FOUR_OF_A_KIND);
                    }
                    else
                        if (SaveValue > 5)
                    {
                        return (MID_FOUR_OF_A_KIND);
                    }
                    else
                    {
                        return (FOUR_OF_A_KIND);// 7);//PayType=7;break; //Four of a kind
                    }
                }
            case 3:
                {
                    threeOfaKind = true;
                    PayType = THREE_OF_A_KIND;//mid and high adjusted below
                    GamePlayers[player].hand.XofaKindValue = SaveValue;
                }
                break;
            case 2:
                {
                    twoOfaKind = true;
                    GamePlayers[player].hand.XofaKindValue = SaveValue;
                }
                break;
            case 1:
                {
                    GamePlayers[player].hand.XofaKindValue = SaveValue;
                    oneOfaKind = true;
                }
                break;
        }//above is the basis for all hands}

        if (threeOfaKind || twoOfaKind)//look for two pair or Full house
        {
            XofaKind = 1;
            for (int x = 0; x < 5; x++)//do a secondary check
            {
                CardValue = FinalValue[x];
                for (int a = x + 1; a < 5; a++)
                {
                    if (CardValue != SaveValue)   //don't use orig find value
                    {
                        if (FinalValue[a] == CardValue)
                        {
                            XofaKind++;
                            if (XofaKind > 1)
                            {
                                if (CardValue == 1)//it was an ace
                                {
                                    CardValue = 14;
                                }
                                GamePlayers[player].hand.TwoPairSecondValue = CardValue;
                            }
                        }
                    }
                }
            }
            if (XofaKind == 2)//we found another pair
            {
                if (twoOfaKind)//three of a kind ???????????
                {
                    twoOfaKind = false;
                    PayType = TWO_PAIR;// THREE_OF_A_KIND;// 2;
                }
                else//we have a full house
                {
                    PayType = FULL_HOUSE;// 6;
                }
            }
            if (twoOfaKind)
                //if (SaveValue > 10 || SaveValue == 1)
                PayType = PAIR;
            //return (PAIR);// 1);//PayType=1;//Jacks or Better
        }

        ////do we have a possible connected straight???
        int connectRank = GetConnectedStraightRank(FinalValue, SaveValue);
        if (AceFound == true)
        {
            int[] FinalValueAces = new int[5];
            for (int x = 0; x < 5; x++)
            {
                FinalValueAces[x] = FinalValue[x];
                if (FinalValue[x] == 1)
                {
                    FinalValueAces[x] = 14;
                }
            }
            int connectRankAces = GetConnectedStraightRank(FinalValueAces, SaveValue);
            if (connectRankAces > connectRank)
            {
                connectRank = connectRankAces;
            }
        }

        /////Do we have a possible inside straight???
        int straightRank = GetInsideStraightRank(FinalValue);
        if (AceFound == true)
        {
            int[] FinalValueAces = new int[5];
            for (int x = 0; x < 5; x++)
            {
                FinalValueAces[x] = FinalValue[x];
                if (FinalValue[x] == 1)
                {
                    FinalValueAces[x] = 14;
                }
            }
            int straightRankAces = GetInsideStraightRank(FinalValueAces);
            if (straightRankAces > straightRank)
            {
                straightRank = straightRankAces;
            }
        }


        if (oneOfaKind)//check for straight
        {
            for (int x = 0; x < 5; x++)//let's get the high card
            {
                if (FinalValue[x] > HighCard)
                    HighCard = FinalValue[x];
            }
            if (KingFound && AceFound)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (FinalValue[x] == 1)
                    {
                        FinalValue[x] = 14;
                        HighCard = 14;             //restore proper high card
                        break;
                    }
                }
            }
            for (int x = 1; x <= 5; x++)
            {
                if (FinalValue[x - 1] == (HighCard - 1))
                {
                    HighCard = FinalValue[x - 1];
                    MembersOfStraight++;
                    x = 0;
                }
            }
            if (MembersOfStraight == 5)
            {
                PayType = STRAIGHT;// 4;//straight 
                if (flush)
                {
                    PayType = STRAIGHT_FLUSH;// 8;//straight flush
                }
                if (flush && AceFound)
                {
                    if (KingFound)
                    {
                        PayType = ROYAL_FLUSH;// 9;//Royal Flush
                    }
                }
            }

        }//end of straight check

        if (straightRank > MembersOfStraight)
        {
            MembersOfStraight = straightRank;
        }
        if (PayType < PAIR)
        {
            if (FlushCount == 3)
            {
                if (PayType < THREE_TO_A_FLUSH)
                {
                    PayType = THREE_TO_A_FLUSH;
                }
            }
            if (FlushCount == 4)
            {
                if (PayType < FOUR_TO_A_FLUSH)
                {
                    PayType = FOUR_TO_A_FLUSH;
                }
            }
            if (MembersOfStraight == 3)
            {
                if (PayType < THREE_TO_A_STRAIGHT_INSIDE)
                {
                    PayType = THREE_TO_A_STRAIGHT_INSIDE;//
                }
            }
            if (MembersOfStraight == 4)
            {
                if (PayType < FOUR_TO_A_STRAIGHT_INSIDE)
                {
                    PayType = FOUR_TO_A_STRAIGHT_INSIDE;
                }
            }
            if (connectRank == 4)
            {
                if (PayType < FOUR_TO_A_STRAIGHT_OUTSIDE)
                {
                    PayType = FOUR_TO_A_STRAIGHT_OUTSIDE;
                }
            }
            if (connectRank == 3)
            {
                if (PayType < THREE_TO_A_STRAIGHT_OUTSIDE)
                {
                    PayType = THREE_TO_A_STRAIGHT_OUTSIDE;
                }
            }
        }
        if (PayType == PAIR || PayType == THREE_OF_A_KIND)
        {
            if (SaveValue > 10 || SaveValue == 1)
            {
                PayType += 2;
            }
            else
            if (SaveValue > 5)
            {
                PayType++;
            }
        }
        GetFiveCardTotal(FinalHand);
        return (PayType);

    }

    public bool IsPokerHandPossible(int hand, int player, int[] FinalHand)
    {
        int XofaKind = 1, SaveValue = 0, CardValue, PayType = 0, HighCard = 0, MembersOfStraight = 1;
        int highValue = 0;
        bool oneOfaKind = false;
        bool twoOfaKind = false;
        bool threeOfaKind = false;
        bool flush = false;
        bool AceFound = false;
        bool KingFound = false;
        int[] FinalSuit = new int[5];
        int[] FinalValue = new int[5];

        bool possible = false;

        int FlushCount = 0;//hold em interim hands 

        //int CardTotal = 0; // for five card hands

        for (int x = 0; x < Settings.pockerHandPossibleSize; x++)
        {
            FinalSuit[x] = GetCardSuit(FinalHand[x]);
            FinalValue[x] = GetCardValue(FinalHand[x]);
            if (FinalValue[x] == 13)
            {
                KingFound = true;
            }
            if (FinalValue[x] == 14)
            {
                AceFound = true;
                FinalValue[x] = 1;
            }
        }
        //lets get the flush count
        FlushCount = GetFlushCount(FinalSuit);

        if (FlushCount == Settings.pockerHandPossibleSize)
        {
            flush = true;
            PayType = FLUSH;
        }

        for (int x = 0; x < Settings.pockerHandPossibleSize; x++)//do preliminary check
        {
            XofaKind = 1;
            CardValue = FinalValue[x];
            for (int a = x + 1; a < Settings.pockerHandPossibleSize; a++)
            {
                if (FinalValue[a] == CardValue)
                {
                    XofaKind++;
                }
            }
            if (XofaKind > highValue)
            {
                highValue = XofaKind;//save highest number of cards
                SaveValue = CardValue;//save that card value
            }
        }
        switch (highValue)
        {
            case 4:
                {
                    GamePlayers[player].hand.XofaKindValue = SaveValue;
                    if (SaveValue > 10 || SaveValue == 1)
                    {
                        if (hand == HIGH_FOUR_OF_A_KIND || hand == HIGH_THREE_OF_A_KIND)
                        {
                            possible = true;
                        }
                    }
                    else
                        if (SaveValue > 5)
                    {
                        if (hand == MID_FOUR_OF_A_KIND || hand == MID_THREE_OF_A_KIND)
                        {
                            possible = true;
                        }
                    }
                    else
                    {
                        if (hand == FOUR_OF_A_KIND || hand == THREE_OF_A_KIND)
                        {
                            possible = true;
                        }
                    }
                }
                break;
            case 3:
                {
                    threeOfaKind = true;
                    if (hand == THREE_OF_A_KIND)
                    {
                        possible = true;
                    }
                    PayType = THREE_OF_A_KIND;// 3;

                }
                break;
            case 2:
                {
                    twoOfaKind = true;
                    GamePlayers[player].hand.XofaKindValue = SaveValue;
                }
                break;
            case 1:
                {
                    GamePlayers[player].hand.XofaKindValue = SaveValue;
                    oneOfaKind = true;
                }
                break;
        }//above is the basis for all hands}

        if (threeOfaKind || twoOfaKind)//look for two pair or Full house
        {
            XofaKind = 1;
            for (int x = 0; x < Settings.pockerHandPossibleSize; x++)//do a secondary check
            {
                CardValue = FinalValue[x];
                for (int a = x + 1; a < Settings.pockerHandPossibleSize; a++)
                {
                    if (CardValue != SaveValue)   //don't use orig find value
                    {
                        if (FinalValue[a] == CardValue)
                        {
                            XofaKind++;
                            if (XofaKind > 1)
                            {
                                if (CardValue == 1)//it was an ace
                                {
                                    CardValue = 14;
                                }
                                GamePlayers[player].hand.TwoPairSecondValue = CardValue;
                            }
                        }
                    }
                }
            }
            if (XofaKind == 2)//we found another pair
            {
                if (twoOfaKind)//three of a kind ???????????
                {
                    twoOfaKind = false;
                    PayType = TWO_PAIR;// THREE_OF_A_KIND;// 2;
                }
                else//we have a full house
                {
                    PayType = FULL_HOUSE;// 6;
                }
            }
            if (twoOfaKind)
                //if (SaveValue > 10 || SaveValue == 1)
                PayType = PAIR;
            //return (PAIR);// 1);//PayType=1;//Jacks or Better
        }

        ////do we have a possible connected straight???
        int connectRank = GetConnectedStraightRank(FinalValue, SaveValue);
        if (AceFound == true)
        {
            int[] FinalValueAces = new int[Settings.pockerHandPossibleSize];
            for (int x = 0; x < Settings.pockerHandPossibleSize; x++)
            {
                FinalValueAces[x] = FinalValue[x];
                if (FinalValue[x] == 1)
                {
                    FinalValueAces[x] = 14;
                }
            }
            int connectRankAces = GetConnectedStraightRank(FinalValueAces, SaveValue);
            if (connectRankAces > connectRank)
            {
                connectRank = connectRankAces;
            }
        }

        /////Do we have a possible inside straight???
        int straightRank = GetInsideStraightRank(FinalValue);
        if (AceFound == true)
        {
            int[] FinalValueAces = new int[Settings.pockerHandPossibleSize];
            for (int x = 0; x < Settings.pockerHandPossibleSize; x++)
            {
                FinalValueAces[x] = FinalValue[x];
                if (FinalValue[x] == 1)
                {
                    FinalValueAces[x] = 14;
                }
            }
            int straightRankAces = GetInsideStraightRank(FinalValueAces);
            if (straightRankAces > straightRank)
            {
                straightRank = straightRankAces;
            }
        }


        if (oneOfaKind)//check for straight
        {
            for (int x = 0; x < Settings.pockerHandPossibleSize; x++)//let's get the high card
            {
                if (FinalValue[x] > HighCard)
                    HighCard = FinalValue[x];
            }
            if (KingFound && AceFound)
            {
                for (int x = 0; x < Settings.pockerHandPossibleSize; x++)
                {
                    if (FinalValue[x] == 1)
                    {
                        FinalValue[x] = 14;
                        HighCard = 14;             //restore proper high card
                        break;
                    }
                }
            }
            for (int x = 1; x <= Settings.pockerHandPossibleSize; x++)
            {
                if (FinalValue[x - 1] == (HighCard - 1))
                {
                    HighCard = FinalValue[x - 1];
                    MembersOfStraight++;
                    x = 0;
                }
            }
            if (MembersOfStraight == Settings.pockerHandPossibleSize)
            {
                PayType = STRAIGHT;// 4;//straight 
                if (flush)
                {
                    PayType = STRAIGHT_FLUSH;// 8;//straight flush
                }
                if (flush && AceFound)
                {
                    if (KingFound)
                    {
                        PayType = ROYAL_FLUSH;// 9;//Royal Flush
                    }
                }
            }

        }//end of straight check

        if (straightRank > MembersOfStraight)
        {
            MembersOfStraight = straightRank;
        }
        if (PayType < PAIR)
        {
            if (FlushCount == 3)
            {
                if (PayType < THREE_TO_A_FLUSH)
                {
                    PayType = THREE_TO_A_FLUSH;
                }
            }
            if (FlushCount == 4)
            {
                if (PayType < FOUR_TO_A_FLUSH)
                {
                    PayType = FOUR_TO_A_FLUSH;
                }
            }
            if (MembersOfStraight == 3)
            {
                if (PayType < THREE_TO_A_STRAIGHT_INSIDE)
                {
                    PayType = THREE_TO_A_STRAIGHT_INSIDE;//
                }
            }
            if (MembersOfStraight == 4)
            {
                if (PayType < FOUR_TO_A_STRAIGHT_INSIDE)
                {
                    PayType = FOUR_TO_A_STRAIGHT_INSIDE;
                }
            }
            if (connectRank == 4)
            {
                if (PayType < FOUR_TO_A_STRAIGHT_OUTSIDE)
                {
                    PayType = FOUR_TO_A_STRAIGHT_OUTSIDE;
                }
            }
            if (connectRank == 3)
            {
                if (PayType < THREE_TO_A_STRAIGHT_OUTSIDE)
                {
                    PayType = THREE_TO_A_STRAIGHT_OUTSIDE;
                }
            }
        }
        if (PayType == PAIR || PayType == THREE_OF_A_KIND)
        {
            if (SaveValue > 10 || SaveValue == 1)
            {
                PayType += 2;
            }
            else
                if (SaveValue > Settings.pockerHandPossibleSize)
            {
                PayType++;
            }
        }
        GetFiveCardTotal(FinalHand);
        return possible;

    }

    private cardValues getCard(int card)
    {
        switch (GetCardValue(card))
        {
            case 2: return cardValues._2;
            case 3: return cardValues._3;
            case 4: return cardValues._4;
            case 5: return cardValues._5;
            case 6: return cardValues._6;
            case 7: return cardValues._7;
            case 8: return cardValues._8;
            case 9: return cardValues._9;
            case 10: return cardValues.T;
            case 11: return cardValues.J;
            case 12: return cardValues.Q;
            case 13: return cardValues.K;
            case 14: return cardValues.A;

        }
        return 0;
    }

    //protected override void OnPaint(PaintEventArgs e)
    //{
    //    base.OnPaint(e);
    //}

    public int GetDealRanking(cardValues C1, cardValues C2, bool suited)
    {
        cardValues FirstCard;
        cardValues SecondCard;
        int rValue = Group.GetLength(0);
        int groupLen = Group.GetLength(0) - 1;
        if (C2 > C1)//sort the cards
        {
            FirstCard = C2;
            SecondCard = C1;
        }
        else
        {
            FirstCard = C1;
            SecondCard = C2;
        }
        for (int x = groupLen; x >= 0; x--)
        {
            if (FirstCard == Group[x, 0] && (SecondCard == Group[x, 1] || Group[x, 1] == cardValues.ANY))
            {
                if (Group[x, 2] == cardValues.S && suited == true)
                    rValue = x;
                if (Group[x, 2] == cardValues.US && suited == false)
                    rValue = x;
            }
        }
        return rValue + 1;//we use 1 based numbers for ranking
    }

    public int GetFiveCardRanking(int player)
    {
        int temp;
        int[] playerHand = new int[5];
        int count = 21;
        int rank = 0;
        int highHand = 0;
        int highTotal = 0;
        int highKicker = 0;
        int tempTotal = 0;
        int tempKicker = 0;
        int highCombo = 0;
        if (cardsDealt < 5)
        {
            return 0;
        }
        if (cardsDealt == 5)
        {
            count = 1;//only do the first one
        }
        if (cardsDealt == 6)
        {
            count = 6;//only do the first five
        }

        for (int x = 0; x < count; x++)//iterate through all possible 5 card hands
        {
            switch (x)
            {
                case 0:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[0];
                        playerHand[3] = communityCards[1];
                        playerHand[4] = communityCards[2];
                    }
                    break;
                case 1:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[0];
                        playerHand[3] = communityCards[1];
                        playerHand[4] = communityCards[3];
                    }
                    break;
                case 2:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[0];
                        playerHand[3] = communityCards[2];
                        playerHand[4] = communityCards[3];
                    }
                    break;
                case 3:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[1];
                        playerHand[3] = communityCards[2];
                        playerHand[4] = communityCards[3];
                    }
                    break;
                case 4:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = communityCards[0];
                        playerHand[2] = communityCards[1];
                        playerHand[3] = communityCards[2];
                        playerHand[4] = communityCards[3];

                    }
                    break;
                case 5:
                    {
                        playerHand[0] = playerHands[player, 1];
                        playerHand[1] = communityCards[0];
                        playerHand[2] = communityCards[1];
                        playerHand[3] = communityCards[2];
                        playerHand[4] = communityCards[3];
                    }
                    break;
                case 6:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[0];
                        playerHand[3] = communityCards[1];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 7:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[0];
                        playerHand[3] = communityCards[2];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 8:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[1];
                        playerHand[3] = communityCards[2];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 9:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = communityCards[0];
                        playerHand[2] = communityCards[1];
                        playerHand[3] = communityCards[2];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 10:
                    {
                        playerHand[0] = playerHands[player, 1];
                        playerHand[1] = communityCards[0];
                        playerHand[2] = communityCards[1];
                        playerHand[3] = communityCards[2];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 11:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[0];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 12:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[1];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 13:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = communityCards[0];
                        playerHand[2] = communityCards[1];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 14:
                    {
                        playerHand[0] = playerHands[player, 1];
                        playerHand[1] = communityCards[0];
                        playerHand[2] = communityCards[1];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 15:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = playerHands[player, 1];
                        playerHand[2] = communityCards[2];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 16:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = communityCards[0];
                        playerHand[2] = communityCards[2];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 17:
                    {
                        playerHand[0] = playerHands[player, 1];
                        playerHand[1] = communityCards[0];
                        playerHand[2] = communityCards[2];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 18:
                    {
                        playerHand[0] = playerHands[player, 0];
                        playerHand[1] = communityCards[1];
                        playerHand[2] = communityCards[2];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 19:
                    {
                        playerHand[0] = playerHands[player, 1];
                        playerHand[1] = communityCards[1];
                        playerHand[2] = communityCards[2];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
                case 20:
                    {
                        playerHand[0] = communityCards[0];
                        playerHand[1] = communityCards[1];
                        playerHand[2] = communityCards[2];
                        playerHand[3] = communityCards[3];
                        playerHand[4] = communityCards[4];
                    }
                    break;
            }

            if (GameState == GameStates.RiverBet)
            {
                temp = EvaluatePokerHand(player, playerHand);

                if (temp >= STRAIGHT && (temp != FOUR_OF_A_KIND || temp != MID_FOUR_OF_A_KIND || temp != HIGH_FOUR_OF_A_KIND))
                {
                    tempTotal = GetFiveCardTotal(playerHand);
                    setPlayerWinCards(player, playerHand);
                    tempKicker = GetKicker(GamePlayers[player].hand.cardHand, GamePlayers[player].winCards);
                }
                if (temp == PAIR || temp == MID_PAIR || temp == HIGH_PAIR ||
                    temp == THREE_OF_A_KIND || temp == MID_THREE_OF_A_KIND || temp == HIGH_THREE_OF_A_KIND ||
                    temp == FOUR_OF_A_KIND || temp == MID_FOUR_OF_A_KIND || temp == HIGH_FOUR_OF_A_KIND)
                {
                    tempTotal = GetXofaKindTotal(GamePlayers[player].hand.XofaKindValue, playerHand);
                    setPlayerWinCards(player, playerHand);
                    tempKicker = GetXofaKindKicker(GamePlayers[player].hand.XofaKindValue, GamePlayers[player].hand.cardHand);
                }
                if (temp == TWO_PAIR || temp == FULL_HOUSE)
                {
                    tempTotal = GetTwoPairTotal(GamePlayers[player].hand.XofaKindValue, GamePlayers[player].hand.TwoPairSecondValue, playerHand);
                    setPlayerWinCards(player, playerHand);
                    tempKicker = GetTwoPairKicker(GamePlayers[player].hand.XofaKindValue, GamePlayers[player].hand.TwoPairSecondValue, GamePlayers[player].hand.cardHand);
                }
                if (temp > highHand || (temp == highHand && tempTotal > highTotal) || (temp == highHand && tempTotal == highTotal && tempKicker > highKicker))
                {
                    highTotal = tempTotal;
                    highHand = temp;
                    highKicker = tempKicker;
                    highCombo = x;
                    GamePlayers[player].hand.CardValueTotal = highTotal;//now we have the total to break a tie
                    GamePlayers[player].hand.Kicker = highKicker;
                    GamePlayers[player].hand.HandRank = temp;
                }
                rank = GamePlayers[player].hand.HandRank;
            }//end fourth round
            else
            {
                temp = EvaluateFlop(player, playerHand);

                if (temp > rank || (temp == rank && highTotal > tempTotal))
                {
                    {
                        rank = temp;
                    }
                }
            }
        }
        return rank;
    }

    public int GetXofaKindTotal(int card, int[] hand)
    {
        int total = 0;
        if (card == 1)
        {
            card = 14;//convert to aces
        }
        for (int x = 0; x < 5; x++)
        {
            if (GetCardValue(hand[x]) == card)
            {
                total += card;
            }
        }
        return total;
    }

    public int GetTwoPairTotal(int firstValue, int secondValue, int[] hand)
    {
        int total = 0;
        if (firstValue == 1)
        {
            firstValue = 14;
        }
        if (secondValue == 1)
        {
            secondValue = 14;
        }
        for (int x = 0; x < 5; x++)
        {
            if (GetCardValue(hand[x]) == firstValue || GetCardValue(hand[x]) == secondValue)
            {
                total += GetCardValue(hand[x]);
            }
        }
        return total;
    }

    public void setPlayerWinCards(int player, int[] Hand)
    {
        for (int x = 0; x < 5; x++)
        {
            GamePlayers[player].winCards[x] = Hand[x];
        }
    }

    public void EnableBettingButtons()
    {
        panelInitBet.SetActive(true);  //show the betting buttons

        if (virtualPlayers[0].AllIn == true)
        {
            if (btnRaise != null) btnRaise.GetComponent<Button>().interactable = false;
            if (btnAllIn != null) btnAllIn.GetComponent<Button>().interactable = true;
            if (btnFold != null) btnFold.GetComponent<Button>().interactable = false;

            if (btnAllIn != null) btnAllIn.GetComponentInChildren<Text>().text = "CONTINUE";
            if (btnCheck != null) btnCheck.GetComponent<Button>().interactable = false;
            if (btnCall != null) btnCall.GetComponent<Button>().interactable = false;
            if (btnSurrender != null) btnSurrender.SetActive(false);

            UpdateDynamicHelp();
            updateBettingButtonTitle();

            return;
        }

        if (btnAllIn != null) btnAllIn.GetComponentInChildren<Text>().text = "ALL IN"; // TODO: null object
        CallAmount = GetCurrentBet() - virtualPlayers[0].CurrentBetAmount;
        updateBettingButtonTitle();

        if (btnRaise != null) btnRaise.GetComponent<Button>().interactable = true;  //we can always raise
        if (btnAllIn != null) btnAllIn.GetComponent<Button>().interactable = true;  //we can alway go all in
        if (btnFold != null) btnFold.GetComponent<Button>().interactable = true;    //we can always fold

        if (CallAmount == 0)
        {
            if (btnCheck != null) btnCheck.GetComponent<Button>().interactable = true;
            if (btnCall != null)
                btnCall.GetComponent<Button>().interactable = false;
        }
        else
        {
            if (btnCheck != null) btnCheck.GetComponent<Button>().interactable = false;

            if (CallAmount > PlayerCredits)
            {
                if (btnCall != null)
                    btnCall.GetComponent<Button>().interactable = false;
                if (btnRaise != null) btnRaise.GetComponent<Button>().interactable = false;
            }
            else
            {
               if (btnCall != null)
                    btnCall.GetComponent<Button>().interactable = true;
            }
        }
        if (PlayerCredits == 0)
        {
            if (btnRaise != null) btnRaise.GetComponent<Button>().interactable = false;
            if (btnCall != null)
                btnCall.GetComponent<Button>().interactable = false;
            if (btnAllIn != null) btnAllIn.GetComponent<Button>().interactable = false;
        }

        if (GameState == GameStates.HoldCardBet)
        {
            int rank = playerHoleCardsRankings[0] + 1;
            if (rank > Settings.surrenderReturnRank && virtualPlayers[0].AllIn == false && GetPlayerPairValue(0) < Settings.surrenderMinimumPair)
            {
                if (panelSurrender != null) panelSurrender.SetActive(true);
                if (btnSurrender != null) btnSurrender.SetActive(true);
            }
        }
        UpdateDynamicHelp();
    }

    public void UpdateDynamicHelp()
    {
        if (virtualPlayers[0].AllIn == true)
        {
            string contstring = continueString + Environment.NewLine;
            lblTemp.text = contstring;
            return;
        }
        string teststring = foldString + Environment.NewLine;

        if (btnCheck != null && btnCheck.GetComponent<Button>().IsInteractable())
        {
            teststring += checkString + Environment.NewLine;
        }
        if (btnCall != null && btnCall.GetComponent<Button>().IsInteractable())
        {
            teststring += callString + Environment.NewLine;
        }
        if (btnRaise != null && btnRaise.GetComponent<Button>().IsInteractable())
        {
            teststring += raiseString + Environment.NewLine;
        }
        if (btnAllIn != null && btnAllIn.GetComponent<Button>().IsInteractable())
        {
            teststring += allInString + Environment.NewLine;
        }
        if (btnSurrender != null && btnSurrender.GetComponent<Button>().IsActive())
        {
            teststring += surrenderString;
        }
        lblTemp.text = teststring;
    }

    public void DisableBettingButtons()
    {
        if (btnRaise != null) btnRaise.GetComponent<Button>().interactable = false;
        if (btnCall != null) btnCall.GetComponent<Button>().interactable = false;
        if (btnCheck != null) btnCheck.GetComponent<Button>().interactable = false;
        if (btnFold != null) btnFold.GetComponent<Button>().interactable = false;
        if (btnAllIn != null) btnAllIn.GetComponent<Button>().interactable = false;
        if (btnSurrender != null) btnSurrender.GetComponent<Button>().interactable = false;
        
        // hide panels
        if (panelSurrender != null) panelSurrender.SetActive(false);
        if (panelInitBet != null) panelInitBet.SetActive(false);
    }


    //*************************************************************************************************
    //*******************************************************************************************
    public double BetPlayer(int player)
    {
        if (CurrentBetPosition == buttonPosition)
        {
            DealButtonPassed = true;//we have serviced all the players now we can check if the betting is done
        }

        if (virtualPlayers[player].Folded == true)
        {
            isNextPlayer = true; // nextPlayerTimer.Start();
            return 0;
        }

        double ThisRoundBet = 0;
        double raise = 0;//local value for amount raised

        EvalPlayerHands(true, false, false, false);
        int rank = playerHoleCardsRankings[player];

        bool limp = virtualPlayers[player].LimpIn;
        bool bluff = virtualPlayers[player].Bluffing;

        bool folding = false;// a more temporary fold
        double ThisPlayersCall;
        ThisPlayersCall = GetCurrentBet() - virtualPlayers[player].CurrentBetAmount;
        double realPlayerPotRaisePercentage = 0;
        BetType = BetTypes.checking;//start out with this and modify it
        bool pocketPair = false; //TODO
        try
        {
            pocketPair = GamePlayers[player].hand.cardHand[0] == GamePlayers[player].hand.cardHand[1];
        }
        catch(Exception e) {
            Debug.LogError("pocketPair error" + e.Message);
            isNextPlayer = false;
            return 0;
        }

        if (player != 0 || AutoPlay == true)//only service  the virtual players here
        {
            clearBetLabel(player);//always clear the bet label

            if (GameState == GameStates.HoldCardBet)//set up the betting rules for hold cards
            {
                if (virtualPlayers[0].RoundRaiseAmount != 0)
                {
                    realPlayerPotRaisePercentage = (100 / (PotAmount / virtualPlayers[0].RoundRaiseAmount));
                }
                int potRaisePercentage = GetPercentPotRaised(player);
                potRaisePercentage = ThisRoundRaisePercentage;
                if (rank <= virtualPlayers[player].HoleMinThreshold)// && (realPlayerPotRaisePercentage >= 0 && realPlayerPotRaisePercentage >= Settings.PlayerRaiseFoldThreshold))//we have to meet the minimum
                {
                    if (ThisPlayersCall > 0)//only check for folds if we can't check we take freebees
                    {
                        for (int x = 0; x < 8; x++)//iterate through the fold levels
                        {
                            if (potRaisePercentage >= virtualPlayers[player].FoldLevels[x].Range[0] && potRaisePercentage <= virtualPlayers[player].FoldLevels[x].Range[1])
                            {
                                for (int i = 0; i < virtualPlayers[player].FoldLevels[x].FoldHands.Count(); i++)
                                {
                                    if (rank == virtualPlayers[player].FoldLevels[x].FoldHands[i])
                                    {
                                        BetType = BetTypes.folding;
                                        break;
                                    }
                                }
                                if (BetType == BetTypes.folding)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (BetType == BetTypes.checking)//we didn't fold
                    {
                        for (int x = 0; x < Settings.playerSize; x++)//iterate through the raise levels
                        {
                            if (potRaisePercentage >= virtualPlayers[player].RaiseLevels[x].Range[0] && potRaisePercentage <= virtualPlayers[player].RaiseLevels[x].Range[1])
                            {
                                for (int i = 0; i < virtualPlayers[player].RaiseLevels[x].RaiseHands.Count(); i++)
                                {
                                    if (rank == virtualPlayers[player].RaiseLevels[x].RaiseHands[i])
                                    {
                                        BetType = BetTypes.raising;
                                        raise = RoundUp(PotAmount * (virtualPlayers[player].RaiseLevels[x].RaisePercentage * 0.01));
                                        break;
                                    }
                                }
                            }
                            if (virtualPlayers[player].RoundRaiseAmount > 0)//did we raise before?
                            {
                                if (potRaisePercentage >= virtualPlayers[player].RaiseLevels[x].ReRaiseRange[0] && potRaisePercentage <= virtualPlayers[player].RaiseLevels[x].ReRaiseRange[1])
                                {
                                    for (int i = 0; i < virtualPlayers[player].RaiseLevels[x].RaiseHands.Count(); i++)
                                    {
                                        if (rank == virtualPlayers[player].RaiseLevels[x].RaiseHands[i])
                                        {
                                            BetType = BetTypes.raising;
                                            raise = RoundUp(PotAmount * (virtualPlayers[player].RaiseLevels[x].ReRaisePercentage * 0.01));
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (potRaisePercentage >= virtualPlayers[player].RaiseLevels[x].ReRaiseRange[1])
                                    {
                                        for (int i = 0; i < virtualPlayers[player].RaiseLevels[x].RaiseHands.Count(); i++)
                                        {
                                            if (rank == virtualPlayers[player].RaiseLevels[x].RaiseHands[i])
                                            {
                                                BetType = BetTypes.folding;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (BetType == BetTypes.checking)//lets see if we are going all in 
                    {
                        if (player < virtualPlayers.Count() && virtualPlayers[player].AllInHands != null)
                            for (int i = 0; i < virtualPlayers[player].AllInHands.Count(); i++)
                            {
                                if (rank == virtualPlayers[player].AllInHands[i])
                                {
                                    if (ThisPlayersCall > 0)
                                    {
                                        BetType = BetTypes.allIn;
                                    }
                                    else
                                    {
                                        BetType = BetTypes.raising;
                                        raise = PotAmount;
                                    }
                                }
                            }
                        else
                        {
                            if (Settings.isDebug) Debug.LogError("error in BetTypes.checking ( lets see if we are going all in )");
                            //TODO: MessageBox.Show("error in BetTypes.checking ( lets see if we are going all in )");
                        }
                    }
                    if (BetType == BetTypes.checking)//lets see if we are going to Slow play
                    {
                        for (int i = 0; i < virtualPlayers[player].SlowPlayHands.Count(); i++)
                        {
                            if (rank == virtualPlayers[player].SlowPlayHands[i])
                            {
                                BetType = BetTypes.raising;
                                raise = PotAmount;
                                if (getWeightedResult(50) == true)
                                {
                                    raise /= 2;
                                }
                            }
                        }
                    }
                    if (bluff == true)//are we bluffing
                    {
                        for (int i = 0; i < virtualPlayers[player].BluffHands.Count(); i++)
                        {
                            if (rank == virtualPlayers[player].BluffHands[i])
                            {
                                if (raise > 0)
                                {
                                    raise += raise * .30;
                                }
                                else
                                {
                                    BetType = BetTypes.raising;
                                    raise = ThisPlayersCall * 0.30;
                                }
                                lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " is BLUFFING with " + String.Format("{0:C}", raise);
                            }
                        }
                    }

                }
                else//didn't meet the minimum 
                {

                    if (ThisPlayersCall > 0 && (/*realPlayerPotRaisePercentage != 0 && realPlayerPotRaisePercentage*/potRaisePercentage > Settings.PlayerRaiseFoldThreshold))
                    {
                        BetType = BetTypes.folding;
                    }
                }
                if (virtualPlayers[player].FoldOnAnyRaise == true && GetTotalRaiseAmount() > Settings.PlayerRaiseFoldThreshold && raise == 0)
                {
                    BetType = BetTypes.folding;
                }
                if (BetType == BetTypes.folding)
                {
                    if (virtualPlayers[0].RoundRaiseAmount >= virtualPlayers[0].Ante * 2)//is he trying to steal the pot 
                    {
                        if (virtualPlayers[player].HighCard >= Settings.highCardThreshhold)//if we have a face card
                        {
                            BetType = BetTypes.checking;
                        }
                    }
                    if (pocketPair == true)
                    {
                        BetType = BetTypes.checking;
                    }
                }
            }
            // END HOLD CARD BETTING

            //FLOP
            if (GameState == GameStates.FlopBet)//the flop
            {
                int fiveCardRank = virtualPlayers[player].FiveCardHandRank;
                int potRaisePercentage = GetPercentPotRaised(player);
                potRaisePercentage = ThisRoundRaisePercentage;
                int[] hand = GamePlayers[player].hand.cardHand;
                //                  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;| RF| SF|H4K|M4K| 4K| FH| FL| ST|H3K|M3K| 3K| 2P| PP| TP| MP| BP| 4F| 3F|4SI|3SI|4SO|3SO                        
                //                  Flop No Raise Bet Percentages =   100,100,100,100,100, 75,100,100, 50, 50, 50, 40, 25, 40, 30, 20, 30,  0, 40,  0,  0,  0
                //possibilities #1
                if (potRaisePercentage == 0)//the pot has not been raised
                                            //if(virtualPlayerRaised < Settings.virtualPlayerRaiseLimit)
                {
                    double tempRaise = 0;
                    int tempRank = fiveCardRank;
                    //double tempRaise = virtualPlayers[player].FlopNoRaiseBetPercentages[20 - fcRank] * .01;
                    if (fiveCardRank == HIGH_PAIR || fiveCardRank == MID_PAIR || fiveCardRank == PAIR)//adjust the pairs
                    {

                        switch (GetPairType(hand))
                        {
                            case PairTypes.Bottom: tempRank = 6; break;
                            case PairTypes.Middle: tempRank = 7; break;
                            case PairTypes.Top: tempRank = 8; break;
                            case PairTypes.Pocket: tempRank = 9; break;
                        }
                    }
                    if (fiveCardRank < PAIR && fiveCardRank > 0)
                    {
                        tempRank--; ;
                    }

                    try
                    {
                        tempRaise = virtualPlayers[player].FlopNoRaiseBetPercentages[21 - tempRank] * .01;
                    }
                    catch (Exception e)
                    {
                        if (Settings.isDebug) Debug.LogError("tempRaise Error:" + e.Message);
                        //TODO: MessageBox.Show("tempRaise Error:" + e.Message);
                    }

                    if (tempRaise > 0)
                    {
                        BetType = BetTypes.raising;
                        raise = RoundUp(PotAmount * tempRaise);
                    }
                    if (tempRaise < 0)
                    {
                        folding = true;
                    }
                    if (tempRaise == 999)
                    {
                        BetType = BetTypes.allIn;
                    }

                }

                //Flop possibilities #2 The pot was raised by anyone  
                if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 0 /*&& flopTurnRiverRaised == true*/)
                {
                    if (fiveCardRank > 0)//we hit our hand
                    {

                        if (fiveCardRank == PAIR || fiveCardRank == MID_PAIR || fiveCardRank == HIGH_PAIR)
                        {
                            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 50)//#2
                            {
                                if (virtualPlayers[player].PocketPair < 10)
                                {
                                    folding = true;
                                }
                            }
                            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 25)//#3 #4
                            {
                                if (GetPairType(hand) == PairTypes.Bottom || GetPairType(hand) == PairTypes.Middle)
                                {
                                    if (fiveCardRank < HIGH_PAIR)
                                    {
                                        folding = true;
                                    }
                                }
                            }
                            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 38)//#5
                            {
                                if (GetPairType(hand) == PairTypes.Top)
                                {
                                    if (fiveCardRank < HIGH_PAIR)
                                    {
                                        folding = true;
                                    }
                                }
                            }

                        }
                        if (fiveCardRank == TWO_PAIR)//#6
                        {
                            int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
                            if (cardTotal < 20)
                            {
                                folding = true;
                            }
                        }
                        if (fiveCardRank == THREE_TO_A_FLUSH || fiveCardRank == THREE_TO_A_STRAIGHT_INSIDE)//#7
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 25)
                            {
                                folding = true;
                            }
                        }
                        if (fiveCardRank == THREE_TO_A_STRAIGHT_OUTSIDE || fiveCardRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#8
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 20)
                            {
                                folding = true;
                            }
                        }
                        if (fiveCardRank == FOUR_TO_A_STRAIGHT_INSIDE || fiveCardRank == FOUR_TO_A_FLUSH)//#9
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 75)
                            {
                                folding = true;
                            }
                        }

                    }
                    else//we missed the hand
                    {
                        folding = true;
                    }

                }
                //Flop possibilities #3 the pot was raised by the real player
                if (virtualPlayers[0].RoundRaiseAmount > 0 && flopTurnRiverRaised == false)//the real player raised
                {
                    if (fiveCardRank < PAIR && rank > 12)//#1
                    {
                        folding = true;
                    }

                    if (fiveCardRank == PAIR)// #2 #3 #4
                    {
                        if (GetPairType(hand) != PairTypes.Pocket)
                        {
                            if (GamePlayers[player].hand.XofaKindValue <= 10)
                            {
                                folding = true;
                            }
                        }
                    }

                    if (fiveCardRank == TWO_PAIR)//#5
                    {
                        int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
                        if (cardTotal < 20)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank == THREE_TO_A_FLUSH || fiveCardRank == THREE_TO_A_STRAIGHT_INSIDE)//#6
                    {
                        if (virtualPlayers[player].RoundRaiseAmount > 50)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank == THREE_TO_A_STRAIGHT_OUTSIDE || fiveCardRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#7
                    {
                        if (virtualPlayers[player].RoundRaiseAmount > 30)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank == FOUR_TO_A_STRAIGHT_INSIDE || fiveCardRank == FOUR_TO_A_FLUSH)//#8
                    {
                        if (virtualPlayers[player].RoundRaiseAmount > 75)
                        {
                            folding = true;
                        }
                    }

                }
                if (fiveCardRank >= THREE_OF_A_KIND && fiveCardRank <= HIGH_THREE_OF_A_KIND)
                {
                    BetType = BetTypes.raising;
                    raise = RoundUp(PotAmount / 2);
                }
                lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " #" + player.ToString() + " Flop Bet Rank = " + fiveCardRank.ToString() + Environment.NewLine;
            }

            //TURN
            if (GameState == GameStates.TurnBet)//the turn
            {
                int fiveCardRank = virtualPlayers[player].FiveCardHandRank;
                int potRaisePercentage = GetPercentPotRaised(player);
                potRaisePercentage = ThisRoundRaisePercentage;
                int[] hand = GamePlayers[player].hand.cardHand;
                //;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;| RF| SF|H4K|M4K| 4K| FH| FL| ST|H3K|M3K| 3K| 2P| PP| TP| MP| BP| 4F| 3F|4SI|3SI|4SO|3SO                        
                //Turn No Raise Bet Percentages =   100,100,100,100,100, 75,100,100, 50, 50, 50, 40, 25, 40, 30, 20, 30,  0, 40,  0,  0,  0
                // Turn possibilities #1
                if (potRaisePercentage == 0)//the pot has not been raised
                                            //if (virtualPlayerRaised < Settings.virtualPlayerRaiseLimit)
                {
                    double tempRaise = 0;
                    int tempRank = fiveCardRank;
                    //double tempRaise = virtualPlayers[player].FlopNoRaiseBetPercentages[20 - fcRank] * .01;
                    if (fiveCardRank == HIGH_PAIR || fiveCardRank == MID_PAIR || fiveCardRank == PAIR)//adjust the pairs
                    {

                        switch (GetPairType(hand))
                        {
                            case PairTypes.Bottom: tempRank = 6; break;
                            case PairTypes.Middle: tempRank = 7; break;
                            case PairTypes.Top: tempRank = 8; break;
                            case PairTypes.Pocket: tempRank = 9; break;
                        }
                    }
                    if (fiveCardRank < PAIR && fiveCardRank > 0)
                    {
                        tempRank--; ;
                    }
                    if (virtualPlayers[player].TurnNoRaiseBetPercentages != null && player < virtualPlayers.Count())
                        tempRaise = virtualPlayers[player].TurnNoRaiseBetPercentages[21 - tempRank] * .01;
                    else
                        tempRaise = 0;

                    if (tempRaise > 0)
                    {
                        BetType = BetTypes.raising;
                        raise = RoundUp(PotAmount * tempRaise);
                    }
                    if (tempRaise < 0)
                    {
                        folding = true;
                    }
                    if (tempRaise == 999)
                    {
                        BetType = BetTypes.allIn;
                    }
                }

                //Turn possibilities #2 the pot was raised by anyone 
                if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 0 /*&& flopTurnRiverRaised == true*/)
                {
                    if (fiveCardRank > 0)//we hit our hand
                    {

                        if (fiveCardRank == PAIR || fiveCardRank == MID_PAIR || fiveCardRank == HIGH_PAIR)
                        {

                            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 50)//#2
                            {
                                if (virtualPlayers[player].PocketPair < 10)
                                {
                                    folding = true;
                                }
                            }
                            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 25)//#3 #4
                            {
                                if (GetPairType(hand) == PairTypes.Bottom || GetPairType(hand) == PairTypes.Middle)
                                {
                                    if (fiveCardRank < HIGH_PAIR)
                                    {
                                        folding = true;
                                    }
                                }
                            }
                            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 48)//#5
                            {
                                if (GetPairType(hand) == PairTypes.Top)
                                {
                                    if (fiveCardRank < HIGH_PAIR)
                                    {
                                        folding = true;
                                    }
                                }
                            }

                        }
                        if (fiveCardRank == TWO_PAIR)//#6
                        {
                            if (potRaisePercentage > 31)
                            {
                                int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
                                if (cardTotal < 20)
                                {
                                    folding = true;
                                }
                            }
                        }
                        if (fiveCardRank == THREE_TO_A_FLUSH || fiveCardRank == THREE_TO_A_STRAIGHT_INSIDE)//#7
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 0)
                            {
                                folding = true;
                            }
                        }
                        if (fiveCardRank == THREE_TO_A_STRAIGHT_OUTSIDE)//#8
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 00)
                            {
                                folding = true;
                            }
                        }
                        if (fiveCardRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#8
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 11)
                            {
                                folding = true;
                            }
                        }

                        if (fiveCardRank == FOUR_TO_A_STRAIGHT_INSIDE || fiveCardRank == FOUR_TO_A_FLUSH)//#9
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 61)
                            {
                                folding = true;
                            }
                        }
                        if (fiveCardRank >= FULL_HOUSE)
                        {
                            BetType = BetTypes.raising;
                            raise = PotAmount;
                        }
                    }
                    else//we missed the hand
                    {
                        folding = true;
                    }

                }
                //Turn possibilities #3 the pot was raised by the real player 
                if (virtualPlayers[0].RoundRaiseAmount > 0 && flopTurnRiverRaised == false)//the real player raised
                {
                    if (fiveCardRank < PAIR && rank > 12)//#1
                    {
                        folding = true;
                    }


                    if (fiveCardRank == PAIR)// #2 #3 #4
                    {
                        if (GetPairType(hand) != PairTypes.Pocket)
                        {
                            if (GamePlayers[player].hand.XofaKindValue <= 10)
                            {
                                folding = true;
                            }
                        }
                    }

                    if (fiveCardRank == TWO_PAIR)//#5
                    {
                        int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
                        if (cardTotal < 20)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank == THREE_TO_A_FLUSH || fiveCardRank == THREE_TO_A_STRAIGHT_INSIDE)//#6
                    {
                        if (virtualPlayers[player].RoundRaiseAmount > 0)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank == THREE_TO_A_STRAIGHT_OUTSIDE || fiveCardRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#7
                    {
                        if (virtualPlayers[player].RoundRaiseAmount > 0)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank == FOUR_TO_A_STRAIGHT_INSIDE || fiveCardRank == FOUR_TO_A_FLUSH)//#8
                    {
                        if (virtualPlayers[player].RoundRaiseAmount > 51)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank >= FULL_HOUSE)
                    {
                        BetType = BetTypes.raising;
                        raise = PotAmount;
                    }
                }
                if (fiveCardRank >= THREE_OF_A_KIND && fiveCardRank <= HIGH_THREE_OF_A_KIND)
                {
                    BetType = BetTypes.raising;
                    raise = RoundUp(PotAmount / 2);
                }
                //lblWinInfo.AppendText(virtualPlayers[player].Name + " #" + player.ToString() + " Turn Bet Rank = " + fiveCardRank.ToString() + Environment.NewLine);
                lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " #" + player.ToString() + " Turn Bet Rank = " + fiveCardRank.ToString() + Environment.NewLine;
            }

            //RIVER
            if (GameState == GameStates.RiverBet)//the river
            {
                int fiveCardRank = virtualPlayers[player].FiveCardHandRank;
                int potRaisePercentage = GetPercentPotRaised(player);
                potRaisePercentage = ThisRoundRaisePercentage;
                int[] hand = GamePlayers[player].hand.cardHand;
                //                  ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;| RF| SF|H4K|M4K| 4K| FH| FL| ST|H3K|M3K| 3K| 2P| PP| TP| MP| BP| 4F| 3F|4SI|3SI|4SO|3SO                        
                //                  Flop No Raise Bet Percentages =   100,100,100,100,100, 75,100,100, 50, 50, 50, 40, 25, 40, 30, 20, 30,  0, 40,  0,  0,  0

                //River possibilities #1 the pot was not raised by anyone
                if (potRaisePercentage == 0)//the pot has not been raised
                                            //if (virtualPlayerRaised < Settings.virtualPlayerRaiseLimit)
                {
                    double tempRaise = 0;
                    int tempRank = fiveCardRank;
                    //double tempRaise = virtualPlayers[player].FlopNoRaiseBetPercentages[20 - fcRank] * .01;
                    if (fiveCardRank == HIGH_PAIR || fiveCardRank == MID_PAIR || fiveCardRank == PAIR)//adjust the pairs
                    {

                        switch (GetPairType(hand))
                        {
                            case PairTypes.Bottom: tempRank = 6; break;
                            case PairTypes.Middle: tempRank = 7; break;
                            case PairTypes.Top: tempRank = 8; break;
                            case PairTypes.Pocket: tempRank = 9; break;
                        }
                    }
                    if (fiveCardRank < PAIR && fiveCardRank > 0)
                    {
                        tempRank--; ;
                    }
                    tempRaise = virtualPlayers[player].RiverNoRaiseBetPercentages[21 - tempRank] * .01;

                    if (tempRaise > 0)
                    {
                        BetType = BetTypes.raising;
                        raise = RoundUp(PotAmount * tempRaise);
                    }
                    if (tempRaise < 0)
                    {
                        folding = true;
                    }
                    if (tempRaise == 999)
                    {
                        BetType = BetTypes.allIn;
                    }

                }

                //River possibilities #2 the pot was raised by anyone 
                if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 0 /*&& flopTurnRiverRaised == true*/)
                {
                    if (fiveCardRank > 0)//we hit our hand
                    {

                        if (fiveCardRank == PAIR || fiveCardRank == MID_PAIR || fiveCardRank == HIGH_PAIR)
                        {

                            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 50)//#2
                            {
                                if (virtualPlayers[player].PocketPair < 10)
                                {
                                    folding = true;
                                }
                            }
                            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 25)//#3 #4
                            {
                                if (GetPairType(hand) == PairTypes.Bottom || GetPairType(hand) == PairTypes.Middle)
                                {
                                    if (fiveCardRank < HIGH_PAIR)
                                    {
                                        folding = true;
                                    }
                                }
                            }
                            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 52)//#5
                            {
                                if (GetPairType(hand) == PairTypes.Top)
                                {
                                    if (fiveCardRank < HIGH_PAIR)
                                    {
                                        folding = true;
                                    }
                                }
                            }

                        }
                        if (fiveCardRank == TWO_PAIR)//#6
                        {
                            if (potRaisePercentage > 26)
                            {
                                int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
                                if (cardTotal < 20)
                                {
                                    folding = true;
                                }
                            }
                        }
                        if (fiveCardRank == THREE_TO_A_FLUSH || fiveCardRank == THREE_TO_A_STRAIGHT_INSIDE)//#7
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 0)
                            {
                                folding = true;
                            }
                        }
                        if (fiveCardRank == THREE_TO_A_STRAIGHT_OUTSIDE || fiveCardRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#8
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 0)
                            {
                                folding = true;
                            }
                        }
                        if (fiveCardRank == FOUR_TO_A_STRAIGHT_INSIDE || fiveCardRank == FOUR_TO_A_FLUSH)//#9
                        {
                            if (/*virtualPlayers[player].RoundRaiseAmount*/potRaisePercentage > 0)
                            {
                                folding = true;
                            }
                        }
                        if (fiveCardRank >= FULL_HOUSE)
                        {
                            BetType = BetTypes.raising;
                            raise = PotAmount;
                        }
                    }
                    else//we missed the hand
                    {
                        folding = true;
                    }


                }
                //River possibilities #3 the pot wa raised by the real player
                if (virtualPlayers[0].RoundRaiseAmount > 0 && flopTurnRiverRaised == false)//the real player raised
                {
                    if (fiveCardRank < PAIR && rank > 6)//#1
                    {
                        folding = true;
                    }


                    if (fiveCardRank == PAIR)// #2 #3 #4
                    {
                        if (GetPairType(hand) != PairTypes.Pocket)
                        {
                            if (GamePlayers[player].hand.XofaKindValue <= 10)
                            {
                                folding = true;
                            }
                        }
                    }

                    if (fiveCardRank == TWO_PAIR)//#5
                    {
                        if (potRaisePercentage > 25)
                        {
                            int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
                            if (cardTotal < 20)
                            {
                                folding = true;
                            }
                        }
                    }
                    if (fiveCardRank == THREE_TO_A_FLUSH || fiveCardRank == THREE_TO_A_STRAIGHT_INSIDE)//#6
                    {
                        if (virtualPlayers[player].RoundRaiseAmount > 0)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank == THREE_TO_A_STRAIGHT_OUTSIDE || fiveCardRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#7
                    {
                        if (virtualPlayers[player].RoundRaiseAmount > 0)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank == FOUR_TO_A_STRAIGHT_INSIDE || fiveCardRank == FOUR_TO_A_FLUSH)//#8
                    {
                        if (virtualPlayers[player].RoundRaiseAmount > 0)
                        {
                            folding = true;
                        }
                    }
                    if (fiveCardRank >= FULL_HOUSE)
                    {
                        BetType = BetTypes.raising;
                        raise = PotAmount;
                    }

                }
                if (fiveCardRank >= THREE_OF_A_KIND && fiveCardRank <= HIGH_THREE_OF_A_KIND)
                {
                    BetType = BetTypes.raising;
                    raise = RoundUp(PotAmount / 2);
                }
                lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " #" + player.ToString() + " River Bet Rank = " + fiveCardRank.ToString() + Environment.NewLine;
            }

            //END RIVER 



            if (folding == true)//The universal high card test for five card hands
            {
                if (GamePlayers[player].hand.HighCard >= Settings.highCardThreshhold)//greater than a ....
                    folding = false;//turn off the fold
            }

            if (folding == true)
            {
                BetType = BetTypes.folding;
            }
            if (ThisPlayersCall == 0 && BetType == BetTypes.folding)
            {
                BetType = BetTypes.checking;
            }

            if (BetType == BetTypes.raising && virtualPlayerRaised > Settings.virtualPlayerRaiseLimit)//no betting wars. 
            {
                BetType = BetTypes.checking;
            }

            if (ThisPlayersCall == 0 && BetType == BetTypes.calling)
            {
                BetType = BetTypes.checking;
            }
            if (ThisPlayersCall > 0 && BetType == BetTypes.checking)
            {
                BetType = BetTypes.calling;
            }

            if (virtualPlayers[0].AllIn == true)
            {
                if (BetType == BetTypes.folding)//this player is attempting to fold
                {
                    if (playerWithBestHand == player)//we have the best hand
                    {
                        BetType = BetTypes.calling;
                    }
                }
            }
            if (virtualPlayers[player].AllIn == true)
            {
                AllInPlayer(player);
                lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " " + player.ToString() + " IS ALL IN" + Environment.NewLine;
            }
            else
            {
                switch (BetType)
                {
                    case BetTypes.folding:
                        {
                            FoldPlayer(player);
                            lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " " + player.ToString() + " FOLDED" + Environment.NewLine;
                        }
                        break;
                    case BetTypes.checking:
                        {
                            CheckPlayer(player);
                            lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " " + player.ToString() + " CHECKED" + Environment.NewLine;
                        }
                        break;
                    case BetTypes.calling:
                        {
                            if (ThisPlayersCall >= virtualPlayers[player].Credits)
                            {
                                BetType = BetTypes.allIn;
                                raise = virtualPlayers[player].Credits - ThisPlayersCall;
                                ThisRoundRaisePercentage += GetPotRaisePercentage(raise);//(int)(100 / (PotAmount / raise));
                                virtualPlayers[player].RoundCallAmount += ThisPlayersCall;

                                ThisRoundBet = virtualPlayers[player].Credits;
                                AllInPlayer(player);
                                //PotAmount += ThisRoundBet;
                                virtualPlayers[player].Credits -= ThisRoundBet;
                                lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " " + player.ToString() + " ALL IN" + Environment.NewLine;
                            }
                            else
                            {
                                ThisRoundBet = ThisPlayersCall;// CallAmount;
                                virtualPlayers[player].Credits -= ThisRoundBet;
                                virtualPlayers[player].RoundCallAmount += ThisPlayersCall;
                                CallPlayer(player);
                                lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " " + player.ToString() + " CALLED" + Environment.NewLine;
                            }
                        }
                        break;
                    case BetTypes.raising:
                        {
                            virtualPlayerRaised++;
                            if (raise == 0)//no one else adjusted the raise amount
                            {
                                raise = 1.00;//temp values till game works
                                raise = RoundUp(PlayerBet / 2);

                                if (bluff == true)
                                {
                                    raise = PlayerBet;// PotAmount / 5;//for now
                                }
                                if (raise == 0)
                                {
                                    raise = virtualPlayers[0].RoundRaiseAmount;
                                }
                            }

                            if (raise > raiseLimit)
                            {
                                raise = raiseLimit;
                            }

                            if (raise + ThisPlayersCall >= virtualPlayers[player].Credits)
                            {
                                BetType = BetTypes.allIn;
                                raise = virtualPlayers[player].Credits - ThisPlayersCall;
                                virtualPlayers[player].RoundCallAmount += ThisPlayersCall;
                                AllInPlayer(player);
                                ThisRoundBet = raise + ThisPlayersCall;// CallAmount;
                                                                       //PotAmount += ThisRoundBet;
                                virtualPlayers[player].Credits -= ThisRoundBet;
                                lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " " + player.ToString() + " ALL IN" + Environment.NewLine;
                                break;
                            }

                            ThisRoundBet = raise + ThisPlayersCall;// CallAmount;
                            virtualPlayers[player].Credits -= ThisRoundBet;
                            //CallAmount += raise;
                            RaisePlayer(player, raise);
                            virtualPlayers[player].RoundCallAmount += ThisPlayersCall;
                            lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " " + player.ToString() + " RAISED" + Environment.NewLine;
                            GamePlayers[player].RoundRaiseCount++;
                            ThisRoundRaisePercentage += GetPotRaisePercentage(raise);// (int)(100 / (PotAmount / raise));
                        }
                        break;
                    case BetTypes.allIn:
                        {
                            raise = virtualPlayers[player].Credits - ThisPlayersCall;
                            ThisRoundRaisePercentage += GetPotRaisePercentage(raise);//(int)(100 / (PotAmount / raise));
                            virtualPlayers[player].RoundCallAmount += ThisPlayersCall;
                            AllInPlayer(player);
                            ThisRoundBet = virtualPlayers[player].Credits;
                            //PotAmount += ThisRoundBet;
                            virtualPlayers[player].Credits -= ThisRoundBet;
                            lblWinInfo.GetComponent<Text>().text += virtualPlayers[player].Name + " " + player.ToString() + " ALL IN" + Environment.NewLine;
                        }
                        break;
                }
            }
            if (AutoPlay == true)
            {
                PlayerCredits = virtualPlayers[0].Credits;
            }
        }
        else//player = 0
        {
            ThisRoundBet = playerCurrentBet;
            if (BetType != BetTypes.raising || BetType != BetTypes.calling || BetType != BetTypes.allIn)
            {
                //PlayerBet += playerCurrentBet;
            }
        }
        string roundStr = "";
        switch (GameState)
        {
            case GameStates.HoldCardBet:
                {
                    virtualPlayers[player].TwoCardBet += ThisRoundBet;
                    virtualPlayers[player].CurrentBetAmount = virtualPlayers[player].TwoCardBet;
                    roundStr = " First Round bet = ";
                }
                break;
            case GameStates.FlopBet:
                {
                    if (BetType == BetTypes.raising)
                    {
                        flopTurnRiverRaised = true;
                    }
                    virtualPlayers[player].FlopBet += ThisRoundBet;
                    virtualPlayers[player].CurrentBetAmount = virtualPlayers[player].FlopBet;
                    roundStr = " Second Round bet = ";
                }
                break;
            case GameStates.TurnBet:
                {
                    if (BetType == BetTypes.raising)
                    {
                        flopTurnRiverRaised = true;
                    }
                    virtualPlayers[player].TurnBet += ThisRoundBet;
                    virtualPlayers[player].CurrentBetAmount = virtualPlayers[player].TurnBet;
                    roundStr = " Third Round bet = ";
                }
                break;
            case GameStates.RiverBet:
                {
                    if (BetType == BetTypes.raising)
                    {
                        flopTurnRiverRaised = true;
                    }
                    virtualPlayers[player].RiverBet += ThisRoundBet;
                    virtualPlayers[player].CurrentBetAmount = virtualPlayers[player].RiverBet;
                    roundStr = " Fourth Round bet = ";
                }
                break;
            case GameStates.EndGame:
                {
                    virtualPlayers[player].LastRoundBet += ThisRoundBet;
                    virtualPlayers[player].CurrentBetAmount = virtualPlayers[player].LastRoundBet;
                    roundStr = " Ending bet = ";
                }
                break;

        }
        //if(player != 0)
        PotAmount += ThisRoundBet;// virtualPlayers[player].FlopBet;
                                  //lblWinInfo.AppendText("Player " + player.ToString() + roundStr + ThisRoundBet.ToString() + Environment.NewLine);
        if (player == 0 && AutoPlay == true)
        {
            Settings.creditsPlayed += ThisRoundBet;
        }

        isNextPlayer = true; // nextPlayerTimer.Start();
        return ThisRoundBet;
    }

    public int GetCardValue(int card)
    {
        return (card - (((GetCardSuit(card) - 1) * 13) - 2));
    }

    public int GetCardSuit(int card)
    {
        return ((card / 13) + 1);
    }
    /// <summary>
    /// Test Buttons
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button2_Click(object sender, EventArgs e)
    {
        //dealFlop();
        //deck[0] = gaffHand[element, 0];// 0 1
        //deck[6] = gaffHand[element, 1];// 0 2

        //deck[1] = gaffHand[element, 2];// 1
        //deck[7] = gaffHand[element, 3];// 1 

        //deck[2] = gaffHand[element, 4];// 2
        //deck[8] = gaffHand[element, 5];// 2

        //deck[3] = gaffHand[element, 6];// 3
        //deck[9] = gaffHand[element, 7];// 3

        //deck[4] = gaffHand[element, 8];// 4
        //deck[10] = gaffHand[element, 9];//4

        //deck[5] = gaffHand[element, 10];// 5
        //deck[11] = gaffHand[element, 11];//5


        //deck[12] = gaffHand[element, 12];//flop 1
        //deck[13] = gaffHand[element, 13];//flop 2//17
        //deck[14] = gaffHand[element, 14];//flop 3

        //deck[15] = gaffHand[element, 15];//turn
        //deck[16] = gaffHand[element, 16];//river
        string playerData = "";
        for (int x = 1; x < Settings.playerSize; x++)
        {
            playerData += "," + virtualPlayers[x].playerNumber.ToString();
        }
        string data2 = deck[0].ToString() + "," + deck[6].ToString() + "," + deck[1].ToString() +
                  "," + deck[7].ToString() + "," + deck[2].ToString() + "," + deck[8].ToString() +
                  "," + deck[3].ToString() + "," + deck[9].ToString() + "," + deck[4].ToString() +
                  "," + deck[10].ToString() + "," + deck[5].ToString() + "," + deck[11].ToString() +
                  "," + deck[12].ToString() + "," + deck[13].ToString() + "," + deck[15].ToString() +
                  "," + deck[15].ToString() + "," + deck[16].ToString() + playerData;

        string data = "";
        for (int x = 0; x < 17; x++)
        {
            if (x != 0)
            {
                data += ",";
            }
            data += deck[x];
        }
        data += playerData;
        data += "," + buttonPosition.ToString();
        lblTemp.text = data;
    }

    private void button3_Click(object sender, EventArgs e)
    {
        IncrementButtonPosition(false);
    }

    private void dealTurnButton_Click(object sender, EventArgs e)
    {
        testDataRetrieved = true;
    }

    private void dealRiverButton_Click(object sender, EventArgs e)
    {
        dealRiver();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //TODO: FoldPlayer(comboBox1.SelectedIndex + 1);
    }
    //end test buttons

    private void StartNewGame()
    {
        virtualPlayerRaised = 0;
        flopTurnRiverRaised = false;
        if (GameState != GameStates.Ante)
        {
            if (PlayerSurrender == false)
                IncrementButtonPosition(false);
        }
        if (anteBet > 0)
        {
            PlayerCredits += anteBet;//return the credits
            PlayerBet = 0;
        }

        if (testDataRetrieved == true)
        {
            UseTestData();
            //testDataRetrieved = false;
        }
        //36,25,12,34,31,47,49,17,11,35,40,5,41,24,38,9,18,2,7,8,6,4,5

        if (testDataRetrieved == false)
        {
            ShuffleVirtualPlayers();
        }

        // testDataRetrieved = false;           
        for (int x = 1; x < Settings.playerSize; x++)
        {
            try {
                lblWinInfo.GetComponentInChildren<Text>().text += " Player " + x.ToString() + " =" + virtualPlayers[x].Name + Environment.NewLine;
            }
            catch (Exception e) {
                if (Settings.isDebug) Debug.LogError("error in lblWinInfo: " + e.Message);
            }
        }

        GameState = GameStates.Ante;
        betStringPtr = _ante;//point at the ANTE title string

        if (AutoPlay == true || autoStart == true)//ANTE
        {
            if (Settings.isDebug) Debug.Log("StartNewGame() autoplay and autostart are TRUE");
            betAmount = 5;

            if (betAmount == 0 || betAmount > PlayerCredits)
                return;

            if (autoStart == true)
            {
                anteBet = lastBet;
            }
            else
            {
                anteBet = betAmount;
            }
            //double thisAnteBet = anteBet;
            PotAmount = 0;
            for (int x = 0; x < Settings.playerSize; x++)//
            {
                PotAmount += anteBet;
            }
            PlayerBet = anteBet;
            PlayerRaise = 0;
            int value = (int)(PlayerBet / Settings.gameDenomination);
            denomUnits = (int)(PlayerBet / Settings.gameDenomination);
            VideoMultiplier = value;

            if (value > Settings.videoBonusMaxMultiplier)
            {
                value = Settings.videoBonusMaxMultiplier;
            }
            if (value < Settings.videoBonusMaxMultiplier)
            {
                payTable.UpdateVideoBonusMaxMultiplier(Settings.videoBonusMaxMultiplier);
            }
            else
            {
                payTable.UpdateVideoBonusMaxMultiplier((int)playerBet);
            }
            selectedColumn = value;
            selectedColumn = denomUnits;
            payTable.SetPaytableSelectedColumn(selectedColumn);
            isNextPlayer = false; //gameOverTimer.Stop();
        }
        else if (btnStartGame.GetComponent<Button>().IsActive())//ANTE
        {
            if (AutoPlay == true)
            {
                betAmount = Settings.betAmountAutoplay;
            }

            if (Settings.isDebug) Debug.Log("betAmount:" + betAmount + " playerCredits" + PlayerCredits);

            // if bet amount between 0 and max player credits then continue game
            if (betAmount == 0 || betAmount > PlayerCredits)
                return;

            if (Settings.isDebug) Debug.Log("betAmount correct:" + betAmount + " continue game ...");

            anteBet = betAmount;
            PotAmount = 0;
            for (int x = 0; x < Settings.playerSize; x++)//
            {
                PotAmount += anteBet;
            }

            PlayerBet = anteBet;
            PlayerRaise = 0;

            int value = (int)(PlayerBet / Settings.gameDenomination);
            denomUnits = (int)(PlayerBet / Settings.gameDenomination);
            VideoMultiplier = value;

            if (value > Settings.videoBonusMaxMultiplier)
            {
                value = Settings.videoBonusMaxMultiplier;
            }
            if (value < Settings.videoBonusMaxMultiplier)
            {
                payTable.UpdateVideoBonusMaxMultiplier(Settings.videoBonusMaxMultiplier);
            }
            else
            {
                payTable.UpdateVideoBonusMaxMultiplier((int)playerBet);
            }
            selectedColumn = value;
            selectedColumn = denomUnits;
            payTable.SetPaytableSelectedColumn(selectedColumn);
            isNextPlayer = false; //gameOverTimer.Stop();
        }

        raiseLimit = anteBet * Settings.raiseLimitMultiplier;

        winnerDeclared = false;
        PotSplit = 1;
        CallAmount = 0;
        clearBetLabels();
        for (int x = 1; x < Settings.playerSize; x++)
        {
            ClearPlayerCards(x);
        }
        clearCreditLabels();
        updateFoldedPlayersImages(false);
        WinAmount = 0;
        lblWin.GetComponent<Text>().text = "";
        videoPokerWin = 0;

        restoreCardDefaults(false);
        DisableBettingButtons();

        cardsDealt = 0;
        deckPtr = 0;

        for (int x = 0; x < Settings.playerSize; x++)//every player starts with the same credits
        {
            virtualPlayers[x].Credits = PlayerCredits;
            virtualPlayers[x].AllIn = false;
            GameWinners[x] = false;
        }
        PlayerCredits -= playerBet;

        panelInitBet.SetActive(true);
        btnStartGame.GetComponentInChildren<Text>().text = "Bet";
        panelInitBet.SetActive(false);

        Settings.creditsPlayed += playerBet;
        if (AutoPlay == true || autoStart == true)
        {
            startGame();
        }
        if (Settings.isDebug) Debug.Log("end startnewgame()");
    }

    public void btnNewGame_Click()//object sender, EventArgs e)//Ante Button
    {
        StartNewGame();
    }

    public void btnTestClearCardsClick() {
        foreach (var card in cardsOfPlayer)
            card.sprite = cardBack;
    }

    public void btnTestShowPlayerCards() {
        Settings.testGame = true;
        dealPlayerCards();
        //Settings.testGame = false;
    }

    public void startGame()
    {
        if (Settings.isDebug) Debug.Log("StartGame()");

        btnCredit.SetActive(false);

        autoStart = false;
        lastBet = anteBet;
        //buttonSound.Play();
        clearBetLabels();//clear all the labels
        AntePotAmount = PotAmount;
        FlopPotAmount = 0;
        TurnPotAmount = 0;
        RiverPotAmount = 0;
        playerCurrentBet = 0;

        for (int x = 0; x < Settings.playerSize; x++)//
        {
            virtualPlayers[x].CurrentBetAmount = 0;
            virtualPlayers[x].Credits = PlayerCredits;
            virtualPlayers[x].Ante = anteBet;
        }
        //paytableGrid.Enabled = false; //TODO: GUILayout.SelectionGrid
        PlayerRaise = 0;
        anteBet = 0;

        PlayerSurrender = false;

        stopGameOverTimer();

        btnNewGame.SetActive(false);
        btnRepeatBet.SetActive(false);

        GameState = GameStates.HoldCardBet;
        RenewVirtualPlayerProfiles();
        ThisRoundRaisePercentage = 0;

        CurrentBetPosition = buttonPosition;
        DealNextRound();
        EvalPlayerHands(true, false, false, false);

        NextPlayer();
    }

    private void RenewVirtualPlayerProfiles()
    {
        for (int x = 0; x < Settings.playerSize; x++)
        {
            virtualPlayers[x].TwoCardBet = 0;
            virtualPlayers[x].FlopBet = 0;
            virtualPlayers[x].TurnBet = 0;
            virtualPlayers[x].RiverBet = 0;
            virtualPlayers[x].CurrentBetAmount = 0;
            virtualPlayers[x].Folded = false;
            virtualPlayers[x].AllIn = false;
            virtualPlayers[x].HighCard = 0;
            virtualPlayers[x].RoundRaiseAmount = 0;
            virtualPlayers[x].RoundChecked = false;
            virtualPlayers[x].FinalHandRank = 0;
            virtualPlayers[x].Bluffing = getWeightedResult(virtualPlayers[x].BluffPercentage);
        }
    }

    private void ResetVirtualPlayerVars()
    {
        playerCurrentBet = 0;
        for (int x = 0; x < Settings.playerSize; x++)
        {
            virtualPlayers[x].CurrentBetAmount = 0;
            virtualPlayers[x].RoundRaiseAmount = 0;
            virtualPlayers[x].RoundChecked = false;
            virtualPlayers[x].FinalHandRank = 0;
            GamePlayers[x].RoundRaiseCount = 0;
        }
    }

    private void StartBetting()
    {
        CurrentBetPosition = buttonPosition + 1;
        do
        {
            if (CurrentBetPosition != 0)
            {
                BetPlayer(CurrentBetPosition);
                CurrentBetPosition++;
            }
        }
        while (CurrentBetPosition != 0);
    }


    private void finishThisRoundBetting()
    {
        if (GameState == GameStates.HoldCardBet)
        {
            GameState = GameStates.FlopBet;
            FlopPotAmount = PotAmount;
        }
        else
        if (GameState == GameStates.FlopBet)
        {
            GameState = GameStates.TurnBet;
            TurnPotAmount = PotAmount;
        }
        else
        if (GameState == GameStates.TurnBet)
        {
            GameState = GameStates.RiverBet;
            RiverPotAmount = PotAmount;
        }
        else
        if (GameState == GameStates.RiverBet)
        {
            GameState = GameStates.EndGame;
        }
        CallAmount = 0;
        ThisRoundRaisePercentage = 0;
        DealButtonPassed = false;
        virtualPlayerRaised = 0;//dont carry over
        flopTurnRiverRaised = false;
        ResetVirtualPlayerVars();
        DisableBettingButtons();
        // btnStartGame.GetComponent<Button>().interactable = false;
    }

    public void btnAllInClick() //_Click((object sender, EventArgs e)
    {
        double playerCallAmount;
        double raiseValue;
        if (virtualPlayers[0].AllIn == false)
        {
            playerCallAmount = GetCurrentBet() - virtualPlayers[0].CurrentBetAmount;
            if (PlayerCredits < playerCallAmount)
            {
                playerCallAmount = PlayerCredits;
            }
            raiseValue = PlayerCredits - playerCallAmount;
            //PotAmount += PlayerCredits;
            virtualPlayers[0].AllIn = true;
            virtualPlayers[0].RoundRaiseAmount += raiseValue;
            PlayerBet += playerCallAmount;
            PlayerRaise += raiseValue;//update the players raise status label
            playerCurrentBet += playerCallAmount + raiseValue;
            Settings.creditsPlayed += PlayerCredits;
            PlayerCredits = 0;
            ThisRoundRaisePercentage += GetPotRaisePercentage(raiseValue);//(int)(100 / (PotAmount / raiseValue));
        }
        else
        {
            playerCurrentBet = 0;
        }
        BetPlayer(CurrentBetPosition);//do the accounting
    }


    public void btnCallClick()//_Click(object sender, EventArgs e)
    {
        if (virtualPlayers.Length > 0 && virtualPlayers[0] != null)
        {
            if (Settings.isDebug) Debug.Log("btnCallClick() virtualPlayers.Count = " + virtualPlayers.Count());
            double pBet = GetCurrentBet() - virtualPlayers[0].CurrentBetAmount;

            PotAmount += pBet;
            PlayerCredits -= pBet;
            Settings.creditsPlayed += pBet;
            playerCurrentBet = pBet;
            PlayerBet += pBet;

            BetPlayer(CurrentBetPosition);//do the accounting
        }
        else
        {
            isNextPlayer = false;
        }
    }

    public void btnCheckClick() //_Click(object sender, EventArgs e)
    {
        if (virtualPlayers.Length > 0 && virtualPlayers[0] != null)
        {
            playerCurrentBet = 0;
            virtualPlayers[0].RoundChecked = true;
            BetPlayer(CurrentBetPosition);
        }
        else
        {
            isNextPlayer = false;
        }
    }

    private bool checkForPlayerWin()
    {
        for (int x = 1; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x].Folded == false)
            {
                return false;
            }
        }
        return true;
    }

    private double GetCurrentBet()
    {
        double highbet = 0;
        for (x = 0; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x].CurrentBetAmount > highbet)
                highbet = virtualPlayers[x].CurrentBetAmount;
        }
        return highbet;
    }

    private double GetTotalRaiseAmount()
    {
        double highbet = 0;
        for (x = 0; x < Settings.playerSize; x++)
        {
            highbet += virtualPlayers[x].RoundRaiseAmount;
        }
        return highbet;
    }

    private int GetPercentPotRaised(int player)
    {
        double StartAmount = 0;
        int ret = 0;
        switch (GameState)
        {
            case GameStates.HoldCardBet: StartAmount = AntePotAmount; break;
            case GameStates.FlopBet: StartAmount = FlopPotAmount; break;
            case GameStates.TurnBet: StartAmount = TurnPotAmount; break;
            case GameStates.RiverBet: StartAmount = RiverPotAmount; break;
        }
        double playerContribution = virtualPlayers[player].RoundRaiseAmount + virtualPlayers[player].RoundCallAmount;
        double difference = (PotAmount - playerContribution) - StartAmount;
        if (difference != 0)
        {
            ret = (int)(100 / (StartAmount / difference));
        }
        if (ret < 0)
        {
            ret = 0;
        }
        return ret;
        //int realPlayerPotRaisePercentage = (int)(100 / (PotAmount / virtualPlayers[0].RoundRaiseAmount));
    }

    private bool CheckForAllInShowdown()
    {
        for (int x = 0; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x].Folded == false)
            {
                if (virtualPlayers[x].AllIn == false)
                {
                    return false;
                }
            }
        }
        if (virtualPlayers[0].AllIn == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int GetPlayerWithBestHand()
    {
        int highHoleHand = 200;
        int highHand = 0;
        int HighHolder = 0;
        if (cardsDealt < 5)
        {
            EvalPlayerHands(true, false, false, false);
        }
        else
        {

        }
        for (int x = 0; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x].Folded == false)
            {
                if (cardsDealt < 5)
                {
                    if (playerHoleCardsRankings[x] < highHoleHand)
                    {
                        highHoleHand = playerHoleCardsRankings[x];
                        HighHolder = x;
                    }
                }
                else
                {
                    if (virtualPlayers[x].FiveCardHandRank > highHand)
                    {
                        if (virtualPlayers[x].FiveCardHandRank > MID_PAIR)
                        {
                            highHand = virtualPlayers[x].FiveCardHandRank;
                            HighHolder = x;
                        }
                    }

                }
            }
        }
        return HighHolder;
    }

    private bool CheckForBetFinish()
    {
        int x;
        double highBet = GetCurrentBet();
        for (x = 0; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x].Folded == false && virtualPlayers[x].AllIn == false)
            {
                if (virtualPlayers[x].CurrentBetAmount != highBet || (highBet == 0 && virtualPlayers[x].RoundChecked == false))
                //if (virtualPlayers[x].CurrentBetAmount != highBet || DealButtonPassed == false)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void AwardPlayerWin()
    {
        winnerDeclared = true;
        double videoWin = 0;
        if (cardsDealt >= 5)
        {
            videoWin = AwardVideoBonus(0);
        }
        PlayerCredits += PotAmount;//award the pot to the player
        Settings.creditsWon += PotAmount;
        PlayerCredits += videoWin;
        Settings.creditsWon += videoWin;
        WinAmount = PotAmount + videoWin;
        clearCreditLabels();
        gameOverStrings[1] = realPlayerName + " WIN THE POT";
        panelInitBet.SetActive(false);
        if (lastBet > 0 && lastBet <= PlayerCredits)
        {
            btnRepeatBet.GetComponent<Text>().text = "REPEAT LAST BET OF " + String.Format("{0:C}", lastBet);
            btnRepeatBet.SetActive(true);
        }
        btnNewGame.SetActive(true);
        try
        {
            Assets.Scripts.Logger.LogResults();
        }
        catch
        {

        }
        startGameOverTimer(true);
    }

    public double AwardVideoBonus(int split)
    {
        if (split == 0)
        {
            split = 1;
        }
        double videoBonus = 0;
        int playerWinRank = GetFiveCardRanking(0);//what rank did the player get??
        if (playerWinRank >= videoPokerLowRank)
        {
            videoBonus = payTable.GetVideoPokerBonus(playerWinRank);
            videoBonus *= videoMultiplier;
            payTable.SetPaytableSelectedWin(playerWinRank);
            if (videoBonus > 0)
            {
                videoBonus /= split;
            }
            videoPokerWin = videoBonus;//makes the message flash
        }
        if (videoBonus > 0)
        {
            //videoWin.Play();
        }

        return videoBonus;
    }

    public void EndGame()
    {
        int winner = 0;
        double videoBonus;
        int playerWinRank = GetFiveCardRanking(0);//what rank did the player get??
        for (int x = 1; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x].Folded)
            {
                ClearPlayerCards(x);
            }
            else
            {
                ShowPlayerCards(x, true);
            }
        }

        clearCreditLabels();
        winner = GetWinner();

        winnerDeclared = true;

        int split = 0;
        if (winner > 5)
        {//we have some ties and need to split the pot

            bool playerWin = false;
            for (int x = 0; x < Settings.playerSize; x++)
            {
                if (GameWinners[x] == true)
                {
                    if (x == 0)
                    {
                        playerWin = true;
                    }
                    split++;
                }
            }
            if (playerWin == true)
            {
                videoBonus = 0;
                if (playerWinRank >= videoPokerLowRank)
                {
                    videoBonus = payTable.GetVideoPokerBonus(playerWinRank);
                    videoBonus *= videoMultiplier;
                    payTable.SetPaytableSelectedWin(playerWinRank);
                    if (videoBonus > 0)
                    {
                        videoBonus /= split;
                    }
                }
                PlayerCredits += PotAmount / split;
                Settings.creditsWon += PotAmount / split;
                PlayerCredits += videoBonus;
                Settings.creditsWon += videoBonus;
                WinAmount = PotAmount / split;
                WinAmount += videoBonus;
                videoPokerWin = videoBonus;
            }
        }
        if (winner == 0)
        {
            videoBonus = 0;
            if (playerWinRank >= videoPokerLowRank)
            {
                videoBonus = payTable.GetVideoPokerBonus(playerWinRank);
                payTable.SetPaytableSelectedWin(playerWinRank);
                videoBonus *= videoMultiplier;
            }
            PlayerCredits += PotAmount;
            Settings.creditsWon += PotAmount;
            PlayerCredits += videoBonus;
            Settings.creditsWon += videoBonus;
            WinAmount = PotAmount;
            WinAmount += videoBonus;
            videoPokerWin = videoBonus;
        }
        if (split < 2)
        {
            string winString;
            lblWin.SetActive(true);
            int winRank = GetFiveCardRanking(winner);
            winRank = payTable.AdjustWinRank(winRank);
            winString = PayTableStrings[ROYAL_FLUSH - winRank];
            if (winner != 0)
            {
                gameOverStrings[1] = "PLAYER " + winner.ToString() + " WIN    " + winString;
            }
            else
            {
                gameOverStrings[1] = realPlayerName + " WIN    " + winString; //correct english
            }
            UpdateBetLabel(winString, winner, false);//, winColor);
        }
        else
        {
            lblWin.SetActive(true);
            lblWin.GetComponent<Text>().text = "The Pot is Split " + split.ToString() + " Ways";
            for (int x = 0; x < Settings.playerSize; x++)
            {
                if (GameWinners[x] == true)
                {
                    int winRank = GetFiveCardRanking(x);
                    winRank = payTable.AdjustWinRank(winRank);
                    string winString = PayTableStrings[ROYAL_FLUSH - winRank];
                    if (winRank == 8)
                    {
                        gameOverStrings[1] = "PLAYERS SPLIT WITH " + winString; //correct english
                    }
                    else
                    {
                        gameOverStrings[1] = "PLAYERS SPLIT WITH A " + winString;
                    }

                    UpdateBetLabel(winString, x, false);// winColor);
                }
            }
        }
        if (winner != 0 && Settings.videoBonusWinOnly == false)
        {
            if (playerWinRank >= videoPokerLowRank)
            {
                videoBonus = payTable.GetVideoPokerBonus(playerWinRank);
                videoBonus *= videoMultiplier;
                payTable.SetPaytableSelectedWin(playerWinRank);
                PlayerCredits += videoBonus;
                Settings.creditsWon += videoBonus;
                WinAmount += videoBonus;
                videoPokerWin = videoBonus;
            }
        }
        try
        {
            Assets.Scripts.Logger.LogResults();
        }
        catch
        {

        }

        startGameOverTimer(true);

        //bonusPokerPanel.SetActive(true); //TODO:
        if (lastBet > 0 && lastBet <= PlayerCredits)
        {
            panelInitBet.SetActive(true);
            btnRepeatBet.SetActive(true);
            btnRepeatBet.GetComponentInChildren<Text>().text = "REPEAT LAST BET OF " + String.Format("{0:C}", lastBet);
        }
        btnNewGame.SetActive(true);
        if (videoPokerWin > 0)
        {
            //videoWin.Play();
        }
        panelInitBet.SetActive(false);
        isNextPlayer = false;
    }


    public int GetWinner()
    {
        int winner = 0;
        //int rank = 0;
        int WinnerCount = 0;
        int[] wins = new int[Settings.playerSize];
        int highHand = 0;
        int highTotal = 0;
        int highKicker = 0;
        int tempRank = 0;
        int tempTotal = 0;
        int tempKicker = 0;
        for (int player = 0; player < Settings.playerSize; player++)
        {
            if (virtualPlayers[player].Folded == false)
            {
                tempTotal = 0;
                tempKicker = 0;
                tempRank = GetFiveCardRanking(player);//sets up the actual rankings
                if (tempRank >= PAIR)
                {
                    tempTotal = GamePlayers[player].hand.CardValueTotal;
                    tempKicker = GamePlayers[player].hand.Kicker;
                }
                if (tempRank > highHand || (tempRank == highHand && tempTotal > highTotal) || (tempRank == highHand && tempTotal == highTotal && tempKicker > highKicker))
                {
                    highHand = tempRank;//the rank
                    highTotal = tempTotal;
                    highKicker = tempKicker;
                    winner = player;
                }
                virtualPlayers[player].FinalHandRank = tempRank;
            }
        }
        if (highHand >= PAIR)
        {
            for (int player = 0; player < Settings.playerSize; player++)//check for ties
            {
                if (virtualPlayers[player].Folded == false)
                {
                    if (GamePlayers[player].hand.HandRank == highHand)
                    {
                        if (GamePlayers[player].hand.CardValueTotal == highTotal)
                        {
                            if (GamePlayers[player].hand.Kicker == highKicker)
                            {
                                WinnerCount++;
                                GameWinners[player] = true;
                            }
                        }
                    }
                }
            }
        }
        else//we had no players with a rankable hand
        {
            int highCard = 0;
            WinnerCount = 0;
            for (int player = 0; player < Settings.playerSize; player++)
            {
                if (virtualPlayers[player].Folded == false)
                {
                    if (virtualPlayers[player].HighCard > highCard)
                    {
                        highCard = virtualPlayers[player].HighCard;
                    }
                }
            }
            for (int player = 0; player < Settings.playerSize; player++)//check for ties
            {
                if (virtualPlayers[player].Folded == false)
                {
                    if (virtualPlayers[player].HighCard == highCard)
                    {
                        WinnerCount++;
                        GameWinners[player] = true;
                    }
                }
            }
            if (WinnerCount > 1)//new kicker stuff
            {
                int HighKicker = 0;
                int TempWinner = 10;
                int kickerCount = 0;
                int[] kickers = new int[Settings.kickerSize];
                for (int x = 0; x < Settings.kickerSize; x++)
                {
                    if (virtualPlayers[x].Folded == false)
                    {
                        if (GameWinners[x] == true)
                        {
                            int temp = GetXofaKindKicker(highCard, GamePlayers[x].hand.cardHand);
                            if (temp == 1)
                            {
                                temp = 14;
                            }
                            kickers[x] = temp;

                            if (temp > HighKicker)
                            {
                                HighKicker = temp;
                            }
                        }
                    }
                }
                for (int x = 0; x < Settings.kickerSize; x++)
                {
                    if (HighKicker == kickers[x])
                    {
                        kickerCount++;
                        TempWinner = x;
                    }
                    else
                    {
                        GameWinners[x] = false;
                    }
                }
                WinnerCount = kickerCount;
                if (WinnerCount == 1)
                {
                    winner = TempWinner;
                }
            }
        }
        if (WinnerCount == 1)
        {
            for (int x = 0; x < Settings.kickerSize; x++)
            {
                if (GameWinners[x] == true)
                {
                    winner = x;
                }
            }
        }
        if (WinnerCount > 1)
        {
            PotSplit = WinnerCount;
            return 10;
        }
        return winner;
    }

    public int GetKickerOld(int[] sevenCardHand, int[] cardsUsed)
    {
        int highCard = 0;
        for (int x = 0; x < 7; x++)
        {
            bool found = false;
            for (int i = 0; i < 5; i++)
            {
                if (cardsUsed[i] == sevenCardHand[x])
                {
                    found = true;
                    break; ;
                }
            }
            if (found == false)
            {
                int card = GetCardValue(sevenCardHand[x]);
                if (card > highCard)
                {
                    highCard = card;
                }
            }
        }
        return highCard;
    }

    public int GetKicker(int[] sevenCardHand, int[] cardsUsed)
    {
        int highCard = 0;
        int highHoldCard = 0;
        for (int x = 0; x < 7; x++)
        {
            bool found = false;
            for (int i = 0; i < 5; i++)
            {
                if (cardsUsed[i] == sevenCardHand[x])
                {
                    found = true;
                    break; ;
                }
            }
            if (found == false)
            {
                int card = GetCardValue(sevenCardHand[x]);
                if (card > highCard)
                {
                    highCard = card;
                    if (card == GetCardValue(sevenCardHand[0]) || card == GetCardValue(sevenCardHand[1]))
                    {
                        highHoldCard = card;
                    }
                }
            }
        }
        if (highHoldCard > 0)
        {
            highCard = highHoldCard;
        }
        return highCard;
    }

    public int GetXofaKindKicker(int card, int[] hand)
    {
        int kicker = 0;
        int holdKicker = 0;
        for (int x = 0; x < 7; x++)
        {
            int cv = GetCardValue(hand[x]);
            if (cv != card)
            {
                if (cv > kicker)
                {
                    kicker = cv;
                    if (cv == GetCardValue(hand[0]) || cv == GetCardValue(hand[1]))
                    {
                        holdKicker = cv;
                    }
                }
            }
        }
        if (holdKicker > 0)
        {
            kicker = holdKicker;
        }
        return kicker;
    }

    public int GetTwoPairKicker(int firstValue, int secondValue, int[] hand)
    {
        int kicker = 0;
        int holdKicker = 0;
        for (int x = 0; x < 7; x++)
        {
            int cv = GetCardValue(hand[x]);
            if (cv != firstValue && cv != secondValue)
            {
                if (cv > kicker)
                {
                    kicker = cv;
                    if (cv == GetCardValue(hand[0]) || cv == GetCardValue(hand[1]))
                    {
                        holdKicker = cv;
                    }
                }
            }
        }
        if (holdKicker > 0)
        {
            kicker = holdKicker;
        }
        return kicker;
    }

    public void NextPlayer()
    {
        isNextPlayer = false; // nextPlayerTimer.Stop();
        int bp = buttonPosition;

        CurrentBetPosition++;
        if (CheckForAllInShowdown() == true)
        {
            for (int x = 1; x < Settings.playerSize; x++)
            {
                if (virtualPlayers[x].AllIn == true)
                {
                    ShowPlayerCards(x, false);
                }
            }
        }

        if (CheckForBetFinish() == true)
        {
            if (checkForPlayerWin() == true)
            {
                AwardPlayerWin();
                return;
            }
            if (GameState != GameStates.RiverBet)//endgame
            {
                finishThisRoundBetting();
                DealNextRound();
            }
            else
            {
                EndGame();
                return;
            }
        }

        if (CurrentBetPosition != 0)
        {
            DisableBettingButtons();
            BetPlayer(CurrentBetPosition);
        }
        else
        {
            if (checkForPlayerWin() == true)
            {
                AwardPlayerWin();
            }
            else
            {
                if (AutoPlay == true)
                {
                    if (PlayerCredits < 100)
                    {
                        PlayerCredits = Settings.jurisdictionalBetLimit;// 1000;
                    }
                    BetPlayer(CurrentBetPosition);
                }
                else
                {
                    EnableBettingButtons();
                }
            }
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        //clearBetLabel(1);
        //btnFold.Enabled = false;
        //paytableGrid[0, 0].Selected = false;

        //paytableGrid[1, 1].Selected = true;
        //paytableGrid[1, 2].Selected = true;
        //payTable.SetPaytableSelectedColumn(9);
        //ShowPlayerCards(3);
        //SetPaytableSelectedWin(MID_THREE_OF_A_KIND);
        //RoundUp(21.6);
        //if (DialogResult.Yes == surrenderWindow.ShowDialog())
        //{
        //    //Button button = btnSurrender;
        //    //Button button = (Button)sender;
        //    //if (button == btnSurrender)
        //    btnFold_Click(btnSurrender, e);
        //}
        //lblTemp.text = "TEST";
        //GetPairType(new int[] { H2, H7, S6, S7, D9 });
        //GetPairType(new int[] { H2, H6, S6, S7, D9 });
        //GetPairType(new int[] { H2, H9, S6, S7, D9 });
    }

    public void btnSurrenderClick()
    {
        PlayerCredits += PlayerBet / 2;//return half the bet
        Settings.creditsWon += PlayerBet / 2;
        WinAmount = PlayerBet / 2;
        PlayerSurrender = true;
        buttonPosition = 0;//reset the button position to 5
        IncrementButtonPosition(false);

        btnFoldClick();
    }

    public void btnFoldClick() //_Click(object sender, EventArgs e)
    {
        //buttonSound.Play();
        if (GameState == GameStates.FlopBet || GameState == GameStates.TurnBet || GameState == GameStates.RiverBet)
        {
            int playerWinRank = GetFiveCardRanking(0);//what rank did the player get??
            if (payTable.AdjustWinRank(playerWinRank) >= videoPokerLowRank)
            {
                double videoBonus = payTable.GetVideoPokerBonus(playerWinRank);
                payTable.SetPaytableSelectedWin(playerWinRank);
                videoBonus *= videoMultiplier;
                PlayerCredits += videoBonus;
                Settings.creditsWon += videoBonus;
                WinAmount += videoBonus;
                videoPokerWin = videoBonus;
            }
        }
        if (videoPokerWin > 0)
        {
            //videoWin.Play(); //TODO: play sound/music when player is win
        }

        GameState = GameStates.PlayerLose;
        PotAmount = 0;
        clearCreditLabels();
        try
        {
            Assets.Scripts.Logger.LogResults();
        }
        catch
        {

        }

        DisableBettingButtons();

        if (panelInitBet != null) panelInitBet.SetActive(false);

        if (lastBet > 0 && lastBet <= PlayerCredits)
        {
            btnRepeatBet.SetActive(true);
            btnRepeatBet.GetComponentInChildren<Text>().text = "REPEAT LAST BET OF " + String.Format("{0:C}", lastBet);
        }
        btnNewGame.SetActive(true);

        startGameOverTimer(false);
    }

    private void playerAllCredits_MouseDoubleClick()//object sender, MouseEventArgs e)
    {
        PlayerCredits += Settings.jurisdictionalBetLimit;// 1000.00;
    }

    private void startGameOverTimer(bool win)
    {
        if (jurisdictionalLimit != 0)
        {
            if (RealPlayerCredits < jurisdictionalLimit)
            {
                if (btnCredit != null) btnCredit.SetActive(true);
            }
            if (RealPlayerCredits < jurisdictionalLimit - PlayerCredits)
            {
                if (btnCredit != null) btnCredit.SetActive(true);

                PlayerCredits += RealPlayerCredits;
                RealPlayerCredits = 0;
            }
            else
            {
                RealPlayerCredits -= jurisdictionalLimit - PlayerCredits;
                PlayerCredits = jurisdictionalLimit;
            }
            if (lastBet > 0 && lastBet <= PlayerCredits)
            {
                btnRepeatBet.SetActive(true);
                btnRepeatBet.GetComponentInChildren<Text>().text = "REPEAT LAST BET OF " + String.Format("{0:C}", lastBet);
            }
        }

        isNextPlayer = true; //gameOverTimer.Start();
        if (win)
        {
            gameOverPtr = 1;
        }
        else
        {
            gameOverPtr = 0;
        }
    }

    private void stopGameOverTimer()
    {
        isNextPlayer = false; //gameOverTimer.Stop();
        lblWin.GetComponentInChildren<Text>().text = "";
    }

    private void lblWin_DoubleClick(object sender, EventArgs e)
    {
        Settings.testGame = !Settings.testGame;
        //TestingGroupBox.Visible = Settings.testGame;// TODO: ??
    }


    private void waitButton_Click(object sender, EventArgs e)
    {
        isNextPlayer = !isNextPlayer;
    }

    private double RoundDown(double Amount)
    {
        double evenAmount = Amount % 1;
        Amount -= evenAmount;
        evenAmount = Amount;
        return evenAmount;
    }

    private double RoundUp(double Amount)
    {
        double fraction = Amount % 1;
        if (Amount % 1 != 0)
        {
            return RoundDown(Amount + 1);
        }
        else
        {
            return Amount;
        }
    }


    public int GetPlayerPairValue(int player)
    {
        int fc = GetCardValue(GamePlayers[player].hand.cardHand[0]);
        int sc = GetCardValue(GamePlayers[player].hand.cardHand[1]);
        if ((fc == sc))
        {
            return fc;
        }
        return 0;
    }


    public int GetNotFoldedPlayerCount()
    {
        int retval = 0;
        for (int x = 1; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x].Folded == false)
                retval++;
        }
        return retval;
    }


    public PairTypes GetPairType(int[] hand)
    {
        int highCard = 0;
        int lowCard = 15;
        int highHoldCard = GetCardValue(hand[0]);
        int lowHoldCard = 0;

        if (GetCardValue(hand[0]) == GetCardValue(hand[1]))
        {
            return PairTypes.Pocket;
        }
        if (GetCardValue(hand[1]) > highHoldCard)
        {
            lowHoldCard = highHoldCard;
            highHoldCard = GetCardValue(hand[1]);
        }

        for (int x = 2; x < 5; x++)
        {
            if (GetCardValue(hand[x]) > highCard)
            {
                highCard = GetCardValue(hand[x]);
            }
            if (GetCardValue(hand[x]) < lowCard)
            {
                lowCard = GetCardValue(hand[x]);
            }
        }
        if (highHoldCard == highCard || lowHoldCard == highCard)
        {
            return PairTypes.Top;
        }
        if (highHoldCard == lowCard || lowHoldCard == lowCard)
        {
            return PairTypes.Bottom;
        }
        return PairTypes.Middle;
    }


    public void UseTestData()
    {
        //TODO: test data ??
        string data = lblTemp.text;
        string[] dataArray = data.Split(',');
        if (dataArray.GetLength(0) != 23)
        {
            return;
        }
        else
        {
            for (int x = 0; x < 17; x++)
            {
                deck[x] = int.Parse(dataArray[x]);
            }
        }
        for (int x = 1; x < virtualPlayerCount; x++)
        {
            virtualPlayers[x] = null;
        }
        for (int x = 1; x < Settings.playerSize; x++)
        {
            if (virtualPlayers[x] == null)
            {
                virtualPlayers[x] = new VirtualPlayer();
                virtualPlayers[x] = virtualTempPlayers[int.Parse(dataArray[x + 16])];
            }
        }
        buttonPosition = 0;
        do
        {
            IncrementButtonPosition(false);
        } while (buttonPosition != int.Parse(dataArray[22]));
    }

    public void btnAutoPlayClick()//_Click(object sender, EventArgs e)
    {
        AutoPlay = !AutoPlay;
        if (AutoPlay == true)
        {
            btnAutoPlay.GetComponentInChildren<Text>().text = "Man. Play";
            // nextPlayerTimer.Interval = 10;
            Settings.tempDelay = Settings.dealDelay;
            Settings.dealDelay = 10;
            //gameOverTimer.Interval = 10;
        }
        else
        {
            btnAutoPlay.GetComponentInChildren<Text>().text = "Auto Play";
            // nextPlayerTimer.Interval = nextPlayerDelay;
            Settings.dealDelay = Settings.tempDelay;
            //gameOverTimer.Interval = Settings.intervalGameOver;// 1000;
        }
    }

    private int GetPotRaisePercentage(double raise)
    {
        if (raise > 0)
        {
            return (int)(1 / (PotAmount / raise) * 100);
        }
        return 0;
    }

    public bool isAnteBet()
    {
        if (GameState == GameStates.Ante)
            return true;
        return false;
    }

    public void btnRepeatBetClick() //_Click(object sender, EventArgs e)
    {
        //TODO: check it // get last bet from input field of the PanelInitBet
        if (lastBet > 0)
        {
            betAmount = lastBet;
            btnBetNowClick();
        }
    }

    public void btnRepeatLastBetClick() {
        autoStart = true;
        StartNewGame();
    }

    public void btnCreditAddClick() //addCredit_Click_1(object sender, EventArgs e)
    {
        if (RealPlayerCredits < jurisdictionalLimit || jurisdictionalLimit == 0)
        {
            RealPlayerCredits += Settings.jurisdictionalBetLimit; //1000
        }
        PlayerCredits += jurisdictionalLimit;
        RealPlayerCredits -= jurisdictionalLimit;
        if (lastBet > 0 && lastBet <= PlayerCredits && (GameState == GameStates.Ante || GameState == GameStates.PlayerLose))
        {
            if (btnRepeatBet != null) btnRepeatBet.SetActive(true);
            btnRepeatBet.GetComponentInChildren<Text>().text = "REPEAT LAST BET OF " + String.Format("{0:C}", lastBet);
        }
        if (btnCredit != null) btnCredit.SetActive(false);
    }


    public string GetBetAndCurrency(double value)
    {
        //string.Format(CultureInfo.CurrentCulture, "{0}{1:C}", Settings.dollar, value);
        return string.Format(CultureInfo.CurrentCulture, "{0:C}", value);
    }

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }

    public void btnStartGameClick()
    {
        string betAmountString = inputBetField.text;
        string betNull = GetBetAndCurrency(Settings.betNull);

        // start assigning the betamount (getted from form input field)
        if (Settings.isDebug) Debug.Log("betAmountString: " + betAmountString);
        betAmountString = betAmountString.Replace("$", "");
        if (Settings.isDebug) Debug.Log("betAmountString: " + betAmountString);
        Double.TryParse(betAmountString, out betAmount);
        if (Settings.isDebug) Debug.Log("bet from form input: " + betAmount);
        // end assigning the betamount (getted from form input field)
        lastBet = betAmount;

        if (betAmount > 0) {
            btnStartGame.GetComponentInChildren<Text>().text = "Bet";
            if (btnRepeatBet != null) btnRepeatBet.SetActive(false);
            if (lblPanelBet != null) lblPanelBet.SetActive(false);

            panelInitBet.SetActive(false);
            panelGame.SetActive(true);
            autoStart = true;
            StartNewGame();
        }
    }

    public void btnRaiseClick()
    {
        panelInitBet.SetActive(true);
        panelGame.SetActive(false);
        btnStartGame.GetComponent<Button>().interactable = true;

        double raiseValue = 0;
        double playerCallAmount;
        betStringPtr = _raise;

        //if (DialogResult.OK == panelInitBet.ShowDialog())//RAISE 
        //{
        playerCallAmount = GetCurrentBet() - virtualPlayers[0].CurrentBetAmount;
        if (betAmount == 0 || (playerCallAmount + betAmount) > PlayerCredits)
            return;
        raiseValue = betAmount;//the value the player entered

        //PotAmount += playerCallAmount + raiseValue;
        playerCurrentBet += playerCallAmount + raiseValue;

        PlayerCredits -= (raiseValue + playerCallAmount);
        Settings.creditsPlayed += (raiseValue + playerCallAmount);
        virtualPlayers[0].RoundRaiseAmount += raiseValue;
        PlayerBet += playerCallAmount;
        PlayerRaise += raiseValue;//update the players raise status label
        virtualPlayerRaised = 0;
        ThisRoundRaisePercentage += GetPotRaisePercentage(raiseValue);//(int)(100 / (PotAmount / raiseValue));
        //}
        BetPlayer(CurrentBetPosition);
    }

    public void btnBetNowClick()
    {
        panelInitBet.SetActive(true);
        btnStartGame.GetComponentInChildren<Text>().text = "Start Game";
        StartNewGame();
    }

    public void btnMaxBetClick()
    {
        Settings.betCurrent = Settings.betMax;
        inputBetField.text = GetBetAndCurrency(Settings.betCurrent);
    }

    public void btnMinBetClick()
    {
        Settings.betCurrent += Settings.betDx;
        if (Settings.betCurrent > Settings.betMax)
            Settings.betCurrent = Settings.betMax;
        inputBetField.text = GetBetAndCurrency(Settings.betCurrent);
    }

    public void btnClearBetClick()
    {
        Settings.betCurrent = Settings.betNull;
        inputBetField.text = GetBetAndCurrency(Settings.betCurrent);
    }

    void UpdateInterval()
    {
        if (isNextPlayer)
        {
            NextPlayer();
        }
        else
        {
            //TODO:
            isNextPlayer = false; // nextPlayerTimer.Stop();
            //if (MessageBox.Show("Next Player Wait", "Waiting", MessageBoxButtons.OK) == DialogResult.OK)
            //{
            //    NextPlayer();
            //}
        }
    }

    // Init game
    public void OtherInits() {
        //InitializeComponent();
        now = DateTime.Now;
        year = now.Year;

        //TODO:Image instructionBitmap = new Image(bonusPokerPanel.BackgroundImage);
        //TODO:pixelColor = instructionBitmap.GetPixel(100, 100);

        //dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);

        //gameOverTimer.Interval = Settings.intervalGameOver;// 1000;
        //gameOverTimer.Tick += new EventHandler(gameOverTimer_Tick);

        //TODO://chipBoxes = new Image[] { chipBox1, chipBox2, chipBox3, chipBox4, chipBox5 };

        //nextPlayerTimer.Interval = 100;
        //nextPlayerTimer.Tick += new EventHandler(nextPlayerTimer_Tick);

        
        string iniFile = Settings.pathToAssetRes + Settings.iniFile;
        string logFile = Settings.pathToAssetRes + Settings.logFile;
        string dataFile = Settings.pathToAssetRes + Settings.datFile;

        if (IniFileHandler.isExists(iniFile))
        {
            IniFileHandler.PrepareIniFile(iniFile);

            int charsTransferred = 0;
            Settings.testGame = IniFileHandler.GetIniBool("Game Parameters", "Test Game", false, iniFile);
            Settings.logging = IniFileHandler.GetIniBool("Game Parameters", "Logging", false, iniFile);
            //TODO: TestingGroupBox.Visible = Settings.testGame;
            Settings.videoBonusWinOnly = IniFileHandler.GetIniBool("Game Parameters", "Pay Video Bonus on Win Only", false, iniFile);
            Settings.surrenderReturnRank = IniFileHandler.GetIniInt("Game Parameters", "Surrender Return Rank", 100, iniFile);
            Settings.PlayerRaiseFoldThreshold = double.Parse(IniFileHandler.GetIniString("Game Parameters", "Minimum Player Raise Threshold", "3.6", out charsTransferred, iniFile));
            Settings.surrenderMinimumPair = IniFileHandler.GetIniInt("Game Parameters", "Surrender Minimum Pair", 4, iniFile);
            Settings.highCardThreshhold = IniFileHandler.GetIniInt("Game Parameters", "High Card Threshold", 4, iniFile);
            Settings.dealDelay = Settings.tempDelay = IniFileHandler.GetIniInt("Game Parameters", "DealDelay", 250, iniFile);
            // nextPlayerTimer.Interval = nextPlayerDelay = IniFileHandler.GetIniInt("Game Parameters", "Next Player Delay", 100, iniFile);
            Settings.virtualPlayerRaiseLimit = IniFileHandler.GetIniInt("Game Parameters", "Virtual Player Raise Limit", 1, iniFile);
            Settings.gameEnable = IniFileHandler.GetIniBool("Game Parameters", "Auto Start Button", false, iniFile);
            Settings.gameDenomination = (double)IniFileHandler.GetIniInt("Game Parameters", "Game Denomination", 25, iniFile);
            Settings.gameDenomination /= Settings.GameDenominationDivider;
            Settings.gameDenomMultiplier = IniFileHandler.GetIniInt("Game Parameters", "Bet Limit Multiplier", 5, iniFile);
            Settings.raiseLimitMultiplier = IniFileHandler.GetIniInt("Game Parameters", "Raise Limit Multiplier", 5, iniFile);

            if (Settings.logging)
            {
                Assets.Scripts.Logger.GetLogFileVars();
            }

            if (Settings.gameDenomMultiplier < Settings.gameMaxDenomMultiplier)
            {
                if (btnAllIn != null) btnAllIn.SetActive(false);
                betLimit = Settings.gameDenomination * Settings.gameDenomMultiplier;
            }

            payTable.paytableEntries = IniFileHandler.GetIniInt("Video Poker Paytable", "Entries", 8, iniFile);
            for (int x = 0; x < 9; x++)
            {
                payTable.PayTableAmounts[x] = IniFileHandler.GetIniInt("Video Poker Paytable", PayTableStrings[x], payTable.PayTableAmounts[x], iniFile);
            }

            foldString = IniFileHandler.GetIniString("Dynamic Help", "FOLD", "FOLD", out charsTransferred, iniFile);
            checkString = IniFileHandler.GetIniString("Dynamic Help", "CHECK", "CHECK", out charsTransferred, iniFile);
            callString = IniFileHandler.GetIniString("Dynamic Help", "CALL", "CALL", out charsTransferred, iniFile);
            raiseString = IniFileHandler.GetIniString("Dynamic Help", "RAISE", "RAISE", out charsTransferred, iniFile);
            allInString = IniFileHandler.GetIniString("Dynamic Help", "ALL IN", "ALL IN", out charsTransferred, iniFile);
            surrenderString = IniFileHandler.GetIniString("Dynamic Help", "SURRENDER", "SURRENDER", out charsTransferred, iniFile);
            continueString = IniFileHandler.GetIniString("Dynamic Help", "CONTINUE", "CONTINUE", out charsTransferred, iniFile);
            surrenderBoxString = IniFileHandler.GetIniString("Dynamic Help", "SURRENDER BOX", "SURRENDER BOX", out charsTransferred, iniFile);
            realPlayerName = IniFileHandler.GetIniString("Game Parameters", "Player Name", "PLAYER", out charsTransferred, iniFile);
            jurisdictionalLimit = (double)IniFileHandler.GetIniInt("Game Parameters", "Jurisdictional Bet Limit", Settings.jurisdictionalBetLimit, iniFile);


            for (int x = 1; x < 11; x++)
            {
                string instString = "Instruction " + x.ToString();
                /*TODO: instrucionStrings[x] = IniFileHandler.GetIniString("Instructions", instString, "", out charsTransferred, iniFile);
                if (instrucionStrings[x].Length == 0)
                    break;*/
                //TODO: make table using GridLayoutGroup dataGridView;
                /*dataGridView1.Rows.Add();
                dataGridView1.Rows[x - 1].Cells[0].Value = x.ToString();
                dataGridView1.Rows[x - 1].Cells[1].Value = instrucionStrings[x];
                dataGridView1.Rows[x - 1].Height = 45;
                */
            }
        }
        else //ini file is not exists
        {

        }

        payTable.BuildVideoBonusPaytable();

        /*creditLimitWindow = new AmountWindow("PLAY CREDITS", 163, 420);
        if (jurisdictionalLimit == 0)
        {
            creditLimitWindow.SetActive(false);
        }

        this.Controls.Add(creditLimitWindow);
        */
        PlayerCredits = Settings.playerCredits;
        startGameOverTimer(false);

        if (IniFileHandler.isExists(iniFile))
            BuildVirtualPlayerProfilesFromIniFile();
        else
            BuildVirtualPlayerProfiles();

        IncrementButtonPosition(false);
        GameState = GameStates.Ante;
        restoreCardDefaults(true);

        DisableBettingButtons();
        payTable.SetPaytableSelectedColumn(9);
        videoPokerLowRank = payTable.AdjustWinRank(ROYAL_FLUSH - (payTable.paytableEntries - 1));
        videoPokerLowRank = adjustedRanks[payTable.GetEntriesCount() - 1];
    }

    public void Start()
    {
        if (Settings.isDebug) Debug.Log("Start()");

        InvokeRepeating("UpdateInterval", Settings.updateInterval, Settings.updateInterval);

        panelInitBet = GameObject.Find("PanelInitBet"); //GameObject.FindGameObjectWithTag("PanelInitBet");
        inputBetField = GameObject.Find("InputBetField").GetComponentInChildren<InputField>();

        lblWinInfo = GameObject.Find("lblWinInfo");
        lblPanelBet = GameObject.Find("lblPanelBet");

        // player game panel
        btnCheck = GameObject.Find("btnCheck");
        btnCall = GameObject.Find("btnCall");
        btnRaise = GameObject.Find("btnRaise");
        btnFold = GameObject.Find("btnFold");
        btnSurrender = GameObject.Find("btnSurrender");
        playerAllCredits = GameObject.Find("playerAllCredits");

        panelGame = GameObject.Find("PanelGame");
        panelGame.SetActive(false);

        // bet panel
        btnRepeatBet = GameObject.Find("btnRepeatBet");
        btnStartGame = GameObject.Find("btnStartGame");

        //left panel
        btnBetNow = GameObject.Find("btnBetNow");
        btnRepeatLastBet = GameObject.Find("btnRepeatLastBet");
        btnRepeatLastBet.SetActive(false);

        btnLSurrender = GameObject.Find("btnLSurrender");
        btnLSurrender.SetActive(false);

        //XYZ panel
        lblTemp = GameObject.Find("lblTemp").GetComponent<Text>();
        panelXYZ = GameObject.Find("PanelXYZ");
        panelInitBet = GameObject.Find("PanelInitBet"); //.Find("panelInitBet");//TODO: check: is panelInitBet and PanelInitBet the same?
        btnCredit = GameObject.Find("btnCredit");
        btnRepeatBet = GameObject.Find("btnRepeatBet");
        btnAutoPlay = GameObject.Find("btnAutoPlay");
        btnNewGame = GameObject.Find("btnNewGame");
        btnAllIn = GameObject.Find("btnAllIn");
        lblPot = GameObject.Find("lblPot");
        lblRaise = GameObject.Find("lblRaise");
        lblBet = GameObject.Find("lblBet");

        lblCall = GameObject.Find("lblCall");
        lblWin = GameObject.Find("lblWin");
        lblBettingGroup = GameObject.Find("lblBettingGroup");

        panelInitBet.SetActive(false);

        //TODO:
        //panel surrender
        panelSurrender = GameObject.Find("PanelSurrender");
        lblSurrender = GameObject.Find("lblSurrender");
        panelSurrender.SetActive(false);

        //cards
        playerhold1 = GameObject.Find("playerhold1");
        playerhold2 = GameObject.Find("playerhold2");
        player1hold1 = GameObject.Find("player1hold1");
        player1hold2 = GameObject.Find("player1hold2");
        player2hold1 = GameObject.Find("player2hold1");
        player2hold2 = GameObject.Find("player2hold2");
        player3hold1 = GameObject.Find("player3hold1");
        player3hold2 = GameObject.Find("player3hold2");
        player4hold1 = GameObject.Find("player4hold1");
        player4hold2 = GameObject.Find("player4hold2");
        player5hold1 = GameObject.Find("player5hold1");
        player5hold2 = GameObject.Find("player5hold2");

        List<GameObject> cardsOfPlayerGameObjects = new List<GameObject>()
        {
            playerhold1, playerhold2, player1hold1, player1hold2, player2hold1, player2hold2,
            player3hold1, player3hold2, player4hold1, player4hold2, player5hold1, player5hold2
        };

        cardsOfPlayer = new List<Image>();
        foreach (var obj in cardsOfPlayerGameObjects)
        {
            cardsOfPlayer.Add(obj.GetComponent<Image>());
        }

        // init cards with images/sprites
        cardsAll = new List<Sprite>();
        List<string> masti = new List<string>() { "spades", "dia", "clubs", "hearts" };
        string separator = "_";
        string path = "";
        Sprite cardSprite;
        List<string> cards = new List<string>() { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        foreach (string card in cards)
            foreach (string mast in masti)
            {
                path = card + separator + mast;
                cardSprite = Resources.Load(path, typeof(Sprite)) as Sprite;
                cardsAll.Add(cardSprite);
            }
        cardBg = Resources.Load("card_background", typeof(Sprite)) as Sprite;
        cardBack = Resources.Load("card_back", typeof(Sprite)) as Sprite;
        cardsAll.Add(cardBack);
        cardsAll.Add(cardBg); //card with the background color

        //cards flop, turn, river
        cardsPublic = new List<Image>()
        {
            GameObject.Find("flop1").GetComponent<Image>(),
            GameObject.Find("flop2").GetComponent<Image>(),
            GameObject.Find("flop3").GetComponent<Image>(),
            GameObject.Find("turn").GetComponent<Image>(),
            GameObject.Find("river").GetComponent<Image>(),
        };

        //init labels for credits and bets of each player
        betLabels = new List<GameObject>() {
            GameObject.Find("lblBetPlayer0"),
            GameObject.Find("lblBetPlayer1"),
            GameObject.Find("lblBetPlayer2"),
            GameObject.Find("lblBetPlayer3"),
            GameObject.Find("lblBetPlayer4"),
            GameObject.Find("lblBetPlayer5")
        };
        creditLabels = new List<GameObject>() {
            GameObject.Find("lblCreditPlayer0"),
            GameObject.Find("lblCreditPlayer1"),
            GameObject.Find("lblCreditPlayer2"),
            GameObject.Find("lblCreditPlayer3"),
            GameObject.Find("lblCreditPlayer4"),
            GameObject.Find("lblCreditPlayer5")
        };
        OtherInits();
    }

    GameObject panelInitBet, panelGame, panelSurrender, panelXYZ; //, bonusPokerPanel;
    GameObject btnCheck, btnCall, btnRaise, btnFold, btnSurrender, btnStartGame, lblPanelBet, lblPanelBetText; // panelInitBet
    GameObject btnBetNow, btnLSurrender, btnRepeatLastBet, playerAllCredits; // left panel (start/restart the game)
    GameObject btnCredit, btnRepeatBet, btnAutoPlay, btnNewGame,
        btnAllIn,
        lblPot, lblRaise, lblBet, lblCall, lblWin,
        lblBettingGroup;
    List<GameObject> betLabels, creditLabels; // for each player
    GameObject txtSurrender, lblSurrender, lblWinInfo;//panel surrender
    GameObject playerhold1, playerhold2, player1hold1, player1hold2, player2hold1, player2hold2, player3hold1, player3hold2, player4hold1, player4hold2, player5hold1, player5hold2;
    double betAmount;
    double dollarAmount;
    InputField inputBetField;
    List<Sprite> cardsAll;
    List<Image> cardsOfPlayer, cardsPublic;
    Sprite cardBg; // background of the desk
    Sprite cardBack; // back card side
    // panel XYZ
    Text lblTemp; // textBox2 for testing
}