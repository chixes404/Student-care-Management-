using Reflections.Framework.RoleBasedSecurity.Extensions;
using System;

namespace Graduation_Project_Dashboard.Services
{
    public class UserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public Guid GetCurrentUserID()
        {
            if (_context.HttpContext.User.Identity.IsAuthenticated)
            {
                return _context.HttpContext.User.Identity.GetUserId();
            }
            else
            {
                return Guid.Empty;
            }
        }

        ////public int GetBusinessUnitId()
        ////{
        ////    if (_context.HttpContext.User.Identity.IsAuthenticated)
        ////    {
        ////        return _context.HttpContext.User.Identity.BusinessUnitId();
        ////    }
        ////    else
        ////    {
        ////        return 0;
        ////    }
        ////}
    }
}
