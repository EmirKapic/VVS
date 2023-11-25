using Microsoft.EntityFrameworkCore;
using System.Collections;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class CustomerRecommendation
    {
        public IEnumerable<Product> _products { get; set; }
        public string favoriteCategory { get; set; }
        public CustomerRecommendation(string favoriteCategory, IEnumerable<Product> products)
        {
            this.favoriteCategory = favoriteCategory;
            this._products = products;
        }

        public IEnumerable<Product> RecommendProducts()
        {
            List<Product> result = new List<Product>();
            //neka logika da izabere onaj product iz products sa najvise
            foreach (var product in _products)
            {
                if (product.Category.Equals(favoriteCategory))
                {
                    result.Add(product);
                }
            }
            return result;

        }


    }
}
