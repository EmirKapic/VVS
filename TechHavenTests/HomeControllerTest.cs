using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using TechHaven.Controllers;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestRecommend()
        {
            var productList = new List<Product>();
            productList.Add(new Product() { Model = "testModel1"});
            productList.Add(new Product() { Model = "testModel2" });
            productList.Add(new Product() { Model = "testModel3" });
            var customerRecommendationMock = new Mock<CustomerRecommendation>("w", new List<Product>());
            customerRecommendationMock.Setup(m => m.RecommendProducts()).Returns(productList);

            var homeController = new HomeController(customerRecommendationMock.Object, null);
            var res = homeController.recommendProduct();
            Assert.AreEqual(3, res.Count());
        }

        [TestMethod]
        public void TestRecommendWithNull()
        {
            List<Product> productList = null;
            var customerRecommendationMock = new Mock<CustomerRecommendation>("w", new List<Product>());
            customerRecommendationMock.Setup(m => m.RecommendProducts()).Returns(productList);
            var homeController = new HomeController(customerRecommendationMock.Object, null);
            Assert.AreEqual(0, homeController.recommendProduct().Count());
        }
    }
}