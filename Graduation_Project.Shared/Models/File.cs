using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Graduation_Project.Shared.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Shared.Models;

public partial class File : BaseEntity
{
    [NotMapped]
    public IFormFile FileData { get; set; }


    [Required]
    public string FileName { get; set; }

    public string FileDescription { get; set; }

    [Required]
    public string FileExtension { get; set; }

    public long FileSize { get; set; }

    [Required]
    [Column("FileURL")]
    public string FileUrl { get; set; }

    public int TeacherID { get; set; }

    public Teacher Teacher { get; set; }    






}
