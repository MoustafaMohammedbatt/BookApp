using Domain.Entites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class UploadCategoryDTO : BaseModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        [Required(ErrorMessage = "CoverImage is required")]
        public IFormFile CoverImage { get; set; } = null!;
        public Language BookLanguage { get; set; }

    }
}
