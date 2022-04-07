var allWords = await WordDictionary.GetAllWords(@"fr.UTF-8.dic");

while (true)
{
    var (firstLetter, length) = SutomSolverRequest.RequestFirstLetterAndLength();
    var wordsToParse = allWords[firstLetter][length];
    var firstAttempt = true;
    while (true)
    {
        if (wordsToParse.Count == 0)
        {
            Console.WriteLine("No word found :(");
            break;
        }
        var wordFound = firstAttempt ? SutomSolver.GetFirstBestWord(firstLetter, length) : SutomSolver.FindBestWord(wordsToParse);
        firstAttempt = false;
        Console.WriteLine();
        Console.WriteLine(wordFound);
        var wordStatus = SutomSolverRequest.RequestWordStatus(wordFound);
        while (wordStatus.WordCompliant is false)
        {
            Console.WriteLine("Finding other word...");
            wordsToParse.Remove(wordFound);
            if (wordsToParse.Count == 0)
            {
                Console.WriteLine("Sorry, no word found :(");
                break;
            }
            wordFound = SutomSolver.FindBestWord(wordsToParse);
            Console.WriteLine(wordFound);
            wordStatus = SutomSolverRequest.RequestWordStatus(wordFound);
        }

        if (wordStatus.IsWordFound()) break;
        Console.WriteLine("Finding new word...");
        wordsToParse = SutomSolver.GetReducedWordsToParse(wordStatus, wordsToParse).ToList();
    }
    Console.WriteLine("Other word to find ?");
}