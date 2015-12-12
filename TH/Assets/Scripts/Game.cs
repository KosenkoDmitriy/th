using System;

public class Game {

    public IGameState GameState { get; set; }
    public IThState ThState { get; set; }

    Game(IGameState gs) {
        GameState = gs;
    }

    Game(IThState ts) {
        ThState = ts;
    }

    public void EndGame() {
        GameState.EndGame(this);
    }

    public void StartNewGame() {
        GameState.StartNewGame(this);
    }
}


public interface IGameState
{
    void EndGame(Game game);
    void StartNewGame(Game game);
    //void Flop(Game game);
    //void Turn(Game game);
    //void River(Game game);
    //void Pause(Game game);
}


public interface IThState
{
    void Fold(Game game);
    void Surrender(Game game);
    void Check(Game game);
    void Call(Game game);
    void AllIn(Game game);
}


class EndGameState : IGameState
{
    public void EndGame(Game game)
    {
        Console.WriteLine("EndGame()");
        //game.GameState = new StartGameState();
    }

    public void StartNewGame(Game game)
    {
        Console.WriteLine("StartNewGame()");
        //game.GameState = new EndGameState();
    }
}


class StartGameState : IGameState
{
    public void EndGame(Game game)
    {
        throw new NotImplementedException();
    }

    public void StartNewGame(Game game)
    {
        throw new NotImplementedException();
    }
}


class CallThState : IThState
{
    public void AllIn(Game game)
    {
        throw new NotImplementedException();
    }

    public void Call(Game game)
    {
        throw new NotImplementedException();
    }

    public void Check(Game game)
    {
        throw new NotImplementedException();
    }

    public void Fold(Game game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(Game game)
    {
        throw new NotImplementedException();
    }
}


class CheckThState : IThState
{
    public void AllIn(Game game)
    {
        throw new NotImplementedException();
    }

    public void Call(Game game)
    {
        throw new NotImplementedException();
    }

    public void Check(Game game)
    {
        throw new NotImplementedException();
    }

    public void Fold(Game game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(Game game)
    {
        throw new NotImplementedException();
    }
}


class FlopThState : IThState
{
    public void AllIn(Game game)
    {
        throw new NotImplementedException();
    }

    public void Call(Game game)
    {
        throw new NotImplementedException();
    }

    public void Check(Game game)
    {
        throw new NotImplementedException();
    }

    public void Fold(Game game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(Game game)
    {
        throw new NotImplementedException();
    }
}

class SurrenderThState : IThState
{
    public void AllIn(Game game)
    {
        throw new NotImplementedException();
    }

    public void Call(Game game)
    {
        throw new NotImplementedException();
    }

    public void Check(Game game)
    {
        throw new NotImplementedException();
    }

    public void Fold(Game game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(Game game)
    {
        throw new NotImplementedException();
    }
}

class AllInThState : IThState
{
    public void AllIn(Game game)
    {
        throw new NotImplementedException();
    }

    public void Call(Game game)
    {
        throw new NotImplementedException();
    }

    public void Check(Game game)
    {
        throw new NotImplementedException();
    }

    public void Fold(Game game)
    {
        throw new NotImplementedException();
    }

    public void Surrender(Game game)
    {
        throw new NotImplementedException();
    }
}