using Newtonsoft.Json;
using SmartBulb.Data.Models;

namespace SmartBulb.TpLinkApi.Models
{
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class BulbStateService
	{
		[JsonProperty("transition_light_state")]
		public BulbState SetState { get; set; } = null;

		[JsonProperty("get_light_state")]
		public BulbState GetState { get; set; } = null;
	}
}