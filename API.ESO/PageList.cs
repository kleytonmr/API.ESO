using System;

namespace API.ESO
{
	internal static class PageList
	{
		/// <summary>
		/// Página de menor tamanho (bytes) da plataforma ESO.
		/// </summary>
		public static Uri Small => new Uri("https://eso.org.br/contact");

		/// <summary>
		/// Página base do ESO.
		/// </summary>
		public static Uri Base => new Uri("https://eso.org.br");

		/// <summary>
		///	Endereço de login.
		/// </summary>
		public static Uri Login => new Uri("https://eso.org.br/api/login_ajax");

		/// <summary>
		/// Endereço das informações do usuário logado.
		/// </summary>
		public static Uri UserInfo => new Uri("https://eso.org.br/api/user/v1/accounts");

		/// <summary>
		/// Página principal da ESO após login.
		/// </summary>
		public static Uri Dashboard => new Uri("https://eso.org.br/dashboard ");

		/// <summary>
		/// Página do curso.
		/// </summary>
		public static Uri Course(string courseKey) => new Uri($"https://eso.org.br/courses/{courseKey}/info");

		/// <summary>
		/// Grade do curso.
		/// </summary>
		public static Uri Grade(string courseKey) => new Uri($"https://eso.org.br/courses/{courseKey}/info");
	}
}
