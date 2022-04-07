namespace Sutom.Solver.Browser;

public abstract class TusmoBaseScraper : ScraperBase
{
    protected TusmoBaseScraper(IWebDriverFactory webDriverFactory, GameConfiguration configuration) : base(webDriverFactory, configuration) { }
    
    public override async Task AsyncPlay()
    {
        const string patternRepeatToFind = "repeat(";

        while (true)
        {
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
            var useComputedFirstWords = true;
            while (true)
            {
                if (wordsToParse.Count == 0) break;
                var wordFound = useComputedFirstWords ? SutomSolver.GetFirstBestWord(firstLetter, wordLength) : SutomSolver.FindBestWord(wordsToParse);
                useComputedFirstWords = false;

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
            await Task.Delay(1500);
            var endElement = WebDriver.FindElements(By.Name("div")).FirstOrDefault(e => e.Text.Contains("C'est gagné pour aujourd'hui"));
            if (endElement is not null) break;
        }
    }

    private async Task<WordStatus> AsyncGetWordStatus(int attempt, string wordFound)
    {
        await Task.Delay(500);
        var catchesNumber = 0;
        while (true)
        {
            await Task.Delay(300);
            var wordStatus = new WordStatus();
            try
            {
                var grid = WebDriver.FindElement(By.ClassName("motus-grid"));
                var letters = grid.FindElements(By.ClassName("cell-content"));
                for (var i = 0; i < wordFound.Length; i++)
                {
                    var currentElement = letters[attempt * wordFound.Length + i];
                    var letter = currentElement.Text[0];
                    var elementClass = currentElement.GetAttribute("class").Split(' ').Last();

                    switch (elementClass)
                    {
                        case "r":
                            wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.GoodPlace));
                            continue;
                        case "y":
                            wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.BadPlace));
                            continue;
                        case "-":
                            wordStatus.AddLetterStatus(new LetterStatus(i, letter, Status.NotPresent));
                            continue;
                        default:
                            wordStatus.WordNotCompliant();
                            return wordStatus;
                    }
                }
                if (wordStatus.LettersStatuses.Count == wordFound.Length) return wordStatus;

            }
            catch
            {
                catchesNumber++;
                if (catchesNumber < 5) continue;
                wordStatus = new WordStatus();
                for (var i = 0; i < wordFound.Length; i++) wordStatus.AddLetterStatus(new LetterStatus(i, '\0', Status.GoodPlace));
                return wordStatus;
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