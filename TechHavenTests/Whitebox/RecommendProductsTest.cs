using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests.Whitebox
{
    [TestClass]
    public class RecommendProductsTest
    {
        public CustomerRecommendation recommendation;
        [TestInitialize]
        public void Setup()
        {
            recommendation = new CustomerRecommendation("fav", null);
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

        [TestMethod]
        public void TestEmptyProducts()
        {
            recommendation._products = new List<Product>();
            recommendation.favoriteCategory = "fav";
            Assert.AreEqual(0, recommendation.RecommendProducts().ToArray().Length);
        }


        [TestMethod]
        public void Test1Product()
        {
            recommendation._products = new List<Product>()
            {
                new Product(){Category = "fav", Id=1}
            };
            recommendation.favoriteCategory = "fav";
            Assert.AreEqual(1, recommendation.RecommendProducts().ElementAt(0).Id);
            Assert.AreEqual(1, recommendation.RecommendProducts().ToArray().Length);
        }

        [TestMethod]
        public void Test2Products()
        {
            recommendation._products = new List<Product>()
            {
                new Product(){Category = "fav", Id=1},
                new Product(){Category = "fav", Id=2}
            };
            recommendation.favoriteCategory = "fav";
            Assert.AreEqual(1, recommendation.RecommendProducts().ElementAt(0).Id);
            Assert.AreEqual(2, recommendation.RecommendProducts().ElementAt(1).Id);
            Assert.AreEqual(2, recommendation.RecommendProducts().ToArray().Length);
        }

        [TestMethod]
        public void TestNoneFavorite()
        {
            recommendation._products = new List<Product>()
            {
                new Product(){Category = "notfav", Id=1},
                new Product(){Category = "notfav", Id=2}
            };
            recommendation.favoriteCategory = "fav";
            Assert.AreEqual(0, recommendation.RecommendProducts().ToArray().Length);
        }
    }
}
