using TechHaven.Models;

namespace TechHaven.Services
{
    public class GuestRecommendation : Recommendation
    {
        public Analyzer analyzer { get; set; }
        private Product product;

        public GuestRecommendation(Analyzer _analyzer, Product? product = null)
        {
            analyzer = _analyzer;
            this.product = product;
        }

        public async Task<IEnumerable<Product>> RecommendProducts()
        {
            if (analyzer is SingleProductAnalyzer)
            {
                (analyzer as SingleProductAnalyzer).product = product;
                return await (analyzer as SingleProductAnalyzer).GetProducts();
            }
            else
            {
                return await analyzer.GetProducts();
            }
        }
    }
}
