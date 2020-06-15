using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartBulb.Data.Models;
using SmartBulb.Data.Repositories.Abstract;
using SmartBulb.TpLinkApi.Abstract;
using SmartBulb.TpLinkApi.HttpClients;
using SmartBulb.TpLinkApi.Models;

namespace SmartBulb.TpLinkApi.Implementation
{
	public class TpLink : ITpLink
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger<TpLink> _logger;
		private readonly ITpLinkHttpClient _httpClient;

		private const string SetDeviceStateMethodName = "passthrough";
		private const string GetDeviceMethodName = "getDeviceList";

		public TpLink(IUserRepository userRepository, ILogger<TpLink> logger, ITpLinkHttpClient httpClient)
		{
			_userRepository = userRepository;
			_logger = logger;
			_httpClient = httpClient;
		}

		public async Task SetDeviceState(Guid userId, string deviceId, BulbState bulbState)
		{
			_logger.LogInformation($"SetDeviceState: {DateTime.Now:hh:mm:ss.fffff}");

			var user = _userRepository.GetBy(userId);
			var comm = new RequestBulb()
			{
				Service = new BulbStateService()
				{
					SetState = bulbState
				}
			};
			string command = JsonConvert.SerializeObject(comm);
			//var response = await SendAsync(user, SetDeviceStateMethodName, new Dictionary<string, string>()
			//{
			//	{"deviceId", deviceId},
			//	{"requestData", command}
			//});
			//var content = await response.Content.ReadAsStringAsync();

			_logger.LogInformation($"SetDeviceState COMPLETE: {DateTime.Now:hh:mm:ss.fffff}");
		}

		public async Task<ResponseDeviceListPayload> GetDeviceList(Guid userId)
		{
			_logger.LogInformation($"GetDeviceList: {DateTime.Now:hh:mm:ss.fffff}");

			var user = _userRepository.GetBy(userId);
			var response = await SendAsync(user, GetDeviceMethodName, null);
			var json = await response.Content.ReadAsStringAsync();
			var data = JsonConvert.DeserializeObject<ResponseDeviceListPayload>(json);

			_logger.LogInformation($"GetDeviceList COMPLETE: {DateTime.Now:hh:mm:ss.fffff}");
			return data;
		}

		private async Task Authorize(User user)
		{
			var token = await _httpClient.Authorize(user.Login, user.Password, user.Id.ToString());
			_userRepository.SetToken(user.Id, token);
		}

		private async Task<HttpResponseMessage> SendAsync(User user, string method, Dictionary<string, string> param)
		{
			if (string.IsNullOrEmpty(user.Token))
				await Authorize(user);

			var response = await _httpClient.CreateRequest(user.Token, method, param);
			var content = await response.Content.ReadAsStringAsync();

			var error = JsonConvert.DeserializeObject<ErrorResponse>(content);
			if (error?.ErrorCode == -20651)
			{
				await Authorize(user);
				return await _httpClient.CreateRequest(user.Token, method, param);
			}
			return response;
		}
	}
}