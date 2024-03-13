using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Graduation_Project.Shared.Models;
using Graduation_Project.API.Data;
namespace Graduation_Project.API.Services;

public class UserService
{
    private readonly ApplicationDatabaseContext _context;

    public UserService(ApplicationDatabaseContext context)
    {
        _context = context;
    }

    public async Task<User> FindByNationalIDAsync(string nationalID)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.NationalId == nationalID);
    }
}
