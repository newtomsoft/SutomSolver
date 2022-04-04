using System.Globalization;
using System.Text;

namespace Sutom.Solver;

public static class StringExtension
{
    public static int Score(this string word, string wordToCompare)
    {
        var states = ComputeState(word, wordToCompare);
        var score = 0;
        foreach (var letterState in states)
        {
            switch (letterState)
            {
                case Status.GoodPlace:
                    score += 3;
                    break;
                case Status.BadPlace:
                    score += 1;
                    break;
                case Status.NotPresent:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return score;
    }
    
    private static IEnumerable<Status> ComputeState(this string word, string wordToCompare)
    {
        var letterStates = new Status[word.Length];
        var goodPlaceIndexes = new List<int>();
        for (var i = wordToCompare.Length - 1; i >= 0; i--)
        {
            if (wordToCompare[i] == word[i])
            {
                goodPlaceIndexes.Add(i);
                letterStates[i] = Status.GoodPlace;
            }
        }

        var allIndexes = Enumerable.Range(0, word.Length);
        var indexesCharToCompareWithOther = allIndexes.Except(goodPlaceIndexes);

        var partWordToCompare = wordToCompare;
        foreach (var index in goodPlaceIndexes) partWordToCompare = partWordToCompare.RemoveAtIndex(index);

        var badPlaceIndexes = new List<int>();
        foreach (var index in indexesCharToCompareWithOther)
        {
            var firstCharEqual = partWordToCompare.FirstOrDefault(c => word[index] == c);
            if (firstCharEqual is '\0') continue;

            letterStates[index] = Status.BadPlace;
            var indexEqual = partWordToCompare.IndexOf(firstCharEqual);
            badPlaceIndexes.Add(indexEqual);
            partWordToCompare = partWordToCompare.RemoveAtIndex(indexEqual);
        }
        return letterStates;
    }

    private static string RemoveAtIndex(this string word, int index)
    {
        var sb = new StringBuilder(word);
        sb.Remove(index, 1);
        word = sb.ToString();
        return word;
    }
}
