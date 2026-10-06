using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SmartShoppingAssistant.BusinessLogic.Helpers;

public static class SlugHelper
{
    // "Cafea & Ceai Brașov" -> "cafea-ceai-brasov"
    public static string ToSlug(string value)
    {
        var normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var withoutDiacritics = new StringBuilder();
        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                withoutDiacritics.Append(c);
        }

        var slug = Regex.Replace(withoutDiacritics.ToString(), "[^a-z0-9]+", "-").Trim('-');
        return slug == "" ? "company" : slug;
    }
}
