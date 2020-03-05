using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SmartBulb.Data.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BulbState
    {
	    [Key]
	    public Guid Id { get; set; }

        [JsonProperty("on_off")]
        public  PowerState? Power { get; set; }
        
        [JsonProperty("brightness")]
        public int? Brightness { get; set; }
        
        [JsonProperty("hue")]
        public int? Hue { get; set; }
        
        [JsonProperty("saturation")]
        public int? Saturation { get; set; }
        
        [JsonProperty("color_temp")]
        public int? ColorTemp { get; set; }
        
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
}