using TechHaven.Models;

namespace TechHaven.Services
{
    public class LowestPriceStrategy : ISortStrategy
    {
        public List<Product> sortProducts(List<Product> products)
        {
            return products.OrderBy(p => p.Price).ToList();
        }
    }
}
