using Graduation_Project.Shared.Models;
using Graduation_Project.Shared.Models.Authentication;

namespace Graduation_Project.API.Services
{
    public interface IAuthService
    {

		Task<AuthModel> GetTokenAsync(TokenRequestModel model);
		Task<string> AddRoleAsync(AddRoleModel model);

	}
}
