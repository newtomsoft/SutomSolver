namespace Sutom.Solver.Console;

public static class SutomSolverRequest
{
    public static (char, int) RequestFirstLetterAndLength()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("First Letter");
        var inputFirstLetter = System.Console.ReadKey().KeyChar.ToString().ToUpperInvariant()[0];
        System.Console.WriteLine();
        System.Console.WriteLine("Length");
        var length = int.Parse(System.Console.ReadKey().KeyChar.ToString());
        System.Console.WriteLine();
        return (inputFirstLetter, length);
    }

    public static WordStatus RequestWordStatus(string word)
    {
        while (true)
        {
            System.Console.WriteLine("\nGBN (Good/Bad/No) ? (empty if word not compliant)");
            var inputString = System.Console.ReadLine();

            var wordStatus = new WordStatus();
            if (string.IsNullOrEmpty(inputString))
            {
                wordStatus.WordNotCompliant();
                return wordStatus;
            }

            inputString = inputString.ToUpperInvariant();
            for (var i = 0; i < word.Length; i++)
            {
                var status = inputString[i] switch
                {
                    'G' => Status.GoodPlace,
                    'B' => Status.BadPlace,
                    'N' => Status.NotPresent,
                    _ => Status.NotPresent,
                };
                wordStatus.AddLetterStatus(new LetterStatus(i, word[i], status));
            }
            return wordStatus;
        }
    }
}
