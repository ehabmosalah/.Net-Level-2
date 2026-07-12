using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;
        [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        // Relationship is flattened for security and simplicity
        public string CategoryName { get; set; } = string.Empty;
    }
}
