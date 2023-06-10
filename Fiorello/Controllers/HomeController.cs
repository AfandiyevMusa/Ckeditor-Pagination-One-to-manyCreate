using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Fiorello.Data;
using Fiorello.ViewModels;
using Fiorello.Models;
using Fiorello.Services;
using Fiorello.Services.Interfaces;
using System.Text.Json;

namespace Fiorello.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _accessor;
    private readonly IBasketService _basketService;
    private readonly IProductService _productService;

    public HomeController(AppDbContext context, IHttpContextAccessor accessor, IBasketService basketService, IProductService productService)
    {
        _context = context;
        _accessor = accessor;
        _basketService = basketService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        List<Slider> sliders = await _context.sliders.Where(m => !m.SoftDelete).ToListAsync();
        SlidersInfo slidersInfo = await _context.slidersInfos.Where(m => !m.SoftDelete).FirstOrDefaultAsync();
        List<Blog> blogs = await _context.blogs.Where(m=>!m.SoftDelete).ToListAsync();
        List<Experts> experts = await _context.experts.Where(m => !m.SoftDelete).ToListAsync();
        Valentine valentine = await _context.valentines.Where(m => !m.SoftDelete).FirstOrDefaultAsync();
        List<About> abouts = await _context.abouts.Where(m => !m.SoftDelete).ToListAsync();
        List<Instagram> instagrams = await _context.instagrams.Where(m => !m.SoftDelete).ToListAsync();
        IEnumerable<Category> categories = await _context.categories.Where(m => !m.SoftDelete).ToListAsync();
        IEnumerable<Product> products = await _productService.GetAllAsync();

        HomeVM home = new HomeVM()
        {
            sliders = sliders,
            slidersInfos = slidersInfo,
            blogs = blogs,
            experts = experts,
            valentine = valentine,
            abouts = abouts,
            instagrams = instagrams,
            categories = categories,
            products = products
        };
        return View(home);
    }
}