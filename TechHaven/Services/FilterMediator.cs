using TechHaven.Models;

namespace TechHaven.Services
{
    public class FilterMediator : IFilterMediator
    {
        public IEnumerable<Product> GetFilteredProducts()
        {
            throw new NotImplementedException();
        }

        public void SetConditions(IEnumerable<Product> products,int minPrice, int maxPrice, IEnumerable<string> categories, IEnumerable<string> manufacturers, SortType sortType)
        {
            throw new NotImplementedException();
        }
    }
}
