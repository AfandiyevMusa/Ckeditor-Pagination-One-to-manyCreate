using System;
using Fiorello.Models;
using Fiorello.Areas.Admin.ViewModels.Slider;

namespace Fiorello.Services.Interfaces
{
	public interface ISliderService
	{
        Task<IEnumerable<Slider>> GetAllAsync();
        Task<Slider> GetByIdAsync(int? id);
        Task<IEnumerable<SliderVM>> GetAllMappedDatasAsync();
        SliderDetailVM GetMappedDatasAsync(Slider dbSlider);
        //Task CreateAsync(List<IFormFile> images);
        Task DeleteAsync(int id);
        //Task EditAsync(Slider slider, IFormFile newImage);
        Task<List<Slider>> GetAllByStatusAsync();
        Task<bool> ChangeStatusAsync(Slider slider);
        Task<int> GetCountAsync();
    }
}

