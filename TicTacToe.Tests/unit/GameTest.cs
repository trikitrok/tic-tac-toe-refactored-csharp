using NSubstitute;
using NUnit.Framework;
using static TicTacToe.Tests.helpers.GameStateDtoBuilder;

namespace TicTacToe.Tests.unit;

[TestFixture]
public class GameTests
{
    private PlayerInteraction _playerXInteraction;
    private PlayerInteraction _playerOInteraction;
    private Game _game;
    private Builder _gameDtoBuilder;
    private List<GameStateDto> _playerXInteractionCalls;
    private List<GameStateDto> _playerOInteractionCalls;

    [SetUp]
    public void SetUp()
    {
        _playerXInteraction = Substitute.For<PlayerInteraction>();
        _playerOInteraction = Substitute.For<PlayerInteraction>();
        _playerXInteractionCalls = new List<GameStateDto>();
        _playerOInteractionCalls = new List<GameStateDto>();

        _playerXInteraction.Display(Arg.Do<GameStateDto>(g => _playerXInteractionCalls.Add(g)));
        _playerOInteraction.Display(Arg.Do<GameStateDto>(g => _playerOInteractionCalls.Add(g)));
        
        _gameDtoBuilder = AGameStateDto();
        _game = new Game(_playerXInteraction, _playerOInteraction);
    }

    [Test]
    public void PlayerXWinsAfterThirdTurn()
    {
        _playerXInteraction.YourTurn().Returns(Field.One, Field.Two, Field.Three);
        _playerOInteraction.YourTurn().Returns(Field.Four, Field.Five);
        
        _game.Start();
        
        ExpectInitialDisplay();
        ExpectPlayerTurn(1, _gameDtoBuilder.AddingFieldToX(Field.One).Build());
        ExpectPlayerTurn(2, _gameDtoBuilder.AddingFieldToO(Field.Four).Build());
        ExpectPlayerTurn(3, _gameDtoBuilder.AddingFieldToX(Field.Two).Build());
        ExpectPlayerTurn(4, _gameDtoBuilder.AddingFieldToO(Field.Five).Build());
        ExpectPlayerTurn(5, _gameDtoBuilder.AddingFieldToX(Field.Three).WinningPlayerX().Build());
    }

    [Test]
    public void PlayerOWinsAfterHerThirdTurn()
    {
        _playerXInteraction.YourTurn().Returns(Field.Four, Field.Five, Field.Seven);
        _playerOInteraction.YourTurn().Returns(Field.One, Field.Two, Field.Three);
        
        _game.Start();
        
        ExpectInitialDisplay();
        ExpectPlayerTurn(1, _gameDtoBuilder.AddingFieldToX(Field.Four).Build());
        ExpectPlayerTurn(2, _gameDtoBuilder.AddingFieldToO(Field.One).Build());
        ExpectPlayerTurn(3, _gameDtoBuilder.AddingFieldToX(Field.Five).Build());
        ExpectPlayerTurn(4, _gameDtoBuilder.AddingFieldToO(Field.Two).Build());
        ExpectPlayerTurn(5, _gameDtoBuilder.AddingFieldToX(Field.Seven).Build());
        ExpectPlayerTurn(6, _gameDtoBuilder.AddingFieldToO(Field.Three).WinningPlayerO().Build());
    }
    
    [Test]
    public void DrawWhenX1O5X9O2X8O7X3O6X4()
    {
        _playerXInteraction.YourTurn().Returns(Field.One, Field.Nine, Field.Eight, Field.Three, Field.Four);
        _playerOInteraction.YourTurn().Returns(Field.Five, Field.Two, Field.Seven, Field.Six);
        
        _game.Start();
        
        ExpectInitialDisplay();
        ExpectPlayerTurn(1, _gameDtoBuilder.AddingFieldToX(Field.One).Build());
        ExpectPlayerTurn(2, _gameDtoBuilder.AddingFieldToO(Field.Five).Build());
        ExpectPlayerTurn(3, _gameDtoBuilder.AddingFieldToX(Field.Nine).Build());
        ExpectPlayerTurn(4, _gameDtoBuilder.AddingFieldToO(Field.Two).Build());
        ExpectPlayerTurn(5, _gameDtoBuilder.AddingFieldToX(Field.Eight).Build());
        ExpectPlayerTurn(6, _gameDtoBuilder.AddingFieldToO(Field.Seven).Build());
        ExpectPlayerTurn(7, _gameDtoBuilder.AddingFieldToX(Field.Three).Build());
        ExpectPlayerTurn(8, _gameDtoBuilder.AddingFieldToO(Field.Six).Build());
        ExpectPlayerTurn(9, _gameDtoBuilder.AddingFieldToX(Field.Four).WithNoOneWinning().Build());
    }

    private void ExpectPlayerTurn(int turnNumber, GameStateDto gameStateDto)
    {
        Assert.That(_playerXInteractionCalls[turnNumber], Is.EqualTo(gameStateDto));
        Assert.That(_playerOInteractionCalls[turnNumber-1], Is.EqualTo(gameStateDto));
    }

    private void ExpectInitialDisplay()
    {
        Assert.That(_playerXInteractionCalls[0], Is.EqualTo(InitialGameStateDto()));
    }
}