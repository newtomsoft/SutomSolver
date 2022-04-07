using System.Reflection;

namespace Sutom.Solver.Browser;

public class GameSolverApplication
{
    private readonly IWebDriverFactory _webDriverFactory;


    public GameSolverApplication(IWebDriverFactory webDriverFactory)
    {
        _webDriverFactory = webDriverFactory;
    }

    public async Task Run(ApplicationConfiguration configuration)
    {
        var gameConfigurations = configuration.GetType().GetProperties();
        foreach (var gameConfiguration in gameConfigurations)
        {
            var configurationValue = (GameConfiguration)gameConfiguration.GetValue(configuration)!;
            if (configurationValue is { Play: false }) continue;

            var type = GetType(gameConfiguration);
            var scraper = (IScraper)Activator.CreateInstance(type, _webDriverFactory, configurationValue)!;

            await Play(scraper);
        }
    }

    private Type GetType(PropertyInfo gameConfiguration)
    {
        var typeName = gameConfiguration.ToString()!.Split(' ').Last();
        typeName = typeName.Replace("Configuration", "Scraper");
        var assemblyName = GetType().Assembly.GetName().Name!;
        return Type.GetType($"{assemblyName}.{typeName}")!;
    }

    private static async Task Play(IScraper scraper)
    {
        scraper.Initialize();
        scraper.NavigateHomePage();
        scraper.NavigateGamePage();
        await scraper.AsyncGetAllWordsToParse();
        scraper.SetKeyButtonsByLetter();
        await scraper.AsyncPlay();
    }
}