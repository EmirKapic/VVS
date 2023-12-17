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

        private bool IsFiltered(int? min, int? max, List<String>? manufacturers, List<String>? categories, ISortStrategy? sortStrategy)
        {
            var productsExpected = (
                    from product in productsOriginal
                    where min == null || product.Price >= min
                    where max == null || product.Price <= max
                    where manufacturers == null || manufacturers.Contains(product.Manufacturer)
                    where categories == null || categories.Contains(product.Category)
                    select product
                ).ToList();

            if (sortStrategy != null)
            {
                productsExpected = sortStrategy.sortProducts(productsExpected);
            }

            var filter = new Filter(min, max, manufacturers, categories, sortStrategy);

            return Enumerable.SequenceEqual(productsExpected, filter.getFilteredProducts(productsOriginal));
        }

        [TestMethod]
        public void MinFilterTest()
        {
            int min = 10;

            Assert.IsTrue(IsFiltered(min, null, null, null, null));
        }

        [TestMethod] public void MaxFilterTest()
        {
            int max = 100;

            Assert.IsTrue(IsFiltered(null, max, null, null, null));
        }

        [TestMethod]
        public void ManufacturerFilterTest()
        {
            var manufacturers = new List<string> { "Mercedes", "Volvo" };

            Assert.IsTrue(IsFiltered(null, null, manufacturers, null, null));
        }

        [TestMethod]
        public void CategoryFilterTest()
        {
            var categories = new List<string> { "Sedan", "Hatchback" };

            Assert.IsTrue(IsFiltered(null, null, null, categories, null));
        }

        [TestMethod]
        public void AlphabeticalSortFilterTest()
        {
            var sortStrategy = new AlphabeticalStrategy();

            Assert.IsTrue(IsFiltered(null, null, null, null, sortStrategy));
        }

        [TestMethod]
        public void HighestPriceSortFilterTest()
        {
            var sortStrategy = new HighestPriceStrategy();

            Assert.IsTrue(IsFiltered(null, null, null, null, sortStrategy));
        }

        [TestMethod]
        public void LowestPriceSortFilterTest()
        {
            var sortStrategy = new LowestPriceStrategy();

            Assert.IsTrue(IsFiltered(null, null, null, null, sortStrategy));
        }
    }
}
