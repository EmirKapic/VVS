using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class HistoryAnalyzer : Analyzer
    {
        public ICollection<Product> products { get; set; }
        public ICollection<Product> UserHistory { get; set; }

        public HistoryAnalyzer(ICollection<Product> products)
        {
            this.products = products;
        }

        public IEnumerable<Product> GetProducts()
        {
            if (UserHistory == null || UserHistory.Count == 0)
            {
                return new Randomizer(products.ToList()).GetRandomized().Take(20);
            }


            return UserHistory;
        }
    }
}
