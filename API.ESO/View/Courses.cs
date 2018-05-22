using API.ESO.ConnectionManager;
using API.ESO.Helper;
using API.ESO.Model;
using System;
using System.Collections.Generic;

namespace API.ESO.View
{
	internal class Courses : IDeserializable<IList<Course>>
	{
		/// <inheritdoc />
		public RequestResponse<IList<Course>> DeserializeResponse(string response)
		{
			string[] containerCoursesHtml = response.Split(new[] { "<article class=\"course\">" }, StringSplitOptions.None);
			IList<Course> courses = new List<Course>(containerCoursesHtml.Length - 1);

			for (int i = 1; i < containerCoursesHtml.Length; i++)
			{
				string courseHtml = containerCoursesHtml[i];
				string name = HtmlManipulation.InnerHtml(courseHtml, "course-title", true);
				string key = HtmlManipulation.AttributeValue(courseHtml, "data-course-key");
				string html = HtmlManipulation.InnerHtml(courseHtml, "course-info");
				string date = HtmlManipulation.AttributeValue(html, "data-datetime");
				date = TextManipulation.ExtractValue(date, "", "T");
				courses.Add(new Course(key, name, DateTime.Parse(date)));
			}

			return new RequestResponse<IList<Course>>(RequestResponse.Success, courses);
		}
	}
}
