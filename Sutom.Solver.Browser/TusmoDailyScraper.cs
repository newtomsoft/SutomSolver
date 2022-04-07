namespace Sutom.Solver.Browser;

public class TusmoDailyScraper : TusmoBaseScraper
{
    public TusmoDailyScraper(IWebDriverFactory webDriverFactory, GameConfiguration configuration) : base(webDriverFactory, configuration) { }

    public override void NavigateGamePage() => WebDriver.FindElements(By.XPath("//div")).First(e => e.Text == Configuration.DivTextToNavigateGamePage).Click();

}