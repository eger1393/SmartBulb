using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartBulb.Data.Models;
using SmartBulb.Data.Repositories.Abstract;
using SmartBulb.TpLinkApi.Abstract;

namespace SmartBulb.TpLinkApi.Implementation
{
	public class TpLink : ITpLink
	{
		private readonly HttpClient _client;
		private readonly IUserRepository _userRepository;

		public TpLink(HttpClient client, IUserRepository userRepository)
		{
			this._client = client;
			_userRepository = userRepository;
		}

		public async Task GetDeviceState(string deviceId)
		{
			//var comm = new RequestBulb()
			//{
			//	Service = new BulbStateService()
			//	{
			//		GetState = new BulbState()
			//	}
			//};
			//string command = JsonConvert.SerializeObject(comm);
			//var response = await CreateRequest("passthrough", new Dictionary<string, string>()
			//{
			//	{"deviceId", deviceId},
			//	{"requestData", command}
			//});
			//var json = await response.Content.ReadAsStringAsync();
			//var data = JsonConvert.DeserializeObject<ResponsePayload>(json);
			//var t = data.Result.First().Value;
			//var state = JsonConvert.DeserializeObject<RequestBulb>(t);
		}

		public async Task StartScript(Script script)
		{
			if (script.StartState?.State != null)
			{
				await SetDeviceState(script.UserId, script.StartState.DeviceId, script.StartState.State);
				await Task.Delay((int)script.StartState.State.TransitionTime);
			}

			for (int i = 0; i < script.RepeatCount; i++)
				await RunTasks(script.UserId, script.RepeatedTasks);

			if (script.EndState?.State != null)
			{
				await SetDeviceState(script.UserId, script.EndState.DeviceId, script.EndState.State);
				await Task.Delay((int)script.EndState.State.TransitionTime);
			}
		}

		public async Task SetDeviceState(Guid userId, string deviceId, BulbState bulbState)
		{
			var user = _userRepository.GetBy(userId);
			var comm = new RequestBulb()
			{
				Service = new BulbStateService()
				{
					SetState = bulbState
				}
			};
			string command = JsonConvert.SerializeObject(comm);
			var response = await SendAsync(user, "passthrough", new Dictionary<string, string>()
			{
				{"deviceId", deviceId},
				{"requestData", command}
			});
			var json = await response.Content.ReadAsStringAsync();
		}

		public async Task<dynamic> GetDeviceList(Guid userId)
		{
			var user = _userRepository.GetBy(userId);
			var response = await SendAsync(user, "getDeviceList", null);
			var json = await response.Content.ReadAsStringAsync();
			var data = JsonConvert.DeserializeObject<ResponseDeviceListPayload>(json);
			return data;
		}

		private async Task Authorize(User user)
		{
			var message = new HttpRequestMessage(HttpMethod.Post,
				$"https://wap.tplinkcloud.com?termID={user.Id.ToString()}");
			var payload = new RequestPayload()
			{
				Method = "login",
				Params = new Dictionary<string, string>()
				{
					{"appType", "Kasa_Android"},
					{"cloudUserName", user.Login},
					{"cloudPassword", user.Password},
					{"terminalUUID", user.Id.ToString()},
				}
			};
			message.Content = new StringContent(JsonConvert.SerializeObject(payload));
			message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var response = await _client.SendAsync(message);
			var content = await response.Content.ReadAsStringAsync();
			var data = JsonConvert.DeserializeObject<ResponsePayload>(content);
			var token = data.Result.First(x => x.Key == "token").Value;
			_userRepository.SetToken(user.Id, token);
		}

		private async Task<HttpResponseMessage> SendAsync(User user, string method, Dictionary<string, string> param)
		{
			if (string.IsNullOrEmpty(user.Token))
				await Authorize(user);

			var response =await CreateRequest(user, method, param);
			var content = await response.Content.ReadAsStringAsync();
			var error = JsonConvert.DeserializeObject<ErrorResponse>(content);
			if (error?.ErrorCode == -20651)
			{
				await Authorize(user);
				return await CreateRequest(user, method, param);
			}
			return response;
		}

		private Task<HttpResponseMessage> CreateRequest(User user, string method, Dictionary<string, string> param)
		{
			var message = new HttpRequestMessage(HttpMethod.Post,
				$"https://wap.tplinkcloud.com?termID={user.Id.ToString()}&token={user.Token}");
			var payload = new RequestPayload()
			{
				Method = method,
				Params = param
			};
			message.Content = new StringContent(JsonConvert.SerializeObject(payload));
			message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return _client.SendAsync(message);
		}

		private async Task RunTasks(Guid userId, List<SetStateTask> tasks)
		{
			foreach (var stateTask in tasks)
			{
				if (stateTask.WaitTime != null)
				{
					await Task.Delay((int)stateTask.WaitTime);
					continue;
				}
				if (stateTask.DeviceId != null)
				{
					SetDeviceState(userId, stateTask.DeviceId, stateTask.State).ConfigureAwait(false);
					continue;
				}
			}
		}
	}

	public class ErrorResponse
	{
		[JsonProperty("error_code")]
		public int ErrorCode { get; set; }

		[JsonProperty("msg")]
		public string Message { get; set; }
	}

	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class RequestBulb
	{
		[JsonProperty("smartlife.iot.smartbulb.lightingservice")]
		public BulbStateService Service { get; set; }
	}

	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class BulbStateService
	{
		[JsonProperty("transition_light_state")]
		public BulbState SetState { get; set; } = null;

		[JsonProperty("get_light_state")]
		public BulbState GetState { get; set; } = null;
	}

	public class GetBulbStateService
	{
		[JsonProperty("get_light_state")]
		public BulbState Info { get; set; }
	}


	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class RequestPayload
	{
		[JsonProperty("method")]
		public string Method { get; set; }
		[JsonProperty("params")]
		public Dictionary<string, string> Params { get; set; }
	}

	public class ResponsePayload
	{
		[JsonProperty("result")]
		public Dictionary<string, string> Result { get; set; }
	}

	public class ResponseDeviceListPayload
	{
		[JsonProperty("result")]
		public DeviceList Result { get; set; }
	}

	public class DeviceList
	{
		[JsonProperty("deviceList")]
		public List<Device> Devices { get; set; }
	}
}