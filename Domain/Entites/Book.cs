using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Book : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public string? CoverImage {  get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
