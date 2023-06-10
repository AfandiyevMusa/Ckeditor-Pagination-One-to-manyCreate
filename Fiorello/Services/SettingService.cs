using System;
using Fiorello.Data;
using Fiorello.Services.Interfaces;

namespace Fiorello.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;

        public SettingService(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, string> GetAll()
        {
            return _context.settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
        }
    }
}

