using System;
using Fiorello.Models;

namespace Fiorello.ViewModels
{
	public class ShopVM
	{
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Image> images { get; set; }
    }
}

