using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TechHaven.Controllers;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;
using static NuGet.Packaging.PackagingConstants;

namespace TechHavenTests
{
    [TestClass]
    public class MyAccountControllerTest
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private ProductManager _productManager;
        private MyAccountController controller;
        private List<Product> _products;

        [TestInitialize]
        public void InitializeTest()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Category = "Laptop", Manufacturer = "HP", Model = "EliteBook", Price = 999.99, NumberOfAvailable = 10 },
                new Product { Id = 2, Category = "Phone", Manufacturer = "Samsung", Model = "Galaxy S21", Price = 799.99, NumberOfAvailable = 20 },
                new Product { Id = 3, Category = "Phone", Manufacturer = "Apple", Model = "iPhone 13", Price = 1399.99, NumberOfAvailable = 15 },
                new Product { Id = 4, Category = "Tablet", Manufacturer = "Lenovo", Model = "Tab M9", Price = 299.99, NumberOfAvailable = 30 },
            };

            _mockDbContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _mockDbContext.Setup(db => db.Product).Returns(DbSetMock.GetDbSetMock(_products).Object);
            _productManager = new ProductManager(_mockDbContext.Object);

            var ordersManagerMock = new Mock<IOrdersManager>();

            controller = new MyAccountController(_productManager);
        }

        [TestMethod]
        public void IndexTest()
        {
            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void WishlistTest()
        {
            var result = controller.Wishlist();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            
        }

        [TestMethod]
        public void WishlistWhenEmptyTest()
        {
            _products = new List<Product>();
            _mockDbContext.Setup(db => db.Product).Returns(DbSetMock.GetDbSetMock(_products).Object);
            _productManager = new ProductManager(_mockDbContext.Object);

            var ordersManagerMock = new Mock<IOrdersManager>();

            controller = new MyAccountController(_productManager);

            var result = controller.Wishlist() as ViewResult;
            CollectionAssert.AreEquivalent(new List<Product>(), result.ViewData.ToList());
        }

        [TestMethod]
        public void AddToFavoritesTest()
        {
            var result = controller.AddToFavorites(5) as JsonResult;
            var data = result.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual("Sucessfully added to favorites!", data.ToString());
        }

        [TestMethod]
        public void AddToFavoritesWhenAlreadyExistsTest()
        {
            var result = controller.AddToFavorites(1) as JsonResult;
            var data = result.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual("Product already in favorites!", data.ToString());
        }

        [TestMethod]
        public void DropFromFavoritesTest()
        {
            var result = controller.DropFromFavorites(1) as JsonResult;
            var data = result.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual("Sucessfully removed from favorites!", data.ToString());
        }
    }
}