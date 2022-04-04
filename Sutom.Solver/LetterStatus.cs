namespace Sutom.Solver;

public class LetterStatus
{
    public int WordIndex { get; }
    public char Letter { get; }
    public Status Status { get; }

    public LetterStatus(int wordIndex, char letter, Status status)
    {
        WordIndex = wordIndex;
        Letter = letter;
        Status = status;
    }
}