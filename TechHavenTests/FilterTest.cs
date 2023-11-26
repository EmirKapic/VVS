using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class FilterTest
    {
        [TestMethod]
        public void GetFilteredProductsTest()
        {
            const int min = 10;
            const int max = 100;
            var manufacturers = new List<string> { "Mercedes", "Volvo" };
            var categories = new List<string> { "Sedan", "Hatchback" };
            var sortStrategy = new AlphabeticalStrategy();

            var filter = new Filter(min, max, manufacturers, categories, sortStrategy);

            var productsOriginal = new List<Product>
            {
                new() { Price = 10, Manufacturer = "Volvo", Category = "Sedan" },
                new() { Price = 100, Manufacturer = "Mercedes", Category = "Hatchback" },
                new() { Price = 9, Manufacturer = "Volvo", Category = "Sedan" },
                new() { Price = 101, Manufacturer = "Volvo", Category = "Hatchback" },
                new() { Price = 10, Manufacturer = "Volkswagen", Category = "Sedan" },
                new() { Price = 10, Manufacturer = "Volvo", Category = "Coupe" }
            };

            var productsExpected = (
                    from product in productsOriginal
                    where product.Price is (>= min and <= max)
                    where manufacturers.Contains(product.Manufacturer)
                    where categories.Contains(product.Category)
                    select product
                ).OrderBy(mbox => mbox.Manufacturer).ToList();

            CollectionAssert.AreEqual(productsExpected, filter.getFilteredProducts(productsOriginal));
        }
    }
}
