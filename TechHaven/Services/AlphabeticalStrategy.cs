using TechHaven.Models;

namespace TechHaven.Services
{
    public class AlphabeticalStrategy : ISortStrategy
    {
        public List<Product> sortProducts(List<Product> products)
        {
            return products.OrderBy(p => (p.Manufacturer + p.Model)).ToList();
        }
    }
}
