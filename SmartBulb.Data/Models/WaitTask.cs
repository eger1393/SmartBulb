using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SmartBulb.Data.Models
{
	public class WaitTask : BaseTask
	{
		/// <summary>
		/// Время на которое приостанавливается процесс выполнения скрипта, исспользуется для формированию цепочек по смене цвета
		/// </summary>
		[JsonProperty("waitTime")]
		[Required]
		public int WaitTime { get; set; }
	}
}