using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Service.Script.Data.Models
{
	public class WaitTask : BaseTask
	{
		/// <summary>
		/// Время на которое приостанавливается процесс выполнения скрипта, используется для формированию цепочек по смене цвета
		/// </summary>
		[JsonProperty("waitTime")]
		[Required]
		public int WaitTime { get; set; }
	}
}