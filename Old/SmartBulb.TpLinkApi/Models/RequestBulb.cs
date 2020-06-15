using Newtonsoft.Json;
using SmartBulb.TpLinkApi.Implementation;

namespace SmartBulb.TpLinkApi.Models
{
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class RequestBulb
	{
		[JsonProperty("smartlife.iot.smartbulb.lightingservice")]
		public BulbStateService Service { get; set; }
	}
}