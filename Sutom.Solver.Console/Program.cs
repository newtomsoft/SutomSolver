using Sutom.Share;
using Sutom.Solver;
using Sutom.Solver.Console;

await using var fileStream = new FileStream(@"fr.UTF-8.dic", FileMode.Open, FileAccess.Read);
using var reader = new StreamReader(fileStream);

var allWords = await WordDictionnary.GetAllWords(reader);

while (true)
{
    var inputFirstLetter = SutomSolverRequest.RequestFirstLetter(out var inputLength);
    var wordsToExplore = allWords[inputFirstLetter][inputLength];
    while (true)
    {
        if (wordsToExplore.Count == 0)
        {
            Console.WriteLine("No word found :(");
            break;
        }
        var wordFound = SutomSolver.FindBestWord(wordsToExplore);
        Console.WriteLine($"\n{wordFound}");
        var letterStatus = SutomSolverRequest.RequestLettersStatus(wordFound);
        while (letterStatus.First().Status == Status.NotPresent)
        {
            Console.WriteLine("Finding new word...");
            wordsToExplore.Remove(wordFound);
            wordsToExplore = SutomSolver.GetWordsToExplore(letterStatus, wordsToExplore).ToList();
            if (wordsToExplore.Count == 0)
            {
                Console.WriteLine("No word found :(");
                break;
            }
            wordFound = SutomSolver.FindBestWord(wordsToExplore);
            Console.WriteLine(wordFound);
            letterStatus = SutomSolverRequest.RequestLettersStatus(wordFound);
        }

        if (letterStatus.All(s => s.Status == Status.GoodPlace)) break;
        Console.WriteLine("Finding new word...");
        wordsToExplore = SutomSolver.GetWordsToExplore(letterStatus, wordsToExplore).ToList();
    }
}