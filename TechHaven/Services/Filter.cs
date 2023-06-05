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
        //privatni konstruktor da zabranimo kreiranje filtera osim posredstvom buildera
        private Filter() { }

        public Filter(FilterBuilder builder)
        {
            minPrice = builder.minPrice;
            maxPrice = builder.maxPrice;
            categories = builder.categories;
            manufacturers = builder.manufacturers;
            sortStrategy = builder.sortStrategy;
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
