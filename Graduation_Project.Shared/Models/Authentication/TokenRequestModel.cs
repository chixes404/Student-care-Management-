using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Shared.Models.Authentication
{
    public class TokenRequestModel
	{

		[Required]
		public string NationalID { get; set; }

		[Required]
		public string Password { get; set; }

	}
}
