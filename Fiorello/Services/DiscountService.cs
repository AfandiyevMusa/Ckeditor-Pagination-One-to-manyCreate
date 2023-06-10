using System;
using Fiorello.Data;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Services
{
	public class DiscountService:IDiscountService
	{
        private readonly AppDbContext _context;

        public DiscountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Discount>> GetAll()
        {
            return await _context.discounts.ToListAsync();
        }
    }
}

