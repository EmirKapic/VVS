using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class SingleProductAnalyzer : Analyzer
    {
        public ICollection<Product> products { get; set; }

        public ICollection<Product> UserHistory { get; set; }


        public SingleProductAnalyzer(ICollection<Product> products)
        {
            this.products = products;
        }

        public IEnumerable<Product> GetProducts()
        {
            if (UserHistory == null || UserHistory.First() == null) { return Enumerable.Empty<Product>(); }
            var result = new List<Product>();
            foreach (var product in products)
            {
                if (product.Category == UserHistory.First().Category)
                {
                    result.Add(product);
                }
            }
            return result;
        }
    }
}
