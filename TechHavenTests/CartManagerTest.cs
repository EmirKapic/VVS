using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class CartManagerTest
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private Mock<UserManager<Customer>> _mockUserManager;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private CartManager _cartManager;
        private List<Product> _products;

        [TestInitialize]
        public void TestInitialize()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Category = "Laptop", Manufacturer = "HP", Model = "EliteBook", Price = 999.99, NumberOfAvailable = 10 },
                new Product { Id = 2, Category = "Phone", Manufacturer = "Samsung", Model = "Galaxy S21", Price = 799.99, NumberOfAvailable = 20 },
                new Product { Id = 3, Category = "Phone", Manufacturer = "Apple", Model = "iPhone 13", Price = 1399.99, NumberOfAvailable = 15 },
                new Product { Id = 4, Category = "Tablet", Manufacturer = "Lenovo", Model = "Tab M9", Price = 299.99, NumberOfAvailable = 30 },
            };

            _mockDbContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            var userStoreMock = new Mock<IUserStore<Customer>>();
            _mockUserManager = new Mock<UserManager<Customer>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var claimsIdentity = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "userId") }, "mock");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };

            _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);
            _mockDbContext.Setup(db => db.Product).Returns(DbSetMock.GetDbSetMock(_products).Object);

            var user = new Customer {
                Id = "userId",
                ShoppingCart = new ShoppingCart { Products = _products }
            };

            _mockUserManager.Setup(um => um.GetUserId(_mockHttpContextAccessor.Object.HttpContext.User)).Returns(user.Id);
            _mockDbContext.Setup(db => db.Customer).Returns(DbSetMock.GetDbSetMock(new List<Customer> { user }).Object);
            
            _cartManager = new CartManager(_mockUserManager.Object, _mockDbContext.Object, _mockHttpContextAccessor.Object, null);
        }

        [TestMethod]
        public void AddToCartTest()
        {
            var product = new Product { Id = 5, Category = "Phone", Manufacturer = "Google", Model = "Pixel 6", Price = 599.99, NumberOfAvailable = 7 };
            var actual = _cartManager.AddToCart(product);
            CollectionAssert.AreEquivalent(_products, actual);
        }

        [TestMethod]
        public void AddToCartIfCartIsNullTest()
        {
            var product = new Product { Id = 5, Category = "Phone", Manufacturer = "Google", Model = "Pixel 6", Price = 599.99, NumberOfAvailable = 7 };
            var user = new Customer
            {
                Id = "userId",
                ShoppingCart = null
            };
            _mockDbContext.Setup(db => db.Customer).Returns(DbSetMock.GetDbSetMock(new List<Customer> { user }).Object);
            _cartManager = new CartManager(_mockUserManager.Object, _mockDbContext.Object, _mockHttpContextAccessor.Object, null);
            
            var actual = _cartManager.AddToCart(product);
            var expected = new List<Product> { product };
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void RemoveFromCartTest()
        {
            var product = new Product { Id = 1, Category = "Laptop", Manufacturer = "HP", Model = "EliteBook", Price = 999.99, NumberOfAvailable = 10 };
            var actual = _cartManager.RemoveFromCart(product);
            CollectionAssert.AreEquivalent(_products, actual);
        }

        [TestMethod]
        public void getAllFromCartTest()
        {
            var actual = _cartManager.getAllFromCart();
            CollectionAssert.AreEquivalent(_products, actual);
        }

        [TestMethod]
        public void getAllFromCartIfCartIsNullTest()
        {
            var user = new Customer
            {
                Id = "userId",
                ShoppingCart = null
            };
            _mockDbContext.Setup(db => db.Customer).Returns(DbSetMock.GetDbSetMock(new List<Customer> { user }).Object);
            _cartManager = new CartManager(_mockUserManager.Object, _mockDbContext.Object, _mockHttpContextAccessor.Object, null);

            var actual = _cartManager.getAllFromCart();
            CollectionAssert.AreEquivalent(new List<Product>{}, actual);
        }

        [TestMethod]
        public void GetCurrentCartTest()
        {
            var actual = _cartManager.GetCurrentCart().Products.ToList();
            CollectionAssert.AreEquivalent(_products, actual);
        }

        [TestMethod]
        public void GetCurrentCartIfCartIsNullTest()
        {
            var user = new Customer
            {
                Id = "userId",
                ShoppingCart = null
            };
            _mockDbContext.Setup(db => db.Customer).Returns(DbSetMock.GetDbSetMock(new List<Customer> { user }).Object);
            _cartManager = new CartManager(_mockUserManager.Object, _mockDbContext.Object, _mockHttpContextAccessor.Object, null);

            var cart = _cartManager.GetCurrentCart();
            Assert.IsInstanceOfType(cart, typeof(ShoppingCart));
            Assert.IsNotNull(cart);
        }

        [TestMethod]
        public void ClearCartTest()
        {
            var userId = "userId";
            _cartManager.ClearCart(userId);
            var actual = _cartManager.getAllFromCart();
            CollectionAssert.AreEquivalent(new List<Product>(), actual);
        }

        [TestMethod]
        public void addToCartTest()
        {
            var product = new Product { Id = 5, Category = "Phone", Manufacturer = "Google", Model = "Pixel 6", Price = 599.99, NumberOfAvailable = 7 };
            var actual = new CartManager().addToCart(product);
            CollectionAssert.AreEquivalent(new List<Product> { product }, actual);
        }

        [TestMethod]
        public void GetAllFromCartTest()
        {
            var actual = new CartManager().GetAllFromCart();
            CollectionAssert.AreEquivalent(new List<Product>(), actual);
        }

        [TestMethod]
        public void removeFromCartTest()
        {
            var product1 = new Product { Id = 1, Category = "Laptop", Manufacturer = "HP", Model = "EliteBook", Price = 999.99, NumberOfAvailable = 10 };
            var product2 = new Product { Id = 2, Category = "Phone", Manufacturer = "Samsung", Model = "Galaxy S21", Price = 799.99, NumberOfAvailable = 20 };
            var cart = new CartManager();
            cart.addToCart(product1); cart.addToCart(product2);

            var productId = 1;
            var actual = cart.removeFromCart(productId);
            var expected = new List<Product> { product2 };
            CollectionAssert.AreEquivalent(expected, actual);
        }
        
        [TestMethod]
        public void getCurrentCartTest()
        {
            var cart = new CartManager();
            Assert.IsNotNull(cart.getCurrentCart());
        }
    }
}
