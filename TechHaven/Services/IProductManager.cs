using Microsoft.AspNetCore.Mvc;
using TechHaven.Models;

namespace TechHaven.Services
{
    public interface IProductManager
    {
        public Task<IEnumerable<Product>> GetAllByCategory(string category);
        public Task<IEnumerable<string>> GetAllManufacturersForCategory(string category);
        public Task<IEnumerable<string>> GetAllCategories();
        public Task<IEnumerable<Product>> GetRandomProducts(int limit);
        public Task<IEnumerable<Product>> GetAllProducts();

    }
}
