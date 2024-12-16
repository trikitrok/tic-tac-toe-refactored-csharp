namespace TicTacToe;

public abstract class Turn
{
    protected readonly Player CurrentPlayer;
    protected readonly Player OtherPlayer;

    protected Turn(Player currentPlayer, Player otherPlayer)
    {
        CurrentPlayer = currentPlayer;
        OtherPlayer = otherPlayer;
    }

    public static Turn Initial(PlayerInteraction xPlayerInteraction, PlayerInteraction oPlayerInteraction)
    {
        var xPlayer = new Player(new List<Field>(), xPlayerInteraction);
        var oPlayer = new Player(new List<Field>(), oPlayerInteraction);
        return new TurnForX(xPlayer, oPlayer);
    }

    public Turn Play()
    {
        CurrentPlayer.PlayTurn();
        DisplayStateAfterTurn();
        return Next();
    }

    public void ShowInitialMessage()
    {
        GetXPlayer().See(ToDto());
    }
    
    protected abstract Turn Next();

    protected abstract GameStateDto CreateWinningDto();
    
    protected abstract Player GetXPlayer();

    protected abstract Player GetOPlayer();

    public bool CanBePlayed()
    {
        return !GetXPlayer().HasWon() && !IsBoardFull() && !GetOPlayer().HasWon();
    }
    
    protected bool IsBoardFull()
    {
        return (GetXPlayer().NumberOfFields() + GetOPlayer().NumberOfFields()) == 9;
    }

    private GameStateDto NoWinnerDto()
    {
        return GameStateDto.NoWinner(GetXPlayer().ToDto(), GetOPlayer().ToDto());
    }

    private GameStateDto OnGoingDto()
    {
        return GameStateDto.OnGoingGame(GetXPlayer().ToDto(), GetOPlayer().ToDto());
    }

    private GameStateDto ToDto()
    {
        if (CurrentPlayer.HasWon())
        {
            return CreateWinningDto();
        }
        if (CanBePlayed())
        {
            return OnGoingDto();
        }
        return NoWinnerDto();
    }

    private void DisplayStateAfterTurn()
    {
        var dto = ToDto();
        GetXPlayer().See(dto);
        GetOPlayer().See(dto);
    }
}

public class TurnForO : Turn
{
    public TurnForO(Player currentPlayer, Player otherPlayer) 
        : base(currentPlayer, otherPlayer)
    {
    }

    protected override Turn Next()
    {
        return new TurnForX(GetXPlayer(), GetOPlayer());
    }

    protected override GameStateDto CreateWinningDto()
    {
        return GameStateDto.WinningO(GetXPlayer().ToDto(), GetOPlayer().ToDto());
    }

    protected override Player GetXPlayer()
    {
        return OtherPlayer;
    }
    
    protected override Player GetOPlayer()
    {
        return CurrentPlayer;
    }
}

public class TurnForX : Turn
{
    public TurnForX(Player currentPlayer, Player otherPlayer) 
        : base(currentPlayer, otherPlayer)
    {
    }

    protected override Turn Next()
    {
        return new TurnForO(GetOPlayer(), GetXPlayer());
    }

    protected override GameStateDto CreateWinningDto()
    {
        return GameStateDto.WinningX(GetXPlayer().ToDto(), GetOPlayer().ToDto());
    }
    
    protected override Player GetXPlayer()
    {
        return CurrentPlayer;
    }
    
    protected override Player GetOPlayer()
    {
        return OtherPlayer;
    }
}