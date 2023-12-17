using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class FilterTest
    {
        private readonly List<Product> productsOriginal = new ()
            {
                new() { Price = 10, Manufacturer = "Volvo", Category = "Sedan" },
                new() { Price = 100, Manufacturer = "Mercedes", Category = "Hatchback" },
                new() { Price = 9, Manufacturer = "Volvo", Category = "Sedan" },
                new() { Price = 101, Manufacturer = "Volvo", Category = "Hatchback" },
                new() { Price = 10, Manufacturer = "Volkswagen", Category = "Sedan" },
                new() { Price = 10, Manufacturer = "Volvo", Category = "Coupe" }
            };

        private bool IsFiltered(int min, int max, List<String> manufacturers, List<String> categories, ISortStrategy sortStrategy)
        {
            var productsExpected = (
                    from product in productsOriginal
                    where (product.Price >= min && product.Price <= max)
                    where manufacturers.Contains(product.Manufacturer)
                    where categories.Contains(product.Category)
                    select product
                ).ToList();

            productsExpected = sortStrategy.sortProducts(productsExpected);

            var filter = new Filter(min, max, manufacturers, categories, sortStrategy);

            return Enumerable.SequenceEqual(productsExpected, filter.getFilteredProducts(productsOriginal));
        }

        [TestMethod]
        public void MinMaxFilterTest()
        {
            const int min = 10;
            const int max = 100;
            var manufacturers = new List<string> {};
            var categories = new List<string> {};
            var sortStrategy = new AlphabeticalStrategy();

            Assert.IsTrue(IsFiltered(min, max, manufacturers, categories, sortStrategy));
        }

        [TestMethod]
        public void ManufacturerFilterTest()
        {
            const int min = 10;
            const int max = 100;
            var manufacturers = new List<string> { "Mercedes", "Volvo" };
            var categories = new List<string> {};
            var sortStrategy = new AlphabeticalStrategy();

            Assert.IsTrue(IsFiltered(min, max, manufacturers, categories, sortStrategy));
        }

        [TestMethod]
        public void CategoryFilterTest()
        {
            const int min = 10;
            const int max = 100;
            var manufacturers = new List<string> {};
            var categories = new List<string> { "Sedan", "Hatchback" };
            var sortStrategy = new AlphabeticalStrategy();

            Assert.IsTrue(IsFiltered(min, max, manufacturers, categories, sortStrategy));
        }

        [TestMethod]
        public void HighestPriceFilterTest()
        {
            const int min = 10;
            const int max = 100;
            var manufacturers = new List<string> {};
            var categories = new List<string> {};
            var sortStrategy = new HighestPriceStrategy();

            Assert.IsTrue(IsFiltered(min, max, manufacturers, categories, sortStrategy));
        }

        [TestMethod]
        public void LowestPriceFilterTest()
        {
            const int min = 10;
            const int max = 100;
            var manufacturers = new List<string> { };
            var categories = new List<string> { };
            var sortStrategy = new LowestPriceStrategy();

            Assert.IsTrue(IsFiltered(min, max, manufacturers, categories, sortStrategy));
        }
    }
}
