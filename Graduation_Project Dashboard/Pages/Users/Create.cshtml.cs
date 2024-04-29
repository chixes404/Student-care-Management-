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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;

namespace Graduation_Project_Dashboard.Pages.Users
{
    [Authorize(Roles = "Administrator")]
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
            ILogger<RegisterModel> logger,
             IWebHostEnvironment environment,

            EmailService emailSender,
            RoleManager<M.Role> roleManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            this._environment = environment;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        //[BindProperty]
        //public Guid? SelectedParentId { get; set; }
        //public IList<User> SearchUsers { get; set; }
       

        

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
            [PageRemote(PageHandler = "IsExist", AdditionalFields = "Id")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            

            [Required, MinLength(1, ErrorMessage = "At least one Role required")]
            [Display(Name = "Roles")]
            public List<string> RoleId { get; set; }

            [Display(Name = "Phone Number")]
            [RegularExpression(@"^\(?([0-9]{3,6})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]//minimum 10 maximum 13
            public string PhoneNumber { get; set; }

            public string Address { get; set; }

            [Required(ErrorMessage = "National ID is required")]
            [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be 14 digits")]
            public string NationalId { get; set; }

            [Compare("NationalId", ErrorMessage = "The NationalId and confirmation NationalId do not match.")]
            public string ConfirmNationalId { get; set; }

        }

        public async Task<IActionResult> OnPostAsync()
        {

            //var User = _context.Users.FirstOrDefault(u => u.Id == _userService.GetCurrentUserID());


            //if (!string.IsNullOrWhiteSpace(searchInput))
            //{
            //    SearchUsers = _context.Users.Where(u => u.NationalId.Contains(searchInput)).ToList();
            //}

            var CurrentUserId = _userService.GetCurrentUserID();
            var CurrentUser = await _context.Users.FindAsync(CurrentUserId);


          
            if (ModelState.IsValid)
            {
                if (Input.NationalId != Input.ConfirmNationalId)
                {
                    ModelState.AddModelError(string.Empty, "National IDs do not match.");
                    return Page();
                }

                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                
                user.PhoneNumber = Input.PhoneNumber;
                user.Email = user.UserName;
                user.EmailConfirmed = true;
                user.IsActive = true;
                user.Address =Input.Address;
                user.NationalId =Input.NationalId;
                user.SchoolId = CurrentUser.SchoolId;
                //user.MustChangePassword = true;


                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                 

                    foreach (string item in Input.RoleId)
                    {
                        var defaultrole = _roleManager.FindByIdAsync(item).Result;
                        if (defaultrole != null)
                        {
                            IdentityResult roleresult = await _userManager.AddToRoleAsync(user, defaultrole.Name);
                        }
                    }






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

            //ViewData["RoleId"] = new SelectList(_context.Roles.OrderBy(r => r.Name).Where(r => r.Name != "User"), "Id", "Name");

            return Page();

        }

        public JsonResult OnGetIsExist()
        {
            var valid = (null == _context.Users.FirstOrDefault(x => x.Email == Request.Query["User.Email"].ToString()));
            if (valid)
            {
                return new JsonResult(valid);
            }
            else
            {
                return new JsonResult("Email already exists.");
            }

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
