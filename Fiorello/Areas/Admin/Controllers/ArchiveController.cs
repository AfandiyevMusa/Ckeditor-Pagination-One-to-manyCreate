using System;
using System.Collections.Generic;
using System.Linq;
using Fiorello.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fiorello.Areas.Admin.ViewModels.Category;
using Fiorello.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArchiveController : Controller
    {
        private readonly AppDbContext _context;

        public ArchiveController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Categories()
        {
            List<CategoryVM> list = new();

            List<Category> datas = await _context.categories.Where(m => m.SoftDelete == true).ToListAsync();

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

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ExtractCategory(int id)
        {
            var existCategory = await _context.categories.FirstOrDefaultAsync(m => m.Id == id);

            existCategory.SoftDelete = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }
    }
}

