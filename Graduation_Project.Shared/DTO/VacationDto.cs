using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Shared.DTO
{
    public class VacationDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Year { get; set; }

        // Navigation properties


    }
}
