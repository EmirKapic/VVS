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
    public class HighestPriceStrategyTest
    {
        [TestMethod]
        public void SortProducts_returnSorted()
        {
            var productList = new List<Product>();
            productList.Add(new Product() { Price = 150 });
            productList.Add(new Product() { Price = 200 });
            productList.Add(new Product() { Price = 100 });

            var highestPriceStrategy = new HighestPriceStrategy();
            productList = highestPriceStrategy.sortProducts(productList);
            Assert.AreEqual(100, productList[2].Price);
            Assert.AreEqual(150, productList[1].Price); 
            Assert.AreEqual(200, productList[0].Price);
        }
    }
}
