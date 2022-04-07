namespace Sutom.Solver.Browser;

[Serializable]
public class ApplicationConfiguration
{
    public GameConfiguration SutomConfiguration { get; set; } = default!;
    public GameConfiguration TusmoDailyConfiguration { get; set; } = default!;
    public GameConfiguration TusmoRowConfiguration { get; set; } = default!;
    public GameConfiguration TusmoSoloConfiguration { get; set; } = default!;
    public GameConfiguration TusmoMultiConfiguration { get; set; } = default!;
}