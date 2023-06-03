using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class HistoryAnalyzer : Analyzer
    {
        private readonly SignInManager<Customer> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OrdersManager _ordersManager;
        private readonly ApplicationDbContext _db;

        public HistoryAnalyzer(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, SignInManager<Customer> signInManager, OrdersManager ordersManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _ordersManager = ordersManager;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _db.Product.ToListAsync();
            if (!_signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User))
            {   
                return new Randomizer(products.ToList()).GetRandomized().Take(20);
            }
            else
            {
                return await _ordersManager.GetProductsFromOrders();
			}
        }
    }
}
