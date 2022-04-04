using Sutom.Share;
using Sutom.Solver;
using Sutom.Solver.Console;

var allWords = await WordDictionary.GetAllWords(@"fr.UTF-8.dic");

while (true)
{
    var (firstLetter, length) = SutomSolverRequest.RequestFirstLetterAndLength();
    var wordsToParse = allWords[firstLetter][length];
    while (true)
    {
        if (wordsToParse.Count == 0)
        {
            Console.WriteLine("No word found :(");
            break;
        }
        var wordFound = SutomSolver.FindBestWord(wordsToParse);
        Console.WriteLine();
        Console.WriteLine(wordFound);
        var letterStatuses = SutomSolverRequest.RequestLettersStatuses(wordFound);
        while (letterStatuses.First().Status == Status.NotPresent)
        {
            Console.WriteLine("Finding other word...");
            wordsToParse.Remove(wordFound);
            wordsToParse = SutomSolver.GetWordsToExplore(letterStatuses, wordsToParse).ToList();
            if (wordsToParse.Count == 0)
            {
                Console.WriteLine("No word found :(");
                break;
            }
            wordFound = SutomSolver.FindBestWord(wordsToParse);
            Console.WriteLine(wordFound);
            letterStatuses = SutomSolverRequest.RequestLettersStatuses(wordFound);
        }

        if (letterStatuses.All(s => s.Status == Status.GoodPlace)) break;
        Console.WriteLine("Finding new word...");
        wordsToParse = SutomSolver.GetWordsToExplore(letterStatuses, wordsToParse).ToList();
    }
}