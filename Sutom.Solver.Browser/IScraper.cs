namespace Sutom.Solver.Browser;

public interface IScraper
{
    void NavigateHomePage();
    void NavigateGamePage();
    void Initialize();
    Task AsyncPlay();
    void Click(string id);
    Task AsyncGetAllWordsToParse();
    void SetKeyButtonsByLetter();
}