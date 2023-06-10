using System;
using Fiorello.Areas.Admin.ViewModels.SliderInfo;
using Fiorello.Models;

namespace Fiorello.Services.Interfaces
{
	public interface ISliderInfoService
	{
        Task<IEnumerable<SlidersInfo>> GetAllAsync();
        Task<SlidersInfo> GetByIdAsync(int? id);
        Task<IEnumerable<SliderInfoVM>> GetAllMappedDatasAsync();
        SliderInfoDetailVM GetMappedDatasAsync(SlidersInfo dbSliderInfo);
        //Task CreateAsync(List<IFormFile> images);
        Task DeleteAsync(int id);
        //Task EditAsync(Slider slider, IFormFile newImage);
        Task<bool> ChangeStatusAsync(SlidersInfo sliderInfo);
        Task<int> GetCountAsync();
    }
}

