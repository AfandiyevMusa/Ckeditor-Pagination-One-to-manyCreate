using System;
using Fiorello.Areas.Admin.ViewModels.Product;
using Fiorello.Data;
using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;

        public ProductService(AppDbContext context, IHttpContextAccessor accessor, IWebHostEnvironment env)
        {
            _context = context;
            _accessor = accessor;
            _env = env;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.products.Include(m => m.Images).Take(8).Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<List<Product>> GetAllWithIncludesAsync()
        {
            return await _context.products.Include(m => m.Images).Include(m => m.Category).Include(m => m.Discount).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int? id)
        {
            return await _context.products.FindAsync(id);
        }

        public async Task<Product> GetByIdWithImagesAsync(int? id)
        {
            return await _context.products.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == id);
        }

        public List<ProductVM> GetMappedDatas(List<Product> products)
        {
            List<ProductVM> list = new();
            foreach (var product in products)
            {
                list.Add(new ProductVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Image = product.Images.FirstOrDefault().Images,
                    CategoryName = product.Category.Name,
                    Discount = product.Discount.Name
                });
            }

            return list;
        }

        public ProductDetailVM GetMappedData(Product product)
        {
            return new ProductDetailVM
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.ToString("0.####"),
                Discount = product.Discount.Name,
                CategoryName = product.Category.Name,
                CreateDate = product.CreatedDate.ToString("MM/dd/yyyy"),
                Images = product.Images.Select(m => m.Images) 
            };
        }

        public async Task<Product> GetWithIncludesAsync(int id)
        {
            return await _context.products.Where(m => m.Id == id).Include(m => m.Images).Include(m => m.Category).Include(m => m.Discount).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetPaginatedDatasAsync(int page, int take)
        {
            return await _context.products.Include(m => m.Images)
                                        .Include(m => m.Category)
                                        .Include(m => m.Discount)
                                        .Skip((page - 1) * take)
                                        .Take(take)
                                        .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.products.CountAsync();
        }

        public async Task CreateAsync(ProductCreateVM model)
        {
            List<Image> images = new();

            foreach (var item in model.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                await item.SaveFileAsync(fileName, _env.WebRootPath, "/img/");
                images.Add(new Image { Images = fileName });
            }

            images.FirstOrDefault().IsMain = true;

            Product product = new()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                DiscountId = model.DiscountId,
                Images = images
            };

            await _context.products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Product dbProduct = await _context.products.FirstOrDefaultAsync(m => m.Id == id);

            _context.products.Remove(dbProduct);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath + "/img/" + dbProduct.Images);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public async Task EditAsync(ProductEditVM model, List<IFormFile> newImage)
        {
            List<Image> images = new();

            foreach (var item in newImage)
            {
                string oldPath = Path.Combine(_env.WebRootPath, "img", item.FileName);

                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }

                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(fileName, _env.WebRootPath, "/img/");

                images.Add(new Image { Images = fileName });
            }

            Product product = new()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Images = images
            };


            _context.Update(product);

            await _context.SaveChangesAsync();
        }
    }
}

