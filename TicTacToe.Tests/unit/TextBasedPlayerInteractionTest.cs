using NSubstitute;
using NUnit.Framework;
using static TicTacToe.Tests.helpers.GameStateDtoBuilder;

namespace TicTacToe.Tests.unit;

public class TextBasedPlayerInteractionTests
{
    private Input _input;
    private Output _output;
    private TextBasedPlayerInteraction _playerInteraction;

    [SetUp]
    public void SetUp()
    {
        _input = Substitute.For<Input>();
        _output = Substitute.For<Output>();
        _playerInteraction = new TextBasedPlayerInteraction(_input, _output);
    }

    [Test]
    [TestCase("1", Field.One)]
    [TestCase("2", Field.Two)]
    [TestCase("3", Field.Three)]
    [TestCase("4", Field.Four)]
    [TestCase("5", Field.Five)]
    [TestCase("6", Field.Six)]
    [TestCase("7", Field.Seven)]
    [TestCase("8", Field.Eight)]
    [TestCase("9", Field.Nine)]
    public void MapsUserInputToValidField(string userInput, Field expectedField)
    {
        _input.Read().Returns(userInput);

        var field = _playerInteraction.YourTurn();

        Assert.That(field, Is.EqualTo(expectedField));
        _output.Received(1).Display("your turn...");
    }

    [Test]
    public void RetriesUntilPlayerEntersValidField()
    {
        _input.Read().Returns("Invalid value", "1");

        var field = _playerInteraction.YourTurn();

        Assert.That(field, Is.EqualTo(Field.One));
        Received.InOrder(() =>
        {
            _output.Display("your turn...");
            _output.Display("invalid input, please try again");
        });
    }

    [Test]
    public void DisplaysInitialGameState()
    {
        var gameDto = InitialGameStateDto();

        _playerInteraction.Display(gameDto);

        _output.Received(1).Display(
            "1 | 2 | 3\n" +
            "---------\n" +
            "4 | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
    }

    [Test]
    public void DisplaysGameStateAfterXPlaysFieldOne()
    {
        var gameStateDto = AGameStateDto()
            .WithFieldsWithX(Field.One)
            .Build();

        _playerInteraction.Display(gameStateDto);

        _output.Received(1).Display(
            "X | 2 | 3\n" +
            "---------\n" +
            "4 | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
    }

    [Test]
    public void DisplaysGameStateAfterOPlaysFieldThree()
    {
        var gameStateDto = AGameStateDto()
            .WithFieldsWithO(Field.Three)
            .Build();

        _playerInteraction.Display(gameStateDto);

        _output.Received(1).Display(
            "1 | 2 | O\n" +
            "---------\n" +
            "4 | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
    }

    [Test]
    public void DisplaysGameStateAfterXWins()
    {
        var gameStateDto = AGameStateDto()
            .WithFieldsWithX(Field.One, Field.Two, Field.Three)
            .WithFieldsWithO(Field.Four, Field.Five)
            .WinningPlayerX()
            .Build();

        _playerInteraction.Display(gameStateDto);

        _output.Received(1).Display(
            "X | X | X\n" +
            "---------\n" +
            "O | O | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n" +
            "X wins!\n"
        );
    }

    [Test]
    public void DisplaysGameStateAfterOWins()
    {
        var gameStateDto = AGameStateDto()
            .WithFieldsWithO(Field.One, Field.Two, Field.Three)
            .WithFieldsWithX(Field.Four, Field.Five, Field.Seven)
            .WinningPlayerO()
            .Build();

        _playerInteraction.Display(gameStateDto);

        _output.Received(1).Display(
            "O | O | O\n" +
            "---------\n" +
            "X | X | 6\n" +
            "---------\n" +
            "X | 8 | 9\n" +
            "O wins!\n"
        );
    }

    [Test]
    public void DisplaysGameStateAfterDraw()
    {
        var gameStateDto = AGameStateDto()
            .WithFieldsWithX(Field.One, Field.Nine, Field.Eight, Field.Three, Field.Four)
            .WithFieldsWithO(Field.Five, Field.Two, Field.Seven, Field.Six)
            .WithNoOneWinning()
            .Build();

        _playerInteraction.Display(gameStateDto);

        _output.Received(1).Display(
            "X | O | X\n" +
            "---------\n" +
            "X | O | O\n" +
            "---------\n" +
            "O | X | X\n" +
            "Draw!\n"
        );
    }
}