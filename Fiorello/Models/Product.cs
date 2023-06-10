using System;
namespace Fiorello.Models
{
	public class Product:BaseEntity
	{
		public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; } 
        public ICollection<Image> Images { get; set; } 
    }
}

