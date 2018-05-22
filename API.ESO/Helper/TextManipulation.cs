namespace API.ESO.Helper
{
	public static class TextManipulation
	{
		public static string ExtractValue(string text, int startIndex, string identifierStart, string identifierEnd)
		{
			startIndex = text.IndexOf(identifierStart, startIndex) + identifierStart.Length;
			int endIndex = text.IndexOf(identifierEnd, startIndex);

			if (endIndex == -1)
			{
				endIndex = text.Length;
			}

			return text.Substring(startIndex, endIndex - startIndex);
		}

		public static string ExtractValue(string text, string identifierStart, string identifierEnd)
		{
			int startIndex = text.IndexOf(identifierStart) + identifierStart.Length;
			int endIndex = text.IndexOf(identifierEnd, startIndex);

			if (endIndex == -1)
			{
				endIndex = text.Length;
			}

			return text.Substring(startIndex, endIndex - startIndex);
		}

		public static int CountOcurrence(string text, string subtext)
		{
			return (text.Length - text.Replace(subtext, "").Length) / subtext.Length;
		}
	}
}
