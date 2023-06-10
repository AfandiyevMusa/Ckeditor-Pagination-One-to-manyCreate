using System;
using Fiorello.Areas.Admin.ViewModels.Product;
using Fiorello.Models;

namespace Fiorello.Services.Interfaces
{
	public interface IProductService
	{
        Task<IEnumerable<Product>> GetAllAsync();
        Task<List<Product>> GetAllWithIncludesAsync();
        Task<Product> GetByIdAsync(int? id);
        Task<Product> GetByIdWithImagesAsync(int? id);
        List<ProductVM> GetMappedDatas(List<Product> products);
        Task<Product> GetWithIncludesAsync(int id);
        ProductDetailVM GetMappedData(Product product);
        Task<List<Product>> GetPaginatedDatasAsync(int page, int take);
        Task<int> GetCountAsync();
        Task CreateAsync(ProductCreateVM model);
        Task DeleteAsync(int id);
    }
}

