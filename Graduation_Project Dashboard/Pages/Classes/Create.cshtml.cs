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
using static Graduation_Project_Dashboard.Pages.Grades.CreateModel;
using System.Reflection.Metadata.Ecma335;
//using IronBarCode;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;



namespace Graduation_Project_Dashboard.Pages.Classes  
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



        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {



            [Required]
            [StringLength(50)]
            [Display(Name = "Name")]
            public string Name { get; set; }


        }
        //public async Task OnGetAsync()
        //{


        //}





        public async Task<IActionResult> OnPostAsync()
        {



            var CurrentUserId = _userService.GetCurrentUserID();

            var CurrentUser = await _context.Users.FindAsync(CurrentUserId);



            if (ModelState.IsValid)
            {
                var classs = new Class();

                classs.ClassTitle = Input.Name;
                classs.CreatedBy = CurrentUserId;
                classs.Created = DateTime.Now;

                _context.Classes.Add(classs);
                await _context.SaveChangesAsync();

                

                return RedirectToPage("./Index");

            }
    
            return Page();

        }





    }


}
