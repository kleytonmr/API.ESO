namespace API.ESO.ConnectionManager
{
	internal interface IDeserializable
	{
		RequestResponse DeserializeResponse(string response);
	}

	internal interface IDeserializable<T> 
    {
	    RequestResponse<T> DeserializeResponse(string response);
    }
}
