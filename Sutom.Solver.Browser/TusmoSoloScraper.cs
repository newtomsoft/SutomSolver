namespace Sutom.Solver.Browser;

public class TusmoSoloScraper : TusmoBaseScraper
{
    public TusmoSoloScraper(IWebDriverFactory webDriverFactory, GameConfiguration configuration) : base(webDriverFactory, configuration) { }

    public override void NavigateGamePage() => WebDriver.FindElements(By.XPath("//span")).First(e => e.Text == Configuration.DivTextToNavigateGamePage).Click();

}