using System;
using System.Collections.Generic;

namespace API.ESO.Model
{
	public class Course
	{
		public Course(string key, string name, DateTime startDate)
		{
			Key = key;
			Name = name;
			StartDate = startDate;
		}

		public Course()
		{

		}

		public Course(string id, string idChapterStart, string title, DateTime endDate)
		{
			Id = id;
			IdChapterStart = idChapterStart;
			Title = title;
			EndDate = endDate;
		}

		public string Id { get; set; }
		public string IdChapterStart { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public IList<Chapter> Chapters { get; set; }
	}
}
