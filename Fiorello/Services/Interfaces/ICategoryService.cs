using System;
using Fiorello.Models;

namespace Fiorello.Services.Interfaces
{
	public interface ICategoryService
	{
        Task<List<Category>> GetAll();
    }
}

