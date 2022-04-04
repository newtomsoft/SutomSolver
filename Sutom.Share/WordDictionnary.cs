namespace Sutom.Share;
public class WordDictionnary
{
    public static async Task<Dictionary<char, Dictionary<int, List<string>>>> GetAllWords(TextReader streamReader)
    {
        const string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var allWords = allChars.ToDictionary(firstChar => firstChar, _ => new Dictionary<int, List<string>>());
        while (true)
        {
            var line = await streamReader.ReadLineAsync();
            if (line is null) break;

            var firstCharToIgnore = line.Length;
            var word = line[..firstCharToIgnore].ToUpperInvariant();
            word = word.RemoveDiacritics();

            if (word.Any(c => !allChars.Contains(c))) continue; //TODO convert words with diacritics ?

            var firstChar = word[0];
            var length = word.Length;

            var subDic = allWords[firstChar];
            var added = subDic.TryAdd(length, new List<string>() {word});
            if (added is false) subDic[length].Add(word);
        }
        return allWords;
    }
}
