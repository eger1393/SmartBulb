using System;
using System.ComponentModel.DataAnnotations;

namespace Service.Script.Data.Models
{
	public class BaseTask
	{
		[Key]
		public Guid Id { get; set; }
	}
}