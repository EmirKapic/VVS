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
    public class AplhabeticalStrategyTest
    {
        [TestMethod]
        public void SortProductsTest()
        {
            var productsOriginal = new List<Product>();

            productsOriginal.Add(new Product { Manufacturer = "Volkswagen", Model = "Golf" });
            productsOriginal.Add(new Product { Manufacturer = "Volkswagen", Model = "Polo" });
            productsOriginal.Add(new Product { Manufacturer = "Volkswagen", Model = "Pasat" });
            productsOriginal.Add(new Product { Manufacturer = "Toyota", Model = "Carola" });
            productsOriginal.Add(new Product { Manufacturer = "Ford", Model = "Sedan" });

            var productsExpected = productsOriginal.OrderBy(p => p.Manufacturer + p.Model).ToList();

            CollectionAssert.AreEqual(productsExpected, new AlphabeticalStrategy().sortProducts(productsOriginal));
        }
    }
}
