namespace Sutom.Solver;

public class WordStatus
{
    public LetterStatus[] LetterStatuses {get;}
    

    public WordStatus(string word)
    {
        LetterStatuses = new LetterStatus[word.Length];
    }

    public WordStatus(LetterStatus[] letterStatuses)
    {
        LetterStatuses = letterStatuses;
    }
}