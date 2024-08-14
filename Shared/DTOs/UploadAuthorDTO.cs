using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class UploadAuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }

        [Required(ErrorMessage = "CoverImage is required")]
        public IFormFile CoverImage { get; set; } = null!;
    }
}
