using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class CustomerRecommendation : Recommendation
    {
        public Analyzer analyzer { get; set; }
        //private Product product;
        private readonly ApplicationDbContext _db;
        private readonly OrdersManager _ordersManager;

        public CustomerRecommendation(ApplicationDbContext db, OrdersManager ordersManager)
        {
            _db = db;
            _ordersManager = ordersManager;
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
                this.analyzer.UserHistory = await _ordersManager.GetProductsFromOrders();
            }
        }

        public IEnumerable<Product> RecommendProducts()
        {
            return analyzer.GetProducts();

        }


    }
}
