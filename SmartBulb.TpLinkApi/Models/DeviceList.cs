using System.Collections.Generic;
using Newtonsoft.Json;
using SmartBulb.Data.Models;

namespace SmartBulb.TpLinkApi.Models
{
	public class DeviceList
	{
		[JsonProperty("deviceList")]
		public List<Device> Devices { get; set; }
	}
}