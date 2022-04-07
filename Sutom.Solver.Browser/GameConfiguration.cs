namespace Sutom.Solver.Browser;

[Serializable]
public class GameConfiguration
{
    public bool Play { get; set; } = default!;
    public string WebsiteUrl { get; set; } = default!;
    public string ElementIdToFindInHomePage { get; set; } = default!;
    public string ElementIdClickToNavigateGamePage { get; set; } = default!;
    public string DivTextToNavigateGamePage { get; set; } = default!;
    public int DelayBetween2Letters { get; set; } = default!;
}