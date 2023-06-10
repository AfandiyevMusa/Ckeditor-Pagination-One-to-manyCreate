using System;
namespace Fiorello.Areas.Admin.ViewModels.SliderInfo
{
	public class SliderInfoEditVM
	{
        public string SignImage { get; set; }
        public IFormFile NewSignImage { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
    }
}

