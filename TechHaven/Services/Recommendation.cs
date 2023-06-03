using TechHaven.Models;

namespace TechHaven.Services
{
    public interface Recommendation
    {
        public Analyzer analyzer { get; set; }

        public Task<IEnumerable<Product>> RecommendProducts();
    }
}
