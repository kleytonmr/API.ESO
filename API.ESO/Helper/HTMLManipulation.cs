using System.Collections.Generic;

namespace API.ESO.Helper
{
	public static class HtmlManipulation
	{
		public static string InnerHtml(string html, string identifier, bool recursive = false)
		{
			int indexIdentifier = html.IndexOf(identifier);
			if (indexIdentifier == -1)
			{
				return string.Empty;
			}
			int indexStartTag = html.LastIndexOf('<', indexIdentifier);
			string parentHtml = ParentHtml(html, indexStartTag);
			int indexStartInnerHtml = parentHtml.IndexOf('>') + 1;
			int indexEndInnerHtml = parentHtml.LastIndexOf('<');
			string innerHtml = parentHtml.Substring(indexStartInnerHtml, indexEndInnerHtml - indexStartInnerHtml);
			if (recursive && innerHtml.IndexOf('>') > -1)
			{
				innerHtml = InnerHtml(innerHtml, "<", true);
			}
			return innerHtml;
		}

		public static string ParentHtml(string html, int index)
		{
			int indexStartTag = index + 1;
			int indexEndTag = html.IndexOf(' ', indexStartTag);
			string tag = html.Substring(indexStartTag, indexEndTag - indexStartTag);
			string closeTag = $"</{tag}>";
			string openTag = $"<{tag} ";
			indexEndTag = html.IndexOf(closeTag, indexEndTag);
			indexStartTag--;
			string innerHtml = html.Substring(indexStartTag, indexEndTag - indexStartTag + closeTag.Length);
			int countOpenedTags = TextManipulation.CountOcurrence(innerHtml, openTag);
			int countClosedTags = TextManipulation.CountOcurrence(innerHtml, closeTag);
			int countTags = countOpenedTags - countClosedTags;
			while (countTags > 0)
			{
				indexEndTag = html.IndexOf(closeTag, indexEndTag + 1);
				innerHtml = html.Substring(indexStartTag, indexEndTag - indexStartTag + closeTag.Length);
				bool hasOpenedTags = TextManipulation.CountOcurrence(innerHtml, openTag) - countOpenedTags == 0;
				if (hasOpenedTags)
				{
					countTags--;
				}
				else
				{
					countOpenedTags++;
				}
			}

			return innerHtml;
		}

		public static string AttributeValue(string html, string attribute)
		{
			int indexAttribute = html.IndexOf(attribute);

			if (indexAttribute == -1)
			{
				return string.Empty;
			}

			indexAttribute += attribute.Length;
			return TextManipulation.ExtractValue(html, indexAttribute, "=\"", "\"");
		}

		public static IList<string> GroupTags(string html)
		{
			List<string> innerHtmlTags = new List<string>();
			int indexStartTag = html.IndexOf('<');
			while (indexStartTag > -1)
			{
				string innerHtml = ParentHtml(html, indexStartTag);
				innerHtmlTags.Add(innerHtml);
				indexStartTag = html.IndexOf('<', innerHtml.Length + indexStartTag);
			}

			return innerHtmlTags;
		}
	}
}
