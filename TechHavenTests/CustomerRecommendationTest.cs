using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class CustomerRecommendationTest
    {
        public CustomerRecommendation recommendation;
        [TestInitialize]
        public void Setup()
        {
            recommendation = new CustomerRecommendation("fav", null);
        }

        [TestMethod]
        public void TestRecommendation()
        {
            List<Product> products = new List<Product>()
            {
                new Product(){Category = "fav", Id= 1},
                new Product(){Category = "notfav", Id= 2},
                new Product(){Category = "notfav", Id= 3},
                new Product(){Category = "fav", Id=4}
            };
            recommendation._products = products;
            var res = recommendation.RecommendProducts().ToArray();
            Assert.AreEqual(2, res.Length);
            Assert.AreEqual(1, res[0].Id);
            Assert.AreEqual(4, res[1].Id);
        }

        [TestMethod]
        public void TestNoneFound()
        {
            List<Product> products = new List<Product>()
            {
                new Product(){Category = "notfav", Id= 1},
                new Product(){Category = "notfav", Id= 2},
                new Product(){Category = "notfav", Id= 3},
                new Product(){Category = "notfav", Id=4}
            };
            recommendation._products = products;
            var res = recommendation.RecommendProducts().ToArray();
            Assert.AreEqual(0, res.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullList()
        {
            recommendation._products = null;
            var res = recommendation.RecommendProducts().ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullFavCategory()
        {
            recommendation._products = new List<Product>();
            recommendation.favoriteCategory = null;
            var res = recommendation.RecommendProducts().ToArray();
        }

    }
}
