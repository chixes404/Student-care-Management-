using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class HomeworkUploadModel
    {

        [Required(ErrorMessage = "GradeId is required")]
        public int ?GradeId { get; set; }

        public int? ClassId { get; set; }

        public int TeacherId { get; set; }

        [Required(ErrorMessage = "File is required")]
        public IFormFile? File { get; set;  }
        public string? Message { get; set; }

    }
}
