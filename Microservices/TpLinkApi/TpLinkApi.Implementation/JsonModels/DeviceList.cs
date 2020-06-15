using System.Collections.Generic;
using Newtonsoft.Json;
using TpLinkApi.Implementation.Models;

namespace TpLinkApi.Implementation.JsonModels
{
	public class DeviceList
	{
		[JsonProperty("deviceList")]
		public List<Device> Devices { get; set; }
	}
}