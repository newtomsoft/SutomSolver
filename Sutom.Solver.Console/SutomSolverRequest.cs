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

    public static HashSet<LetterStatus> RequestLettersStatuses(string word)
    {
        while (true)
        {
            System.Console.WriteLine("\nGB0 ? (space if word not compliant)");
            var lettersStatus = new HashSet<LetterStatus>();
            var inputString = System.Console.ReadLine();
            if (string.IsNullOrEmpty(inputString)) continue;
            if (inputString.Contains(' ')) return new HashSet<LetterStatus>() { new(0, 'Z', Status.NotPresent) };
            inputString = inputString.ToUpperInvariant();
            for (var i = 0; i < word.Length; i++)
            {
                var status = inputString[i] switch
                {
                    'G' => Status.GoodPlace,
                    'B' => Status.BadPlace,
                    '0' => Status.NotPresent,
                    _ => Status.NotPresent,
                };
                lettersStatus.Add(new LetterStatus(i, word[i], status));
            }
            return lettersStatus;
        }
    }
}
