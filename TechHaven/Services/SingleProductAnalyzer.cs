using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class SingleProductAnalyzer : Analyzer
    {
        public Product? product;
        private readonly ApplicationDbContext _db; 

        public SingleProductAnalyzer(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            if (product == null) { return Enumerable.Empty<Product>(); }
            var sameCategoryProducts = await _db.Product.Where(p => p.Category == product.Category).ToListAsync();
            return new Randomizer(sameCategoryProducts).GetRandomized();
        }
    }
}
