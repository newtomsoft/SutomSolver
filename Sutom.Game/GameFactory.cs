namespace Sutom.Game;

public static class GameFactory
{
    public static async Task<string> AsyncGetRandomWord()
    {
        var allWords = await WordDictionary.GetAllWords(@"fr.UTF-8.dic");

        const string allLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var indexLetter = Random.Shared.Next(0, allLetters.Length);
        var letter = allLetters[indexLetter];

        var wordsByLength = allWords[letter];

        List<string>? words;
        while (true)
        {
            var wordLength = Random.Shared.Next(5, 11);
            var isNumberLettersSelected = wordsByLength.TryGetValue(wordLength, out words);
            if (isNumberLettersSelected) break;
        }
        var indexWord = Random.Shared.Next(0, words!.Count);
        var wordSelected = words[indexWord];
        return wordSelected;
    }

    public static WordStatus GetWordStatus(string word, string wordToFind)
    {
        word = word.ToUpperInvariant();
        var wordStatus = new WordStatus();
        for (var i = 0; i < wordToFind.Length; i++)
            if (word[i] == wordToFind[i])
                wordStatus.AddLetterStatus(new LetterStatus(i, word[i], Status.GoodPlace));

        var allGoodIndexes = wordStatus.LettersStatuses.Select(l => l.WordIndex).ToList();
        var allGoodLetters = wordStatus.LettersStatuses.Select(l => l.Letter).ToList();

        for (var i = 0; i < wordToFind.Length; i++)
        {
            if (allGoodIndexes.Contains(i)) continue;
            var letter = word[i];
            var inWordCount = wordToFind.Count(l => l == letter);
            var foundCount = allGoodLetters.Count(l => l == letter);
            if (foundCount == inWordCount)
                wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.NotPresent));
            else
            {
                wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.BadPlace));
                allGoodLetters.Add(letter);
            }
        }
        return wordStatus;
    }
}
