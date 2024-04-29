using Graduation_Project.API.Data;
using Graduation_Project.Shared.DTO;
using Graduation_Project.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly ApplicationDatabaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;


    public StudentController(ApplicationDatabaseContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

        /// <summary>
        /// Endpoint to retrieve Needed Info For this student.
        /// </summary>   
        ///   /// <remarks>
        /// The Student Button in the Parent Profile.
        /// </remarks>
        //GET: api/student/{studentId }
        [HttpGet("{studentId}")]
    public async Task<ActionResult<Student>> GetStudentById(int studentId)
    {
        try
        {
            var student = await _context.Students
                     .Include(s => s.wallet) // Include wallet information
                          .Include(s => s.Grade) // Include grade information
                          .Include(s => s.Class)
                               .FirstOrDefaultAsync(p => p.Id == studentId);

            if (student == null)
            {
                return NotFound($"Student with ID {studentId} not found.");
            }

            // Check if the user associated with the parent is active
            if (!student.Active)
            {
                return Unauthorized("Student is not active.");
            }

            var StudentProfile = new StudentInfoDto
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.age,
                GradeTitle = student.Grade.GradeTitle,
                ClassTitle = student.Class.ClassTitle,
                WalletBalance = student.wallet.Balance,
                DailyLimit = student.wallet.DailyLimit,
            };

            return Ok(StudentProfile);
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving parent: {ex.Message}");
        }
    }



    [HttpPut("UpdateDailyLimit/{studentId}")]
    public async Task<IActionResult> UpdateDailyLimit(int studentId,  decimal DailyLimit)
    {
        try
        {
            // Retrieve the student from the database
            var student = await _context.Students
                .Include(s => s.wallet)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound($"Student with ID {studentId} not found.");
            }

            // Update the student's wallet daily limit
            student.wallet.DailyLimit = DailyLimit;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return Ok($"Daily limit updated successfully for student {studentId}.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }



        /// <summary>
        /// Endpoint to retrieve all transactions for this student.
        /// </summary>      
        ///   /// <remarks>
        /// The Outer List of Canteen Transaction which have few Info.
        /// </remarks>
        [HttpGet("transactions/{studentId}")]
        public IActionResult GetTransactionsByStudentId(int studentId)
        {
            try
            {
                // Retrieve the transactions related to the specified student
                var studentTransactions = _context.CanteenTransactions
              .AsNoTracking() // Use AsNoTracking to improve performance
              .Include(x => x.Student)
              .Where(t => t.StudentID == studentId)
              .ToList();

                // Create a list to store CanteenTransactionDto objects
                var canteenTransactionsDtoList = new List<CanteenTransactionDto>();

                // Iterate through each transaction and populate the CanteenTransactionDto objects
                foreach (var transaction in studentTransactions)
                {
                    var canteenTransactionDto = new CanteenTransactionDto
                    {
                        Id = transaction.Id,
                        Created = transaction.Created,
                        StudentID = transaction.StudentID,
                        StudentName = transaction.Student?.Name, // Assuming you have a navigation property to the Student entity
                        TransactionDate = transaction.TransactionDate,
                        TransactionAmount = transaction.TransactionAmount,
                        TransactionType = transaction.TransactionType,
                        Description = transaction.Description
                    };

                    // Add the populated CanteenTransactionDto object to the list
                    canteenTransactionsDtoList.Add(canteenTransactionDto);
                }

                return Ok(canteenTransactionsDtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        /// <summary>
        /// Endpoint to retrieve all transactions Details for this Transaction.
        /// </summary>      
        ///   /// <remarks>
        /// The Inner List of Canteen Transaction which have All Info (Bill).
        /// </remarks>

        [HttpGet("transaction/{transactionId}/details")]
    public async Task<IActionResult> GetTransactionDetails(int transactionId)
    {
        // Retrieve the transaction by Id including its associated products
        var transaction = await _context.CanteenTransactions
                .Include(x=>x.Student)
            .Include(t => t.CanteenTransactionProducts)
            .ThenInclude(tp => tp.Product)
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        if (transaction == null)
        {
            return NotFound(); // Return 404 if transaction is not found
        }

        // Extract the list of CanteenTransactionProducts
        var transactionProducts = transaction.CanteenTransactionProducts.ToList();

        // Calculate total price
        decimal totalPrice = transactionProducts.Sum(tp => tp.TotalPrice);

        // Create a DTO (Data Transfer Object) to hold transaction details along with product information
        var transactionDetails = new
        {
            TransactionId = transaction.Id,
            Created = transaction.Created,
            StudentName = transaction.Student.Name,
            Products = transactionProducts.Select(tp => new
            {
                ProductId = tp.Product.Id,
                ProductName = tp.Product.Name,
                ProductDescription = tp.Product.Description,
                Quantity = tp.Quantity,
                UnitPrice = tp.Product.Price,
                TotalPrice = tp.Product.Price * tp.Quantity
            }),
            TotalPrice = totalPrice
        };

        return Ok(transactionDetails);
    }







    }
}
