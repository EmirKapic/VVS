using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using TechHaven.Controllers;
using TechHaven.Models;
using TechHaven.Services;



namespace TechHaven.Tests.Controllers
{
    [TestClass]
    public class ShoppingCartControllerTests
    {
        [TestMethod]
        public void Index_ReturnsViewWithCartItems()
        {
            // Arrange
            var cartManagerMock = new Mock<ICartManager>();
            var controller = new ShoppingCartController(cartManagerMock.Object);

            var products = new List<Product>
        {
            new Product { Id = 1, Category = "Laptop", Manufacturer = "HP", Model = "EliteBook", Price = 999.99, NumberOfAvailable = 10 },
            new Product { Id = 2, Category = "Smartphone", Manufacturer = "Samsung", Model = "Galaxy S21", Price = 799.99, NumberOfAvailable = 20 }
        };

            cartManagerMock.Setup(manager => manager.GetAllFromCart()).Returns(products);

            // Act
            var result = controller.Index() as ViewResult;
            var model = result?.Model as List<CartItemViewModel>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count); // Adjust based on your expected number of items
        }

        [TestMethod]
        public void Index_ReturnsNotFoundWhenCartManagerThrowsException()
        {
            // Arrange
            var cartManagerMock = new Mock<ICartManager>();
            var controller = new ShoppingCartController(cartManagerMock.Object);

            cartManagerMock.Setup(manager => manager.GetAllFromCart()).Throws<Exception>();

            // Act
            var result = controller.Index() as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RemoveFromCart_ReturnsJsonSuccess()
        {
            // Arrange
            var cartManagerMock = new Mock<ICartManager>();
            var controller = new ShoppingCartController(cartManagerMock.Object);

            // Act
            var result = controller.RemoveFromCart(1) as JsonResult;
            var data = result?.Value as dynamic;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("{ message = Successfully removed from cart! }", data?.ToString());
        }

        [TestMethod]
        public void RemoveFromCart_ReturnsJsonError()
        {
            // Arrange
            var cartManagerMock = new Mock<ICartManager>();
            var controller = new ShoppingCartController(cartManagerMock.Object);

            cartManagerMock.Setup(manager => manager.removeFromCart(It.IsAny<int>())).Throws<Exception>();

            // Act
            var result = controller.RemoveFromCart(1) as JsonResult;
            var data = result?.Value as dynamic;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("{ message = Couldn't remove from cart! }", data?.ToString());
        }
    }
}