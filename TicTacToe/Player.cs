namespace TicTacToe;

public class Player
{
    private readonly Field[][] _winningCombinations = 
    {
        new[] {Field.One, Field.Two, Field.Three},
        new[] {Field.Four, Field.Five, Field.Six},
        new[] {Field.Seven, Field.Eight, Field.Nine},
        new[] {Field.One, Field.Four, Field.Seven},
        new[] {Field.Two, Field.Five, Field.Eight},
        new[] {Field.Three, Field.Six, Field.Nine},
        new[] {Field.One, Field.Five, Field.Nine},
        new[] {Field.Three, Field.Five, Field.Seven}
    };

    private readonly List<Field> _fields;
    private readonly PlayerInteraction _playerInteraction;

    public Player(List<Field> fields, PlayerInteraction playerInteraction)
    {
        _fields = fields;
        _playerInteraction = playerInteraction;
    }

    public bool HasWon()
    {
        return _winningCombinations.Any(combination =>
            combination.All(field => Owns(field))
        );
    }

    public void PlayTurn()
    {
        AddField(_playerInteraction.YourTurn());
    }

    public List<Field> ToDto()
    {
        return new List<Field>(_fields);
    }

    public int NumberOfFields()
    {
        return _fields.Count;
    }

    public void See(GameStateDto gameStateDto)
    {
        _playerInteraction.Display(gameStateDto);
    }

    private void AddField(Field field)
    {
        _fields.Add(field);
    }

    private bool Owns(Field field)
    {
        return _fields.Contains(field);
    }
}