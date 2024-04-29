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
using System.Text.RegularExpressions;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.IdentityModel.Tokens;
using NuGet.DependencyResolver;

namespace Graduation_Project_Dashboard.Pages.Teacher
{
    [Authorize(Roles = "Administrator,Configuration")]
    public class CreateModel : BasePageModel<CreateModel>
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly EmailService _emailSender;
        private readonly RoleManager<M.Role> _roleManager;
        private readonly IWebHostEnvironment _environment;


        public CreateModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context,
            UserResolverService userService,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            IWebHostEnvironment environment,
            ILogger<RegisterModel> logger,
            EmailService emailSender,
            RoleManager<M.Role> roleManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            this._environment = environment;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        //public IActionResult OnGet()
        //{
        //    //ViewData["Clinics"] = _context.Clinics.ToList();
        //    //ViewData["Specializtions"] = _context.Specializations.ToList();

        //    //return Page();
        //}



        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }



        public class InputModel
        {
            [Required]
            [StringLength(50)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(50)]
            public string LastName { get; set; }


            [Required]

            [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
            [Display(Name = "Email")]
            //[PageRemote(PageHandler = "IsExist", AdditionalFields = "Id")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Phone Number")]
            [RegularExpression(@"^\(?([0-9]{3,6})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]//minimum 10 maximum 13
            public string PhoneNumber { get; set; }

            public string Address { get; set; }

            [Required(ErrorMessage = "National ID is required")]
            [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be 14 digits")]
            public string NationalId { get; set; }

            [Compare("NationalId", ErrorMessage = "The NationalId and confirmation NationalId do not match.")]
            public string ConfirmNationalId { get; set; }



            [Required(ErrorMessage = "Please select at least one subject.")]
            public List<int> SelectedSubjects { get; set; }

            [Required(ErrorMessage = "Please select at least one grade.")]
            public List<int> SelectedGradeIds { get; set; }

            [Required(ErrorMessage = "Please select at least one class.")]
            public List<int> SelectedClassIds { get; set; }


        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Subjects"] = await _context.Subjects.ToListAsync();
            ViewData["Grades"] = await _context.Grades.ToListAsync();
            ViewData["Classes"] = await _context.Classes.ToListAsync();

            return Page();
        }
        public async Task<IActionResult>
        OnPostAsync()
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

            var SecureFileName = Regex.Replace($"{Input.FirstName.ToLower()}{Input.LastName.ToLower()}", "[^0-9A-Za-z_-]", "") + ext;
            var file = Path.Combine(_environment.WebRootPath, "uploads\\Teachers", SecureFileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }



            if (!ModelState.IsValid)
            {
                return Page();
            }

            else
            {

                var Teacher = new M.Teacher
                {
                    Name = Input.FirstName + " " + Input.LastName,
                    NationalID = Input.NationalId,
                    SchoolId = CurrentUser.SchoolId, //Just Fuckin test this
                    Created = DateTime.Now,
                    CreatedBy = CurrentUserId,

                    IsActive = true

                };


                _context.Teachers.Add(Teacher);
                await _context.SaveChangesAsync();


                foreach (var subjectId in Input.SelectedSubjects)
                {
                    var teacherSubject = new TeacherSubject { TeacherId = Teacher.Id, SubjectId = subjectId };
                    _context.TeacherSubjects.Add(teacherSubject);
                }
                await _context.SaveChangesAsync();

                // Associate the selected grade with the teacher
                foreach (var gradeId in Input.SelectedGradeIds)
                {
                    var teacherGrade = new TeacherGrade { TeacherId = Teacher.Id, GradeId = gradeId };
                    _context.TeacherGrades.Add(teacherGrade);
                }
                await _context.SaveChangesAsync();

                foreach (var classId in Input.SelectedClassIds)
                {
                    var teacherClass = new TeacherClass { TeacherId = Teacher.Id, ClassId = classId };
                    _context.TeacherClasses.Add(teacherClass);
                }
                await _context.SaveChangesAsync();


                var user = CreateUser();
                user.ImageURL = "/uploads/Teachers/" + SecureFileName;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.PhoneNumber = Input.PhoneNumber;
                user.Email = Input.Email;
                user.SchoolId = CurrentUser.SchoolId; //Just Fuckin test this
                user.EmailConfirmed = true;
                user.NationalId = Input.NationalId;
                user.IsActive = true;
                user.Address = Input.Address;
                //user.MustChangePassword = true;


                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    //var roles = _context.Roles
                    //    .Where (r => Input.RoleId.Contains(r.Id.ToString())).ToList();

                    //if (Input.RoleId != null)
                    //    await _userManager.AddToRolesAsync(user, roles);


                    var roleId = "b4018cb5-755e-468b-9802-a9917c37730e";
                    var defaultrole = _roleManager.FindByIdAsync(roleId).Result;
                    if (defaultrole != null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, defaultrole.Name);
                    }



                    Teacher.UserId = user.Id;
                    await _context.SaveChangesAsync();


                    var sb = new StringBuilder();
                    sb.Append("<p>Hi " + user.FirstName + " " + user.LastName + ",</p>");
                    sb.Append("<p><b>  Student Care Managment Tool URL: </b>");
                    sb.Append($"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}");
                    sb.Append("</p>");
                    sb.Append("<p><strong>Username: </strong>");
                    sb.Append(user.UserName);
                    sb.Append("</p>");
                    sb.Append("<p><strong>Password: </strong>");
                    sb.Append(Input.Password);
                    sb.Append("</p>");
                    sb.Append("<p style='color:#FF0000'>" + "You Should Renew Your Password when log into the Application ." + "</p>");

                    _emailSender.SendEmail(user.Email, "Your User credentials for School Care Managment tool", sb.ToString());

                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToPage("./Index");


                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }





            }
            return Page();

        }
        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        //public JsonResult OnGetIsExist()
        //{
        //    var valid = (null == _context.Users.FirstOrDefault(x => x.Email == Request.Query["User.Email"].ToString()));
        //    if (valid)
        //    {
        //        return new JsonResult(valid);
        //    }
        //    else
        //    {
        //        return new JsonResult("Email already exists.");
        //    }

        //}

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}


