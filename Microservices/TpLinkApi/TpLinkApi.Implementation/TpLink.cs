using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TpLinkApi.Implementation.HttpClients;
using TpLinkApi.Implementation.JsonModels;
using TpLinkApi.Implementation.Models;

namespace TpLinkApi.Implementation
{
	public class TpLink : ITpLink
	{
		private readonly ILogger<TpLink> _logger;
		private readonly ITpLinkHttpClient _httpClient;

		private const string SetDeviceStateMethodName = "passthrough";
		private const string GetDeviceMethodName = "getDeviceList";

		public TpLink(ILogger<TpLink> logger, ITpLinkHttpClient httpClient)
		{
			_logger = logger;
			_httpClient = httpClient;
		}

		public async Task<BulbState> SetDeviceState(string token, string deviceId, BulbState newBulbState)
		{
			var command = new JObject
			{
				["smartlife.iot.smartbulb.lightingservice"] = new JObject()
				{
					["transition_light_state"] = JObject.FromObject(newBulbState)
				}
			};
			var response = await _httpClient.SendPassthroughRequest(token, deviceId, command.ToString());
			var t = JObject.Parse(response);
			var t2 = JObject.Parse(t["result"]["responseData"].ToString());
			var state = t2["smartlife.iot.smartbulb.lightingservice"]["transition_light_state"];
			if (state["dft_on_state"] != null)
			{
				var result = JsonConvert.DeserializeObject<BulbState>(state["dft_on_state"].ToString());
				result.Power = PowerState.Off;
				return result;
			}

			return JsonConvert.DeserializeObject<BulbState>(state.ToString());
		}

		public async Task<List<Device>> GetDeviceList(string token)
		{

			var response = await _httpClient.SendRequest(token, GetDeviceMethodName, null);

			var data = JsonConvert.DeserializeObject<ResponseDeviceList>(response);
			return data.Result.Devices;
		}

		public async Task<BulbState> GetDeviceState(string token, string deviceId)
		{
			var command = new JObject
			{
				["smartlife.iot.smartbulb.lightingservice"] = new JObject()
				{
					{"get_light_state", null}
				}
			};
			var response = await _httpClient.SendPassthroughRequest(token, deviceId, command.ToString());
			var t = JObject.Parse(response);
			var t2 = JObject.Parse(t["result"]["responseData"].ToString());
			var state = t2["smartlife.iot.smartbulb.lightingservice"]["get_light_state"];
			if (state["dft_on_state"] != null)
			{
				var result =  JsonConvert.DeserializeObject<BulbState>(state["dft_on_state"].ToString());
				result.Power = PowerState.Off;
				return result;
			}

			return JsonConvert.DeserializeObject<BulbState>(state.ToString());
		}

		public Task<string> Authorize(string login, string password)
		{
			return  _httpClient.Authorize(login, password);
		}
	}
}