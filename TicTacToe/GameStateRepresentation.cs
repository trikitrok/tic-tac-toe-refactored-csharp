namespace TicTacToe;

public class GameStateRepresentation
{
    private readonly string[][] _board;
    private readonly GameStateDto _gameStateDto;

    public GameStateRepresentation(GameStateDto gameStateDto)
    {
        _board = new[]
        {
            new[] { "1", "2", "3" },
            new[] { "4", "5", "6" },
            new[] { "7", "8", "9" }
        };
        _gameStateDto = gameStateDto;
    }

    public string Create()
    {
        return RepresentBoard() + ComposeFinalMessage();
    }

    private string RepresentBoard()
    {
        return string.Join("---------\n", FillBoard().Select(RepresentRow));

        static string RepresentRow(string[] row)
        {
            return string.Join(" | ", row) + "\n";
        }
    }

    private string[][] FillBoard()
    {
        return _board.Select(row => row.Select(RepresentField).ToArray()).ToArray();
    }

    private string RepresentField(string fieldString)
    {
        var field = FieldsRepresentations.Get(fieldString);

        if (_gameStateDto.PlayerXFields.Contains(field)) return "X";

        if (_gameStateDto.PlayerOFields.Contains(field)) return "O";

        return fieldString;
    }

    private string ComposeFinalMessage()
    {
        var gameStatus = _gameStateDto.GetStatus();
        if (gameStatus == Status.OnGoing) return "";
        return ComposeGameOverMessage(gameStatus);

        static string ComposeGameOverMessage(Status gameStatus)
        {
            if (gameStatus == Status.OverOWins)
                return "O wins!\n";
            if (gameStatus == Status.OverXWins) return "X wins!\n";
            return "Draw!\n";
        }
    }
}