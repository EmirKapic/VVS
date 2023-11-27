using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHaven.Tests.Services
{
    [TestClass]
    public class OrdersManagerTests
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private Mock<UserManager<Customer>> _mockUserManager;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private Mock<CartManager> _mockCartManager;
        private OrdersManager _ordersManager;
        private List<Product> _products;

        [TestInitialize]
        public void TestInitialize()
        {
            // Create a list of products
            _products = new List<Product>
        {
            new Product { Id = 1, Category = "Laptop", Manufacturer = "HP", Model = "EliteBook", Price = 999.99, NumberOfAvailable = 10 },
            new Product { Id = 2, Category = "Smartphone", Manufacturer = "Samsung", Model = "Galaxy S21", Price = 799.99, NumberOfAvailable = 20 },
        };

            // Mock DbContext
            _mockDbContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            // Mock UserManager
            var userStoreMock = new Mock<IUserStore<Customer>>();
            _mockUserManager = new Mock<UserManager<Customer>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Mock HttpContextAccessor
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            // Set up HttpContext User
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, "userId")
            }, "mock");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            // Mock CartManager
            _mockCartManager = new Mock<CartManager>(_mockUserManager.Object, _mockDbContext.Object, _mockHttpContextAccessor.Object, null);

            // Create OrdersManager
            _ordersManager = new OrdersManager(_mockDbContext.Object, _mockUserManager.Object, _mockHttpContextAccessor.Object, _mockCartManager.Object);
        }

        [TestMethod]
        public void GetProductsFromOrders_ReturnsListOfProducts()
        {
            // Arrange
            _mockDbContext.Setup(db => db.Product).Returns(DbSetMock.GetDbSetMock(_products).Object);

            // Set up the user and their orders
            var user = new Customer
            {
                Id = "userId",
                Orders = new List<Order> { new Order { Products = new List<Product> { _products[0], _products[1] } } }
            };

            _mockUserManager.Setup(um => um.GetUserId(_mockHttpContextAccessor.Object.HttpContext.User)).Returns(user.Id);
            _mockDbContext.Setup(db => db.Customer).Returns(DbSetMock.GetDbSetMock(new List<Customer> { user }).Object);

            // Act
            var result = _ordersManager.GetProductsFromOrders();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetProductsFromOrders_NoOrders_ReturnsRandomProducts()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserId(_mockHttpContextAccessor.Object.HttpContext.User)).Returns("userId");

            // Set up the DbContext with the Product property
            _mockDbContext.Setup(db => db.Product).Returns(DbSetMock.GetDbSetMock(_products).Object);

            // Set up the user with no orders
            _mockDbContext.Setup(db => db.Customer).Returns(DbSetMock.GetDbSetMock(new List<Customer> { new Customer { Id = "userId", Orders = null } }).Object);

            // Act
            var result = _ordersManager.GetProductsFromOrders();

            // Assert
            Assert.IsNotNull(result);
        }



        [TestMethod]
        public void GetProductsFromOrders_NoCommonCategory_ReturnsEmptyList()
        {
            // Arrange
            var user = new Customer
            {
                Id = "userId",
                Orders = new List<Order> { new Order { Products = new List<Product> { } } }
            };
            _mockUserManager.Setup(um => um.GetUserId(_mockHttpContextAccessor.Object.HttpContext.User)).Returns(user.Id);
            _mockDbContext.Setup(db => db.Customer).Returns(DbSetMock.GetDbSetMock(new List<Customer> { user }).Object);

            // Act
            var result = _ordersManager.GetProductsFromOrders();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}


