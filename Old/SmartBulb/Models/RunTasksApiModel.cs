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