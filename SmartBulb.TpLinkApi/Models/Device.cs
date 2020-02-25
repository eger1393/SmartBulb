﻿using Newtonsoft.Json;

namespace SmartBulb.TpLinkApi.Models
{
    public class Device
    {
        [JsonProperty("alias")] 
        public string Alias { get; set; }
        
        [JsonProperty("status")] 
        public string Status { get; set; }
        
        [JsonProperty("deviceId")] 
        public string DeviceId { get; set; }
        
        [JsonProperty("role")] 
        public string Role { get; set; } 
        
        [JsonProperty("deviceMac")] 
        public string Mac { get; set; }
        [JsonProperty("deviceName")] 
        public string Name { get; set; }
        [JsonProperty("deviceType")] 
        public string Type { get; set; }
        [JsonProperty("deviceModel")] 
        public string Model { get; set; }
        
        [JsonProperty("appServerUrl")]
        public string AppServerUrl { get; set; }
    }
}