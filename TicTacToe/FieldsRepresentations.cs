namespace TicTacToe;

public static class FieldsRepresentations
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