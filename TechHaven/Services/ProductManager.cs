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
        public List<Product> GetAllByCategory(string category)
        {
            if (category == null) { throw new NullReferenceException("Passed category was null!"); }

            var products = _db.Product.Where(p => p.Category == category).ToList();
            return products;
        }

        public List<string> GetAllCategories()
        {
            var categories = _db.Product.Select(p => p.Category).Distinct().ToList();
            return categories;
        }

        public List<string> GetAllCategoriesFromProducts(List<Product> prods)
        {
            var uniqueCategories = new HashSet<string>();
            foreach(var product in prods)
            {
                uniqueCategories.Add(product.Category);
            }
            return uniqueCategories.ToList();
        }

        public List<string> GetAllManufacturersFromProducts(List<Product> prods)
        {
            var uniqueManufacturers = new HashSet<string>();
            foreach(var product in prods)
            {
                uniqueManufacturers.Add(product.Manufacturer);
            }
            return uniqueManufacturers.ToList();
        }

        public List<string> GetAllManufacturersForCategory(string category)
        {
            if (category == null) { throw new NullReferenceException("Passed category was null!"); }
            return _db.Product.Where(p => p.Category == category).Select(p => p.Manufacturer).Distinct().ToList();
        }
        public List<Product> GetRandomProducts(int limit)
        {
            if (limit <= 0) { throw new ArgumentOutOfRangeException("Limit must be greater than 0"); }
            var products = _db.Product.ToList();

            return new Randomizer(products).GetRandomized().Take(limit).ToList();
        }
        public List<Product> GetAllProducts()
        {
            return _db.Product.ToList();
        }


        public List<Product> GetProductsFromIds(List<int> ids)
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                products.Add(_db.Product.First(p => p.Id == id));
            }
            return products;
        }

        public List<Product> GetProductsContainingString(string query)
        {
            return _db.Product.Where(p => (p.Category + p.Manufacturer + p.Model).ToUpper().Contains(query.ToUpper())).ToList();
        }
	}
}
