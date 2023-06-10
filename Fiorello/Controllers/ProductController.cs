using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Fiorello.Data;
using Fiorello.Models;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public ProductController(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<IActionResult> Index(int? id)
        {
            //if (id is null) return BadRequest();
            //Product product = await _context.products.Include(m=>m.Images).Where(m => !m.SoftDelete).FirstOrDefaultAsync(n =>n.Id == id);
            Product product = await _context.products.Include(m=>m.Images).FirstOrDefaultAsync(n =>n.Id == id);
            //if (product is null) return NotFound();

            //ProductDetailVM model = new ProductDetailVM()
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    CategoryName = product.Category.Name,
            //    ActualPrice = product.Price,
            //    DiscountedPrice = product.Price - (product.Price * product.Discount.Percent / 100),
            //    Percent = product.Discount.Percent,
            //    Description = product.Description,
            //    Images = product.Images.ToList()
            //};
            return View(product);
        }

        private int GetBasketCount()
        {
            List<BasketVM> basket;
            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basket = JsonSerializer.Deserialize<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }

            return basket.Sum(m => m.Count);
        }
    }
}