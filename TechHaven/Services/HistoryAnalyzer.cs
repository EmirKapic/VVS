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
        private readonly ApplicationDbContext _db;

        public HistoryAnalyzer(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, SignInManager<Customer> signInManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
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
                //Take from database products where category is any of the ones our customer has purchased
                //prvo se trebaju ubaciti neki orderi da bi ovdje mogli nesto odradit
                return new List<Product>();
            }
        }
    }
}
