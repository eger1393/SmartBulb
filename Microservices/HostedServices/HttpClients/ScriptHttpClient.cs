using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Script.Data.Models;

namespace HostedServices.HttpClients
{
	public interface IScriptHttpClient
	{
		 Task RunScriptAsync(Script script);
	}
	public class ScriptHttpClient : IScriptHttpClient
	{
		private const string StartScriptUrl = "api/script/run";
		private readonly ILogger<ScriptHttpClient> _logger;
		private readonly HttpClient _client;

		public ScriptHttpClient(HttpClient client, ILogger<ScriptHttpClient> logger, Config config)
		{
			_client = client;
			_logger = logger;
			_client.BaseAddress = config.ScriptServiceUrl;
		}

		public async Task RunScriptAsync(Script script)
		{
			var message = new HttpRequestMessage(HttpMethod.Post, StartScriptUrl)
			{
				Content = new StringContent(JsonConvert.SerializeObject(script))
			};
			message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = await _client.SendAsync(message);
			var result = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(result);
			}
		}
	}
}