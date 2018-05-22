using System;
using API.ESO.ConnectionManager;
using API.ESO.Helper;
using API.ESO.Model;

namespace API.ESO.View
{
	internal class CouseInitial : IDeserializable<Course>
	{
		public RequestResponse<Course> DeserializeResponse(string response)
		{
			string description = HtmlManipulation.InnerHtml(response, " article-content ").Trim().Replace("\r", "").Replace("\t", "").Replace("<p>", "").Replace("</p>", Environment.NewLine);
			string startLinkCourse = HtmlManipulation.InnerHtml(response, "page-header-secondary");
			startLinkCourse = HtmlManipulation.AttributeValue(startLinkCourse, "href");
			string[] identifiers = startLinkCourse.Split('/');
			int length = identifiers.Length;
			string dateHtml = HtmlManipulation.InnerHtml(response, "date-summary-end-date");
			string date = HtmlManipulation.AttributeValue(dateHtml, "data-datetime"); 
			date = TextManipulation.ExtractValue(date, "", "+");
			Course course = new Course(identifiers[length - 1], identifiers[length - 2], description, DateTime.Parse(date));

			return new RequestResponse<Course>(RequestResponse.Success, course);
		}
	}
}
