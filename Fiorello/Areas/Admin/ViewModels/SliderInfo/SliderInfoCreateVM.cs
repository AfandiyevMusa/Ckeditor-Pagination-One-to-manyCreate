using System;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.Admin.ViewModels.SliderInfo
{
	public class SliderInfoCreateVM
	{
        [Required]
        public IFormFile SignImage { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Info { get; set; }
    }
}

