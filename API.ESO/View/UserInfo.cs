using System.Collections.Generic;
using System.Linq;
using API.ESO.ConnectionManager;
using API.ESO.Model;
using Newtonsoft.Json;

namespace API.ESO.View
{
	internal class UserInfo : IDeserializable<User>
    {
	    /// <inheritdoc />
	    public RequestResponse<User> DeserializeResponse(string response)
	    {
		    User user = JsonConvert.DeserializeObject<IList<User>>(response).First();
		    return new RequestResponse<User>(RequestResponse.Success, user);
	    }
    }
}
