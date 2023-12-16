using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using TechHaven.Services;

namespace TechHaven.Services.Tests
{
    [TestClass]
    public class HavenCoinsServiceTests
    {
        private HavenCoinsService havenCoinsService;

        [TestInitialize]
        public void Setup()
        {
            var lines = new string[] { "1,28", "2,12" };
            var fileOperationsMock = new Mock<IFileOperations>();
            fileOperationsMock.Setup(fo => fo.ReadAllLines(It.IsAny<string>())).Returns(lines);
            havenCoinsService = new HavenCoinsService(fileOperationsMock.Object);
        }

        [TestMethod]
        public void SubtractHavenCoins_ValidSubtraction_ReturnsTrue()
        {
            // Arrange
            int userId = 1; 
            int initialCoins = 28; 
            int coinsToSubtract = 5; 

            // Act
            bool result = havenCoinsService.SubtractHavenCoins(userId, coinsToSubtract);

            // Assert
            Assert.IsTrue(result, "SubtractHavenCoins should return true for valid subtraction");
            Assert.AreEqual(initialCoins - coinsToSubtract, havenCoinsService.csv.Find(e => e[0] == userId)[1],
                "The number of coins in the CSV should be updated correctly");
        }

        [TestMethod]
        public void SubtractHavenCoins_InvalidSubtraction_ThrowsArgumentException()
        {
            // Arrange
            int userId = 1; 
            int coinsToSubtract = -5; 

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => havenCoinsService.SubtractHavenCoins(userId, coinsToSubtract),
                "SubtractHavenCoins should throw ArgumentException for invalid subtraction");
        }

        [TestMethod]
        public void SubtractHavenCoins_InsufficientCoins_ReturnsFalse()
        {
            // Arrange
            int userId = 2; 
            int initialCoins = 12; 
            int coinsToSubtract = 15; 

            // Act
            bool result = havenCoinsService.SubtractHavenCoins(userId, coinsToSubtract);

            // Assert
            Assert.IsFalse(result, "SubtractHavenCoins should return false for insufficient coins");
            Assert.AreEqual(initialCoins, havenCoinsService.csv.Find(e => e[0] == userId)[1],
                "The number of coins in the CSV should not be updated");
        }
    }
}
