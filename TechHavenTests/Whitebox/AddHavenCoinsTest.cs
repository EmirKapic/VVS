using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using TechHaven.Services;

namespace TechHaven.Services.Tests
{
    [TestClass]
    public class AddHavenCoinsTest
    {
        private HavenCoinsService havenCoinsService;
        private List<int[]> csv;

        [TestInitialize]
        public void TestInitialize()
        {
            havenCoinsService = new HavenCoinsService();
            csv = new List<int[]>(havenCoinsService.csv);
        }

        [TestMethod]
        public void AddHavenCoins_ValidIdAndPositiveCoins_ReturnTrue()
        {
            int id = 1; 
            int coins = 28;
            int addCoins = 7; 

            havenCoinsService.csv = new List<int[]>{ new int[]{ id, coins } };

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

            havenCoinsService.csv = new List<int[]> { new int[] { id, coins } };

            bool result = havenCoinsService.AddHavenCoins(id, addCoins);

            Assert.IsFalse(result, "AddHavenCoins must return false for exceeding the maximum (50) coins.");
            Assert.AreEqual(coins, havenCoinsService.getHavenCoins(id));
        }
        
        [TestCleanup]
        public void TestCleanup()
        {
            havenCoinsService.csv = csv;
            havenCoinsService.SaveNewCsv();
        }
    }
}
