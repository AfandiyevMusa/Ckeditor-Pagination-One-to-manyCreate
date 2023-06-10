using System;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.Admin.ViewModels.Category
{
	public class CategoryCreateVM
	{
		[Required(ErrorMessage = "Dont be empty!")]
		public string Name { get; set; }
	}
}

