using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartBulb.Data.Models;
using SmartBulb.TpLinkApi.Abstract;

namespace SmartBulb.TpLinkApi.Implementation
{
    public class TpLink : ITpLink
    {
        private static Guid TermId { get; } = Guid.NewGuid();
        private static string Token { get; set; }
        private readonly HttpClient _client;

        public TpLink(HttpClient client)
        {
            Guid.NewGuid();
            this._client = client;
            if(string.IsNullOrEmpty(Token))
                Authorize().Wait();
        }

        public async Task GetDeviceState(string deviceId)
        {
            var comm = new RequestBulb()
            {
                Service = new BulbStateService()
                {
                    GetState = new BulbState()
                }
            };
            string command = JsonConvert.SerializeObject(comm);
            var response = await CreateRequest("passthrough", new Dictionary<string, string>()
            {
                {"deviceId", deviceId},
                {"requestData", command}
            });
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponsePayload>(json);
            var t = data.Result.First().Value;
            var state = JsonConvert.DeserializeObject<RequestBulb>(t);
        }

        public async Task StartScript(Script script)
        {
	        if (script.StartState?.State != null)
	        {
		        await SetDeviceState(script.StartState.DeviceId, script.StartState.State);
		        await Task.Delay((int)script.StartState.State.TransitionTime);
	        }

	        for (int i = 0; i < script.RepeatCount; i++)
		        await RunTasks(script.RepeatedTasks);

	        if (script.EndState?.State != null)
	        {
		        await SetDeviceState(script.EndState.DeviceId, script.EndState.State);
		        await Task.Delay((int)script.EndState.State.TransitionTime);
	        }
        }

        public async Task SetDeviceState(string deviceId, BulbState bulbState)
        {
            var comm = new RequestBulb()
            {
                Service = new BulbStateService()
                {
                    SetState = bulbState
                }
            };
            string command = JsonConvert.SerializeObject(comm);
            var response = await CreateRequest("passthrough", new Dictionary<string, string>()
            {
                {"deviceId", deviceId},
                {"requestData", command}
            });
            var json = await response.Content.ReadAsStringAsync();
        }

        public async Task<dynamic> GetDeviceList()
        {
            var response = await CreateRequest("getDeviceList", null);
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseDeviceListPayload>(json);
            return data;
        }

        private async Task Authorize()
        {
            var response = await CreateRequest("login", new Dictionary<string, string>()
            {
                {"appType", "Kasa_Android"},
                {"cloudPassword", "K@$@P@$$w0rd"},
                {"cloudUserName", "andronov.dmitry@gmail.com"},
                {"terminalUUID", "957EE20D-4122-4146-8514-C4F26264E350"},
            });
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponsePayload>(content);
            Token = data.Result.First(x => x.Key == "token").Value;
        }

        private Task<HttpResponseMessage> CreateRequest(string method, Dictionary<string, string> param)
        {
            var message = new HttpRequestMessage(HttpMethod.Post,
                $"https://wap.tplinkcloud.com?termID={TermId.ToString()}&token={Token}");
            var payload = new RequestPayload()
            {
                Method = method,
                Params = param
            };
            message.Content = new StringContent(JsonConvert.SerializeObject(payload));
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return _client.SendAsync(message);
        }

        private async Task RunTasks(List<SetStateTask> tasks)
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
			        SetDeviceState(stateTask.DeviceId, stateTask.State).ConfigureAwait(false);
			        continue;
		        }
	        }
        }
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