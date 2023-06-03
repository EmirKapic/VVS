using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class ProductManager : IProductManager
    {
        private readonly ApplicationDbContext _db;

        public ProductManager(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<IEnumerable<Product>> GetAllByCategory(string category)
        {
            if (category == null) { throw new NullReferenceException("Passed category was null!"); }

            var products = await _db.Product.Where(p => p.Category == category).ToListAsync();
            return products;
        }

        public async Task<IEnumerable<string>> GetAllCategories()
        {
            var categories = await _db.Product.Select(p => p.Category).Distinct().ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<string>> GetAllManufacturersForCategory(string category)
        {
            if (category == null) { throw new NullReferenceException("Passed category was null!"); }
            return await _db.Product.Where(p => p.Category == category).Select(p => p.Manufacturer).Distinct().ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetRandomProducts(int limit)
        {
            if (limit <= 0) { throw new ArgumentOutOfRangeException("Limit must be greater than 0"); }
            var products = await _db.Product.ToListAsync();

            return new Randomizer(products).GetRandomized().Take(limit);
        }
    }
}
