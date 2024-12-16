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