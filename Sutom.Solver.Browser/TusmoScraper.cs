namespace Sutom.Solver.Browser;

public class TusmoScraper : ScraperBase
{
    public TusmoScraper(IWebDriverFactory webDriverFactory, GameConfiguration configuration) : base(webDriverFactory, configuration) { }

    public override void NavigateGamePage() => WebDriver.FindElements(By.XPath("//div")).First(e => e.Text == Configuration.DivTextToNavigateGamePage).Click();

    public override async Task AsyncPlay()
    {
        const string patternRepeatToFind = "repeat(";

        var grid = WebDriver.FindElement(By.ClassName("motus-grid"));
        var style = grid.GetAttribute("style");
        var repeatLastIndex = style.LastIndexOf(patternRepeatToFind, StringComparison.Ordinal);
        var wordLengthIndex = repeatLastIndex + patternRepeatToFind.Length;
        var lengthString = style.Substring(wordLengthIndex, 2);
        lengthString = lengthString.Replace(',', '\0');
        var wordLength = int.Parse(lengthString);
        var letters = grid.FindElements(By.ClassName("cell-content"));
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

    private async Task<WordStatus> AsyncGetWordStatus(int attempt, string wordFound)
    {
        await Task.Delay(250);
        while (true)
        {
            await Task.Delay(150);
            var wordStatus = new WordStatus();
            try
            {
                var grid = WebDriver.FindElement(By.ClassName("motus-grid"));
                var letters = grid.FindElements(By.ClassName("cell-content"));
                for (var i = 0; i < wordFound.Length; i++)
                {
                    var currentElement = letters[attempt * wordFound.Length + i];
                    var letter = currentElement.Text[0];
                    var elementClass = currentElement.GetAttribute("class").Last();

                    switch (elementClass)
                    {
                        case 'r':
                            wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.GoodPlace));
                            continue;
                        case 'y':
                            wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.BadPlace));
                            continue;
                        case '-':
                            wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.NotPresent));
                            continue;
                    }
                    break;
                }
                if (wordStatus.LettersStatuses.Count == wordFound.Length) return wordStatus;

            }
            catch
            {
                // ignored
            }
        }
    }

    public override void SetKeyButtonsByLetter()
    {
        var elements = WebDriver.FindElements(By.ClassName("key"));
        KeyButtonsByLetter = elements.Where(letterElement => !string.IsNullOrEmpty(letterElement.Text)).ToDictionary(letterElement => letterElement.Text.ToUpperInvariant());
        var enter = WebDriver.FindElement(By.ClassName("fa-sign-in-alt"));
        KeyButtonsByLetter.Add(ValidWordKey, enter);
    }
}