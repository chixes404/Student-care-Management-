using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.API.Data;
using Graduation_Project.Shared.Models;
using Graduation_Project.Shared.DTO;
using System.Linq;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeworkController : ControllerBase
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _uploadDir;

        public HomeworkController(ApplicationDatabaseContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        }

        [HttpPost]
        public IActionResult UploadHomework([FromForm] HomeworkUploadModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var teacher = _context.Teachers.FirstOrDefault(x => x.Id == model.TeacherId);
                if (teacher == null)
                {
                    return NotFound("Teacher not found");
                }

                // Get the teacher's name
                string teacherName = teacher.Name;

                // Ensure uploads folder exists
                string uploadFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads");
                Directory.CreateDirectory(uploadFolder);

                // Generate unique file name using the teacher's name
                string fileName = $"{teacherName}_{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";
                string filePath = Path.Combine(uploadFolder, fileName);

                // Save file to server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.File.CopyTo(fileStream);
                }

                // Save homework information to database
                var homework = new Homework
                {
                    GradeId = model.GradeId,
                    TeacherId = model.TeacherId,
                    ClassId = model.ClassId,
                    FilePath = fileName,
                    Created = DateTime.Now,
                    Message = model.Message
                };
                _context.Homeworks.Add(homework);
                _context.SaveChanges();

                return Ok(new { message = "Homework uploaded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetFile")]
        public ActionResult GetFile(int studentId)
        {
            try
            {
                // Get the student from the database
                var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
                if (student == null)
                {
                    return NotFound("Student not found.");
                }

                // Find the homework file for the student's grade along with teacher name and upload date
                var homework = _context.Homeworks
                    .Where(h => h.GradeId == student.GradeId)
                    .Join(
                        _context.Teachers,
                        h => h.TeacherId,
                        t => t.Id,
                        (h, t) => new
                        {
                            FilePath = h.FilePath,
                            TeacherName = t.Name,
                            UploadedDate = h.Created
                        }
                    )
                    .FirstOrDefault();

                if (homework == null)
                {
                    return NotFound("No homework file found for the student's grade.");
                }

                var filePath = Path.Combine(_uploadDir, homework.FilePath);

                // Check if the file exists in the uploads directory
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                // Read the file bytes
                var bytes = System.IO.File.ReadAllBytes(filePath);

                // Return the file as a downloadable attachment along with teacher name and upload date
                return File(bytes, "application/octet-stream", homework.FilePath);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetFileinTheFlutter")]
        public ActionResult<HomeworkFileResponseDto> GetFileByDetails(int studentId)
        {
            try
            {
                // Get the student from the database
                var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
                if (student == null)
                {
                    return NotFound("Student not found.");
                }

                // Find the homework file for the student's grade along with teacher name and upload date
                var homework = _context.Homeworks
                    .Where(h => h.GradeId == student.GradeId)
                    .Join(
                        _context.Teachers,
                        h => h.TeacherId,
                        t => t.Id,
                        (h, t) => new HomeworkFileResponseDto
                        {
                            TeacherName = t.Name,
                            FilePath = h.FilePath
                        }
                    )
                    .FirstOrDefault();

                if (homework == null)
                {
                    return NotFound("No homework file found for the student's grade.");
                }

                return Ok(homework);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
