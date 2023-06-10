using System;
using Fiorello.Models;

namespace Fiorello.Services.Interfaces
{
	public interface IDiscountService
	{
        Task<List<Discount>> GetAll();
    }
}

