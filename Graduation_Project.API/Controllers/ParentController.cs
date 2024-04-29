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

        ///<summary>
        ///
        /// Endpoint To retrieve all Parent Info For His Profile
        /// </summary>
   

        // GET: api/parent/{parentId}
        [HttpGet("{parentId}")]
        public async Task<ActionResult<Parent>> GetParentById(int parentId)
        {
            try
            {
                var parent = await _context.Parents
                                   .Include(p => p.User) // Include the User navigation property
                                   .Include(x=>x.School)
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
                    SchoolName = parent.School.Name,
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
                var students = await _context.Students
                          .Where(x => x.ParentId == parentId)
                          .Include(s => s.wallet) // Include wallet information
                          .Include(s => s.Grade) // Include grade information
                          .Include(s=>s.Class)
                          .ToListAsync();

                if (students.Count == 0) 
                {
                    return NotFound($"No Children Found For This Parent");
                }


                var childrenInfo = new List<StudentInfoDto>();

                foreach (var studente in students)
                {
                    var studentInfo = new StudentInfoDto
                    {
                        Id = studente.Id,   
                        Name = studente.Name,
                        Age = studente.age ?? 0,
                        GradeTitle = studente.Grade?.GradeTitle,
                        ClassTitle = studente.Class?.ClassTitle,
                        WalletBalance = studente.wallet != null ? studente.wallet.Balance : 0,
                        DailyLimit = studente.wallet != null ? studente.wallet.DailyLimit : 0
                    };

                    childrenInfo.Add(studentInfo);
                }


                return Ok(childrenInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
         }


        ///<summary>
        ///
        /// Endpoint To retrieve Total Parent's Children Wallet Balance
        /// </summary>
        [HttpGet("{parentId}/TotalWalletsBalance")]
        public async Task<ActionResult<decimal>> GetParentTotalWalletBalance(int parentId)
        {
            try
            {
                // Retrieve the parent by ID
                var parent = await _context.Parents.FindAsync(parentId);

                if (parent == null)
                {
                    return NotFound($"Parent with ID {parentId} not found.");
                }

                // Retrieve all children (students) of the parent
                var children = await _context.Students
                        .Include(s => s.wallet)
                    .Where(s => s.ParentId == parentId)
                    .ToListAsync();

                // Initialize total balance
                decimal totalBalance = 0;

                // Sum up the wallet balances of all children
                foreach (var child in children)
                {
                    if (child.wallet != null)
                    {
                        totalBalance += child.wallet.Balance;
                    }
                }

                return Ok(totalBalance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        ///<summary>
        ///
        /// Endpoint To retrieve all Transaction related by All Parent's Children
        /// </summary>
        [HttpGet("transactions/{parentId}")]
        public IActionResult GetTransactionsByParentId(int parentId)
        {
            try
            {
                // Retrieve the parent's students
                var parentStudents = _context.Students
                    .Where(s => s.ParentId == parentId)
                    .Select(s => s.Id)
                    .ToList();

                // Retrieve transactions related to the parent's students ordered by created date
                var studentTransactions = _context.CanteenTransactions
                    .Include(s=>s.Student)
                    .Where(t => parentStudents.Contains(t.StudentID))
                    .OrderByDescending(t => t.Created)
                    .ToList();

                return Ok(studentTransactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
