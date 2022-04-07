using System.Globalization;

namespace Sutom.Share;

public static class StringExtension
{
    public static string RemoveDiacritics(this string inputString)
    {
        var formDString = inputString.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var currentChar in formDString)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(currentChar);
            if (uc != UnicodeCategory.NonSpacingMark) sb.Append(currentChar);
        }
        return (sb.ToString().Normalize(NormalizationForm.FormC));
    }
}
