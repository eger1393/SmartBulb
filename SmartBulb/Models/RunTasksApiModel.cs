using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartBulb.Data.Models;

namespace SmartBulb.Models
{
	public class RunTasksApiModel
	{
		/// <summary>
		/// Список задач
		/// </summary>
		[Required]
		public List<BaseTask> Tasks { get; set; }

		/// <summary>
		/// Кол-во повторений
		/// </summary>
		[Required]
		public int Count { get; set; }
	}

	public class BaseTaskConverter : JsonConverter<BaseTask>
	{
		//public override bool CanConvert(Type objectType)
		//{
		//	return (objectType == typeof(BaseFoo));
		//}

		//public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		//{
		//	JObject jo = JObject.Load(reader);
		//	if (jo["FooBarBuzz"].Value<string>() == "A")
		//		return jo.ToObject<AFoo>(serializer);

		//	if (jo["FooBarBuzz"].Value<string>() == "B")
		//		return jo.ToObject<BFoo>(serializer);

		//	return null;
		//}

		//public override bool CanWrite
		//{
		//	get { return false; }
		//}

		//public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		//{
		//	throw new NotImplementedException();
		//}
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