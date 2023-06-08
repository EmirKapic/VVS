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

        public IEnumerable<string> GetAllCategoriesFromProducts(IEnumerable<Product> prods)
        {
            var uniqueCategories = new HashSet<string>();
            foreach(var product in prods)
            {
                uniqueCategories.Add(product.Category);
            }
            return uniqueCategories.ToList();
        }

        public IEnumerable<string> GetAllManufacturersFromProducts(IEnumerable<Product> prods)
        {
            var uniqueManufacturers = new HashSet<string>();
            foreach(var product in prods)
            {
                uniqueManufacturers.Add(product.Manufacturer);
            }
            return uniqueManufacturers.ToList();
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
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _db.Product.ToListAsync();
        }


        public async Task<IEnumerable<Product>> GetProductsFromIds(List<int> ids)
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                products.Add(await _db.Product.FirstAsync(p => p.Id == id));
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsContainingString(string query)
        {
            return await _db.Product.Where(p => (p.Category + p.Manufacturer + p.Model).ToUpper().Contains(query.ToUpper())).ToListAsync();
        }
	}
}
