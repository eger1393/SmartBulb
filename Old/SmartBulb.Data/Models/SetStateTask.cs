using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SmartBulb.Data.Models
{
	public class SetStateTask : BaseTask
	{

		/// <summary>
		/// Ид устройства
		/// </summary>
		[JsonProperty("deviceId")]
		[Required]
		public string DeviceId { get; set; }
		/// <summary>
		/// Новое состояние устройства
		/// </summary>
		[JsonProperty("state")]
		[Required]
		public BulbState State { get; set; }

		
	}
}
