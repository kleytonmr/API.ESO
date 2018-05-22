using API.ESO.ConnectionManager;
using API.ESO.Helper;
using API.ESO.Model;
using System;
using System.Collections.Generic;

namespace API.ESO.View
{
	internal class Chapters : IDeserializable<IList<Chapter>>
	{
		public RequestResponse<IList<Chapter>> DeserializeResponse(string response)
		{
			string containerChaptersHtml = HtmlManipulation.InnerHtml(response, "course-navigation");
			IList<string> tags = HtmlManipulation.GroupTags(containerChaptersHtml);
			IList<Chapter> chapters = new List<Chapter>((tags.Count / 2) - 1);
			Chapter chapter = new Chapter();
			foreach (string tag in tags)
			{
				if (tag.StartsWith("<a"))
				{
					const string identifyText = "</span>";
					int indexStart = tag.IndexOf(identifyText) + identifyText.Length;
					int indexEnd = tag.IndexOf(identifyText, indexStart);
					chapter.Title = tag.Substring(indexStart, indexEnd - indexStart).Trim();
				}
				else if (tag.StartsWith("<div"))
				{
					string inner = HtmlManipulation.InnerHtml(tag, "chapter-menu");
					IList<string> divs = HtmlManipulation.GroupTags(inner);
					IList<Chapter> subChapters = new List<Chapter>(divs.Count);

					foreach (string div in divs)
					{
						string title = HtmlManipulation.InnerHtml(div, "p", true).Trim();
						string date = HtmlManipulation.AttributeValue(div, "data-datetime");
						date = TextManipulation.ExtractValue(date, "", "+");

						if (string.IsNullOrEmpty(date) || date == "None")
						{
							subChapters.Add(new Chapter(title));
						}
						else
						{
							subChapters.Add(new Chapter(title, DateTime.Parse(date)));
						}
					}

					chapter.SubChapters = subChapters;
					chapters.Add(chapter);
					chapter = new Chapter();
				}
			}

			return new RequestResponse<IList<Chapter>>(RequestResponse.Success, chapters);
		}
	}
}
