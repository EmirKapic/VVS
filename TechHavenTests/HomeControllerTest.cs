using Moq;
using TechHaven.Controllers;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var productList = new List<Product>();
            productList.Add(new Product() { Model = "testModel1"});
            productList.Add(new Product() { Model = "testModel2" });
            productList.Add(new Product() { Model = "testModel3" });
            var customerRecommendationMock = new Mock<CustomerRecommendation>();
            customerRecommendationMock.Setup(m => m.RecommendProducts()).Returns(productList);

            var homeController = new HomeController(customerRecommendationMock.Object);
            var res = homeController.recommendProduct().ToArray();
            Assert.AreEqual("testModel1", res[0].Model);
        }
    }
}