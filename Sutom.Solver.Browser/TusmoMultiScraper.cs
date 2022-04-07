namespace Sutom.Solver.Browser;

public class TusmoMultiScraper : TusmoBaseScraper
{
    public TusmoMultiScraper(IWebDriverFactory webDriverFactory, GameConfiguration configuration) : base(webDriverFactory, configuration) { }

    public override void NavigateGamePage()
    {
        WebDriver.FindElements(By.XPath("//span")).First(e => e.Text == Configuration.DivTextToNavigateGamePage).Click();
        EditName();
        ClickJoin();
        WaitGameStart();
    }
    
    private void EditName()
    {
        Task.Delay(1000).Wait();
        WebDriver.FindElement(By.ClassName("edit-username-button")).Click();
        for (var i = 0; i < 10; i++) WebDriver.FindElement(By.ClassName("menu-input")).SendKeys(Keys.Backspace);
        WebDriver.FindElement(By.ClassName("menu-input")).SendKeys("Mélenchon (bot)");
    }

    private void ClickJoin() => WebDriver.FindElements(By.XPath("//span")).First(e => e.Text == "REJOINDRE").Click();

    private void WaitGameStart()
    {
        while (true)
        {
            try
            {
                WebDriver.FindElement(By.ClassName("motus-grid"));
                break;
            }
            catch
            {
                Task.Delay(1000).Wait();
            }
        }
    }
}