using System.Collections.Generic;

namespace API.ESO.Model
{
    public class User
    {
	    public string Username { get; set; }
	    public string Name { get; set; }
	    public string Email { get; set; }
	    public string Password { get; set; }
	    public IList<Course> Courses { get; set; }
    }
}
