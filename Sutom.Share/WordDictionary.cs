namespace Sutom.Share;

public static class WordDictionary
{
    public static async Task<Dictionary<char, Dictionary<int, List<string>>>> GetAllWords(string fileName)
    {
        await using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(fileStream);
        
        const string allLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var wordsByFirstLetterThenLength = allLetters.ToDictionary(firstChar => firstChar, _ => new Dictionary<int, List<string>>());
        while (true)
        {
            var line = await reader.ReadLineAsync();
            if (line is null) break;

            var firstLetterIgnored = line.Length;
            var word = line[..firstLetterIgnored].ToUpperInvariant();
            word = word.RemoveDiacritics();

            if (word.Any(letter => !allLetters.Contains(letter))) continue; //TODO convert words with diacritics ?

            var firstLetter = word[0];
            var length = word.Length;
            var wordsByLength = wordsByFirstLetterThenLength[firstLetter];
            var added = wordsByLength.TryAdd(length, new List<string> {word});
            if (added is false) wordsByLength[length].Add(word);
        }
        return wordsByFirstLetterThenLength;
    }
}
