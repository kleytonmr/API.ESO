using API.ESO.ConnectionManager;
using Newtonsoft.Json;

namespace API.ESO.View
{
	internal class Login : IDeserializable
	{
		public string Value { get; set; }
		public bool Success { get; set; }
		/// <inheritdoc />
		public RequestResponse DeserializeResponse(string response)
		{
			Login result = JsonConvert.DeserializeObject<Login>(response);
			if (result.Success)
			{
				return RequestResponse.LoginSuccessed;
			}
			switch (result.Value)
			{
				case "Endereço de e-mail ou senha incorretos.":
					return RequestResponse.LoginUserWrong;

				case "403 Forbidden":
					return RequestResponse.CsrfInvalid;

				case "Esta conta foi temporariamente bloqueada por excesso de falhas de acesso. Tente novamente mais tarde.":
					return RequestResponse.AccountBlocked;

				default:
					return RequestResponse.LoginUserWrong;
			}
		}
	}
}
