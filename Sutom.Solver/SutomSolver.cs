namespace Sutom.Solver;

public static class SutomSolver
{
    public static IEnumerable<string> GetReducedWordsToParse(WordStatus wordStatus, List<string> wordsToParse)
    {
        if (wordStatus.LettersStatuses.Count != wordsToParse.First().Length) throw new ArgumentException("wordStatus with bad letter status number");
        var letterFound = new List<char>();
        foreach (var letterStatus in wordStatus.LettersStatuses.Where(item => item.Status == Status.GoodPlace))
        {
            wordsToParse = wordsToParse.Where(word => word[letterStatus.WordIndex] == letterStatus.Letter).ToList();
            letterFound.Add(letterStatus.Letter);
        }
        foreach (var letterStatus in wordStatus.LettersStatuses.Where(item => item.Status == Status.BadPlace))
        {
            var letter = letterStatus.Letter;
            var index = letterStatus.WordIndex;
            var minNumberToCount = letterFound.Count(l => l == letter) + 1;
            wordsToParse = wordsToParse.Where(word => word[index] != letter && word.Count(l => l == letter) >= minNumberToCount).ToList();
            letterFound.Add(letter);
        }
        foreach (var letterStatus in wordStatus.LettersStatuses.Where(item => item.Status == Status.NotPresent))
        {
            var letter = letterStatus.Letter;
            var numberToCount = letterFound.Count(l => l == letter);
            wordsToParse = wordsToParse.Where(word => word.Count(l => l == letter) == numberToCount).ToList();
        }
        return wordsToParse;
    }

    public static string FindBestWord(List<string> wordsToParse)
    {
        var maxScore = 0;
        var currentSelectedWord = string.Empty;

        foreach (var wordToCompare in wordsToParse)
        {
            var score = wordsToParse.Sum(currentWord => wordToCompare.Score(currentWord));

            if (score > maxScore)
            {
                maxScore = score;
                currentSelectedWord = wordToCompare;
            }
        }
        return currentSelectedWord;
    }

    public static string GetFirstBestWord(char firstLetter, int length)
    {
        var words = new List<string>
        {
            "AIRE",
            "BAIE",
            "CALE",
            "DOUE",
            "EMIE",
            "FILE",
            "GARE",
            "HAIE",
            "INDE",
            "JOUE",
            "KILS",
            "LAIE",
            "MARE",
            "NOIE",
            "ORES",
            "PAIE",
            "QUAI",
            "RAIE",
            "SOIE",
            "TARE",
            "UNES",
            "VILE",
            "WATT",
            "ARISE",
            "BASEE",
            "CARIE",
            "DOREE",
            "ETAIE",
            "FORES",
            "GAREE",
            "HALES",
            "IODES",
            "JOUES",
            "KALIS",
            "LAIES",
            "MATES",
            "NOIES",
            "ORTIE",
            "PAIES",
            "QUINE",
            "RAIES",
            "SAURE",
            "TARES",
            "USINE",
            "VILES",
            "WHIGS",
            "XERUS",
            "YACKS",
            "ZIBES",
            "ARASEE",
            "BRAIES",
            "CARIES",
            "DERAIE",
            "ETAIES",
            "FARTES",
            "GAINES",
            "HARLES",
            "INDUES",
            "JAUNES",
            "KARMAS",
            "LAITES",
            "MARIES",
            "NOIRES",
            "OUTRES",
            "PARIES",
            "QUITTE",
            "RENAIS",
            "SAURES",
            "TARIES",
            "URINES",
            "VARIES",
            "WATERS",
            "XENONS",
            "YEOMEN",
            "ZIBEES",
            "ALAIRES",
            "BORATES",
            "CANTRES",
            "DERAIES",
            "ENRAIES",
            "FLAIRES",
            "GRAINES",
            "HARPIES",
            "INCITES",
            "JAUNIES",
            "KARITES",
            "LATINES",
            "MARIEES",
            "NAVIRES",
            "OTARIES",
            "PARTIES",
            "QUITTES",
            "RELAIES",
            "SAURIES",
            "TARITES",
            "URANIES",
            "VARIEES",
            "WALLONS",
            "XIMENIE",
            "YOUPINS",
            "ZIBATES",
            "ASTERIES",
            "BARIOLES",
            "CORNATES",
            "DEPARIES",
            "ENCRATES",
            "FOIRATES",
            "GRANITES",
            "HALEINES",
            "INCURIES",
            "JAUNITES",
            "KANTIENS",
            "LANIERES",
            "MANIERES",
            "NOTARIES",
            "OUATINES",
            "PANIERES",
            "QUARTEES",
            "RENIATES",
            "SAURITES",
            "TARTINES",
            "URINATES",
            "VARIETES",
            "WARRANTS",
            "XANTHINE",
            "YACHTING",
            "ZIGUATES",
            "ACTUAIRES",
            "BATTERIES",
            "CONTERAIS",
            "DEPARTIES",
            "ENCIRATES",
            "FLANERIES",
            "GRANITEES",
            "HARMONIES",
            "INVERTIES",
            "JAUGERONS",
            "KERATINES",
            "LAITERIES",
            "MARONITES",
            "NOTARIEES",
            "OUATINEES",
            "PANTIERES",
            "QUARTILES",
            "RECRIATES",
            "SURLIATES",
            "TRANSITES",
            "URINAIRES",
            "VANTERIES",
            "WARRANTES",
            "XENOPHILE",
            "YTTERBINE",
            "ZIGUERONS",
            "ARSENIATES",
            "BALAIERONS",
            "CONTRARIES",
            "DELAIERONS",
            "ENTOLERAIS",
            "FAUTERIONS",
            "GRANULITES",
            "HARMONISES",
            "INCERTAINS",
            "JAUGERIONS",
            "KIDNAPPEES",
            "LAMINERONS",
            "MONETAIRES",
            "NOTARIALES",
            "ORIENTALES",
            "PRELATINES",
            "QUARTAIENT",
            "REPAIERONS",
            "SURFILATES",
            "UNITARIENS",
            "VALISERONS",
            "WARRANTEES",
            "XENOPHILES",
            "YTTERBINES",
            "ZIEUTERONS",
        };
        return words.Single(w => w[0] == firstLetter && w.Length == length);
    }
}
