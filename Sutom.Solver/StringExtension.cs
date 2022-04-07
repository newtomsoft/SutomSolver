namespace Sutom.Solver;

public static class StringExtension
{
    public static int Score(this string word, string wordToCompare) =>
        ComputeStatuses(word, wordToCompare).Sum(letterStatus => letterStatus switch
        {
            Status.GoodPlace => 7,
            Status.BadPlace => 3,
            Status.NotPresent => 1,
            _ => throw new NotImplementedException(),
        });

    private static IEnumerable<Status> ComputeStatuses(this string word, string wordToCompare)
    {
        var lettersStatuses = new Status[word.Length];
        var goodPlaceIndexes = new List<int>();
        for (var i = wordToCompare.Length - 1; i >= 0; i--)
        {
            if (wordToCompare[i] == word[i])
            {
                goodPlaceIndexes.Add(i);
                lettersStatuses[i] = Status.GoodPlace;
            }
        }

        var allIndexes = Enumerable.Range(0, word.Length);
        var indexesLetterToCompareWithOther = allIndexes.Except(goodPlaceIndexes);

        var partWordToCompare = wordToCompare;
        foreach (var index in goodPlaceIndexes)
            partWordToCompare = partWordToCompare.RemoveAtIndex(index);

        foreach (var index in indexesLetterToCompareWithOther)
        {
            var firstLetterEqual = partWordToCompare.FirstOrDefault(c => word[index] == c);
            if (firstLetterEqual is '\0') continue;

            lettersStatuses[index] = Status.BadPlace;
            var indexEqual = partWordToCompare.IndexOf(firstLetterEqual);
            partWordToCompare = partWordToCompare.RemoveAtIndex(indexEqual);
        }
        return lettersStatuses;
    }

    private static string RemoveAtIndex(this string word, int index) => new StringBuilder(word).Remove(index, 1).ToString();
}
