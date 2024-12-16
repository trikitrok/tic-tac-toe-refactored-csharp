namespace TicTacToe;

public class Game
{
    private Turn _turn;

    public Game(PlayerInteraction playerXInteraction, PlayerInteraction playerOInteraction)
    {
        _turn = Turn.Initial(playerXInteraction, playerOInteraction);
    }

    public void Start()
    {
        _turn.ShowInitialMessage();
        StartTurns();
    }

    private void StartTurns()
    {
        while (_turn.CanBePlayed())
        {
            _turn = _turn.Play();
        }
    }
}