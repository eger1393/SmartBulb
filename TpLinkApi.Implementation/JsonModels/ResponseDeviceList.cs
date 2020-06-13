using Newtonsoft.Json;

namespace TpLinkApi.Implementation.JsonModels
{
	public class ResponseDeviceList
	{
		[JsonProperty("result")]
		public DeviceList Result { get; set; }
	}
}