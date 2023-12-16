using Microsoft.EntityFrameworkCore;
using Moq;
using TechHaven.Controllers;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        private ProductsController productController;

        public ProductsControllerTest()
        {
            var productList = new List<Product>();
            productList.Add(new Product() { Id = 1, Model = "testModel1" });

            var mockDbSet = DbSetMock.GetDbSetMock(productList);

            var applicationDbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            applicationDbContextMock.Setup(mbox => mbox.Product).Returns(mockDbSet.Object);

            productController = new ProductsController(applicationDbContextMock.Object, null, null);
        }

        [TestMethod]
        public void TestProductExists()
        {
            Assert.IsTrue(productController.ProductExists(1));
        }

        [TestMethod]
        public void TestDecodeSortTypeHighestFirst()
        {
            Assert.AreEqual(SortType.HighestFirst, productController.DecodeSortType("HighestFirst"));
        }

        [TestMethod]
        public void TestDecodeSortTypeLowestFirst()
        {
            Assert.AreEqual(SortType.LowestFirst, productController.DecodeSortType("LowestFirst"));
        }

        [TestMethod]
        public void TestDecodeSortTypeAlphabetical()
        {
            Assert.AreEqual(SortType.Alphabetical, productController.DecodeSortType(""));
        }

    }
}
