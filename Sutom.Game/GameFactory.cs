using Sutom.Share;
using Sutom.Solver;

namespace Sutom.Game;

public static class GameFactory
{
    public static async Task<string> GetRandomWord()
    {
        var allWords = await WordDictionary.GetAllWords(@"fr.UTF-8.dic");

        const string allLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var indexLetter = Random.Shared.Next(0, allLetters.Length);
        var letter = allLetters[indexLetter];

        var wordsByLength = allWords[letter];

        List<string>? words;
        while (true)
        {
            var numberLetters = Random.Shared.Next(4, 11);
            var isNumberLettersSelected = wordsByLength.TryGetValue(numberLetters, out words);
            if (isNumberLettersSelected) break;
        }
        var indexWord = Random.Shared.Next(0, words!.Count);
        var wordSelected = words[indexWord];
        return wordSelected;
    }

    public static HashSet<LetterStatus> GetLetterStatuses(string word, string wordToFind)
    {
        word = word.ToUpperInvariant();
        var lettersStatus = new HashSet<LetterStatus>();
        for (var i = 0; i < wordToFind.Length; i++)
            if (word[i] == wordToFind[i]) lettersStatus.Add(new LetterStatus(i, word[i], Status.GoodPlace));

        var allGoodIndexes = lettersStatus.Select(l => l.WordIndex).ToList();
        var allGoodLetters = lettersStatus.Select(l => l.Letter).ToList();
        for (var i = 0; i < wordToFind.Length; i++)
        {
            if (allGoodIndexes.Contains(i)) continue;
            var letter = word[i];
            var inWordCount = wordToFind.Count(l => l == letter);
            var foundCount = allGoodLetters.Count(l => l == letter);
            if (foundCount == inWordCount)
                lettersStatus.Add(new LetterStatus(i, letter, Status.NotPresent));
            else
            {
                lettersStatus.Add(new LetterStatus(i, letter, Status.BadPlace));
                allGoodLetters.Add(letter);
            }
        }

        return lettersStatus;
    }
}
