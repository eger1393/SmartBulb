using System.Collections.Generic;
using Newtonsoft.Json;

namespace TpLinkApi.Implementation.Models
{
	public class ResponsePayload
	{
		[JsonProperty("result")]
		public Dictionary<string, string> Result { get; set; }
	}
}