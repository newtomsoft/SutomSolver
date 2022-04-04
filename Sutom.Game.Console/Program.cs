using Sutom.Game;
using Sutom.Solver;

var wordToFind = await GameFactory.GetRandomWord();

Console.Write($"{wordToFind[0]}");
for (var i = 1; i < wordToFind.Length; i++)
{
    Console.Write("-");
}

while (true)
{
    Console.WriteLine("\nwrite word");
    var wordToCompute = Console.ReadLine();
    Console.WriteLine();

    //TODO is word in dictionary

    var lettersStatus = GameFactory.GetLetterStatus(wordToCompute, wordToFind);
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