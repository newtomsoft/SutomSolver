namespace Sutom.Solver.Test.Arrange;

public static class TestArrange
{
    public static WordStatus ComputeWordStatus(WordStatusTestInput testInput)
    {
        var wordStatus = new WordStatus();
        if (string.IsNullOrEmpty(testInput.WordStatusInStringFormat)) wordStatus.WordNotCompliant();
        else
        {
            for (var i = 0; i < testInput.WordStatusInStringFormat.Length; i++)
            {
                var status = testInput.WordStatusInStringFormat[i] switch
                {
                    'G' => Status.GoodPlace,
                    'B' => Status.BadPlace,
                    'N' => Status.NotPresent,
                    _ => Status.NotPresent,
                };
                wordStatus.AddLetterStatus(new LetterStatus(i, testInput.WordToTest[i], status));
            }
        }
        return wordStatus;
    }
}
