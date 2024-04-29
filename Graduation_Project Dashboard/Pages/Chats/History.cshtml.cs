using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project.Shared.Models;
using Graduation_Project_Dashboard.Services;




namespace Graduation_Project.Dashboard.Pages.Chats
{
    public class HistoryModel : PageModel
    {

        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;
        public HistoryModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context, UserResolverService userService)
        {
            _context = context;
            _userService = userService;

        }


        [BindProperty(SupportsGet = true)]
        public Guid UserId { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public List<Chat>? ChatHistory { get; set; }

       
        public async Task OnGetAsync(Guid Id)
        {



            UserId = Id; // Assign the userId to the UserId property
            ChatHistory = await _context.Chats
            .Where(c => c.UserId == UserId)
            .OrderBy(c => c.Created)
            .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var CurrentUserId = _userService.GetCurrentUserID();

            var CurrentUser = await _context.Users.FindAsync(CurrentUserId);

            if (!string.IsNullOrEmpty(Message))
            {
                var chat = new Chat(UserId)
                {
                    UserId = UserId,
                    Message = Message,
                    CreatedBy = CurrentUserId, // Assuming the admin is sending the message
                    Created = DateTime.Now , 
                    SchoolId = CurrentUser.SchoolId
                };

                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();


                var lastMessage = await _context.Chats
         .Where(c => c.UserId == UserId && c.CreatedBy == UserId)
         .OrderByDescending(c => c.Created)
         .FirstOrDefaultAsync();

                if (lastMessage != null)
                {
                    lastMessage.IsRead = true;
                    await _context.SaveChangesAsync();
                }
            }
            Guid Id = UserId;
            return RedirectToPage("./History", new { Id });
        }
    }
}



