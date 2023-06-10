using System;
using Fiorello.Areas.Admin.ViewModels.Slider;
using Fiorello.Models;
using Fiorello.Data;
using Fiorello.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Fiorello.Helpers;

namespace Fiorello.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<SliderVM>> GetAllMappedDatasAsync()
        {
            List<SliderVM> sliderList = new();

            IEnumerable<Slider> sliders = await GetAllAsync();

            foreach (Slider slider in sliders)
            {
                SliderVM model = new()
                {
                    Id = slider.Id,
                    Image = slider.Image,
                    Status = slider.Status,
                    CreateDate = slider.CreatedDate.ToString("dd-MM-yyyy")
                };

                sliderList.Add(model);
            }

            return sliderList;
        }

        public async Task<Slider> GetByIdAsync(int? id)
        {
            return await _context.sliders.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Slider>> GetAllAsync()
        {
            return await _context.sliders.ToListAsync();
        }

        public SliderDetailVM GetMappedDatasAsync(Slider dbSlider)
        {
            SliderDetailVM model = new()
            {
                Image = dbSlider.Image,
                CreateDate = dbSlider.CreatedDate.ToString("dd-MM-yyyy"),
                Status = dbSlider.Status,
            };

            return model;
        }

        //public async Task CreateAsync(List<IFormFile> images)
        //{
        //    foreach (var item in images)
        //    {
        //        string filename = Guid.NewGuid().ToString() + "_" + item.FileName;

        //        await item.SaveFileAsync(filename, _env.WebRootPath, "/img/");

        //        Slider slider = new()
        //        {
        //            Image = filename
        //        };

        //        await _context.sliders.AddAsync(slider);
        //    }
        //    await _context.SaveChangesAsync();
        //}

        public async Task DeleteAsync(int id)
        {
            Slider dbSlider = await _context.sliders.FirstOrDefaultAsync(m => m.Id == id);

            _context.sliders.Remove(dbSlider);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath + "/img/" + dbSlider.Image);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public Task<List<Slider>> GetAllByStatusAsync()
        {
            return _context.sliders.Where(m => m.Status).ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.sliders.Where(m => m.Status).CountAsync();
        }

        public async Task<bool> ChangeStatusAsync(Slider slider)
        {
            if (slider.Status && await GetCountAsync() != 1)
            {
                slider.Status = false;
            }
            else
            {
                slider.Status = true;
            }

            await _context.SaveChangesAsync();

            return slider.Status;
        }

        //public async Task EditAsync(Slider slider, IFormFile newImage)
        //{
        //    string oldPath = Path.Combine(_env.WebRootPath + "/img/" + slider.Image);

        //    if (System.IO.File.Exists(oldPath))
        //    {
        //        System.IO.File.Delete(oldPath);
        //    }

        //    string fileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;

        //    await newImage.SaveFileAsync(fileName, _env.WebRootPath, "/img/");

        //    slider.Image = fileName;

        //    await _context.SaveChangesAsync();
        //}
    }
}

