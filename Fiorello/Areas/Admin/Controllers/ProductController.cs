using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fiorello.Models;
using Fiorello.Helpers;
using Fiorello.Areas.Admin.ViewModels.Product;
using Fiorello.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Fiorello.Services;
using Fiorello.Data;
using Fiorello.Areas.Admin.ViewModels.Slider;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISettingService _settingService;
        private readonly IDiscountService _discountService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public ProductController(IProductService productService,
                                 ISettingService settingService,
                                 IDiscountService discountService,
                                 ICategoryService categoryService,
                                 IWebHostEnvironment env,
                                 AppDbContext context)
        {
            _productService = productService;
            _settingService = settingService;
            _discountService = discountService;
            _categoryService = categoryService;
            _env = env;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var settingData = _settingService.GetAll();
            int take = int.Parse(settingData["AdminProductPaginateTake"]);
            var paginatedDatas = await _productService.GetPaginatedDatasAsync(page, take);
            var pageCount = await GetCountAsync(take);

            ViewBag.count = pageCount;

            List<ProductVM> mappedDatas = _productService.GetMappedDatas(paginatedDatas);

            Paginate<ProductVM> result = new(mappedDatas, page, pageCount);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Product product = await _productService.GetWithIncludesAsync((int)id);
            if (product is null) return NotFound();

            return View(_productService.GetMappedData(product));
        }

        private async Task<int> GetCountAsync(int take)
        {
            int count = await _productService.GetCountAsync();

            var res = (int) Math.Ceiling((decimal)count / take);

            return res;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetCategoriesAndDiscounts();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            await GetCategoriesAndDiscounts();

            if (!ModelState.IsValid)
            {
                return View();
            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "Please select only image file");
                    return View();
                }


                if (item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Image", "Image size must be max 200 KB");
                    return View();
                }
            }

            await _productService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        private async Task GetCategoriesAndDiscounts()
        {
            ViewBag.categories = await GetCategories();
            ViewBag.discounts = await GetDiscounts();
        }

        private async Task<SelectList> GetCategories()
        {
            List<Category> categories = await _categoryService.GetAll();
            return new SelectList(categories, "Id", "Name");
        }

        private async Task<SelectList> GetDiscounts()
        {
            List<Discount> discounts = await _discountService.GetAll();
            return new SelectList(discounts, "Id", "Name");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductEditVM request)
        {
            await GetCategoriesAndDiscounts();

            if (id is null) return BadRequest();

            Product existProduct = await _context.products.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (existProduct is null) return NotFound();

            if (existProduct.Name.Trim() == request.Name)
            {
                return RedirectToAction(nameof(Index));
            }
            if (existProduct.Description.Trim() == request.Description)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var item in request.newImages)
            {
                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Please select only image file");
                    request.newImages = (List<IFormFile>)existProduct.Images;
                    return View(request);
                }

                if (item.CheckFileSize(200))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 200KB");
                    request.newImages = (List<IFormFile>)existProduct.Images;
                    return View(request);
                }
            }         

            await _productService.EditAsync(request,request.newImages);

            return RedirectToAction(nameof(Index));
        }
    }
}

