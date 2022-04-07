namespace Sutom.Solver;

public class WordStatus
{
    public HashSet<LetterStatus> LettersStatuses { get; }
    public bool WordCompliant { get; private set; }

    public WordStatus()
    {
        LettersStatuses = new HashSet<LetterStatus>();
        WordCompliant = true;
    }

    public void AddLetterStatus(LetterStatus letterStatus) => LettersStatuses.Add(letterStatus);

    public bool IsWordFound() => LettersStatuses.All(l => l.Status == Status.GoodPlace);

    public void WordNotCompliant() => WordCompliant = false;
}