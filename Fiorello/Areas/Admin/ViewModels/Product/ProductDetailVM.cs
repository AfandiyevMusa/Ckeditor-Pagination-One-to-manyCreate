using System;
namespace Fiorello.Areas.Admin.ViewModels.Product
{
	public class ProductDetailVM
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Discount { get; set; }
        public IEnumerable<string> Images { get; set; }
        public string CategoryName { get; set; }
        public string CreateDate { get; set; }
    }
}

