using System;
using System.Collections.Generic;
using System.Linq;
using Fiorello.Data;
using System.Threading.Tasks;
using Fiorello.Models;
using Fiorello.Services;
using Fiorello.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiorello.Areas.Admin.ViewModels.SliderInfo;
using Fiorello.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderInfoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISliderInfoService _sliderInfoService;

        public SliderInfoController(AppDbContext context, IWebHostEnvironment env, ISliderInfoService sliderInfoService)
        {
            _context = context;
            _env = env;
            _sliderInfoService = sliderInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _sliderInfoService.GetAllMappedDatasAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            SlidersInfo dbSliderInfo = await _sliderInfoService.GetByIdAsync(id);

            if (dbSliderInfo is null) return NotFound();

            return View(_sliderInfoService.GetMappedDatasAsync(dbSliderInfo));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderInfoCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!request.SignImage.CheckFileType("image"))
            {
                ModelState.AddModelError("SignImage", "Please select only image file");
                return View();
            }

            if (request.SignImage.CheckFileSize(200))
            {
                ModelState.AddModelError("SignImage", "Image size maximum 20KB");
                return View();
            }

            string filename = Guid.NewGuid().ToString() + "_" + request.SignImage.FileName;

            await request.SignImage.SaveFileAsync(filename, _env.WebRootPath, "/img/");

            SlidersInfo sliderInfo = new()
            {
                SignImage = filename,
                Title = request.Title,
                Info = request.Info
            };

            await _context.slidersInfos.AddAsync(sliderInfo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sliderInfoService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            SlidersInfo dbSliderInfo = await _sliderInfoService.GetByIdAsync(id);
            if (dbSliderInfo is null) return NotFound();

            return View(new SliderInfoEditVM {  SignImage = dbSliderInfo.SignImage,
                                                Info = dbSliderInfo.Info,
                                                Title = dbSliderInfo.Title });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderInfoEditVM request)
        {
            if (id is null) return BadRequest();
            SlidersInfo dbSliderInfo = await _sliderInfoService.GetByIdAsync(id);
            if (dbSliderInfo is null) return NotFound();

            if (request.NewSignImage is null) return RedirectToAction(nameof(Index));

            if (!request.NewSignImage.CheckFileType("image"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file");
                request.SignImage = dbSliderInfo.SignImage;
                return View(request);
            }

            if (request.NewSignImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Image size maximum 200KB");
                request.SignImage = dbSliderInfo.SignImage;
                return View(request);
            }

            string oldPath = Path.Combine(_env.WebRootPath + "/img/" + dbSliderInfo.SignImage);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + request.NewSignImage.FileName;

            await request.NewSignImage.SaveFileAsync(fileName, _env.WebRootPath, "/img/");

            dbSliderInfo.SignImage = fileName;
            dbSliderInfo.Title = request.Title;
            dbSliderInfo.Info = request.Info;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id is null) return BadRequest();
            SlidersInfo dbSliderInfo = await _sliderInfoService.GetByIdAsync(id);
            if (dbSliderInfo is null) return NotFound();

            return Ok(await _sliderInfoService.ChangeStatusAsync(dbSliderInfo));
        }
    }
}

