namespace Sutom.Solver.Browser;

public sealed class SutomScraper : ScraperBase
{
    public SutomScraper(IWebDriverFactory webDriverFactory, GameConfiguration configuration) : base(webDriverFactory, configuration) { }

    public override void NavigateGamePage() => Click(Configuration.ElementIdClickToNavigateGamePage);

    public override async Task AsyncPlay()
    {
        var grid = WebDriver.FindElement(By.Id("grille"));
        var letters = grid.FindElements(By.XPath("//table/tr/td"));
        var lines = grid.FindElements(By.XPath("//table/tr"));
        var wordLength = letters.Count / lines.Count;
        var firstLetter = letters[0].Text[0];

        var wordsToParse = AllWords[firstLetter][wordLength];
        var attempt = 0;
        while (true)
        {
            if (wordsToParse.Count == 0) break;
            var wordFound = SutomSolver.FindBestWord(wordsToParse);
            await AsyncEnterLettersInGrid(wordFound);
            var wordStatus = await AsyncGetWordStatus(attempt, wordFound);
            while (wordStatus.WordCompliant is false)
            {
                wordsToParse.Remove(wordFound);
                if (wordsToParse.Count == 0) break;
                wordFound = SutomSolver.FindBestWord(wordsToParse);
                await AsyncEnterLettersInGrid(wordFound);
                wordStatus = await AsyncGetWordStatus(attempt, wordFound);
            }
            if (wordStatus.IsWordFound()) break;
            attempt++;
            wordsToParse = SutomSolver.GetReducedWordsToParse(wordStatus, wordsToParse).ToList();
        }
    }

    public override void SetKeyButtonsByLetter() =>
        KeyButtonsByLetter = WebDriver.FindElements(By.ClassName("input-lettre"))
                                        .Where(element => element.GetAttribute("data-lettre") is not null)
                                        .ToDictionary(letterElement => letterElement.GetAttribute("data-lettre").ToUpperInvariant());

    private async Task<WordStatus> AsyncGetWordStatus(int attempt, string wordFound)
    {
        await Task.Delay(500);
        while (true)
        {
            await Task.Delay(300);
            var wordStatus = new WordStatus();
            try
            {
                var grid = WebDriver.FindElement(By.Id("grille"));
                var letters = grid.FindElements(By.XPath("//table/tr/td"));
                for (var i = 0; i < wordFound.Length; i++)
                {
                    var currentElement = letters[attempt * wordFound.Length + i];
                    var letter = currentElement.Text[0];
                    var elementClass = currentElement.GetAttribute("class");
                    if (elementClass.Contains("bien-place")) wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.GoodPlace));
                    else if (elementClass.Contains("mal-place")) wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.BadPlace));
                    else if (elementClass.Contains("non-trouve")) wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.NotPresent));
                }
                if (wordStatus.LettersStatuses.Count == wordFound.Length) return wordStatus;

            }
            catch
            {
                // ignored
            }
        }
    }
}