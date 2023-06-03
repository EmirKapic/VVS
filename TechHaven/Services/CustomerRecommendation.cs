using TechHaven.Models;

namespace TechHaven.Services
{
    public class CustomerRecommendation : Recommendation
    {     
        public Analyzer analyzer { get; set; }
        private Product product;

        public CustomerRecommendation(Analyzer analyzer, Product? product = null) {
            this.analyzer = analyzer;
            this.product = product;
        }
        public async Task<IEnumerable<Product>> RecommendProducts()
        {
            if (analyzer is SingleProductAnalyzer)
            {
                (analyzer as SingleProductAnalyzer).product = product;
                return await(analyzer as SingleProductAnalyzer).GetProducts();
            }
            else
            {
                return await analyzer.GetProducts();
            }
            
        }


    }
}
