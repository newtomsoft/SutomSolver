using System.Threading.Tasks;

namespace Sutom.Game.Test;

public class GameFactoryTest
{
    [Theory]
    [InlineData("G", "A", "A")]
    [InlineData("GG", "AB", "AB")]
    [InlineData("GN", "AB", "AC")]
    [InlineData("GBB", "ABC", "ACB")]
    [InlineData("GBN", "ABC", "ADB")]
    [InlineData("GNNNGN", "LANIER", "LEQUEL")]
    [InlineData("GNNNGB", "LIVREE", "LEQUEL")]
    [InlineData("GNBBBN", "ENTREE", "EXPERT")]
    public void GetWordStatusTest(string expectedWordStatusString, string word, string wordToFind)
    {
        var wordStatus = GameFactory.GetWordStatus(word, wordToFind);
        wordStatus.LettersStatuses.Count.ShouldBe(expectedWordStatusString.Length);
        var statuses = wordStatus.LettersStatuses.OrderBy(l => l.WordIndex).Select(l => l.Status).ToList();
        for (var i = 0; i < statuses.Count; i++)
        {
            switch (statuses[i])
            {
                case Status.GoodPlace:
                    expectedWordStatusString[i].ShouldBe('G');
                    break;
                case Status.BadPlace:
                    expectedWordStatusString[i].ShouldBe('B');
                    break;
                case Status.NotPresent:
                    expectedWordStatusString[i].ShouldBe('N');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [Fact]
    public async Task GetRandomWordTest()
    {
        var word = await GameFactory.AsyncGetRandomWord();
        word.Length.ShouldBeGreaterThanOrEqualTo(5);
        word.Length.ShouldBeLessThan(11);
    }
}