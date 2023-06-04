using TechHaven.Models;

namespace TechHaven.Services
{
    public interface IFilterMediator
    {
        public IEnumerable<Product> GetFilteredProducts();
        public void SetConditions(IEnumerable<Product> products,int minPrice, int maxPrice, IEnumerable<string> categories,
            IEnumerable<string> manufacturers, SortType sortType);
    }
}
