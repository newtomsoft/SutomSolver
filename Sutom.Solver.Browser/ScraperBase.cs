namespace Sutom.Solver.Browser;

public abstract class ScraperBase : IScraper
{
    private readonly IWebDriverFactory _webDriverFactory;
    protected readonly GameConfiguration Configuration;
    protected IWebDriver WebDriver = default!;
    protected Dictionary<char, Dictionary<int, List<string>>> AllWords = default!;
    protected Dictionary<string, IWebElement> KeyButtonsByLetter = default!;

    protected const string ValidWordKey = "_ENTREE";


    protected ScraperBase(IWebDriverFactory webDriverFactory, GameConfiguration configuration)
    {
        _webDriverFactory = webDriverFactory;
        Configuration = configuration;
    }

    public abstract Task AsyncPlay();
    public abstract void SetKeyButtonsByLetter();
    public abstract void NavigateGamePage();

    public void Initialize()
    {
        WebDriver = _webDriverFactory.CreateDriver();
        _ = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(10));
    }
    public async Task AsyncGetAllWordsToParse() => AllWords = await WordDictionary.GetAllWords(@"fr.UTF-8.dic");

    public void NavigateHomePage()
    {
        WebDriver.Manage().Window.Size = new System.Drawing.Size(620, 980);
        WebDriver.Navigate().GoToUrl(Configuration.WebsiteUrl);
        var wait = FluentWait.Create(WebDriver);
        wait.WithTimeout(TimeSpan.FromMilliseconds(30000));
        wait.PollingInterval = TimeSpan.FromMilliseconds(250);
        wait.Until(IsPageLoaded);

        bool IsPageLoaded(IWebDriver webDriver)
        {
            try
            {
                _ = WebDriver.FindElement(By.Id(Configuration.ElementIdToFindInHomePage));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    
    public void Click(string id)
    {
        if (string.IsNullOrEmpty(id) is not true) WebDriver.FindElement(By.Id(id)).Click();
    }

    protected async Task AsyncEnterLettersInGrid(string wordFound)
    {
        foreach (var letter in wordFound)
        {
            KeyButtonsByLetter[letter.ToString()].Click();
            await Task.Delay(Configuration.DelayBetween2Letters);
        }
        KeyButtonsByLetter[ValidWordKey].Click();
    }
}