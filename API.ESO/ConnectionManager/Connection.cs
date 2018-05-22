using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.ESO.ConnectionManager
{
	internal class Connection
	{
		/// <summary>
		///     Instância compartilhada de HttpClient.
		/// </summary>
		private readonly HttpClient _httpClient;

		/// <summary>
		///     Instância compartilhada do handler do <see cref="_handlerClient" />
		/// </summary>
		private readonly HttpClientHandler _handlerClient;

		private Connection()
		{
			_handlerClient = new HttpClientHandler { UseCookies = true, CookieContainer = new CookieContainer(), AutomaticDecompression = DecompressionMethods.GZip };
			_httpClient = new HttpClient(_handlerClient);
		}

		public static async Task<Connection> CreateAsync()
		{
			Connection connection = new Connection();
			return await connection.InitializeAsync();
		}

		private async Task<Connection> InitializeAsync()
		{
			await _httpClient.GetAsync(PageList.Small).ConfigureAwait(false);
			string csfr = _handlerClient.CookieContainer.GetCookies(PageList.Base).Cast<Cookie>().First(x => x.Name == "csrftoken").Value;
			_httpClient.DefaultRequestHeaders.Add("X-CSRFToken", csfr);
			return this;
		}

		/// <summary>
		///     Efetua uma requisição POST com formulário para um determinado Endpoint.
		/// </summary>
		/// <typeparam name="TPage">View que efetuará o tratamento da resposta obtida.</typeparam>
		/// <param name="url">Endpoint que será feito a requisição.</param>
		/// <param name="bodyForm">Formulário a ser enviado na requisição.</param>
		/// <returns>
		///     Resposta no tipo <see cref="IDeserializable{TObject}" /> tratada pela <see cref="IDeserializable{TPage}" />
		/// </returns>
		public async Task<RequestResponse> PostPageAsync<TPage>(Uri url, IDictionary<string, string> bodyForm)
			where TPage : IDeserializable
		{
			using (FormUrlEncodedContent form = new FormUrlEncodedContent(bodyForm))
			{
				using (HttpResponseMessage response = await _httpClient.PostAsync(url, form).ConfigureAwait(false))
				{
					return await DeserializeResponseAsync<TPage>(response);
				}
			}
		}

		/// <summary>
		///     Efetua uma requisição GET para um determinado Endpoint e faz o tratamento da resposta.
		/// </summary>
		/// <typeparam name="TPage">View que efetuará o tratamento da resposta obtida.</typeparam>
		/// <typeparam name="TObject">Tipo da resposta que será obtido da deserialização.</typeparam>
		/// <param name="url">Endpoint que será feito a requisição.</param>
		/// <returns>
		///     Resposta no tipo <see cref="IDeserializable{TObject}" /> tratada pela <see cref="IDeserializable{TPage}" />
		/// </returns>
		public async Task<RequestResponse<TObject>> GetPageAsync<TPage, TObject>(Uri url)
			where TObject : class
			where TPage : IDeserializable<TObject>
		{
			using (HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false))
			{
				return await DeserializeResponseAsync<TPage, TObject>(response);
			}
		}

		/// <summary>
		///     Efetua uma requisição GET com formulário para um determinado Endpoint e faz o tratamento da resposta.
		/// </summary>
		/// <typeparam name="TPage">View que efetuará o tratamento da resposta obtida.</typeparam>
		/// <typeparam name="TObject">Tipo da resposta que será obtido do tratamento.</typeparam>
		/// <param name="url">Endpoint que será feito a requisição.</param>
		/// <param name="queryForm">Formulário que será enviado na requisição.</param>
		/// <returns>
		///     Resposta no tipo <see cref="IDeserializable{TObject}" /> tratada pela <see cref="IDeserializable{TPage}" />
		/// </returns>
		public async Task<RequestResponse<TObject>> GetPageAsync<TPage, TObject>(Uri url, IDictionary<string, string> queryForm)
			where TObject : class
			where TPage : IDeserializable<TObject>
		{
			using (HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false))
			{
				return await DeserializeResponseAsync<TPage, TObject>(response);
			}
		}


		/// <summary>
		///     Efetua uma requisição POST com formulário para um determinado Endpoint e faz o tratamento da resposta.
		/// </summary>
		/// <typeparam name="TPage">View que efetuará o tratamento da resposta obtida.</typeparam>
		/// <typeparam name="TObject">Tipo da resposta que será obtido da deserialização.</typeparam>
		/// <param name="url">Endpoint que será feito a requisição.</param>
		/// <param name="bodyForm">Formulário a ser enviado na requisição.</param>
		/// <returns>
		///     Resposta no tipo <see cref="IDeserializable{TObject}" /> tratada pela <see cref="IDeserializable{TPage}" />
		/// </returns>
		public async Task<RequestResponse<TObject>> PostPageAsync<TPage, TObject>(Uri url, IList<KeyValuePair<string, string>> bodyForm)
			where TObject : class
			where TPage : IDeserializable<TObject>
		{
			using (FormUrlEncodedContent form = new FormUrlEncodedContent(bodyForm))
			{
				using (HttpResponseMessage response = await _httpClient.PostAsync(url, form).ConfigureAwait(false))
				{
					return await DeserializeResponseAsync<TPage, TObject>(response);
				}
			}
		}

		/// <summary>
		///     Faz o tratamento da resposta obtida de uma requisição.
		/// </summary>
		/// <typeparam name="TPage">View que efetuará o tratamento da resposta.</typeparam>
		/// <param name="response">Resposta obtida da requisição.</param>
		/// <returns>
		///     Resposta no tipo <see cref="IDeserializable{TObject}" /> tratada pela <see cref="IDeserializable{TPage}" />
		/// </returns>
		private static async Task<RequestResponse> DeserializeResponseAsync<TPage>(HttpResponseMessage response)
			where TPage : IDeserializable
		{
			string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			TPage instancePage = Activator.CreateInstance<TPage>();

			//Deserializa TObject do resultado da página.
			return instancePage.DeserializeResponse(result);
		}

		/// <summary>
		///     Faz o tratamento da resposta obtida de uma requisição.
		/// </summary>
		/// <typeparam name="TPage">View que efetuará o tratamento da resposta.</typeparam>
		/// <typeparam name="TObject">Tipo que será retornado pela view.</typeparam>
		/// <param name="response">Resposta obtida da requisição.</param>
		/// <returns>
		///     Resposta no tipo <see cref="IDeserializable{TObject}" /> tratada pela <see cref="IDeserializable{TPage}" />
		/// </returns>
		private static async Task<RequestResponse<TObject>> DeserializeResponseAsync<TPage, TObject>(HttpResponseMessage response)
			where TObject : class
			where TPage : IDeserializable<TObject>
		{
			string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			TPage instancePage = Activator.CreateInstance<TPage>();

			//Deserializa TObject do resultado da página.
			return instancePage.DeserializeResponse(result);
		}
	}
}
