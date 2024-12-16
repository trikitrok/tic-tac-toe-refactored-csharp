namespace TicTacToe;

public interface PlayerInteraction
{
    Field YourTurn();
    void Display(GameStateDto gameStateDto);
}