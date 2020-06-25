using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TpLinkApi.Implementation.Exceptions;
using TpLinkApi.Implementation.JsonModels;
using TpLinkApi.Implementation.Models;

namespace TpLinkApi.Implementation.HttpClients
{
	public class TpLinkHttpClient : ITpLinkHttpClient
	{
		private readonly ILogger<TpLinkHttpClient> _logger;
		private readonly HttpClient _client;
		private const string PassthroughMethod = "passthrough";
		private const string TpLinkAuthUrl = "https://wap.tplinkcloud.com?termID={0}";
		private const string TpLinkUrl = "https://eu-wap.tplinkcloud.com?token={0}";

		public TpLinkHttpClient(ILogger<TpLinkHttpClient> logger, HttpClient client)
		{
			_logger = logger;
			_client = client;
		}

		public async Task<string> Authorize(string login, string password)
		{
			Guid termId = Guid.NewGuid();
			_logger.LogInformation($"Authorize: {DateTime.Now:hh:mm:ss.fffff}");
			var message = new HttpRequestMessage(HttpMethod.Post,
				string.Format(TpLinkAuthUrl, termId.ToString()));
			var payload = new RequestPayload()
			{
				Method = "login",
				Params = new Dictionary<string, string>()
				{
					{"appType", "Kasa_Android"},
					{"cloudUserName", login},
					{"cloudPassword", password},
					{"terminalUUID", termId.ToString()},
				}
			};
			message.Content = new StringContent(JsonConvert.SerializeObject(payload));
			message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = await _client.SendAsync(message);
			var result = await response.Content.ReadAsStringAsync();
			_logger.LogTrace(result);
			ErrorChecking(result);
			var data = JsonConvert.DeserializeObject<ResponsePayload>(result);
			var token = data.Result.First(x => x.Key == "token").Value;
			return token;
		}

		public Task<string> SendPassthroughRequest(string token, string deviceId, string command)
		{
			return SendRequest(token, PassthroughMethod, new Dictionary<string, string>()
			{
				{"deviceId", deviceId},
				{"requestData", command}

			});
		}

		public async Task<string> SendRequest(string token, string method, Dictionary<string, string> param)
		{
			var message = new HttpRequestMessage(HttpMethod.Post, string.Format(TpLinkUrl, token));

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
			var response = await _client.SendAsync(message);
			var result = await response.Content.ReadAsStringAsync();
			_logger.LogTrace(result);
			ErrorChecking(result);
			return result;
		}

		private void ErrorChecking(string content)
		{
			var error = JsonConvert.DeserializeObject<ErrorResponse>(content);
			// TODO подумать как это переделать что-бы не писать милион ифов
			if(error.ErrorCode == -20651)
				throw new TpLinkAuthorizeException(error.Message);
			if (error.ErrorCode != 0)
				throw new BaseBusinessException(error.Message);
		}
	}
}