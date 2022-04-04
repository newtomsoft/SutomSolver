using Sutom.Share;
using Sutom.Solver;

namespace Sutom.Game;

public class GameFactory
{
    public static async Task<string> GetRandomWord()
    {
        await using var fileStream = new FileStream(@"fr.UTF-8.dic", FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(fileStream);

        var allWords = await WordDictionnary.GetAllWords(reader);

        const string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var indexLetter = Random.Shared.Next(0, allChars.Length);
        var letter = allChars[indexLetter];

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

    public static HashSet<LetterStatus> GetLetterStatus(string word, string wordToFind)
    {
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
