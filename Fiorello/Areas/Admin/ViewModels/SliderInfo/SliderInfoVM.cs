using System;
namespace Fiorello.Areas.Admin.ViewModels.SliderInfo
{
	public class SliderInfoVM
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public string SignImage { get; set; }
        public bool Status { get; set; } = true;
        public string CreateDate { get; set; }
    }
}

