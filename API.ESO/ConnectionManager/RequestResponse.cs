namespace API.ESO.ConnectionManager
{

	public enum LoginResponse
{

}
	public enum RequestResponse
	{
		Success,
		LoginUserWrong,
		CsrfInvalid,
		AccountBlocked,
		LoginSuccessed
	}

	internal class RequestResponse<TObject>
    {
		public RequestResponse(RequestResponse response, TObject @object)
		{
			Response = response;
			Object = @object;
		}

		public RequestResponse Response { get; set; }
	    public TObject Object { get; set; }
    }
}
