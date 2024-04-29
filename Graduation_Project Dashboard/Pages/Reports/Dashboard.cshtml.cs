using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Shared.Models;
using M = Graduation_Project.Shared.Models;
using Graduation_Project_Dashboard.Code;
using Graduation_Project_Dashboard.Services;
using Graduation_Project_Dashboard.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Dashboard.Pages.Reports
{

    [Authorize(Roles = "Administrator,Reports")]
    public class DashboardModel : BasePageModel<DashboardModel>

    {
        private readonly ApplicationDatabaseContext _context;
        private readonly UserResolverService _userService;


        public DashboardModel(ApplicationDatabaseContext context, UserResolverService userService)
        {
            _context = context;
            _userService = userService;

        }


        public class MessageViewModel
        {
            public string UserName { get; set; }
            public string Description { get; set; }
            public string CreatedDate { get; set; }

            public string ImageUrl { get; set; }
        }

        public class BookingData
        {
            public string Date { get; set; }
            public string Alpha { get; set; }
        }

        //public class CalenderBooking
        //{
        //    public int id { get; set; }
        //    public int title { get; set; }
        //    public int start { get; set; }
        //    public int end { get; set; }
        //}
        public class UserBookingViewModel
        {
            public Guid UserId { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string MobileNumber { get; set; }
            //public string BookingStatus { get; set; }
            ////public int InsuranceId { get; set; }
            //public string ClinicName { get; set; }
            //public int  { get; set; }

        }


        public string getUserNameById(int parentId)
        {
            var parent = _context.Parents.FirstOrDefault(x => x.Id == parentId);
            return parent.Name;
        }

        public IList<User> User { get; set; } = default!;
        public int UserCount { get; set; } // All Users
        public decimal UserAvg { get; set; }

        public IList<Student> Students { get; set; } = new List<Student>(); // All Clinics
        public IList<Grade> Grades { get; set; } = new List<Grade>(); // All Clinics
        public int StudentCount { get; set; }
        public decimal StudentAvg { get; set; }

        public IList<M.CanteenTransaction> CanteenTransactionToday { get; set; } = new List<M.CanteenTransaction>(); // All Booking Today
        public IList<M.ParentTransaction> CalenderBookings { get; set; } = new List<M.ParentTransaction>(); // All Booking 
        public int BookingCountToday { get; set; }
        public decimal BookingAvgToday { get; set; }

        public class GradeCount
        {
            public string GradeTitle { get; set; }
            public int Count { get; set; }
        }

        public Dictionary<string, int> GradeCountsPerStudent { get; set; }  // Number of bookings in each clinic
        public IList<int> GetUserCountsPerMonth { get; set; } = new List<int>(); // Number of bookings per Month
        public IList<int> BookingCountsPerYear { get; set; } = new List<int>(); // Number of bookings per Year
        public IList<int> ParentTransactionCountsPerYear { get; set; } = new List<int>(); // Number of bookings per Year


        public List<BookingData> BookingCountsPerWeek { get; set; } = new List<BookingData>();

        public int[] ProfileCountsPerMonth { get; set; } = new int[12]; // Number of Users per Month

        public IList<MessageViewModel> NewMessages { get; set; } = new List<MessageViewModel>();
        public IList<MessageViewModel> PendingMessages { get; set; } = new List<MessageViewModel>();
        public IList<MessageViewModel> ArchivedMessages { get; set; } = new List<MessageViewModel>();

        public int TotalNewMessages { get; set; }
        public int TotalPendingMessages { get; set; }
        public int TotalArchivedMessages { get; set; }
        public int TotalMessages { get; set; }







        public async Task OnGetAsync()
        {
            //User = await _context.Users
            //.ToListAsync();

            CalenderBookings = await _context.ParentTransactions.ToListAsync();

            UserCount = await _context.Users
                .Where(u => _context.UserRoles
                .Any(ur => ur.UserId == u.Id && _context.Roles
                .Any(r => r.Id == ur.RoleId && r.Name == "User")))
                .CountAsync();

            int UserSum = 0;

            // Calculate the sum of numbers from 1 to 15
            for (int i = 1; i <= UserCount; i++)
            {
                UserSum += i;
            }
            //UserAvg = UserCount > 0 ? (decimal)UserCount / UserSum : 0;

            Students = await PGetAllStudentsAsyncaUser();
            StudentCount = Students.Count;
            int clinicSum = 0;

            // Calculate the sum of numbers from 1 to 15
            for (int i = 1; i <= StudentCount; i++)
            {
                clinicSum += i;
            }

            StudentAvg = StudentCount > 0 ? (decimal)StudentCount / clinicSum : 0;


            Grades = await GetAllGradesAsyncaUser();



            CanteenTransactionToday = await GetCanteenTransactionTodayAsync();
            BookingCountToday = CanteenTransactionToday.Count;
            int BookingsTodaySum = 0;

            // Calculate the sum of numbers from 1 to 15
            for (int i = 1; i <= BookingCountToday; i++)
            {
                BookingsTodaySum += i;
            }
            BookingAvgToday = BookingCountToday > 0 ? (decimal)BookingCountToday / BookingsTodaySum : 0;


            GradeCountsPerStudent = await GetgradePerStudentCountsAsync();  // booking per clinic 


            BookingCountsPerYear = await GetProductColumnCountsPerMonthAsync();   // booking per Year

            ParentTransactionCountsPerYear = await GetParentTransactionCountsPerYear();


            GetUserCountsPerMonth = await GetUsersLast30DaysAsync();  //Parent in last 30 days (First Card)



            ProfileCountsPerMonth = await GetProfileCountsLast12MonthsAsync();   // Canteen Transaction  per Year

            //await GetUserAndBookingCountsPerMonthAsync();

            // FullCalender

            //Bookings = await _context.Bookings.ToListAsync();

            //Bookings = await (
            //    from booking in _context.Bookings
            //    join clinic in _context.Clinics on booking.ClinicId equals clinic.Id
            //    join bookingStatus in _context.BookingStatuses on booking.StatusId equals bookingStatus.Id
            //    join user in _context.Users on booking.UserId equals user.Id
            //    join profile in _context.Profiles on user.Id equals profile.UserId
            //    select new UserBookingViewModel
            //    {
            //        FirstName = user.FirstName,
            //        LastName = user.LastName,
            //        Email = user.Email,
            //        MobileNumber = profile.MobileNumber,
            //        ClinicName = clinic.Name,
            //        BookingStatus = bookingStatus.Name
            //    }
            //).ToListAsync();


            // bookingsData will contain the retrieved data















            //// All Messages 

            //NewMessages = await GetNewMessagesAsync();

            //// Get the total count of new messages
            //TotalNewMessages = _context.ContactUs
            //        .Count(c => c.StatusId == 1);


            //PendingMessages = await GetPendingMessagesAsync();

            //// Get the total count of PendingMessages
            //TotalPendingMessages = _context.ContactUs
            //     .Count(c => c.StatusId == 2);



            //ArchivedMessages = await GetArchivedMessagesAsync();

            //// Get the total count of ArchiveMessages
            //TotalArchivedMessages = _context.ContactUs
            //     .Count(c => c.StatusId == 3);

            //TotalMessages = _context.ContactUs.Count();








            var CurrentUserId = _userService.GetCurrentUserID();

            var CurrentUser = await _context.Users.FindAsync(CurrentUserId);




            TempData["WelcomeMessage"] = $"Hello {CurrentUser.FirstName}";




        }

        public string GetUserRole(Guid id)
        {
            List<Role> roles = _context.Roles.FromSqlRaw(@"SELECT dbo.Roles.Id, dbo.Roles.Description, dbo.Roles.Name, dbo.Roles.NormalizedName, dbo.Roles.ConcurrencyStamp
                FROM dbo.Roles INNER JOIN
                 dbo.UserRoles ON dbo.Roles.Id = dbo.UserRoles.RoleId
                WHERE (dbo.UserRoles.UserId = {0})", id.ToString()).ToList<Role>();

            string rolesString = "";
            foreach (Role item in roles)
            {
                if (item != roles.Last())
                {
                    rolesString += item.Name + "<br/>";
                }
                else
                {
                    rolesString += item.Name;
                }
            }

            return rolesString;
        }

        public async Task<IList<Student>> PGetAllStudentsAsyncaUser()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }
        public async Task<IList<Grade>> GetAllGradesAsyncaUser()
        {
            return await _context.Grades.AsNoTracking().ToListAsync();
        }
        public async Task<IList<M.CanteenTransaction>> GetCanteenTransactionTodayAsync()
        {
            DateTime today = DateTime.Today;
            return await _context.CanteenTransactions
                .Where(b => b.Created.Date == today)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Dictionary<string, int>> GetgradePerStudentCountsAsync()
        {
            // Fetch the students and grades from the database
            var studentsWithGrades = await _context.Students
                .Join(_context.Grades,
                    student => student.GradeId,
                    grade => grade.Id,
                    (student, grade) => new { Student = student, Grade = grade })
                .AsNoTracking()
                .ToListAsync();

            // Group the students by grade in memory
            var studentCounts = studentsWithGrades
    .GroupBy(item => item.Grade.GradeTitle)
    .Select(group => new { Grade = group.Key, Count = group.Count() })
    .ToDictionary(item => item.Grade, item => (int)item.Count); // Explicitly cast Count to int


            return studentCounts;
        }


        ////private async Task<IList<int>> GetBookingCountsPerMonthAsync()
        ////{
        ////    // Fetch the number of bookings for each month
        ////    var bookingCountsPerMonth = await _context.Bookings
        ////        .GroupBy(b => new { b.Created.Year, b.Created.Month })
        ////        .Select(group => group.Count())
        ////        .ToListAsync();

        ////    return bookingCountsPerMonth;
        ////}

        //Get from last 12 months
        public async Task<int[]> GetProductColumnCountsPerMonthAsync()
        {
            int[] BookingCounts = new int[12];

            // Fetch the number of users created in each month for the last 12 months
            var actualCounts = await _context.Products
                .AsNoTracking()
                .Where(c => c.Created >= DateTime.Now.AddMonths(-11)) // Adjust this to consider the last 12 months
                .GroupBy(c => c.Created.Month)
                .Select(group => new { Month = group.Key, Count = group.Count() })
                .ToListAsync();

            // Update counts in the array with actual values
            foreach (var entry in actualCounts)
            {
                int monthIndex = entry.Month - 1;
                BookingCounts[monthIndex] = entry.Count;
            }

            return BookingCounts;



        }


        public async Task<int[]> GetParentTransactionCountsPerYear()
        {
            int[] ParentTransactionsCounts = new int[12];

            // Fetch the number of users created in each month for the last 12 months
            var actualCounts = await _context.ParentTransactions
                .AsNoTracking()
                .Where(c => c.TransactionDate >= DateTime.Now.AddMonths(-11)) // Adjust this to consider the last 12 months
                .GroupBy(c => c.TransactionDate.Month)
                .Select(group => new { Month = group.Key, Count = group.Count() })
                .ToListAsync();

            // Update counts in the array with actual values
            foreach (var entry in actualCounts)
            {
                int monthIndex = entry.Month - 1;
                ParentTransactionsCounts[monthIndex] = entry.Count;
            }

            return ParentTransactionsCounts;



        }


        //private async Task<List<BookingData>> GetBookingLast7DaysAsync()
        //{
        //    List<BookingData> bookingData = new List<BookingData>();

        //    // Fetch the number of bookings for each day in the last 7 days
        //    var actualCounts = await _context.Bookings
        //        .AsNoTracking()
        //        .Where(b => b.BookingDate >= DateTime.Now.Date.AddDays(-6)) // Adjust this to consider the last 7 days
        //        .GroupBy(b => b.BookingDate.Date)
        //        .Select(group => new { Date = group.Key, Count = group.Count() })
        //        .ToListAsync();

        //    // Convert to the desired format
        //    foreach (var entry in actualCounts)
        //    {
        //        var dataEntry = new BookingData
        //        {
        //            Date = entry.Date.ToString("MM/dd/yy"),
        //            Alpha = entry.Count.ToString()
        //        };

        //        bookingData.Add(dataEntry);
        //    }

        //    return bookingData;
        //}





        //private async Task<int[]> GetProfileCountsPerMonthAsync()
        //    {
        //        // Initialize array with zero counts for all months
        //        int[] userCounts = new int[12];

        //        // Fetch the number of users created in each month
        //        var actualCounts = await _context.Clinics
        //            .GroupBy(c => c.Created.Month)
        //            .Select(group => new { Month = group.Key, Count = group.Count() })
        //            .ToListAsync();

        //        // Update counts in the array with actual values
        //        foreach (var entry in actualCounts)
        //        {
        //            userCounts[entry.Month - 1] = entry.Count;
        //        }

        //        return userCounts;
        //    }

        public async Task<int[]> GetProfileCountsLast12MonthsAsync()
        {
            int[] CanteenTransactionsCounts = new int[12];

            // Fetch the number of users created in each month for the last 12 months
            var actualCounts = await _context.CanteenTransactions
                .AsNoTracking()
                .Where(c => c.Created >= DateTime.Now.AddMonths(-11)) // Adjust this to consider the last 12 months
                .GroupBy(c => c.Created.Month)
                .Select(group => new { Month = group.Key, Count = group.Count() })
                .ToListAsync();

            // Update counts in the array with actual values
            foreach (var entry in actualCounts)
            {
                int monthIndex = entry.Month - 1;
                CanteenTransactionsCounts[monthIndex] = entry.Count;
            }

            return CanteenTransactionsCounts;
        }

        public async Task<int[]> GetUsersLast30DaysAsync()
        {
            int[] bookingCount = new int[30];

            // Fetch the number of profiles created for each day in the last 30 days
            var actualCounts = await _context.Parents
                .AsNoTracking()
                .Where(c => c.Created >= DateTime.Now.Date.AddDays(-30)) // Adjust this to consider the last 30 days
                .GroupBy(c => c.Created.Date)
                .Select(group => new { Day = group.Key, Count = group.Count() })
                .ToListAsync();

            // Update counts in the array with actual values
            foreach (var entry in actualCounts)
            {
                int dayIndex = (DateTime.Now.Date - entry.Day).Days;

                // Ensure dayIndex is within the range [0, 29]
                if (dayIndex >= 0 && dayIndex < 30)
                {
                    bookingCount[dayIndex] = entry.Count;
                }
            }

            return bookingCount;
        }



















        //private async Task<IList<MessageViewModel>> GetNewMessagesAsync()
        //{
        //    var newMessages = await _context.ContactUs
        //        .AsNoTracking()
        //        .Where(c => c.StatusId == 1)
        //        .OrderBy(c => c.Created) // Order by the date of messages in ascending order
        //        .Select(c => new MessageViewModel
        //        {
        //            UserName = $"{c.CreatedByNavigation.FirstName} {c.CreatedByNavigation.LastName}",
        //            Description = c.Description,
        //            CreatedDate = $"{(DateTime.Now - c.Created).TotalHours:F0} hours ago",
        //            ImageUrl = c.CreatedByNavigation.ImageURL
        //        })
        //        .ToListAsync();

        //    return newMessages;
        //}


        //private async Task<IList<MessageViewModel>> GetPendingMessagesAsync()
        //{
        //    var pendingMessages = await _context.ContactUs
        //        .AsNoTracking()
        //        .Where(c => c.StatusId == 2)
        //        .OrderBy(c => c.Created) // Order by the date of messages in ascending order
        //        .Select(c => new MessageViewModel
        //        {
        //            UserName = $"{c.CreatedByNavigation.FirstName} {c.CreatedByNavigation.LastName}",
        //            Description = c.Description,
        //            CreatedDate = $"{(DateTime.Now - c.Created).TotalHours:F0} hours ago",
        //            ImageUrl = c.CreatedByNavigation.ImageURL

        //        })
        //        .ToListAsync();

        //    return pendingMessages;
        //}

        //private async Task<IList<MessageViewModel>> GetArchivedMessagesAsync()
        //{
        //    var archivedMessages = await _context.ContactUs
        //        .AsNoTracking()
        //        .Where(c => c.StatusId == 3)
        //        .OrderBy(c => c.Created)
        //        .Select(c => new MessageViewModel
        //        {
        //            UserName = $"{c.CreatedByNavigation.FirstName} {c.CreatedByNavigation.LastName}",
        //            Description = c.Description,
        //            CreatedDate = $"{(DateTime.Now - c.Created).TotalHours:F0} hours ago",
        //            ImageUrl = c.CreatedByNavigation.ImageURL

        //        })
        //        .ToListAsync();

        //    return archivedMessages;
        //}


       



    }
}




