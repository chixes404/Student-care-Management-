using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.API.Data;
using Graduation_Project.Shared.Models;
using Graduation_Project.Shared.DTO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;


namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanteenController : ControllerBase
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CanteenController(ApplicationDatabaseContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }




        // GET: api/canteen/BlockedProducts/{studentId}
        /// <summary>
        /// Retrieve the list of products for a student, including their blocked status.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        [HttpGet("AllProductsWithStatus/{studentId}")]
        public async Task<ActionResult<IEnumerable<ProductDtoss>>> GetProductsForStudent(int studentId)
        {
            // Retrieve all products
            var products = await _context.Products.ToListAsync();

            // Retrieve blocked products for the student
            // Retrieve blocked product ids for the student
            var blockedProductIds = await _context.BlockedProducts
                .Where(bp => bp.StudentId == studentId)
                .Select(bp => new { bp.ProductId, bp.IsBlocked })
                .ToListAsync();

            // Map the products to ProductDto objects and mark blocked products
            var productDtos = products.Select(p => new ProductDtoss
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                IsActive = p.IsActive,
                ImageURL = p.ImageURL,
                Amount = p.Amount,
                Price = p.Price,
                IsBlocked = blockedProductIds.Any(bp => bp.ProductId == p.Id && bp.IsBlocked) // Check if the current product's Id is in the list of blocked ProductIds and IsBlocked is true
            }).ToList();

            return productDtos;

        }


        ///<summary>
        ///
        /// Endpoint To retrieve List Of Category's Product , Like Drink will get all Product with Drink Category With their Blocked Status
        /// </summary>


        [HttpGet("ProductsByCategoryWithStatus/{studentId}/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDtoss>>> GetProductsByCategoryForStudent(int studentId, int categoryId)
        {
            try
            {
                // Retrieve products for the specified category
                var products = await _context.Products
                    .Where(p => p.CategoryId == categoryId)
                    .ToListAsync();

                // Retrieve blocked product ids for the student
                var blockedProductIds = await _context.BlockedProducts
                    .Where(bp => bp.StudentId == studentId)
                    .Select(bp => bp.ProductId)
                    .ToListAsync();

                // Map the products to ProductDto objects and mark blocked products
                var productDtos = products.Select(p => new ProductDtoss
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    IsActive = p.IsActive,
                    ImageURL = p.ImageURL,
                    Amount = p.Amount,
                    Price = p.Price,
                    IsBlocked = blockedProductIds.Contains(p.Id) // Check if the current product's Id is in the list of blocked ProductIds
                }).ToList();

                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



      







        // GET: api/canteen/BlockedProducts/{studentId}
        /// <summary>
        /// Retrieve the list of Categories With Their Products.
        /// </summary>
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            // Retrieve all categories
            var categories = await _context.Categories
                .Include(x=>x.Products)
                .ToListAsync();

            return categories;
        }



        [HttpGet("CanteenTransactionProducts")]
        public async Task<ActionResult<IEnumerable<CanteenTransactionProduct>>> GetCanteenTransactionProducts()
        {
            var canteenTransactionProducts = await _context.CanteenTransactionProducts.ToListAsync();
            return Ok(canteenTransactionProducts);
        }





        /// <summary>
        /// Endpoint to submit a transaction for purchasing process in the canteen.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a student to purchase products from the canteen by submitting a transaction.
        /// The submitted transaction includes information about the student, the products purchased, and the transaction type.
        /// </remarks>

        [HttpPost("SubmitTransaction")]
        public async Task<IActionResult> SubmitTransaction(TransactionRequestDto transactionRequest)
        {
            //var userId = HttpContext.Items["UserId"] as Guid?;
            
            
            // Check if the provided student ID is valid
            var student = await _context.Students.FindAsync(transactionRequest.StudentId);
            if (student == null)
            {
                return BadRequest("Invalid student ID.");
            }

            // Check if the student has a wallet
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.StudentId == student.Id);

            if (wallet == null)
            {
                return BadRequest("Student wallet not found.");
            }


            // Calculate the total transaction amount
            decimal totalTransactionAmount = transactionRequest.Products.Sum(p => p.Price * p.Amount);

            // Check if the transaction amount exceeds the daily limit of the wallet
            if (totalTransactionAmount > wallet.DailyLimit)
            {
                return BadRequest("Transaction amount exceeds the daily limit.");
            }

            // Check if the transaction type is 'wallet' and if the wallet has sufficient balance
            if (transactionRequest.TransactionType == "wallet")
            {
                // Check if the wallet has sufficient balance
                if (wallet.Balance < totalTransactionAmount)
                {
                    return BadRequest("Insufficient balance in the wallet.");
                }

                // Subtract the totalTransactionAmount from the wallet balance
                wallet.Balance -= totalTransactionAmount;

                // Update the transaction description
                transactionRequest.Description = $"Paid from wallet. {transactionRequest.Description}";
            }

            // Create a new transaction object
            var transaction = new CanteenTransaction
            {
                StudentID = transactionRequest.StudentId,
                TransactionDate = DateTime.Now,
                TransactionAmount = totalTransactionAmount,
                Created = DateTime.Now,
                TransactionType = transactionRequest.TransactionType,
                Description = transactionRequest.Description,
                CanteenTransactionProducts = new List<CanteenTransactionProduct>(),
                //CreatedBy = userId
            };

            // Add products to the transaction and update product amounts
            foreach (var productDto in transactionRequest.Products)
            {
                var product = await _context.Products.FindAsync(productDto.ProductId);
                if (product != null)
                {
                    // Check if the product amount is sufficient
                    if (product.Amount < productDto.Amount)
                    {
                        return BadRequest($"Insufficient amount of product '{product.Name}'.");
                    }

                    // Subtract the sold amount from the product
                    product.Amount -= productDto.Amount;

                    // Add the product to the transaction
                    transaction.CanteenTransactionProducts.Add(new CanteenTransactionProduct
                    {
                        Product = product,
                        Quantity = productDto.Amount,
                        TotalPrice = productDto.Price * productDto.Amount
                    });
                }
                else
                {
                    return BadRequest($"Product with ID {productDto.ProductId} not found.");
                }
            }

            // Add the transaction to the database
            _context.CanteenTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok();
        }


        // POST: api/BlockedProducts

        /// <summary>
        /// Block or unblock a product for a student.
        /// </summary>
        /// <param name="block">The block object containing product and student information.</param>
        [HttpPost("block-products")]
        public async Task<IActionResult> BlockProducts(List<BlockedProductDto> blockDtos)
        {
            if (blockDtos == null || !blockDtos.Any())
            {
                return BadRequest("No products provided to block.");
            }

            try
            {
                // Validate each BlockedProductDto
                foreach (var blockDto in blockDtos)
                {
                    if (!_context.Products.Any(p => p.Id == blockDto.ProductId) ||
                        !_context.Students.Any(s => s.Id == blockDto.StudentId))
                    {
                        return BadRequest($"Invalid product or student ID for product ID: {blockDto.ProductId}.");
                    }
                }

                // Update existing blocks or create new blocks for each product
                foreach (var blockDto in blockDtos)
                {
                    var existingBlock = await _context.BlockedProducts
                        .FirstOrDefaultAsync(bp => bp.ProductId == blockDto.ProductId && bp.StudentId == blockDto.StudentId);

                    if (existingBlock != null)
                    {
                        existingBlock.IsBlocked = blockDto.IsBlocked;
                    }
                    else
                    {
                        var newBlock = new BlockedProduct
                        {
                            ProductId = blockDto.ProductId,
                            StudentId = blockDto.StudentId,
                            IsBlocked = blockDto.IsBlocked
                        };
                        _context.BlockedProducts.Add(newBlock);
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        //private bool BlockedProductExists(int id)
        //{
        //    return _context.BlockedProducts.Any(e => e.Id == id);
        //}





     
    }


    

    


 

   
}
