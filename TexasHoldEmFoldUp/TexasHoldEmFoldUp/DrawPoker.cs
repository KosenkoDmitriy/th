using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Media;

namespace Poker
{
    public enum gameState
    {
        Idle ,
        Bet,
        Deal,
        Draw,
        Evaluate,
        Pay,
        Disabled
    }

//public enum gameState
//{
//    Idle = 0,
//    Bet = 1,
//    Deal = 2,
//    Draw = 3,
//    Evaluate = 4,
//    Pay = 5,
//    Disabled = 6
//}


    public partial class Form1 : Form
    {
        SoundPlayer GameSound = new SoundPlayer();
        Label[] HoldLabel = new Label[5];//create an array of labels
        Label[] PayTypeLabel = new Label[9];
        Label[,] PayLabel = new Label[5,9];
        int cardY = 350;
        int Spade = 1;
        int Club = 2;
        int Heart = 3;
        int Diamond = 4;
        int CardBack = 5;
        int J = 11;
        int Q = 12;
        int K = 13;
        int A = 14;
        //int Down = 0;
        //int Up = 1;
        gameState GameState = gameState.Idle;
        int CurrentBet = 1;
        int LastState = 255;
        int CurrentDenom = 0;
        int[] denoms = new int[] { 1, 5, 10, 25, 100 };

        
        public const int WM_USER = 0x0400;
        public const int WM_APP = 0x8000;
        public const int WM_GAME_CREDITS = WM_USER + 122;
        int[] FaceX = new int[]{  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,333,415,497,  9}; //266 497
        int[] FaceY = new int[]{190,190,190,190,190,190,190,190,190,190,190, 94, 94, 94,190};
        int[] FinalHand = new int[10];
        public static int[] Pick = new int[10];
        public static bool[] HeldState = new bool[5]{false,false,false,false,false};
        string[] PayTableStrings = new string[9]{"ROYAL FLUSH","STRAIGHT FLUSH","FOUR OF A KIND","FULL HOUSE",
                            "FLUSH","STRAIGHT","THREE OF A KIND","TWO PAIR","JACKS OR BETTER"};
        string[] PayStrings = new string[10]{"               ","JACKS OR BETTER","    TWO PAIR   ","THREE OF A KIND","   STRAIGHT    ",
                            "     FLUSH     ","  FULL HOUSE   "," FOUR OF A KIND ","STRAIGHT FLUSH ","  ROYAL FLUSH  "};
        int[] PayTable = new int[10]{0,1,2,3,4,6,9,25,50,250};
        int[] BonusTable = new int[10]{0,1,2,3,4,6,9,25,50,800};
        int[] PayTableLefts = new int[5] { 244, 331, 417, 504, 591 };
        int[] PayTableWidths = new int[5] { 83, 83, 83, 83, 101 };

        public static long CurrentGameCredits;
        public static long LastGameCredits = 0;
        public static bool CreditAmountReady = false;
        public static int ButtonPressed = 0xFF;
        public static string DoorOpenText;
        public static string DoorClosedText;
        
        //static Message message;
        Bitmap cardBmp;
        Bitmap fontBmp;

        string SoundDir = "C:\\SAWork\\MPSystem\\image\\MPGPOK\\SOUNDS\\EFFECTS";
            

        public Form1()
        {
            InitializeComponent();
            GameMessageFilter GMF = new GameMessageFilter();
            Application.AddMessageFilter(GMF);
            Process proc = Process.GetCurrentProcess();
            Application.Idle += new EventHandler(Application_Idle);
            MouseDown += new MouseEventHandler(Form1_MouseDown);
            cardBmp = new Bitmap("C:\\SAWork\\MPSystem\\src\\SAGames\\Poker\\Poker\\bitmaps\\cardimg8.bmp");   //create a new bitmap with the image in card resource
            fontBmp = new Bitmap("C:\\SAWork\\MPSystem\\src\\SAGames\\Poker\\Poker\\bitmaps\\chars.bmp");   //create a new bitmap with the image in card resource
            DealButton.BackgroundImage = imageList1.Images[0];
            DealButton.EnabledChanged += new EventHandler(DealButton_EnabledChanged);
            Initialize();
            
        }

        void DealButton_EnabledChanged(object sender, EventArgs e)
        {
            int enabled = 0;
            if(DealButton.Enabled == false)
                enabled = 1;
            DealButton.BackgroundImage = imageList1.Images[enabled];
        }

        public class Win32Support
        {
            public enum Bool
            {
                False = 0,
                True
            };
            public enum TRO //TernaryRasterOperations
            {
                SRCCOPY = 0x00CC0020,// dest = source             
                SRCPAINT = 0x00EE0086,// dest = source OR dest     
                SRCAND = 0x008800C6,// dest = source AND dest    
                SRCINVERT = 0x00660046,// dest = source XOR dest        
                SRCERASE = 0x00440328,// dest = source AND (NOT dest )  
                NOTSRCCOPY = 0x00330008,// dest = (NOT source)          
                NOTSRCERASE = 0x001100A6,// dest = (NOT src) AND (NOT dest) 
                MERGECOPY = 0x00C000CA,// dest = (source AND pattern)     
                MERGEPAINT = 0x00BB0226,// dest = (NOT source) OR dest     
                PATCOPY = 0x00F00021,// dest = pattern                  
                PATPAINT = 0x00FB0A09,// dest = DPSnoo                   
                PATINVERT = 0x005A0049,// dest = pattern XOR dest         
                DSTINVERT = 0x00550009,// dest = (NOT dest)               
                BLACKNESS = 0x00000042,// dest = BLACK                    
                WHITENESS = 0x00FF0062,// dest = WHITE                    
            };
            //The API is your friend
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]//used to send messages through the API
            public static extern int SendMessage(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);//use the API SendMessage routine
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]//used to send messages through the API
            public static extern int PostMessage(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);//use the API SendMessage routine
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TRO dwRop);
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern Bool DeleteDC(IntPtr hdc);
            [DllImport("gdi32.dll", ExactSpelling = true)]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern Bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hObject, int width, int height);
        }
        public class GameMessageFilter : IMessageFilter
        {
            public static uint WM_USER = 0x0400;
            public static uint LOCAL_BASE = 5363;
            public static uint GAME_MSG = WM_USER + LOCAL_BASE;

            public bool PreFilterMessage(ref Message gameMessage)
            {
                if (gameMessage.Msg >= 0x8000)
                {
                    //MsgBox.Text = "Got an app Message from the Game";
                    //SendToBack();
                }
                return false;

            }
        }
        //*******************************************************GAME CODE******************************************************
        public string DoorOpen
        {
            set
            {
                doorOpenlabel.Text = DoorOpenText;
            }
        }

        void OnStateChange(int GameState)
        {
            doorOpenlabel.Text = DoorOpenText;
            doorClosedlabel.Text = DoorOpenText;

        }
         
        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int zone = 0;
            if (GameState == gameState.Draw)
            {
                if (e.Y > cardY && e.Y < (cardY + 150))
                {
                    zone = (e.X - 200) / 130;

                    if (e.X > 200 && e.X < 310)//fix all this stuff to do an array of label structs
                        zone = 0;
                    if (e.X > 330 && e.X < 440)
                        zone = 1;
                    if (e.X > 460 && e.X < 570)
                        zone = 2;
                    if (e.X > 590 && e.X < 700)
                        zone = 3;
                    if (e.X > 720 && e.X < 830)
                        zone = 4;
                        
                    if (HeldState[zone] == false)
                        HoldLabel[zone].Text = "HELD";
                    else
                        HoldLabel[zone].Text = "";
                    HeldState[zone] = !HeldState[zone];
                    GameSound.SoundLocation = SoundDir + "\\hold.wav";
                    GameSound.Play();
                }
            }
            if ((e.X > 1300) && (e.Y < 100))
            {
                Application.Exit();
            }
        }

        private void Initialize()
        {
            int xpos = 217;
            int ypos = 520;// 470;
            int x,y;
            Win32Support.PostMessage(PokerClass.baseHwnd, WM_GAME_CREDITS, IntPtr.Zero, IntPtr.Zero);//request the current credits. 
            for (x = 0; x < 5; x++)
            {
                HoldLabel[x] = new System.Windows.Forms.Label();//actually build the label
                HoldLabel[x].BackColor = System.Drawing.Color.Transparent;
                HoldLabel[x].Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                HoldLabel[x].ForeColor = System.Drawing.Color.White;
                HoldLabel[x].Location = new System.Drawing.Point(xpos + (x * 130), ypos - 200);
                HoldLabel[x].Size = new System.Drawing.Size(100, 24);
                HoldLabel[x].Text = "";
                HoldLabel[x].Visible = true;
                this.Controls.Add(HoldLabel[x]);
            }   
             for( x =0;x<9;x++)//build the Paytable titles
            {
                PayTypeLabel[x] = new Label();
                PayTypeLabel[x].Parent = PaytableBox;
                PayTypeLabel[x].BackColor = Color.Transparent;
                PayTypeLabel[x].Font = new Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                PayTypeLabel[x].TextAlign = ContentAlignment.MiddleLeft;
                PayTypeLabel[x].ForeColor = Color.Yellow;
                PayTypeLabel[x].Location =  new System.Drawing.Point(10, 6 + (x * 25));     
                PayTypeLabel[x].Size = new System.Drawing.Size(230, 26);
                PayTypeLabel[x].Text = PayTableStrings[x];
                PayTypeLabel[x].Visible = true;
                PaytableBox.Controls.Add(PayTypeLabel[x]); 
            }           
            for (x = 0;x<5;x++)//build the paytable amounts
            {
                for(y = 0;y<9;y++)
                {
                    PayLabel[x,y] = new Label();
                    PayLabel[x,y].BackColor = Color.Transparent;
                    PayLabel[x,y].Font = new Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                    PayLabel[x,y].ForeColor = Color.Yellow;
                    PayLabel[x, y].TextAlign = ContentAlignment.MiddleRight;
                    PayLabel[x,y].Location =  new System.Drawing.Point(PayTableLefts[x], 6 + y * 25);
                    PayLabel[x,y].Size = new System.Drawing.Size(PayTableWidths[x], 26);
                    if (x < 4)
                    {
                        PayLabel[x, y].Text = (PayTable[10 - (y+1)]*(x+1)).ToString();//get the proper value
                    }
                    else
                    {
                        PayLabel[x, y].Text = (BonusTable[10 - (y+1)]*(x+1)).ToString();
                    }
                    
                         
                    PayLabel[x,y].Visible = true;
                    PaytableBox.Controls.Add(PayLabel[x,y]); 
                }
            }
            DrawCardBacks(false);
            RefreshPaytable(CurrentBet-1);
            SetGameState(gameState.Idle);
            denomBox.Image = imageList2.Images[CurrentDenom];
        }
        
        public void DisplayCurrentCredits()
        {
            BlankText(720, 550, 20);
            string creditString = "CREDITS " + (CurrentGameCredits / denoms[CurrentDenom]).ToString();
            BigTextOut(720, 550, creditString);

        }

        public void DisplayWinMeter(int win)
        {
            BlankText(220, 550, 20);
            string winString;
            if (win > 0)
            {
                winString = "WIN " + win.ToString();
                GameSound.SoundLocation = SoundDir + "\\coins3.wav";
                GameSound.Play();
            }
            else
                winString = "         ";
            BigTextOut(220, 550, winString);
        }
        
        public void BlankText(int x, int y, int chars)
        {
            string blank= " ";
            for( int i = 0; i < chars; i++)
                blank += " ";
            BigTextOut(x, y, blank);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            if (DoorOpenText != null)
            {
                //if(DoorOpenText.Length > 1)
                //    SetGameState((int)gameState.Disabled);
            }
            if (CurrentGameCredits < (CurrentBet * denoms[CurrentDenom]) )
            {
                DealButton.Enabled = false;
            }
            else
            {
               // if(GameState == (int)gameState.Idle)
                   // DealButton.Enabled = true;
            }
            if (PokerClass.ranReceived < PokerClass.ranRequested)
            {
                if (PokerClass.randomRequested == false)
                {
                    RequestRandomNumber(51);
                    PokerClass.randomRequested = true;
                }
            }
            else
            if (PokerClass.ranRequested > 0)
            {
                PokerClass.ranRequested = 0;
                BuildPokerHand();
                SetGameState(gameState.Draw);
            }
            if (CurrentGameCredits != LastGameCredits)
            {
                int enable = 0;
                if (CurrentGameCredits > 0)
                    enable = 1;
                DisplayCurrentCredits();//always update the display
                LastGameCredits = CurrentGameCredits;
                if (CurrentGameCredits > 0)
                    if (GameState == gameState.Idle)
                        DealButton.Enabled = true;

                Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 126, (IntPtr)0, (IntPtr)enable);//tell the base to light up the Cashout button.  

            }
            if (ButtonPressed != 0xFF)
            {
                switch (ButtonPressed)
                {
                    case 8:
                        {
                            if(DealButton.Enabled)
                                DealButton_Click(sender, e);
                        }
                        break;
                }
                ButtonPressed = 0xFF;
            }
            doorOpenlabel.Text = DoorOpenText;
            doorClosedlabel.Text = DoorClosedText;
        }

        public void ClearHoldLabels()
        {
            for (int x = 0; x < 5; x++)
                HoldLabel[x].Text = "";
        }
       
        public void BuildPokerHand()
        {
            if (GameState == gameState.Idle)
            {
                ClearHoldLabels();
                for (int x = 0; x < 5; x++)
                {
                    FinalHand[x] = Pick[x];
                    HeldState[x] = false;
                }
 
            }
            if (GameState == gameState.Draw)
            {
                int drawPointer = 5;
                for (int x = 0; x < 5; x++)
                {
                    if (HeldState[x] == false)
                    {
                        FinalHand[x] = Pick[drawPointer];
                        drawPointer++;
                    }
                }
            }
            BuildFinalHand(true);
            if (GameState == gameState.Draw)
            {
                betOneButton.Visible = true;
                Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 126, (IntPtr)8, (IntPtr)1);//tell the base to light up the deal button.  
            }
            int winPtr = EvaluatePokerHand();
            if (winPtr > 0)
            {
                PayTypeLabel[9 - winPtr].ForeColor = System.Drawing.Color.White;
                for (int i = 0; i < 5; i++)
                    PayLabel[i, 9 - winPtr].ForeColor = System.Drawing.Color.White;
            }
        }

        public void BuildFinalHand(bool delay)
        {
            int xpos = 200;
            int ypos = cardY;
            int offset = 130;
            DealButton.Enabled = false;
            
            GameSound.SoundLocation = SoundDir+"\\Draw1.wav";
            for (int x = 0; x < 5; x++)
            {
                BuildIGTCard(GetCardValue(FinalHand[x]), GetCardSuit(FinalHand[x]), xpos + (offset * x), ypos);
                GameSound.Play();
                if (delay)
                {
                    Thread.Sleep(150);
                    //Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 125, IntPtr.Zero, IntPtr.Zero);//play a click sound. 
                    
                    
                }
            }
            DisplayCurrentCredits();
            DealButton.Enabled = true;
        }

        public void DrawCardBacks(bool held)
        {
            int xpos = 200;
            int ypos = cardY;
            int offset = 130;
            for (int x = 0; x < 5; x++)
            {
                if (held)
                {
                    if (HeldState[x] == false)
                    {
                        BuildIGTCard(2, CardBack, xpos + (offset * x), ypos);
                    }
                }
                else
                    BuildIGTCard(2, CardBack, xpos + (offset * x), ypos);
            }
        }

        private void RefreshPaytable(int position)
        {
            int x, y;
            for (x = 0; x < 5; x++)
            {
                for (y = 0; y < 9; y++)
                {
                    if (position == x)
                        PayLabel[x, y].BackColor = Color.Red;
                    else
                        PayLabel[x, y].BackColor = Color.Transparent;
                }
            }

        }
        private void RestoreLabelColor()
        {
            int x,y;
            for(x = 0;x < 9;x++)
            {
                PayTypeLabel[x].ForeColor = System.Drawing.Color.Yellow;
                for(y = 0;y < 5;y++)
                {
                    PayLabel[y, x].ForeColor = System.Drawing.Color.Yellow;

                }
            }
        }

        private void DealButton_Click(object sender, EventArgs e)//working
        {
            //Win32Support.PostMessage(PokerClass.baseHwnd, WM_GAME_CREDITS, IntPtr.Zero, IntPtr.Zero);//update the current credits
            RestoreLabelColor();
            if (GameState == gameState.Idle)
            {
                betOneButton.Visible = false;
                Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 126, (IntPtr)8, IntPtr.Zero);//tell the base to light up the deal button.  
                DealButton.Enabled = false;//don't allow another press until ready
                int winPtr = 0;
                Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 123, IntPtr.Zero, (IntPtr)(CurrentBet * denoms[CurrentDenom]));//tell the base we are betting some credits.  
                DrawCardBacks(false);
                for (int x = 0; x < 10; x++)
                    Pick[x] = 0;
                GetPokerHand();
                InformationLabel.Text = "Hold 1 to 5 Cards";
                DisplayWinMeter(0);
                BigTextOut(375, 275, PayStrings[0]);//blank the pay string
                CurrentGameCredits -= CurrentBet * denoms[CurrentDenom];
                LastGameCredits = CurrentGameCredits;
                DisplayCurrentCredits();
            }
            else
            if (GameState == gameState.Draw)
            {
                int totalWin;
                int winPtr;
                DrawCardBacks(true);
                BuildPokerHand();
                SetGameState(gameState.Idle);
                InformationLabel.Text = "";
                //InformationLabel.Text = PayStrings[EvaluatePokerHand()];
                winPtr = EvaluatePokerHand();
                totalWin = PayTable[winPtr] * (CurrentBet * denoms[CurrentDenom]) ; 
                Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 124,(IntPtr)totalWin,(IntPtr)(CurrentBet * denoms[CurrentDenom]));//tell the base about the results.  
                if (totalWin > 0)
                {
                    //Win32Support.PostMessage(PokerClass.baseHwnd, WM_GAME_CREDITS, IntPtr.Zero, IntPtr.Zero);//update the current credits
                    CurrentGameCredits += totalWin;
                    LastGameCredits = CurrentGameCredits;
                    DisplayCurrentCredits();
                    BigTextOut(375, 275, PayStrings[winPtr]);//show the customer what hand they won
                }
                DisplayWinMeter(PayTable[winPtr] * CurrentBet);
            }

        
        
        }

        protected override void WndProc(ref Message msg)
        {

            switch (msg.Msg)
            {
                case WM_USER + 100:
                    {
                        this.TopMost = false;
                        SendToBack();
                        
                    } break;
                case WM_USER + 101:
                    {
                        this.TopMost = false;
                        bool test = this.Modal;
                    }
                    break;
                case WM_USER + 110:
                    {
                        
                        this.TopMost = true;
                        this.Focus(); 
                        BuildFinalHand(false);//restore the cards
                        
                    } break;
                case 0x000F://wmpaint
                    {
                       // BuildFinalHand(false);
                    } break;
            }

 
            base.WndProc(ref msg);
        }
 

  

        public void RequestRandomNumber(int range)
        {
            //message = Message.Create(PokerClass.baseHwnd, WM_USER + 121, (IntPtr)0, (IntPtr)range);
            //this.WndProc(ref message);
            Win32Support.PostMessage(PokerClass.baseHwnd,WM_USER + 121,IntPtr.Zero,(IntPtr)range);
        }
       
        

        private void GetPokerHand()
        {
            //get 10 random numbers without duplicates
            PokerClass.ranRequested = 10;
            PokerClass.ranReceived = 0;
        }

        public int GetCardValue(int card)
        {
            return (card - (((GetCardSuit(card) - 1) * 13) - 2));
        }

        public int GetCardSuit(int card)
        {
            return ((card / 13) + 1);
        }

        public int EvaluatePokerHand()
        {
            int XofaKind = 1, SaveValue = 0, CardValue, PayType = 0, HighCard = 0, MembersOfStraight = 1;
            int HighValue = 0;
            bool OneOfaKind = false;
            bool TwoOfaKind = false;
            bool ThreeOfaKind = false;
            bool Flush = true;
            bool AceFound = false;
            bool KingFound = false;
            int[] FinalSuit = new int[5];
            int[] FinalValue = new int[5];
            for (int x = 0; x < 5; x++)
            {
                FinalSuit[x] = GetCardSuit(FinalHand[x]);
                FinalValue[x] = GetCardValue(FinalHand[x]);
                if (FinalValue[x] == 13)
                    KingFound = true;
                if (FinalValue[x] == 14)
                {
                    AceFound = true;
                    FinalValue[x] = 1;
                }
            }
            int SuitSave = FinalSuit[0];
            for (int x = 1; x < 5; x++)//check for flush
                if (SuitSave != FinalSuit[x])
                    Flush = false;
            if (Flush)
                PayType = 5;

            for (int x = 0; x < 5; x++)//do preliminary check
            {
                XofaKind = 1;
                CardValue = FinalValue[x];
                for (int a = x + 1; a < 5; a++)
                {
                    if (FinalValue[a] == CardValue)
                        XofaKind++;
                }
                if (XofaKind > HighValue)
                {
                    HighValue = XofaKind;//save highest number of cards
                    SaveValue = CardValue;//save that card value
                }
            }
            switch (HighValue)
            {
                case 4:
                    return (7);//PayType=7;break; //Four of a kind
                case 3:
                    ThreeOfaKind = true;
                    PayType = 3;
                    break;
                case 2:
                    TwoOfaKind = true;
                    break;
                case 1:
                    OneOfaKind = true;
                    break;
            }//above is the basis for all hands}

            if (ThreeOfaKind || TwoOfaKind)//look for two pair or Full house
            {
                XofaKind = 1;
                for (int x = 0; x < 5; x++)//do a secondary check
                {
                    CardValue = FinalValue[x];
                    for (int a = x + 1; a < 5; a++)
                        if (CardValue != SaveValue)   //don't use orig find value
                            if (FinalValue[a] == CardValue)
                                XofaKind++;
                }
                if (XofaKind == 2)//we found another pair
                {
                    if (TwoOfaKind)
                    {
                        TwoOfaKind = false;
                        PayType = 2;
                    }
                    else//we have a full house
                        PayType = 6;
                }
                if (TwoOfaKind)
                    if (SaveValue > 10 || SaveValue == 1)
                        return (1);//PayType=1;//Jacks or Better
            }

            if (OneOfaKind)//check for straight
            {
                for (int x = 0; x < 5; x++)//let's get the high card
                {
                    if (FinalValue[x] > HighCard)
                        HighCard = FinalValue[x];
                }
                if (KingFound && AceFound)
                    for (int x = 0; x < 5; x++)
                    {
                        if (FinalValue[x] == 1)
                        {
                            FinalValue[x] = 14;
                            HighCard = 14;             //restore proper high card
                            break;
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
                    PayType = 4;
                    if (Flush)
                        PayType = 8;
                    if (Flush && AceFound)
                        if (KingFound)
                            PayType = 9;
                }
            }//end of straight check
            return (PayType);

        }

        public void BuildIGTCard(int value, int Suit, int XPos, int YPos)
        {
            Graphics formDC = this.CreateGraphics();
            IntPtr hdc = formDC.GetHdc();//now we have the hdc for the main form
            IntPtr cardHDC = Win32Support.CreateCompatibleDC(hdc);
            Win32Support.SelectObject(cardHDC, cardBmp.GetHbitmap());

            if (value == 1)
            {
                value = A;// A;
            }
            int YOffset = 0;
            if (Suit > 2)
            {
                YOffset = 42;
            }
            if (Suit == CardBack)
            {
                Win32Support.BitBlt(hdc, XPos, YPos, 112, 152, cardHDC, 139, 187, Win32Support.TRO.SRCCOPY);
            }
            else
            {
                Win32Support.BitBlt(hdc, XPos + 0, YPos + 0, 112, 152, cardHDC, 2, 188, Win32Support.TRO.SRCCOPY);//build the cardback
                Win32Support.BitBlt(hdc, XPos + 47, YPos + 2, 60, 85, cardHDC, FaceX[value], FaceY[value], Win32Support.TRO.SRCCOPY);//get the face
                Win32Support.BitBlt(hdc, XPos + 7, YPos + 5, 30, 37, cardHDC, (value - 2) * 44, YOffset, Win32Support.TRO.SRCCOPY);//get the proper value
                Win32Support.BitBlt(hdc, XPos + 5, YPos + 44, 39, 37, cardHDC, (Suit - 1) * 46, 80, Win32Support.TRO.SRCCOPY);//build the small suit
                Win32Support.BitBlt(hdc, XPos + 31, YPos + 87, 50, 53, cardHDC, (Suit - 1) * 53, 127, Win32Support.TRO.SRCCOPY);//build the big suit
            }
            formDC.ReleaseHdc(hdc);
        }


        public int GetAlphaPos(char cnv)
        {
            string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,.!>?$^%/ " ;

            for (int x = 0; x < 46; x++)
            {
                if (cnv == Alphabet[x])
                    return x;
            }
            return 0xFF;
        }       
//---------------------------------------------------------------------------
        public void BigTextOut(int x, int y, string outString)
        {

            Graphics formDC = this.CreateGraphics();
            IntPtr hdc = formDC.GetHdc();//now we have the hdc for the main form
            IntPtr fontHDC = Win32Support.CreateCompatibleDC(hdc);
            Win32Support.SelectObject(fontHDC, fontBmp.GetHbitmap());

            int Charpos,    CharWidth;
            int Xpos = 0;
            bool space;
            bool JKAdj = false;
            
            for(int i=0; i < outString.Length; i++)
            {
                JKAdj = false;
                space = false;
                CharWidth = 20;
                Charpos = GetAlphaPos(outString[i]);
                if(Charpos >= 45)
                    space = true;
                if(Charpos == 8)
                    CharWidth = 9;
                if((Charpos == 10)||(Charpos == 9))
                    JKAdj = true;
                if(Charpos > 8)
                    Charpos=(Charpos*20)-15;
                else
                    Charpos=(Charpos * 20);
                if(JKAdj)
                    Charpos += 4;
                if(space == false)
                    Win32Support.BitBlt(hdc, Xpos+x, y, CharWidth, 25, fontHDC, (Charpos)+ 8, 2,Win32Support.TRO.SRCCOPY);
                else
                    Win32Support.BitBlt(hdc, Xpos+x, y, CharWidth, 25, hdc, 0,0,Win32Support.TRO.SRCCOPY);
                Xpos+=CharWidth; 
            }  
        }

        public void SetGameState(gameState state)
        {
            GameState = state;
            CheckGameState();    
        }

        public void CheckGameState()
        {
            	int GP_IDLE  	= 0;
	            int GP_IN_PLAY  = 1;
                int GP_FINISHED	= 3;
                int GP_WINNER	= 4;

            int enable = 0;
            int state = 0;
            int DealEnable = 0;
            switch (GameState)
            {
                case gameState.Idle:
                    DealButton.Enabled = true;
                    DealButton.Text = "DEAL";
                    betOneButton.Visible = true;
                    if(CurrentGameCredits > 0)
                        enable = 1;
                    state = GP_IDLE;
                    break;
                case gameState.Deal:
                    state = GP_IN_PLAY;
                    break;
                case gameState.Draw:
                    DealButton.Text = "DRAW";
                    betOneButton.Visible = false;
                    state = GP_IN_PLAY;
                    break;
                case gameState.Evaluate:
                    state = GP_IN_PLAY;
                    break;
                case gameState.Pay:
                    state = GP_IN_PLAY;
                    break;
                case gameState.Disabled:
                    DealButton.Enabled = false;
                    DealButton.Visible = false;
                    betOneButton.Visible = false;
                    state = GP_IN_PLAY;
                    break;
            }
            if (DealButton.Enabled)
                DealEnable = 1;
            Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 126, (IntPtr)8, (IntPtr)DealEnable);//tell the base to light up the deal button.  
            Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 126, (IntPtr)0, (IntPtr)enable);//tell the base to handle the Cashout button.  
            if(LastState != state)
                Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 127, (IntPtr)0, (IntPtr)state);//tell the base our GameState. 
            DisplayCurrentCredits();//keep the credit value diaplayed
            LastState = state;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 122, IntPtr.Zero, IntPtr.Zero);//request the current credits. 
            timer1.Enabled = false;
            DrawCardBacks(false);
        }

        private void betOneButton_Click(object sender, EventArgs e)
        {
            CurrentBet++;
            if (CurrentBet > 5)
                CurrentBet = 1;
            RefreshPaytable(CurrentBet-1);
            GameSound.SoundLocation = SoundDir+"\\BUTTON.WAV";
            GameSound.Play();
        }

        private void denomBox_Click(object sender, EventArgs e)
        {
            if (GameState != gameState.Idle)
                return;//no changing demoms while not in idle
            CurrentDenom++;
            if (CurrentDenom > 4)
                CurrentDenom = 0;
            denomBox.Image = imageList2.Images[CurrentDenom]; ;
            DisplayCurrentCredits();
            Win32Support.PostMessage(PokerClass.baseHwnd, WM_USER + 128, (IntPtr)0, (IntPtr)(denoms[CurrentDenom]));//tell the base to light up the Cashout button. 

        }

               
        
    }
    
    
   
}