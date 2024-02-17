using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Graduation_Project_Dashboard.Code
{
    [Authorize]
    public class BasePageModel<T> : PageModel where T : BasePageModel<T>
    {

    }
}
