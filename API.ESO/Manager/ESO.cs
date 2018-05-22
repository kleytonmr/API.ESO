using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.ESO.ConnectionManager;
using API.ESO.Model;
using API.ESO.View;

namespace API.ESO.Manager
{
	public class Eso
	{
		private readonly Connection _connection;
		private static readonly IDictionary<string, string> Form = new Dictionary<string, string>();
		private static User _user;

		private Eso(Connection connection)
		{
			_connection = connection;
		}

		public static async Task<Eso> CreateAsync()
		{
			Connection connection = await Connection.CreateAsync();
			return new Eso(connection);
		}

		public async Task<RequestResponse> LoginAsync(User user)
		{
			Form.Add("email", user.Email);
			Form.Add("password", user.Password);
			RequestResponse response = await _connection.PostPageAsync<Login>(PageList.Login, Form);
			Form.Clear();
			_user = user;
			return response;
		}

		public async Task<User> GetUserInfo()
		{
			RequestResponse<User> response = await _connection.GetPageAsync<UserInfo, User>(PageList.UserInfo);
			User user = response.Object;
			return user;
		}

		public async Task<IList<Course>> GetCoursesFromUser()
		{
			RequestResponse<IList<Course>> response = await _connection.GetPageAsync<Courses, IList<Course>>(PageList.Dashboard);
			IList<Course> courses = response.Object;
			_user.Courses = courses;
			return courses;
		}

		public async Task<IList<Chapter>> GetChaptersFromCourse(Course course)
		{
			Uri uri = PageList.Course(course.Key);
			RequestResponse<IList<Chapter>> response = await _connection.GetPageAsync<Chapters, IList<Chapter>>(uri);
			IList<Chapter> chapters = response.Object;
			course.Chapters = chapters;
			return chapters;
		}
	}
}
