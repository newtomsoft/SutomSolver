namespace Sutom.Solver.Browser;

public class TusmoRowScraper : TusmoBaseScraper
{
    public TusmoRowScraper(IWebDriverFactory webDriverFactory, GameConfiguration configuration) : base(webDriverFactory, configuration) { }

    public override void NavigateGamePage() => WebDriver.FindElements(By.XPath("//div")).First(e => e.Text == Configuration.DivTextToNavigateGamePage).Click();

}