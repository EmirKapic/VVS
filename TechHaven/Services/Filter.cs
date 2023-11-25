using TechHaven.Models;

namespace TechHaven.Services
{
    public class Filter
    {
        private int? minPrice = null;
        private int? maxPrice = null;
        private IEnumerable<string>? manufacturers = null;
        public IEnumerable<string>? categories = null;
        private ISortStrategy? sortStrategy = null;

        public Filter(int minPrice, int maxPrice, IEnumerable<string> manufacturers, IEnumerable<string> categories, ISortStrategy sortStrategy)
        {
            this.manufacturers = manufacturers;
            this.minPrice = minPrice;
            this.maxPrice = maxPrice;
            this.categories = categories;
            this.sortStrategy = sortStrategy;
        }


        public List<Product> getFilteredProducts(List<Product> products)
        {
            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];

                if (minPrice != null && product.Price < minPrice) {
                    products.Remove(product);
                    i--;
                    continue;
                }
                if (maxPrice != null && product.Price > maxPrice)
                {
                    products.Remove(product);
                    i--;
                    continue;
                }
                if (manufacturers != null)
                {
                    bool found = false;
                    foreach (var man in manufacturers)
                    {
                        if (product.Manufacturer.Equals(man))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        products.Remove(product);
                        i--;
                        continue;
                    }
                }
                if (categories != null)
                {
                    bool found = false;
                    foreach(var cat in categories)
                    {
                        if (product.Category.Equals(cat))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        products.Remove(product);
                        i--;
                        continue;
                    }
                }
            }
            if (sortStrategy != null)
            {
                products = sortStrategy.sortProducts(products);
            }
            return products;
        }





    }
}
