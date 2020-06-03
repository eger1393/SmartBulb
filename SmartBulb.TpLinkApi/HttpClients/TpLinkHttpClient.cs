using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartBulb.Data.Models;
using SmartBulb.TpLinkApi.Models;

namespace SmartBulb.TpLinkApi.HttpClients
{
	public class TpLinkHttpClient : ITpLinkHttpClient
	{
		private readonly ILogger<TpLinkHttpClient> _logger;
		private readonly HttpClient _client;

		public TpLinkHttpClient(ILogger<TpLinkHttpClient> logger, HttpClient client)
		{
			_logger = logger;
			_client = client;
		}

		public async Task<string> Authorize(string login, string password, string termId)
		{
			_logger.LogInformation($"Authorize: {DateTime.Now.ToString("hh:mm:ss.fffff")}");
			var message = new HttpRequestMessage(HttpMethod.Post,
				$"https://wap.tplinkcloud.com?termID={termId}");
			var payload = new RequestPayload()
			{
				Method = "login",
				Params = new Dictionary<string, string>()
				{
					{"appType", "Kasa_Android"},
					{"cloudUserName", login},
					{"cloudPassword", password},
					{"terminalUUID", termId},
				}
			};
			message.Content = new StringContent(JsonConvert.SerializeObject(payload));
			message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = await _client.SendAsync(message);
			var content = await response.Content.ReadAsStringAsync();
			var data = JsonConvert.DeserializeObject<ResponsePayload>(content);
			var token = data.Result.First(x => x.Key == "token").Value;
			return token;
		}

		public Task<HttpResponseMessage> CreateRequest(string token, string method, Dictionary<string, string> param)
		{
			var message = new HttpRequestMessage(HttpMethod.Post, $"https://eu-wap.tplinkcloud.com?token={token}");

			// у Тп Линка какая-то лажа с кэшем, если не передавать `CacheControl : no-cache` то часть запросов отваливается по таймауту
			message.Headers.CacheControl = CacheControlHeaderValue.Parse("no-cache");
			// Хз нафиг этот заголовок но возможно он необходим (он зачем-то подставляется в репозитории касы)
			message.Headers.Add("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 6.0.1; A0001 Build/M4B30X)");
			var payload = new RequestPayload()
			{
				Method = method,
				Params = param
			};
			message.Content = new StringContent(JsonConvert.SerializeObject(payload));
			message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return _client.SendAsync(message);
		}
	}
}