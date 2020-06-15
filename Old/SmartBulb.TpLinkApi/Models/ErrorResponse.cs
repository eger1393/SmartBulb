using Newtonsoft.Json;

namespace SmartBulb.TpLinkApi.Models
{
	public class ErrorResponse
	{
		[JsonProperty("error_code")]
		public int ErrorCode { get; set; }

		[JsonProperty("msg")]
		public string Message { get; set; }
	}
}