using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Script.Data.Models;

namespace Service.Script.JsonConverters
{
	public class BaseTaskConverter : JsonConverter<BaseTask>
	{
		public override void WriteJson(JsonWriter writer, BaseTask value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override BaseTask ReadJson(JsonReader reader, Type objectType, BaseTask existingValue, bool hasExistingValue,
			JsonSerializer serializer)
		{
			JObject jo = JObject.Load(reader);
			if (jo.ContainsKey("deviceId"))
			{
				return jo.ToObject<SetStateTask>();
			}

			if (jo.ContainsKey("waitTime"))
			{
				return jo.ToObject<WaitTask>();
			}

			return new BaseTask();
		}
	}
}