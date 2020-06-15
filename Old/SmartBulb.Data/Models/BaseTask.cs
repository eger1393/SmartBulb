using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SmartBulb.Data.Models
{
	public class BaseTask
	{
		[Key]
		public Guid Id { get; set; }
	}
}