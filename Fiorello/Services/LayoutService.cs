using System;
using System.Text.Json;
using Fiorello.Data;
using Fiorello.Services.Interfaces;
using Fiorello.ViewModels;

namespace Fiorello.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IBasketService _basketService;

        public LayoutService(AppDbContext context, IHttpContextAccessor accessor, IBasketService basketService)
        {
            _context = context;
            _accessor = accessor;
            _basketService = basketService;
        }

        public LayoutVM GetAllDatas()
        {
            //int count = GetBasketCount();
            int count = _basketService.GetCount();
            Dictionary<string, string> datas = _context.settings.AsEnumerable().ToDictionary(m=>m.Key, m=> m.Value);
            return new LayoutVM { BasketCount = count, SettingData = datas };
        }
    }
}

