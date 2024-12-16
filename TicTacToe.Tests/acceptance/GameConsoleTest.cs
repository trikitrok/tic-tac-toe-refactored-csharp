using NSubstitute;
using NUnit.Framework;

namespace TicTacToe.Tests.acceptance;

public class GameConsoleTest
{
    private Game _game;
    private Input _inputO;
    private Input _inputX;
    private Output _outputO;
    private List<string> _outputOCalls;
    private int _outputODisplayNumCalls;
    private Output _outputX;
    private List<string> _outputXCalls;
    private int _outputXDisplayNumCalls;

    [SetUp]
    public void SetUp()
    {
        _outputXDisplayNumCalls = 0;
        _outputODisplayNumCalls = 0;

        _inputX = Substitute.For<Input>();
        _inputO = Substitute.For<Input>();

        _outputX = Substitute.For<Output>();
        _outputO = Substitute.For<Output>();

        PlayerInteraction playerXInteraction = new TextBasedPlayerInteraction(_inputX, _outputX);
        PlayerInteraction playerOInteraction = new TextBasedPlayerInteraction(_inputO, _outputO);
        _game = new Game(playerXInteraction, playerOInteraction);
        _outputXCalls = new List<string>();
        _outputOCalls = new List<string>();

        _outputX.Display(Arg.Do<string>(s => _outputXCalls.Add(s)));
        _outputO.Display(Arg.Do<string>(s => _outputOCalls.Add(s)));
    }

    [Test]
    public void PlayerXWinsAfterHerThirdTurn()
    {
        _inputX.Read().Returns("1", "2", "3");
        _inputO.Read().Returns("4", "5");

        _game.Start();

        ExpectInitialDisplay();
        ExpectTurnForPlayerX(
            "X | 2 | 3\n" +
            "---------\n" +
            "4 | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerO(
            "X | 2 | 3\n" +
            "---------\n" +
            "O | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerX(
            "X | X | 3\n" +
            "---------\n" +
            "O | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerO(
            "X | X | 3\n" +
            "---------\n" +
            "O | O | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerX(
            "X | X | X\n" +
            "---------\n" +
            "O | O | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n" +
            "X wins!\n"
        );
    }

    [Test]
    public void PlayerOWinsAfterHerThirdTurn()
    {
        _inputX.Read().Returns("4", "5", "7");
        _inputO.Read().Returns("1", "2", "3");

        _game.Start();

        ExpectInitialDisplay();
        ExpectTurnForPlayerX(
            "1 | 2 | 3\n" +
            "---------\n" +
            "X | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerO(
            "O | 2 | 3\n" +
            "---------\n" +
            "X | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerX(
            "O | 2 | 3\n" +
            "---------\n" +
            "X | X | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerO(
            "O | O | 3\n" +
            "---------\n" +
            "X | X | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerX(
            "O | O | 3\n" +
            "---------\n" +
            "X | X | 6\n" +
            "---------\n" +
            "X | 8 | 9\n"
        );
        ExpectTurnForPlayerO(
            "O | O | O\n" +
            "---------\n" +
            "X | X | 6\n" +
            "---------\n" +
            "X | 8 | 9\n" +
            "O wins!\n"
        );
    }

    [Test]
    public void DrawGame()
    {
        _inputX.Read().Returns("1", "9", "8", "3", "4");
        _inputO.Read().Returns("5", "2", "7", "6");

        _game.Start();

        ExpectInitialDisplay();
        ExpectTurnForPlayerX(
            "X | 2 | 3\n" +
            "---------\n" +
            "4 | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerO(
            "X | 2 | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerX(
            "X | 2 | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "7 | 8 | X\n"
        );
        ExpectTurnForPlayerO(
            "X | O | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "7 | 8 | X\n"
        );
        ExpectTurnForPlayerX(
            "X | O | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "7 | X | X\n"
        );
        ExpectTurnForPlayerO(
            "X | O | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "O | X | X\n"
        );
        ExpectTurnForPlayerX(
            "X | O | X\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "O | X | X\n"
        );
        ExpectTurnForPlayerO(
            "X | O | X\n" +
            "---------\n" +
            "4 | O | O\n" +
            "---------\n" +
            "O | X | X\n"
        );
        ExpectTurnForPlayerX(
            "X | O | X\n" +
            "---------\n" +
            "X | O | O\n" +
            "---------\n" +
            "O | X | X\n" +
            "Draw!\n"
        );
    }


    [Test]
    public void RetriesUntilPlayerEntersValidField()
    {
        _inputX.Read().Returns("Invalid value", "1", "9", "8", "3", "4");
        _inputO.Read().Returns("5", "2", "7", "6");

        _game.Start();

        ExpectInitialDisplay();
        ExpectTurnForPlayerX(
            "X | 2 | 3\n" +
            "---------\n" +
            "4 | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n",
            true
        );
        ExpectTurnForPlayerO(
            "X | 2 | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        );
        ExpectTurnForPlayerX(
            "X | 2 | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "7 | 8 | X\n"
        );
        ExpectTurnForPlayerO(
            "X | O | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "7 | 8 | X\n"
        );
        ExpectTurnForPlayerX(
            "X | O | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "7 | X | X\n"
        );
        ExpectTurnForPlayerO(
            "X | O | 3\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "O | X | X\n"
        );
        ExpectTurnForPlayerX(
            "X | O | X\n" +
            "---------\n" +
            "4 | O | 6\n" +
            "---------\n" +
            "O | X | X\n"
        );
        ExpectTurnForPlayerO(
            "X | O | X\n" +
            "---------\n" +
            "4 | O | O\n" +
            "---------\n" +
            "O | X | X\n"
        );
        ExpectTurnForPlayerX(
            "X | O | X\n" +
            "---------\n" +
            "X | O | O\n" +
            "---------\n" +
            "O | X | X\n" +
            "Draw!\n"
        );
    }

    private void ExpectInitialDisplay()
    {
        Assert.That(_outputXCalls[_outputXDisplayNumCalls], Is.EqualTo(
            "1 | 2 | 3\n" +
            "---------\n" +
            "4 | 5 | 6\n" +
            "---------\n" +
            "7 | 8 | 9\n"
        ));
        _outputXDisplayNumCalls++;
    }

    private void ExpectTurnForPlayerX(string boardRepresentation, bool wrongInput = false)
    {
        Assert.That(_outputXCalls[_outputXDisplayNumCalls], Is.EqualTo(
            "your turn..."
        ));
        if (wrongInput)
        {
            Assert.That(_outputXCalls[_outputXDisplayNumCalls + 1], Is.EqualTo(
                "invalid input, please try again"
            ));
            _outputXDisplayNumCalls++;
        }

        Assert.That(_outputXCalls[_outputXDisplayNumCalls + 1], Is.EqualTo(
            boardRepresentation
        ));
        _outputXDisplayNumCalls += 2;

        Assert.That(_outputOCalls[_outputODisplayNumCalls], Is.EqualTo(
            boardRepresentation
        ));
        _outputODisplayNumCalls++;
    }

    private void ExpectTurnForPlayerO(string boardRepresentation)
    {
        Assert.That(_outputOCalls[_outputODisplayNumCalls], Is.EqualTo(
            "your turn..."
        ));
        Assert.That(_outputOCalls[_outputODisplayNumCalls + 1], Is.EqualTo(
            boardRepresentation
        ));
        _outputODisplayNumCalls += 2;

        Assert.That(_outputXCalls[_outputXDisplayNumCalls], Is.EqualTo(
            boardRepresentation
        ));
        _outputXDisplayNumCalls++;
    }
}