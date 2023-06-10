using System;
using Fiorello.Models;
using Fiorello.Responses;
using Fiorello.ViewModels;

namespace Fiorello.Services.Interfaces
{
	public interface IBasketService
	{
        //Task<List<BasketVM>> GetAll();
        void AddProduct(List<BasketVM> basket, Product product);
        Task<BasketDeleteResponse> DeleteProduct(int? id);
        int GetCount();
        Task<IEnumerable<Product>> GetAllAsync();
    }
}

