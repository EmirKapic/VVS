using TechHaven.Models;

namespace TechHaven.Services
{
    public class FilterMediator : IFilterMediator
    {
        private IFilterBuilder _filterBuilder;
        private List<Product> products;

        public FilterMediator(FilterBuilder filterBuilder)
        {
            _filterBuilder = filterBuilder;
        }

        public IEnumerable<Product> GetFilteredProducts()
        {
            return _filterBuilder.Build().getFilteredProducts(products);
        }

        public void SetConditions(IEnumerable<Product> products,int minPrice, int maxPrice, IEnumerable<string> categories, IEnumerable<string> manufacturers, SortType sortType)
        {
            if (minPrice != 0)
            {
                _filterBuilder.AddMinPrice(minPrice);
            }
            if (maxPrice != 0)
            {
                _filterBuilder.AddMaxPrice(maxPrice);
            }
            if (categories != null && categories.Any())
            {
                _filterBuilder.AddWantedCategories(categories);
            }
            if (manufacturers != null && manufacturers.Any())
            {
                _filterBuilder.AddWantedManufacturers(manufacturers);
            }
            _filterBuilder.AddSortStrategy(sortType);
            this.products = products.ToList();
        }
    }
}
