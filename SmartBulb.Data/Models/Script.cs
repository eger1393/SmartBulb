using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBulb.Data.Models
{
	public class Script
	{
		[Key]
		public Guid Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("startState")]
		public SetStateTask StartState { get; set; }

		[JsonProperty("endState")]
		public SetStateTask EndState { get; set; }

		[JsonProperty("repeatedTasks")]
		[Required]
		public List<SetStateTask> RepeatedTasks { get; set; }

		[JsonProperty("repeatCount")]
		[Required]
		public int RepeatCount { get; set; } = 1;

		[JsonProperty("startHour")]
		public int? StartHour { get; set; }

		[JsonProperty("startMinute")]
		public int? StartMinute { get; set; }

		[JsonProperty("userId")]
		public Guid UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public User User { get; set; }
	}
}
