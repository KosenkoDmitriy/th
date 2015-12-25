using System;

public class GameTest {

    public IGameState GameState { get; set; }
	public IThState ThState { get; set; }
	public IMathState MathState { get; set; }

	public GameUI ui;
	public GameTest(GameUI ui) {
		this.ui = ui;
		GameState = new StartGameState ();
	}

    GameTest(IGameState gs) {
        GameState = gs;
    }

    GameTest(IThState ts) {
        ThState = ts;
    }

    public void EndGame() {
        GameState.EndGame(this);
    }

    public void StartNewGame() {
        GameState.StartNewGame(this);
    }
}

public interface IMathState
{
	void Preflop(GameTest game);
//	void Flop(Game game);
//	void Turn(Game game);
//	void River(Game game);
//	void BetRound1(Game game);
//	void BetRound2(Game game);
//	void BetRound3(Game game);
//	void BetRound4(Game game);
}

class PreflopState : IMathState {
	public void Preflop(GameTest game) {
		// Bet round 1
		// check players hand strength
		// chose pattern and alternative patterns
	}
}

public interface IGameState
{
    void EndGame(GameTest game);
    void StartNewGame(GameTest game);
    //void Flop(Game game);
    //void Turn(Game game);
    //void River(Game game);
    //void Pause(Game game);
}


public interface IThState
{
    void Fold(GameTest game);
    void Surrender(GameTest game);
    void Check(GameTest game);
    void Call(GameTest game);
    void AllIn(GameTest game);
}


class EndGameState : IGameState
{
    public void EndGame(GameTest game)
    {
        Console.WriteLine("EndGame()");
        //game.GameState = new StartGameState();
    }

    public void StartNewGame(GameTest game)
    {
//        Console.WriteLine("StartNewGame()");
//		game.ui.panelInitBet.SetActive (false);
//		game.ui.panelGame.SetActive (true);


//		game.MathState = new PreflopState ();
        //game.GameState = new EndGameState();
    }
}


class StartGameState : IGameState
{
    public void EndGame(GameTest game)
    {
		game.ui.panelWin.SetActive (true);
    }

    public void StartNewGame(GameTest game)
    {
		Console.WriteLine("StartNewGame()");
		game.ui.HideDynamicPanels ();
		game.ui.panelInitBet.SetActive (true);
//		game.ui.panelGame.SetActive (true);
    }
}


class CallThState : IThState
{
    public void AllIn(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Call(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Check(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Fold(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(GameTest game)
    {
        throw new NotImplementedException();
    }
}


class CheckThState : IThState
{
    public void AllIn(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Call(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Check(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Fold(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(GameTest game)
    {
        throw new NotImplementedException();
    }
}


class FlopThState : IThState
{
    public void AllIn(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Call(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Check(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Fold(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(GameTest game)
    {
        throw new NotImplementedException();
    }
}

class SurrenderThState : IThState
{
    public void AllIn(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Call(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Check(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Fold(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(GameTest game)
    {
        throw new NotImplementedException();
    }
}

class AllInThState : IThState
{
    public void AllIn(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Call(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Check(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Fold(GameTest game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(GameTest game)
    {
        throw new NotImplementedException();
    }
}