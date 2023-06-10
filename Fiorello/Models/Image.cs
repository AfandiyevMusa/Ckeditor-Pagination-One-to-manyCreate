using System;
namespace Fiorello.Models
{
	public class Image:BaseEntity
	{
		public string Images { get; set; }
		public int ProductId { get; set; }
		public bool IsMain { get; set; }
		public Product Product { get; set; }
    }
}

