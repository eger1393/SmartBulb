using System.ComponentModel.DataAnnotations;

namespace Service.TpLinkApi.Models
{
	public class AuthorizeRequest
	{
		[Required]
		public string Login { get; set; }
		[Required]
		public string Password { get; set; }
	}
}