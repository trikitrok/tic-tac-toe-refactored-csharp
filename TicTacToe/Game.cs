namespace TicTacToe;

internal enum Turn
{
    X,
    O
}

public class Game
{
    private Turn _turn;
    private readonly Player _playerX;
    private readonly Player _playerO;

    public Game(PlayerInteraction playerXInteraction, PlayerInteraction playerOInteraction)
    {
        _turn = Turn.X;
        _playerX = new Player(new List<Field>(), playerXInteraction);
        _playerO = new Player(new List<Field>(), playerOInteraction);
    }

    public void Start()
    {
        _playerX.See(ToDto());
        StartTurns();
    }

    private void StartTurns()
    {
        while (IsOnGoing())
            if (_turn == Turn.X)
            {
                PlayTurn(_playerX);
                _turn = Turn.O;
            }
            else
            {
                PlayTurn(_playerO);
                _turn = Turn.X;
            }
    }

    private void PlayTurn(Player player)
    {
        player.PlayTurn();
        DisplayStateAfterTurn();
    }

    private bool IsOnGoing()
    {
        return !_playerX.HasWon() &&
               !IsBoardFull() &&
               !_playerO.HasWon();
    }

    private void DisplayStateAfterTurn()
    {
        _playerX.See(ToDto());
        _playerO.See(ToDto());
    }

    private GameStateDto ToDto()
    {
        var playerXFields = _playerX.ToDto();
        var playerOFields = _playerO.ToDto();

        if (_playerX.HasWon())
            return GameStateDto.WinningX(playerXFields, playerOFields);
        if (IsBoardFull())
            return GameStateDto.NoWinner(playerXFields, playerOFields);
        if (_playerO.HasWon())
            return GameStateDto.WinningO(playerXFields, playerOFields);
        return GameStateDto.OnGoingGame(playerXFields, playerOFields);
    }

    private bool IsBoardFull()
    {
        return _playerX.NumberOfFields() + _playerO.NumberOfFields() == 9;
    }
}