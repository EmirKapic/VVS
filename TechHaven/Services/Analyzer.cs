using TechHaven.Models;

namespace TechHaven.Services
{
    public interface Analyzer
    {
        public Task<IEnumerable<Product>> GetProducts();
    }
}
