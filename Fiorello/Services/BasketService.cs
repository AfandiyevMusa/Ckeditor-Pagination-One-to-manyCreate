using System;
using System.Text.Json;
using Fiorello.Data;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Fiorello.ViewModels;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Fiorello.Responses;

namespace Fiorello.Services
{
	public class BasketService : IBasketService
	{
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IProductService _productService;

        public BasketService(AppDbContext context, IHttpContextAccessor accessor, IProductService productService)
        {
            _context = context;
            _accessor = accessor;
            _productService = productService;
        }

        public void AddProduct(List<BasketVM> basket, Product product)
        {
            
        }

        public async Task<BasketDeleteResponse> DeleteProduct(int? id)
        {
            List<BasketVM> basketDatas = JsonSerializer.Deserialize<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);

            var data = basketDatas.FirstOrDefault(m => m.Id == id);

            basketDatas.Remove(data);

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(basketDatas));

            decimal total = 0;

            foreach (var basketData in basketDatas)
            {
                Product dbProduct = await _productService.GetByIdAsync(basketData.Id);
                total += (dbProduct.Price * basketData.Count);
            }

            int count = basketDatas.Sum(m => m.Count);

            return new BasketDeleteResponse { Count = count, Total = total };
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.products.Include(m => m.Images).ToListAsync();
        }

        public int GetCount()
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

