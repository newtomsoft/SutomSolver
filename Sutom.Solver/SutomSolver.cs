namespace Sutom.Solver;

public static class SutomSolver
{
    public static IEnumerable<string> GetWordsToExplore(IReadOnlyCollection<LetterStatus> lettersStatus, IEnumerable<string> inputWordsToExplore)
    {
        var wordsToExplore = inputWordsToExplore.ToList();
        var letterFound = new List<char>();
        foreach (var letterStatus in lettersStatus.Where(item => item.Status == Status.GoodPlace))
        {
            wordsToExplore = wordsToExplore.Where(word => word[letterStatus.WordIndex] == letterStatus.Letter).ToList();
            letterFound.Add(letterStatus.Letter);
        }
        foreach (var letterStatus in lettersStatus.Where(item => item.Status == Status.BadPlace))
        {
            var letter = letterStatus.Letter;
            var index = letterStatus.WordIndex;
            var minNumberToCount = letterFound.Count(l => l == letter) + 1;
            wordsToExplore = wordsToExplore.Where(word => word[index] != letter && word.Count(l => l == letter) >= minNumberToCount).ToList();
            letterFound.Add(letter);
        }
        foreach (var letterStatus in lettersStatus.Where(item => item.Status == Status.NotPresent))
        {
            var letter = letterStatus.Letter;
            var numberToCount = letterFound.Count(l => l == letter);
            wordsToExplore = wordsToExplore.Where(word => word.Count(l => l == letter) == numberToCount).ToList();
        }
        return wordsToExplore;
    }

    public static string FindBestWord(List<string> wordsToExplore)
    {
        var maxAverage = 0d;
        var currentSelectedWord = string.Empty;


        foreach (var wordToCompare in wordsToExplore)
        {
            var score = 0;
            var count = 0;
            foreach (var currentWord in wordsToExplore)
            {
                count++;
                score += wordToCompare.Score(currentWord);
            }

            var currentAverage = score / (double)count;

            if (currentAverage > maxAverage)
            {
                currentSelectedWord = wordToCompare;
                maxAverage = currentAverage;
            }
        }
        return currentSelectedWord;
    }
}
