using System;
using System.Collections.Generic;
using System.Text;

namespace API.ESO.Model
{
    public class Chapter
    {
	    public Chapter()
	    {
		    
	    }

		public Chapter(string title)
		{
			Title = title;
		}

		public Chapter(string title, DateTime endDate)
		{
			Title = title;
			EndDate = endDate;
		}

		public string Id { get; set; }
	    public string Title { get; set; }
	    public DateTime EndDate { get; set; }
	    public IList<Chapter> SubChapters { get; set; }
    }
}
