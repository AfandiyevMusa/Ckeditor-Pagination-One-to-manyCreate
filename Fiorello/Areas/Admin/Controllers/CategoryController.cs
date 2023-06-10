using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fiorello.Data;
using Fiorello.Models;
using Fiorello.Areas.Admin.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<CategoryVM> list = new();

            List <Category> datas = await _context.categories.Where(m => !m.SoftDelete).OrderByDescending(m=>m.Id).ToListAsync();

            foreach (var item in datas)
            {
                list.Add(new CategoryVM
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Category newCategory = new()
            {
                Name = request.Name
            };


            await _context.categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var existCategory = await _context.categories.FirstOrDefaultAsync(m => m.Id == id);
            if (existCategory is null) return NotFound();

            CategoryEditVM model = new()
            {
                Id = existCategory.Id, 
                Name = existCategory.Name
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (id is null) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var existCategory = await _context.categories.FirstOrDefaultAsync(m => m.Id == id);
            if (existCategory is null) return NotFound();

            if(existCategory.Name.Trim() == request.Name.Trim())
            {
                return RedirectToAction(nameof(Index));
            }

            existCategory.Name = request.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            var existCategory = await _context.categories.FirstOrDefaultAsync(m => m.Id == id);

            _context.Remove(existCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SoftDelete(int? id)
        {
            var existCategory = await _context.categories.FirstOrDefaultAsync(m => m.Id == id);

            existCategory.SoftDelete = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

