using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class GuestRecommendation : Recommendation
    {
        public Analyzer analyzer { get; set; }
        private readonly ApplicationDbContext _db;
        private readonly ProductManager _productManager;
        private Product product;

        public GuestRecommendation(ApplicationDbContext db, ProductManager productManager)
        {
            _db = db;
            _productManager = productManager;
        }

        public async Task Setup(Analyzer analyzer, Product? product = null)
        {
            this.analyzer = analyzer;
            if (product != null)
            {
                this.analyzer.UserHistory = new List<Product> { product };
            }
            else
            {
                var randoms = await _productManager.GetRandomProducts(20);
                this.analyzer.UserHistory = randoms.ToList();
            }
        }

        public IEnumerable<Product> RecommendProducts()
        {
            return analyzer.GetProducts();
        }
    }
}
