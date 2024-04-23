using Graduation_Project.API.Data;
using Graduation_Project.Shared.Models;
using Graduation_Project.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly ApplicationDatabaseContext _context;


        public ParentController(ApplicationDatabaseContext context)
        {
            _context = context;
        }


        // GET: api/parent/{parentId}
        [HttpGet("{parentId}")]
        public async Task<ActionResult<Parent>> GetParentById(int parentId)
        {
            try
            {
                var parent = await _context.Parents
                                   .Include(p => p.User) // Include the User navigation property
                                   .FirstOrDefaultAsync(p => p.Id == parentId);

                if (parent == null)
                {
                    return NotFound($"Parent with ID {parentId} not found.");
                }

                // Check if the user associated with the parent is active
                if (!parent.User.IsActive)
                {
                    return Unauthorized("User is not active.");
                }

                var ParentProfile = new ParentWithoutStudentDto
                {
                    ParentId = parent.Id,
                    Name = $"{parent.User.FirstName} {parent.User.LastName}",
                    PhoneNumber = parent.User.PhoneNumber,
                    UserId = parent.UserId,
                    Email = parent.User.Email,
                    SchoolId = parent.SchoolId,
                    ImageUrl = parent.User.ImageURL
                };

                return Ok(ParentProfile);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving parent: {ex.Message}");
            }
        }



        // GET: api/parent/{parentId}/Childrens
        [HttpGet("{parentId}/Childrens")]
        public async Task<ActionResult<IEnumerable<Student>>> GetParentAllChildren(int parentId)
        {
            try
            {
                var Students = await _context.Students.Where(x=>x.ParentId == parentId).ToListAsync();

                if (Students.Count == 0) 
                {
                    return NotFound($"No Children Found For This Parent");
                }

                return Ok(Students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
         }


    }
}
