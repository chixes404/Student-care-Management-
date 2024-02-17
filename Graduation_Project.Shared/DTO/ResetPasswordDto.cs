using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Shared.DTO
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "New Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 8 characters long")]
        public string NewPassword { get; set; }

     
        public string EmailAddress { get; set; }
        public string OTP { get; set; }
    }
}
