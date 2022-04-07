namespace Sutom.Solver.Browser.WebDriverFactory;

public class EdgeDriverFactory : IWebDriverFactory
{
    public IWebDriver CreateDriver()
    {
        new DriverManager().SetUpDriver(new EdgeConfig());
        return new EdgeDriver();
    }
}