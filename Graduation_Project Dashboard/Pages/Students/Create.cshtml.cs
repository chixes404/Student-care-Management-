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
using static Graduation_Project_Dashboard.Pages.Students.CreateModel;
using System.Reflection.Metadata.Ecma335;
using IronBarCode;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.IO;



namespace Graduation_Project_Dashboard.Pages.Students
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

        public CreateModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context,
         IWebHostEnvironment webHostEnvironment,
            UserResolverService userService,
            ILogger<RegisterModel> logger,
            EmailService emailSender
           )
        {
            _context = context;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _emailSender = emailSender;
        }

        //[BindProperty]
        //public string SelectedParentId { get; set; }
        //public IList<User> SearchUsers { get; set; }
        [BindProperty]
        public IList<M.Grade> Gradees { get; set; } // Declare the Gradees property

      
        [BindProperty]
        public InputModel Input { get; set; }

    
        public class InputModel
        {
            [Required]
            [StringLength(50)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            public DateOnly DateOfBirth { get; set; }

            public int GradeId { get; set; }

            //public int ParentId { get; set; }

            public string NationalId {  get; set; }

            [Display]
            [Required]
            public int? Age { get; set; }

        }
        public async Task OnGetAsync()
        {
            Gradees =  await _context.Grades.ToListAsync();


        }


   


        public async Task<IActionResult> OnPostAsync()
        {

           

            var CurrentUserId = _userService.GetCurrentUserID();

            var CurrentUser = await _context.Users.FindAsync(CurrentUserId);

            var parentUser = _context.Users.FirstOrDefault(c => c.NationalId == Input.NationalId);

            if (parentUser != null) 
            {
                var userId = parentUser.Id;

                var parent = _context.Parents.FirstOrDefault(x => x.UserId == userId);

                if (parent != null) 
                {
                    _parentId = parent.Id;
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "There is No National ID With this Number .");
            }

            if (ModelState.IsValid)
            {
                var student = new Student();

                student.Name = Input.Name;
                student.SchoolId = CurrentUser.SchoolId;
                student.ParentId = _parentId;
                student.GradeId = Input.GradeId;
                student.DateOfBirth = Input.DateOfBirth;
                student.QrCodeUrl = null;
                student.age = Input.Age; 

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                student.QrCodeUrl = student.Id.ToString();

           
                    GeneratedBarcode qrcode = QRCodeWriter.CreateQrCode(student.QrCodeUrl, 200);
                    qrcode.SetMargins(10);
                    qrcode.ChangeBarCodeColor(Color.BlueViolet);

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "GeneratedQRCode");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, $"{ Input.Name}.png");
                    qrcode.SaveAsPng(filePath);

                    string fileName = Path.GetFileName(filePath);
                    string imageUrl = $"{Request.Scheme}://{Request.Host}/GeneratedQRCode/{fileName}";
                    student.QrCodeUrl = "/GeneratedQRCode/" + $"{Input.Name}.png";


                    _context.Attach(student).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    //ViewData["QRCodeCodeUri"] = imageUrl;

                    //return RedirectToPage("/Students/Index", new { QrCodeUrl = imageUrl }); // imageUrl is the generated QR code URL
                
            

                return RedirectToPage("./Index");

            }
            //ViewData["RoleId"] = new SelectList(_context.Roles.OrderBy(r => r.Name).Where(r => r.Name != "User"), "Id", "Name");

            // If ModelState is not valid, reload the page
            Gradees = await _context.Grades.ToListAsync();
            return Page();

        }

    

   

    }


}
