using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project_Dashboard.Code;
using Graduation_Project_Dashboard.Services;
using Graduation_Project.Shared.Models;
using M = Graduation_Project.Shared.Models;
using Graduation_Project_Dashboard.Data;
using Microsoft.AspNetCore.Identity;
using Reflections.Framework.RoleBasedSecurity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Graduation_Project_Dashboard.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using Microsoft.EntityFrameworkCore;
using static Graduation_Project_Dashboard.Pages.Products.CreateModel;
using System.Reflection.Metadata.Ecma335;
//using IronBarCode;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;



namespace Graduation_Project_Dashboard.Pages.Products
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : BasePageModel<CreateModel>
    {

        private int _parentId;
        private readonly ApplicationDatabaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<RegisterModel> _logger;
        private readonly EmailService _emailSender;
        private readonly UserResolverService _userService;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context,
            IWebHostEnvironment environment,
            UserResolverService userService,
            ILogger<RegisterModel> logger,
            EmailService emailSender
           )
        {
            _context = context;
            _userService = userService;
            this._environment = environment;
            _logger = logger;
            _emailSender = emailSender;
        }

        //[BindProperty]
        //public string SelectedParentId { get; set; }
        //public IList<User> SearchUsers { get; set; }
        [BindProperty]
        public IList<M.Category> Categories { get; set; } // Declare the Gradees property


        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Required]
            [StringLength(50)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]

            [StringLength(100)]
            public string Description { get; set; }
            [Required]

            public int CategoryId { get; set; }


            public bool IsActive { get; set; }
         
            [Required]

            public int Amount { get; set; }
            [Required]

            public decimal Price { get; set; }
            [Required(ErrorMessage = "Please select at least one grade.")]
            public int SelectedCategoryIds { get; set; }
        }
        public async Task OnGetAsync()
        {

            ViewData["Categories"] = await _context.Categories.ToListAsync();

        }





        public async Task<IActionResult> OnPostAsync()
        {



            var CurrentUserId = _userService.GetCurrentUserID();

            var CurrentUser = await _context.Users.FindAsync(CurrentUserId);


            string[] permittedExtensions = { ".png", ".jpg" };

            var ext = Path.GetExtension(Upload.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                // The extension is invalid ... discontinue processing the file
                ModelState.AddModelError("user.ImageUrl", "The file extension is invalid");
                return Page();
            }

            var SecureFileName = Regex.Replace($"{Input.Name.ToLower()}", "[^0-9A-Za-z_-]", "") + ext;
            var file = Path.Combine(_environment.WebRootPath, "uploads\\products", SecureFileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }

            if (ModelState.IsValid)
            {
                var product = new Product();

                product.Name = Input.Name;
                //product.SchoolId = CurrentUser.SchoolId;
                product.Description = Input.Description;
                product.CategoryId = Input.SelectedCategoryIds;
                product.ImageURL = "/uploads/products/" + SecureFileName;
                product.Amount = Input.Amount;
                product.Price = Input.Price;
                product.IsActive = true;
                product.CreatedBy = CurrentUserId;
                product.Created = DateTime.Now;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();



                return RedirectToPage("./Index");

            }
            //ViewData["RoleId"] = new SelectList(_context.Roles.OrderBy(r => r.Name).Where(r => r.Name != "User"), "Id", "Name");

            // If ModelState is not valid, reload the page
                Categories = await _context.Categories.ToListAsync();
            return Page();

        }





    }


}
