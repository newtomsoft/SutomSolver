namespace Sutom.Solver.Console;

public static class SutomSolverRequest
{
    public static char RequestFirstLetter(out int length)
    {
        System.Console.WriteLine();
        System.Console.WriteLine("First Letter");
        var inputFirstLetter1 = System.Console.ReadKey().KeyChar.ToString().ToUpperInvariant()[0];
        System.Console.WriteLine();
        System.Console.WriteLine("Length");
        length = int.Parse(System.Console.ReadKey().KeyChar.ToString());
        System.Console.WriteLine();
        return inputFirstLetter1;
    }

    public static HashSet<LetterStatus> RequestLettersStatus(string word)
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
