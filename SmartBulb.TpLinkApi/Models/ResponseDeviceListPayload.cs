using Newtonsoft.Json;
using SmartBulb.TpLinkApi.Implementation;

namespace SmartBulb.TpLinkApi.Models
{
	public class ResponseDeviceListPayload
	{
		[JsonProperty("result")]
		public DeviceList Result { get; set; }
	}
}