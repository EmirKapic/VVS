using System;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class Randomizer
    {
        private readonly Random _random;
        private List<Product> products;
        public Randomizer(List<Product> products)
        {
            _random = new Random();
            this.products = products;
        }

        public List<Product> GetRandomized()
        {
            return products.OrderBy(a => _random.Next()).ToList();
        }
    }
}
