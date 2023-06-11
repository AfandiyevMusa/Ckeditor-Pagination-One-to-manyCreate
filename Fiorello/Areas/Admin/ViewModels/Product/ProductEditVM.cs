using System;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.Admin.ViewModels.Product
{
	public class ProductEditVM
	{
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<IFormFile> newImages { get; set; }
    }
}

