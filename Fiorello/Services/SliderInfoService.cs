using System;
using Fiorello.Areas.Admin.ViewModels.SliderInfo;
using Fiorello.Data;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Services
{
	public class SliderInfoService : ISliderInfoService
	{
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderInfoService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<bool> ChangeStatusAsync(SlidersInfo sliderInfo)
        {
            if (sliderInfo.Status && await GetCountAsync() != 1)
            {
                sliderInfo.Status = false;
            }
            else
            {
                sliderInfo.Status = true;
            }

            await _context.SaveChangesAsync();

            return sliderInfo.Status;
        }

        public async Task DeleteAsync(int id)
        {
            SlidersInfo dbSliderInfo = await _context.slidersInfos.FirstOrDefaultAsync(m => m.Id == id);

            _context.slidersInfos.Remove(dbSliderInfo);

            string path = Path.Combine(_env.WebRootPath + "/img/" + dbSliderInfo.SignImage);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public async Task<IEnumerable<SlidersInfo>> GetAllAsync()
        {
            return await _context.slidersInfos.ToListAsync();
        }

        public async Task<IEnumerable<SliderInfoVM>> GetAllMappedDatasAsync()
        {
            List<SliderInfoVM> sliderInfoList = new();
            IEnumerable<SlidersInfo> slidersInfos = await GetAllAsync();

            foreach (SlidersInfo slidersInfo in slidersInfos)
            {
                SliderInfoVM model = new()
                {
                    Id = slidersInfo.Id,
                    Info = slidersInfo.Info,
                    Title = slidersInfo.Title,
                    Status = slidersInfo.Status,
                    SignImage = slidersInfo.SignImage,
                    CreateDate = slidersInfo.CreatedDate.ToString("MM/dd/yyyy")
                };
                sliderInfoList.Add(model);
            }

            return sliderInfoList;
        }

        public async Task<SlidersInfo> GetByIdAsync(int? id)
        {
            return await _context.slidersInfos.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.slidersInfos.Where(m => m.Status).CountAsync();
        }

        public SliderInfoDetailVM GetMappedDatasAsync(SlidersInfo dbSliderInfo)
        {
            SliderInfoDetailVM model = new()
            {
                SignImage = dbSliderInfo.SignImage,
                Info = dbSliderInfo.Info,
                Title = dbSliderInfo.Title,
                CreateDate = dbSliderInfo.CreatedDate.ToString("dd-MM-yyyy"),
                Status = dbSliderInfo.Status,
            };

            return model;
        }
    }
}

