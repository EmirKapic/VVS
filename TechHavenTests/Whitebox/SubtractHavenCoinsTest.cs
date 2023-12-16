using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private List<int[]> originalCsvData;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the service with test data
            havenCoinsService = new HavenCoinsService();

            // Saving a copy of the original CSV data
            originalCsvData = new List<int[]>(havenCoinsService.csv);
        }

        [TestMethod]
        public void SubtractHavenCoins_ValidSubtraction_ReturnsTrue()
        {
            // Arrange
            int userId = 1; 
            int initialCoins = 28; 
            int coinsToSubtract = 5; 

            // Set up the CSV data for the test
            havenCoinsService.csv = new List<int[]>
            {
                new int[] { 1, initialCoins },
            };

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

            // Set up the CSV data for the test
            havenCoinsService.csv = new List<int[]>
            {
                new int[] { 2, initialCoins },
                // Add more user data if needed
            };

            // Act
            bool result = havenCoinsService.SubtractHavenCoins(userId, coinsToSubtract);

            // Assert
            Assert.IsFalse(result, "SubtractHavenCoins should return false for insufficient coins");
            Assert.AreEqual(initialCoins, havenCoinsService.csv.Find(e => e[0] == userId)[1],
                "The number of coins in the CSV should not be updated");
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Restore the original CSV data after each test
            havenCoinsService.csv = originalCsvData;

            // Optionally, you can save the restored data back to the CSV file
            havenCoinsService.SaveNewCsv();
        }
    }
}
