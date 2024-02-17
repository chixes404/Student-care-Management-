using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Graduation_Project.Shared.DTO
{
    public class FileUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }

        //[Required]
        public string? FileName { get; set; }

        //[Required]
        public string? FileDescription { get; set; }
    }
}
