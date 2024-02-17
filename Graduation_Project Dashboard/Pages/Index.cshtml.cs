using Graduation_Project_Dashboard.Code;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Graduation_Project_Dashboard.Pages
{
    public class IndexModel : BasePageModel<IndexModel>
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}