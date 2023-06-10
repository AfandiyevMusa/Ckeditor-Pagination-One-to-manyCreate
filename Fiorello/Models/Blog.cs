using System;
namespace Fiorello.Models
{
	public class Blog:BaseEntity
	{
		public string Image { get; set; }
		public string Title { get; set; }
        public string Info { get; set; }
    }
}

