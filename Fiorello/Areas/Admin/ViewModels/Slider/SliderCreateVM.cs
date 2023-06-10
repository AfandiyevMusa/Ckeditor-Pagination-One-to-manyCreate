using System;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.Admin.ViewModels.Slider
{
	public class SliderCreateVM
	{
		[Required]
		public List<IFormFile> Images { get; set; }
	}
}

