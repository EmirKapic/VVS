using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechHaven.Controllers;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class OrdersControllerTest
    {
        [TestMethod]
        public void GetOrderById_Returns_Order_With_Correct_Id()
        {
            // Arrange
            var ordersController = new OrdersController();
            int orderId = 1; // Assuming order ID 1 exists

            // Act
            var result = ordersController.GetOrderById(orderId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(orderId, result.Id);
        }
    }
}
