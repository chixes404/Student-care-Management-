using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project.Shared.Models;
using Graduation_Project_Dashboard.Code;
using Graduation_Project_Dashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Graduation_Project.Dashboard.Pages.Chats
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : BasePageModel<IndexModel>
    {
        private readonly Graduation_Project_Dashboard.Data.ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;
        public List<UserWithLatestMessage> UsersWithLatestMessage { get; set; }

        public IndexModel(Graduation_Project_Dashboard.Data.ApplicationDatabaseContext context   , UserResolverService userService)
        {
            _context = context;
            _userService = userService;

        }


        public async Task OnGetAsync()
        {

            var CurrentUserId = _userService.GetCurrentUserID();

            var CurrentUser = await _context.Users.FindAsync(CurrentUserId);



            UsersWithLatestMessage = await _context.Chats
        .Where(c => c.UserId != null && c.UserId == c.CreatedBy && c.SchoolId == CurrentUser.SchoolId)
        .GroupBy(c => c.UserId)
        .Select(g => new
        {
            UserId = g.Key,
            LatestMessageDate = g.Max(c => c.Created),
            UnreadCount = g.Count(c => c.IsRead == null || c.IsRead == false)
        })
        .OrderBy(x => x.UnreadCount)
        .ThenByDescending(x => x.LatestMessageDate)
        .Select(x => new UserWithLatestMessage
        {
            UserId = x.UserId,
            UserName = _context.Users.FirstOrDefault(u => u.Id == x.UserId).UserName,
            LatestMessage = _context.Chats
                .Where(c => c.UserId == x.UserId && c.Created == x.LatestMessageDate)
                .OrderByDescending(c => c.Created)
                .FirstOrDefault()
        })
        .ToListAsync();
        }

    }
    public class UserWithLatestMessage
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Chat LatestMessage { get; set; }
    }
}
