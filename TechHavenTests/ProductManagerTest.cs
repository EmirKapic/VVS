using Microsoft.EntityFrameworkCore;
using Moq;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class ProductManagerTest
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private ProductManager _productManager;
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
            _mockDbContext.Setup(db => db.Product).Returns(DbSetMock.GetDbSetMock(_products).Object);
            _productManager = new ProductManager(_mockDbContext.Object);
        }

        [TestMethod]
        public void GetAllByCategoryTest()
        {
            var category = "Phone";
            var actual = _productManager.GetAllByCategory(category);
            var expected = _products.Where(prod => prod.Category == category).ToList();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAllByNullCategoryTest()
        {
            _productManager.GetAllByCategory(null);
        }

        [TestMethod]
        public void GetAllCategoriesTest()
        {
            var actual = _productManager.GetAllCategories();
            var expected = _products.Select(prod => prod.Category).Distinct().ToList();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void GetAllCategoriesFromProductsTest()
        {
            var actual = _productManager.GetAllCategoriesFromProducts(_products);
            var expected = _products.Select(prod => prod.Category).Distinct().ToList();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void GetAllManufacturersFromProductsTest()
        {
            var actual = _productManager.GetAllManufacturersFromProducts(_products);
            var expected = _products.Select(prod => prod.Manufacturer).Distinct().ToList();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void GetAllManufacturersForCategoryTest()
        {
            var category = "Phone";
            var actual = _productManager.GetAllManufacturersForCategory(category);
            var expected = _products.Where(prod => prod.Category == category).Select(prod => prod.Manufacturer).Distinct().ToList();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetAllManufacturersForNullCategoryTest()
        {
            _productManager.GetAllManufacturersForCategory(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetRandomProductsNegativeLimitTest()
        {
            _productManager.GetRandomProducts(-1);
        }

        [TestMethod]
        public void GetAllProductsTest()
        {
            CollectionAssert.AreEquivalent(_products, _productManager.GetAllProducts());
        }

        [TestMethod]
        public void GetProductsFromIdsTest()
        {
            var ids = new List<int> { 1, 3 };
            var actual = _productManager.GetProductsFromIds(ids);
            var expected = _products.Where(prod => ids.Exists(id => id == prod.Id)).ToList();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void GetProductsContainingStringTest()
        {
            var query = "phone";
            var actual = _productManager.GetProductsContainingString(query);
            var expected = _products.Where(prod => (prod.Model + prod.Category + prod.Manufacturer).ToLower().Contains(query.ToLower())).ToList();
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
