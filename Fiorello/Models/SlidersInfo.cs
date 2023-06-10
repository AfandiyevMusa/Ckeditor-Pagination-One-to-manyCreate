using System;
namespace Fiorello.Models
{
	public class SlidersInfo:BaseEntity
	{
        public string Title { get; set; }
        public string Info { get; set; }
        public string SignImage { get; set; }
        public bool Status { get; set; } = true;
    }
}

