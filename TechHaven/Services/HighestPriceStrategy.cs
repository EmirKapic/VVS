using TechHaven.Models;

namespace TechHaven.Services
{
    public class HighestPriceStrategy : ISortStrategy
    {
        public List<Product> sortProducts(List<Product> products)
        {
            return products.OrderByDescending(p => p.Price).ToList();
        }
    }
}
