using NSubstitute;
using NUnit.Framework;

namespace TicTacToe.Tests.unit;

[TestFixture]
public class PlayerVictoryDetectionTests
{
    [Test]
    [TestCase(Field.One, Field.Two, Field.Three)]
    [TestCase(Field.Four, Field.Five, Field.Six)]
    [TestCase(Field.Seven, Field.Eight, Field.Nine)]
    public void DetectsWinnerForAllRowsWinningCombinationsInAnyOrder(params Field[] fields)
    {
        var player = CreatePlayer(Shuffle(fields.ToList()));
        Assert.That(player.HasWon(), Is.True);
    }

    [Test]
    [TestCase(Field.One, Field.Four, Field.Seven)]
    [TestCase(Field.Two, Field.Five, Field.Eight)]
    [TestCase(Field.Three, Field.Six, Field.Nine)]
    public void DetectsWinnerForAllColumnsWinningCombinationsInAnyOrder(params Field[] fields)
    {
        var player = CreatePlayer(Shuffle(fields.ToList()));
        Assert.That(player.HasWon(), Is.True);
    }

    [Test]
    [TestCase(Field.One, Field.Five, Field.Nine)]
    [TestCase(Field.Three, Field.Five, Field.Seven)]
    public void DetectsWinnerForDiagonalAndAntiDiagonalWinningCombinationsInAnyOrder(params Field[] fields)
    {
        var player = CreatePlayer(Shuffle(fields.ToList()));
        Assert.That(player.HasWon(), Is.True);
    }

    [Test]
    [TestCase]
    [TestCase(Field.Six)]
    [TestCase(Field.One, Field.Two)]
    [TestCase(Field.One, Field.Two, Field.Four)]
    [TestCase(Field.One, Field.Two, Field.Four, Field.Nine)]
    [TestCase(Field.One, Field.Nine, Field.Eight, Field.Three, Field.Four)]
    [TestCase(Field.Five, Field.Two, Field.Seven, Field.Six)]
    public void DoesNotDetectWinnerForNotWinningCombination(params Field[] fields)
    {
        var player = CreatePlayer(Shuffle(fields.ToList()));
        Assert.That(player.HasWon(), Is.False);
    }

    private List<Field> Shuffle(List<Field> fields)
    {
        return fields.OrderBy(_ => Guid.NewGuid()).ToList();
    }
    
    private Player CreatePlayer(List<Field> fields)
    {
        var playerInteraction = Substitute.For<PlayerInteraction>();
        return new Player(fields, playerInteraction);
    }
}