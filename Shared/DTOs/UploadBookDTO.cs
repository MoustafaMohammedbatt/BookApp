using Domain.Entites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
   
        public class UploadBookDTO : BaseModelDTO
        { 
            public string Title { get; set; } = null!;
            public string Description { get; set; } = null!;
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public IFormFile? CoverImage { get; set; }
            public DateTime PublicationDate { get; set; }
            public int AuthorId { get; set; }
            public int CategoryId { get; set; }
    
        public Language BookLanguage { get; set; }
        }

    }

