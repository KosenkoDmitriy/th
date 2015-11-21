public class CardHand
{
    public int HandRank;//0=Royal,1=Straight,etc 
    public int CardValueTotal;//the total value of cards included in the hand .. tie breaker
    public int Kicker;
    public int HighCard;
    public int XofaKindValue;
    public int XofaKindKicker;
    public int TwoPairSecondValue;
    //public Form1.cardValues[] FHcards = new Form1.cardValues[2];//Full house card values
    public int[] cardHand = new int[7];//the complete set of cards available
}

public class GamePlayer
{
    //public VirtualPlayer vPlayer = new VirtualPlayer();
    public CardHand hand = new CardHand();
    public int[] winCards = new int[5];
    public int RoundRaiseCount;//how many time this player has raised this round 
}

public class RaiseLevel
{
    public int[] RaiseHands;// = new int[40];
    public double[] Range = new double[2];
    public int RaisePercentage;
    public double[] ReRaiseRange = new double[2];
    public int ReRaisePercentage;

}
public class FoldLevel
{
    public int[] FoldHands;// = new int[40];
    public double[] Range = new double[2];
}

public class VirtualPlayer
{
    public string Name = new string(' ', Settings.playerNameSize);
    public int playerNumber;
    public bool FoldOnAnyRaise;
    public RaiseLevel[] RaiseLevels = new RaiseLevel[6];
    public FoldLevel[] FoldLevels = new FoldLevel[8];
    public int HoleMinThreshold;                            //the lowest rank for play anything lower folds
    public int[] BluffHands;// = new int[40];                  //the hands that we bluff with
    public int[] SlowPlayHands;// = new int[40];               //hands that are slow played  

    public int[] AllInHands;// = new int[40];               //we go all in after any raise. 
    public int BluffPercentage;
    public int LimpPercentage;
    public int BluffCallRaisePercentage;

    public int[] FlopNoRaiseBetPercentages;
    public int[] TurnNoRaiseBetPercentages;
    public int[] RiverNoRaiseBetPercentages;

    public int MinimumFlopThreshold;
    public int MinimumTurnThreshold;
    public int MiniumuRiverThreshold;

    public double Credits;
    public double TwoCardBet;
    public double FlopBet;
    public double TurnBet;
    public double RiverBet;
    public double CurrentBetAmount;
    public double LastRoundBet;
    public double Ante;
    public double RoundCallAmount;
    public double RoundRaiseAmount;
    public int RoundRaiseCount;
    public bool Folded;
    public int FiveCardHandRank;
    public int HighCard;
    public int PocketPair;
    public bool Bluffing;

    public bool LimpIn;
    public bool RoundChecked;
    public int FinalHandRank;
    public bool AllIn = false;
}