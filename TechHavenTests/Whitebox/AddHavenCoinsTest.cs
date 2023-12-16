using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechHaven.Services;

namespace TechHaven.Services.Tests
{
    [TestClass]
    public class AddHavenCoinsTest
    {
        private HavenCoinsService havenCoinsService;

        [TestInitialize]
        public void TestInitialize()
        {
            var lines = new string[] { "1,28", "2,12" };
            var fileOperationsMock = new Mock<IFileOperations>();
            fileOperationsMock.Setup(fo => fo.ReadAllLines(It.IsAny<string>())).Returns(lines);
            havenCoinsService = new HavenCoinsService(fileOperationsMock.Object);
        }

        [TestMethod]
        public void AddHavenCoins_ValidIdAndPositiveCoins_ReturnTrue()
        {
            int id = 1; 
            int coins = 28;
            int addCoins = 7;

            bool result = havenCoinsService.AddHavenCoins(id, addCoins);

            Assert.IsTrue(result, "AddHavenCoins must return true for valid addition.");
            Assert.AreEqual(coins + addCoins, havenCoinsService.getHavenCoins(id));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddHavenCoins_InvalidId_ShouldThrowArgumentException()
        {
            int id = -1; 
            int addCoins = 5;

            havenCoinsService.AddHavenCoins(id, addCoins);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddHavenCoins_NegativeCoins_ShouldThrowArgumentException()
        {
            int id = 1;
            int addCoins = -1;

            havenCoinsService.AddHavenCoins(id, addCoins);
        }

        [TestMethod]
        public void AddHavenCoins_ExceedsMaximumCoins_ReturnFalse()
        {
            int id = 1; 
            int coins = 28; 
            int addCoins = 23;

            bool result = havenCoinsService.AddHavenCoins(id, addCoins);

            Assert.IsFalse(result, "AddHavenCoins must return false for exceeding the maximum (50) coins.");
            Assert.AreEqual(coins, havenCoinsService.getHavenCoins(id));
        }
    }
}
