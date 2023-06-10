using System;
using Fiorello.ViewModels;
using Fiorello.Data;
using Fiorello.Services;
using Fiorello.Services.Interfaces;
using Fiorello.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.ViewComponents
{
	public class SliderViewComponent:ViewComponent
	{
		private readonly AppDbContext _context;
        private readonly ISliderService _sliderService; 

        public SliderViewComponent(AppDbContext context, ISliderService sliderService)
		{
			_context = context;
            _sliderService = sliderService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
            IEnumerable<Slider> sliders = await _sliderService.GetAllByStatusAsync();
            SlidersInfo sliderInfo = await _context.slidersInfos.Where(m => !m.SoftDelete).FirstOrDefaultAsync();
            SliderVM model = new()
            {
                Sliders = sliders,
                SlidersInfo = sliderInfo
            };

            return await Task.FromResult(View(model));
        }
	}
}

