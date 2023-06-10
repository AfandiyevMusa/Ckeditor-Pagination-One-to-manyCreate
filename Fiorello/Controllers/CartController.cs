using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Fiorello.Data;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public CartController(IHttpContextAccessor accessor,
                              IProductService productService,
                              IBasketService basketService)
        {
            _accessor = accessor;
            _productService = productService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            List<BasketDetailVM> basketList = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                List<BasketVM> basketDatas = JsonSerializer.Deserialize<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
                foreach (var item in basketDatas)
                {
                    var dbProduct = await _productService.GetByIdWithImagesAsync(item.Id);

                    if (dbProduct != null)
                    {
                        BasketDetailVM basketDetail = new()
                        {
                            Id = dbProduct.Id,
                            Name = dbProduct.Name,
                            Image = dbProduct.Images.Where(m => m.IsMain).FirstOrDefault().Images,
                            Price = dbProduct.Price,
                            Count = item.Count,
                            TotalPrice = dbProduct.Price * item.Count
                        };
                        basketList.Add(basketDetail);
                    }

                }
            }
            return View(basketList);
        }

        [HttpPost]
        public async Task<IActionResult> AddBasket(int? id)
        {
            Product product = await _productService.GetByIdAsync(id);

            List<BasketVM> basket = GetBasketDatas();

            AddProductToBasket(basket, product);

            return Ok(basket.Sum(m => m.Count));
        }

        private List<BasketVM> GetBasketDatas()
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

            return basket;
        }

        private void AddProductToBasket(List<BasketVM> basket, Product product)
        {
            BasketVM existProduct = basket.FirstOrDefault(m => m.Id == product.Id);
            if (existProduct is null)
            {
                basket.Add(new BasketVM
                {
                    Id = product.Id,
                    Count = 1
                });
            }
            else
            {
                existProduct.Count++;
            }

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(basket));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteProductFromBasket(int? id)
        {
            //_basketService.DeleteProduct(id);
            //int count = _basketService.GetCount();
            return Ok(await _basketService.DeleteProduct(id));
            //return Ok(_basketService.DeleteProduct(id));
        }

    }
}

