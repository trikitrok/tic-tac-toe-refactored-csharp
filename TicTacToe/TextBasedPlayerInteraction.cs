namespace TicTacToe;

public class TextBasedPlayerInteraction : PlayerInteraction
{
    private readonly Input _input;
    private readonly Output _output;

    public TextBasedPlayerInteraction(Input input, Output output)
    {
        _input = input;
        _output = output;
    }

    public Field YourTurn()
    {
        _output.Display("your turn...");
        while (true)
        {
            var input = _input.Read();
            if (FieldsRepresentations.Exists(input))
            {
                return FieldsRepresentations.Get(input);
            }

            _output.Display("invalid input, please try again");
        }
    }

    public void Display(GameStateDto gameStateDto)
    {
        _output.Display(RepresentGameState(gameStateDto));
    }

    private string RepresentGameState(GameStateDto gameStateDto)
    {
        return new GameStateRepresentation(gameStateDto).Create();
    }
}

internal class GameStateRepresentation
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

internal static class FieldsRepresentations
{
    private static readonly Dictionary<string, Field> FieldsByRepresentation = new()
    {
        { "1", Field.One },
        { "2", Field.Two },
        { "3", Field.Three },
        { "4", Field.Four },
        { "5", Field.Five },
        { "6", Field.Six },
        { "7", Field.Seven },
        { "8", Field.Eight },
        { "9", Field.Nine }
    };

    public static Field Get(string field)
    {
        return FieldsByRepresentation[field];
    }

    public static bool Exists(string field)
    {
        return FieldsByRepresentation.ContainsKey(field);
    }
}
