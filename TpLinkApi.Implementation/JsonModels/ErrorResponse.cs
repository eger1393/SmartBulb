using Newtonsoft.Json;

namespace TpLinkApi.Implementation.JsonModels
{
	public class ErrorResponse
	{
		[JsonProperty("error_code")]
		public int ErrorCode { get; set; }

		[JsonProperty("msg")]
		public string Message { get; set; }
	}
}