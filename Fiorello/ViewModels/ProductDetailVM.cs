using System;
using Fiorello.Models;

namespace Fiorello.ViewModels
{
	public class ProductDetailVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CategoryName { get; set; }
		public decimal ActualPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
		public byte Percent { get; set; }
		public string Description { get; set; }
		public ICollection<Image> Images { get; set; }
	}
}

