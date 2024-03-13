using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Shared.Models.Authentication
{
    public class AddRoleModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }


    }
}
