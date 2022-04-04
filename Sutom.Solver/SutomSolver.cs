namespace Sutom.Solver;

public static class SutomSolver
{
    public static IEnumerable<string> GetWordsToExplore(IReadOnlyCollection<LetterStatus> lettersStatus, List<string> wordsToParse)
    {
        var letterFound = new List<char>();
        foreach (var letterStatus in lettersStatus.Where(item => item.Status == Status.GoodPlace))
        {
            wordsToParse = wordsToParse.Where(word => word[letterStatus.WordIndex] == letterStatus.Letter).ToList();
            letterFound.Add(letterStatus.Letter);
        }
        foreach (var letterStatus in lettersStatus.Where(item => item.Status == Status.BadPlace))
        {
            var letter = letterStatus.Letter;
            var index = letterStatus.WordIndex;
            var minNumberToCount = letterFound.Count(l => l == letter) + 1;
            wordsToParse = wordsToParse.Where(word => word[index] != letter && word.Count(l => l == letter) >= minNumberToCount).ToList();
            letterFound.Add(letter);
        }
        foreach (var letterStatus in lettersStatus.Where(item => item.Status == Status.NotPresent))
        {
            var letter = letterStatus.Letter;
            var numberToCount = letterFound.Count(l => l == letter);
            wordsToParse = wordsToParse.Where(word => word.Count(l => l == letter) == numberToCount).ToList();
        }
        return wordsToParse;
    }

    public static string FindBestWord(List<string> wordsToParse)
    {
        var maxAverage = 0d;
        var currentSelectedWord = string.Empty;


        foreach (var wordToCompare in wordsToParse)
        {
            var score = 0;
            var count = 0;
            foreach (var currentWord in wordsToParse)
            {
                count++;
                score += wordToCompare.Compare(currentWord);
            }

            var currentAverage = score / (double)count;

            if (currentAverage > maxAverage)
            {
                maxAverage = currentAverage;
                currentSelectedWord = wordToCompare;
            }
        }
        return currentSelectedWord;
    }
}
