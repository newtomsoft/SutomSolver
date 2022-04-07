using Sutom.Share;
using Sutom.Solver;

var allWords = await WordDictionary.GetAllWords(@"fr.UTF-8.dic");


const string allLetters = "RSTUVWXYZ";
var lengths = Enumerable.Range(4, 7);

for (var length = 10 ; length < 11 ; length++)
{
    foreach (var firstLetter in allLetters)
    {
        var wordsToParse = allWords[firstLetter][length];
        var wordFound = SutomSolver.FindBestWord(wordsToParse);
        Console.WriteLine(wordFound);
    }
}


Console.ReadLine();