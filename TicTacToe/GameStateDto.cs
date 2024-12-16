namespace TicTacToe;

public enum Status
{
    OverXWins,
    OverOWins,
    OverDraw,
    OnGoing
}

public class GameStateDto
{
    private readonly Status _status;

    public GameStateDto(List<Field> playerXFields, List<Field> playerOFields, Status status)
    {
        PlayerXFields = playerXFields;
        PlayerOFields = playerOFields;
        _status = status;
    }

    public List<Field> PlayerXFields { get; }
    public List<Field> PlayerOFields { get; }

    public static GameStateDto WinningX(List<Field> playerXFields, List<Field> playerOFields)
    {
        return new GameStateDto(playerXFields, playerOFields, Status.OverXWins);
    }

    public static GameStateDto WinningO(List<Field> playerX, List<Field> playerO)
    {
        return new GameStateDto(playerX, playerO, Status.OverOWins);
    }

    public static GameStateDto NoWinner(List<Field> playerX, List<Field> playerO)
    {
        return new GameStateDto(playerX, playerO, Status.OverDraw);
    }

    public static GameStateDto OnGoingGame(List<Field> playerX, List<Field> playerO)
    {
        return new GameStateDto(playerX, playerO, Status.OnGoing);
    }

    public Status GetStatus()
    {
        return _status;
    }
   
    public override string ToString()
    {
        return $"{nameof(PlayerXFields)}: {string.Join(",", PlayerXFields)}, " +
               $"{nameof(PlayerOFields)}: {string.Join(",", PlayerOFields)}, " +
               $"Status: {_status}";
    }

    protected bool Equals(GameStateDto other)
    {
        return _status == other._status && 
              PlayerXFields.SequenceEqual(other.PlayerXFields) &&
              PlayerOFields.SequenceEqual(other.PlayerOFields);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((GameStateDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)_status, PlayerXFields, PlayerOFields);
    }
}