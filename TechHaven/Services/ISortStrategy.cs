using TechHaven.Models;

namespace TechHaven.Services
{
    public interface ISortStrategy
    {
        public List<Product> sortProducts(List<Product> products);
    }
}
