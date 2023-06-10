using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fiorello.Areas.Admin.ViewModels.Slider;
using Fiorello.Data;
using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISliderService _sliderService;

        public SliderController(AppDbContext context, IWebHostEnvironment env, ISliderService sliderService)
        {
            _context = context;
            _env = env;
            _sliderService = sliderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _sliderService.GetAllMappedDatasAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Slider dbSlider = await _sliderService.GetByIdAsync(id);

            if (dbSlider is null) return NotFound();

            return View(_sliderService.GetMappedDatasAsync(dbSlider));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileType("image"))
                {
                    ModelState.AddModelError("Image", "Please select only image file");
                    return View();
                }

                if (item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Image", "Image size maximum 20KB");
                    return View();
                }
            }

            foreach (var item in request.Images)
            {
                string filename = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(filename, _env.WebRootPath, "/img/");

                Slider slider = new()
                {
                    Image = filename
                };

                await _context.sliders.AddAsync(slider);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sliderService.DeleteAsync(id);


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            Slider dbSlider = await _sliderService.GetByIdAsync(id);
            if (dbSlider is null) return NotFound();

            return View(new SliderEditVM { Image = dbSlider.Image });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderEditVM request)
        {
            if (id is null) return BadRequest();
            Slider dbSlider = await _sliderService.GetByIdAsync(id);
            if (dbSlider is null) return NotFound();

            if (request.NewImage is null) return RedirectToAction(nameof(Index));

            if (!request.NewImage.CheckFileType("image"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file");
                request.Image = dbSlider.Image;
                return View(request);
            }

            if (request.NewImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Image size maximum 200KB");
                request.Image = dbSlider.Image;
                return View(request);
            }

            string oldPath = Path.Combine(_env.WebRootPath + "/img/" + dbSlider.Image);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + request.NewImage.FileName;

            await request.NewImage.SaveFileAsync(fileName, _env.WebRootPath, "/img/");

            dbSlider.Image = fileName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id is null) return BadRequest();
            Slider dbSlider = await _sliderService.GetByIdAsync(id);
            if (dbSlider is null) return NotFound();
            
            return Ok(await _sliderService.ChangeStatusAsync(dbSlider)); 
        }
    }
}

