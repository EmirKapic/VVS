using TechHaven.Models;

namespace TechHaven.Services
{
    public interface Recommendation
    {
        public Analyzer analyzer { get; set; }

        public IEnumerable<Product> RecommendProducts();

        public Task Setup(Analyzer analyzer, Product? product = null);
    }
}
