using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartBulb.TpLinkApi.Abstract;
using SmartBulb.TpLinkApi.Models;

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

        public async Task SetDeviceState(string deviceId, LightState lightState)
        {
            // string deviceId = "80123F24B6E23A46BE3207A29DC1958C1BAEBF37";
            var comm = new RequestBulb()
            {
                Service = new LightService()
                {
                    State = lightState
                    //     new LightState()
                    // {
                    //     Power = PowerState.On,
                    //     Brightness = 100,
                    //     Hue = 100,
                    //     Saturation = 100,
                    //     TransitionTime = 4000
                    // }
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
    }

    public class RequestBulb
    {
        [JsonProperty("smartlife.iot.smartbulb.lightingservice")]
        public LightService Service { get; set; }
    }

    public class LightService
    {
        [JsonProperty("transition_light_state")]
        public LightState State { get; set; }
    }
    
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LightState
    {
        [JsonProperty("on_off")]
        public  PowerState? Power { get; set; }
        
        [JsonProperty("brightness")]
        public int? Brightness { get; set; }
        
        [JsonProperty("hue")]
        public int? Hue { get; set; }
        
        [JsonProperty("saturation")]
        public int? Saturation { get; set; }
        
        /// <summary>
        /// Время перехода из одного состояния в другое
        /// </summary>
        [JsonProperty("transition_period")]
        public  long? TransitionTime { get; set; }

        /// <summary>
        ///  Хз что это, но в доке часто исспользуется для внутренних комманд
        /// </summary>
        [JsonProperty("ignore_default")]
        private int IgnoreDefault { get; set; } = 1;
    }
    
    public enum PowerState {
        Off,
        On,
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