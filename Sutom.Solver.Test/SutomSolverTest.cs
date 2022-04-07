namespace Sutom.Solver.Test;

public class SutomSolverTest
{
    [Theory]
    [InlineData("A", "A", 7)]
    [InlineData("A", "B", 1)]
    [InlineData("AB", "AB", 7 + 7)]
    [InlineData("AB", "AC", 7 + 1)]
    [InlineData("AB", "CD", 1 + 1)]
    [InlineData("AB", "CA", 3 + 1)]
    [InlineData("AB", "BA", 3 + 3)]
    [InlineData("TOTO", "TATA", 16)]
    [InlineData("TOTO", "TADA", 10)]
    [InlineData("TOTO", "ATDA", 6)]
    [InlineData("IDOLATRIE", "IMITATEUR", 33)]
    public void ScoreTest(string word1, string word2, int expectedScore)
    {
        var score = word1.Score(word2);
        score.ShouldBe(expectedScore);
    }


    [Theory]
    [InlineData("")]
    [InlineData("ABC", "AAA", "ABB", "ABC", "ACC")]
    [InlineData("ABCD", "AAAA", "ABBB", "ACCC", "ADDD", "ABAA", "ABCC", "ABDD", "ACAA", "ACBB", "ACDD", "ADAA", "ADBB", "ADCC", "ABCD")]
    [InlineData("ABCD", "AAAA", "ABBB", "ACCC", "ADDD", "ABAA", "ABCC", "ABDD", "ACAA", "ACBB", "ACDD", "ADAA", "ADBB", "ADCC", "ABCD", "ACBD")]
    [InlineData("ACBD", "AAAA", "ABBB", "ACCC", "ADDD", "ABAA", "ABCC", "ABDD", "ACAA", "ACBB", "ACDD", "ADAA", "ADBB", "ADCC", "ACBD", "ABCD")]
    public void FindBestWordTest(string bestWordExpected, params string[] words)
    {
        var wordsToParse = words.ToList();
        var word = SutomSolver.FindBestWord(wordsToParse);
        word.ShouldBe(bestWordExpected);
    }


    public static TheoryData<GetReducedWordsToParseTestInput> TestInput => new()
    {
        new() { WordStatusTestInput = new() { WordToTest = "NotCompliantWord", WordStatusInStringFormat = "" }, WordsToParse = new() { "AB", "AC", "CA", "DA" } },
        new() { ExpectedReducedWordsToParse = new() { "AB", "AC" }, WordsToParse = new() { "AB", "AC", "CA", "DA" }, WordStatusTestInput = new() { WordToTest = "AZ", WordStatusInStringFormat = "GN" } },
        new() { ExpectedReducedWordsToParse = new() { "ACP", "ACT" }, WordsToParse = new() { "ABC", "ACB", "ACP", "ACT" }, WordStatusTestInput = new() { WordToTest = "ABC", WordStatusInStringFormat = "GNB" } },
    };
    [Theory]
    [MemberData(nameof(TestInput))]
    public void GetReducedWordsToParseTest(GetReducedWordsToParseTestInput testInput)
    {
        var wordsToParse = testInput.WordsToParse;
        var wordStatus = TestArrange.ComputeWordStatus(testInput.WordStatusTestInput);

        var getReducedWordsToParse = () => SutomSolver.GetReducedWordsToParse(wordStatus, wordsToParse);
        if (wordStatus.WordCompliant is false)
        {
            getReducedWordsToParse.ShouldThrow<ArgumentException>().Message.ShouldBe("wordStatus with bad letter status number");
            return;
        }

        var reducedWordsToParse = getReducedWordsToParse();
        reducedWordsToParse.ShouldBe(testInput.ExpectedReducedWordsToParse);
    }


    public static TheoryData<IsWordFoundTestInput> IsWordFoundTestInput => new()
    {
        new IsWordFoundTestInput { ResultExpected = true, WordStatusTestInput = new WordStatusTestInput() { WordToTest = "AB", WordStatusInStringFormat = "GG" } },
        new IsWordFoundTestInput { ResultExpected = false, WordStatusTestInput = new WordStatusTestInput() { WordToTest = "AB", WordStatusInStringFormat = "GB" } },
        new IsWordFoundTestInput { ResultExpected = false, WordStatusTestInput = new WordStatusTestInput() { WordToTest = "AB", WordStatusInStringFormat = "GN" } },
    };
    [Theory]
    [MemberData(nameof(IsWordFoundTestInput))]
    public void IsWordFoundTest(IsWordFoundTestInput testInput)
    {
        var wordStatus = TestArrange.ComputeWordStatus(testInput.WordStatusTestInput);
        wordStatus.IsWordFound().ShouldBe(testInput.ResultExpected);
    }
}