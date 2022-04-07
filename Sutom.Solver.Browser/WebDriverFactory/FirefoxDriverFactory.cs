﻿namespace Sutom.Solver.Browser.WebDriverFactory;

public class FirefoxDriverFactory : IWebDriverFactory
{
    public IWebDriver CreateDriver()
    {
        new DriverManager().SetUpDriver(new FirefoxConfig());
        var driver = new FirefoxDriver();
        return driver;
    }
}