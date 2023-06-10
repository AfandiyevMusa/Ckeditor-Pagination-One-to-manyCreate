using System;
namespace Fiorello.Models
{
	public class About : BaseEntity
	{
		public string Photo { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public string Profession { get; set; }
	}
}

