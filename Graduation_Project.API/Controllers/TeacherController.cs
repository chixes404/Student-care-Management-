using Graduation_Project.API.Data;
using Graduation_Project.Shared.DTO;
using Graduation_Project.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {

        private readonly ApplicationDatabaseContext _context;


        public TeacherController(ApplicationDatabaseContext context)
        {
            _context = context;
        }


        // GET: api/teacher/{teacherId}
        [HttpGet("{teacherId}")]
        public async Task<ActionResult<Teacher>> GetTeacherById(int teacherId)
        {
            try
            {
                var teacher = await _context.Teachers
                                   .Include(p => p.User) // Include the User navigation property
                                   .FirstOrDefaultAsync(p => p.Id == teacherId);

                if (teacher == null)
                {
                    return NotFound($"Teacher with ID {teacherId} not found.");
                }

                // Check if the user associated with the parent is active
                if (!teacher.User.IsActive)
                {
                    return Unauthorized("User is not active.");
                }

                var TeacherProfile = new TeacherInfoDto
                {
                    Id = teacher.Id,
                    Name = $"{teacher.User.FirstName} {teacher.User.LastName}",
                    NationalId = teacher.NationalID,
                    UserId = teacher.UserId,
                    Email = teacher.User.Email,
                    SchoolId = teacher.SchoolId,
                };

                return Ok(TeacherProfile);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving parent: {ex.Message}");
            }
        }


        [HttpGet("grades")]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetAllGrades()
        {
            try
            {
                var grades = await _context.Grades.AsNoTracking().ToListAsync();

                var gradeDtos = grades.Select(g => new GradeDto
                {
                    Id = g.Id,
                    GradeTitle = g.GradeTitle,
                });

                return Ok(gradeDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("classes")]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetAllClasses()
        {
            try
            {
                var grades = await _context.Classes.AsNoTracking().ToListAsync();

                var gradeDtos = grades.Select(g => new ClassDto
                {
                    id = g.Id,
                    ClassTitle = g.ClassTitle,
                });

                return Ok(gradeDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("subjects")]
        public async Task<ActionResult<IEnumerable<SubjectsDto>>> GetAllSubjects()
        {
            try
            {
                var subjects = await _context.Subjects.AsNoTracking().ToListAsync();

                var subjectsDtos = subjects.Select(g => new SubjectsDto
                {
                    id = g.Id,
                    name = g.SubjectName,
                });

                return Ok(subjectsDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
