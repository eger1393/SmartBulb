using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Script.Data.Models;

namespace Gate.HttpClients
{
	public interface ITpLinkApiHttpClient
	{
		Task<string> AuthorizeAsync(string login, string password);
		Task SetStateAsync(string token, string deviceId, BulbState state);
	}


	public class TpLinkApiHttpClient : ITpLinkApiHttpClient
	{
		private const string AuthorizeUrl = "api/Authorize";
		private const string SetStateUrl = "api/Device/{0}/setState";
		private readonly ILogger<TpLinkApiHttpClient> _logger;
		private readonly HttpClient _client;

		public TpLinkApiHttpClient(HttpClient client, ILogger<TpLinkApiHttpClient> logger, Config config)
		{
			_client = client;
			_logger = logger;
			client.BaseAddress = config.TpLinkApiServiceUrl;
		}

		public async Task<string> AuthorizeAsync(string login, string password)
		{
			var message = new HttpRequestMessage(HttpMethod.Post, AuthorizeUrl)
			{
				Content = new StringContent(JsonConvert.SerializeObject(new { login, password }))
			};
			message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = await _client.SendAsync(message);
			var result = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"Ошибка авторизации\n {result}");
			}
			return result;
		}

		public async Task SetStateAsync(string token, string deviceId, BulbState state)
		{
			var message = new HttpRequestMessage(HttpMethod.Post, string.Format(SetStateUrl, deviceId))
			{
				Content = new StringContent(JsonConvert.SerializeObject(state))
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