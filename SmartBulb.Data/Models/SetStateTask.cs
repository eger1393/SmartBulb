using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SmartBulb.Data.Models
{
	public class SetStateTask
	{
		[Key]
		public Guid Id { get; set; }

		[JsonProperty("deviceId")]
		public string DeviceId { get; set; }
		[JsonProperty("state")]
		public BulbState State { get; set; }

		[JsonProperty("waitTime")]
		public int? WaitTime { get; set; }
	}
}
