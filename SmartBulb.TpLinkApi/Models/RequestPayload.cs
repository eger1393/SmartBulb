using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartBulb.TpLinkApi.Models
{
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class RequestPayload
	{
		[JsonProperty("method")]
		public string Method { get; set; }
		[JsonProperty("params")]
		public Dictionary<string, string> Params { get; set; }
	}
}