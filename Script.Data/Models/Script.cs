using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Service.Script.Data.Models
{
	public class Script
	{
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Название скрипта
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Состояние в которое устройство переходит при запуске скрипта
		/// </summary>
		[JsonProperty("startState")]
		public SetStateTask StartState { get; set; }

		/// <summary>
		/// Состояние в которое устройство переходит при окончании скрипта
		/// </summary>
		[JsonProperty("endState")]
		public SetStateTask EndState { get; set; }

		/// <summary>
		/// Список задач для повторения
		/// </summary>
		[JsonProperty("repeatedTasks")]
		[Required]
		public List<SetStateTask> RepeatedTasks { get; set; }

		/// <summary>
		/// Кол-во повторений списка задач
		/// </summary>
		[JsonProperty("repeatCount")]
		[Required]
		public int RepeatCount { get; set; } = 1;

		/// <summary>
		/// Час в который запустится скрипт
		/// </summary>
		[JsonProperty("startHour")]
		public int? StartHour { get; set; }

		/// <summary>
		/// Минут в которую запустится скрипт
		/// </summary>
		[JsonProperty("startMinute")]
		public int? StartMinute { get; set; }

		/// <summary>
		/// Пользователь из под которого будут выполняться команды
		/// </summary>
		public User User { get; set; }
	}
}
