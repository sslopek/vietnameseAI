using System.Globalization;
using System.Text;

namespace VietnameseAI.Shared.Utilities;
public static class StringExtensions
{
	public static bool ContainsIgnoringAccentsAndCase(this string source, string target)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(target);

		string normalizedSource = RemoveDiacritics(source);
		string normalizedSubstring = RemoveDiacritics(target);

		return normalizedSource.ToLower().Contains(normalizedSubstring.ToLower());
	}

	private static string RemoveDiacritics(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}

		// Normalize the string to FormD (decomposed)
		var normalizedString = text.Normalize(NormalizationForm.FormD);

		// Remove diacritics by filtering out non-letter characters
		var stringBuilder = new StringBuilder();
		foreach (var c in normalizedString)
		{
			var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
			if (unicodeCategory != UnicodeCategory.NonSpacingMark)
			{
				stringBuilder.Append(c);
			}
		}

		// Normalize back to FormC (composed)
		return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
	}
}
