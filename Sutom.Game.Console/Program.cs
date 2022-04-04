using Sutom.Game;
using Sutom.Solver;

while (true)
{

    var wordToFind = await GameFactory.GetRandomWord();

    Console.Write($"{wordToFind[0]}");
    for (var i = 1; i < wordToFind.Length; i++)
    {
        Console.Write("-");
    }

    while (true)
    {
        Console.WriteLine("\nwrite word");
        var word = Console.ReadLine();
        Console.WriteLine();
        if (string.IsNullOrEmpty(word) || word.Length < wordToFind.Length)
        {
            Console.WriteLine("\nWord too short");
            continue;
        }

        if (word.Length > wordToFind.Length)
        {
            var newWord = word[..wordToFind.Length];
            Console.WriteLine($"{word} too long -> {newWord}");
            word = newWord;
        }
        //TODO is word in dictionary

        var lettersStatus = GameFactory.GetLetterStatuses(word, wordToFind);
        if (lettersStatus.All(l => l.Status == Status.GoodPlace)) break;
        foreach (var letterStatus in lettersStatus.OrderBy(l => l.WordIndex))
        {
            switch (letterStatus.Status)
            {
                case Status.GoodPlace:
                    Console.Write(letterStatus.Letter);
                    break;
                case Status.BadPlace:
                    Console.Write(letterStatus.Letter.ToString().ToLower());
                    break;
                case Status.NotPresent:
                    Console.Write('-');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine("great!");

    Console.WriteLine("New game ? (y/n)");
    var response = Console.ReadKey().KeyChar.ToString().ToLowerInvariant()[0];
    if (response != 'y') break;
}