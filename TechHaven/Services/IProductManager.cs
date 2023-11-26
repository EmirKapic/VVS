using Microsoft.AspNetCore.Mvc;
using TechHaven.Models;

namespace TechHaven.Services
{
    public interface IProductManager
    {
        public List<Product> GetAllByCategory(string category);
        public List<string> GetAllManufacturersForCategory(string category);
        public List<string> GetAllCategories();
        public List<Product> GetRandomProducts(int limit);
        public List<Product> GetAllProducts();
        public List<Product> GetProductsFromIds(List<int> ids);

        public List<Product> GetProductsContainingString(string query);

    }
}
