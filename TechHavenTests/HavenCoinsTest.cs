using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class HavenCoinsTest
    {
        public HavenCoinsService coinsService;
        [TestInitialize] public void Setup()
        {
            coinsService = new HavenCoinsService();
        }
        /*
         * In .net6 where this project is built, the DataRows way of reading csv does not work
         * So I have manually loaded the csv and used dynamic data to feed it into the function
         * */
        public static IEnumerable<object[]> TestData()
        {
            // Read data from the CSV file
            var lines = File.ReadAllLines("../../../usercoins.csv");

            // Skip the header line and convert data to object arrays for parameterized testing
            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                yield return new object[] { int.Parse(values[0]), int.Parse(values[1]) };
            }
        }


        [TestMethod]
        [DynamicData(nameof(TestData), DynamicDataSourceType.Method)]
        public void TestGetHavenCoins(int id, int coins)
        {
            var usersCoins = coinsService.getHavenCoins(id);
            Assert.IsTrue(usersCoins >= 0 && usersCoins <= 50);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNonExistantId()
        {
            coinsService.getHavenCoins(-3);
        }

        [TestMethod]
        public void TestAddHavenCoins()
        {
            var prevcoins = coinsService.getHavenCoins(1);
            Assert.IsTrue(coinsService.AddHavenCoins(1, 1));
            Assert.AreEqual(prevcoins + 1, coinsService.getHavenCoins(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNegativeCoins()
        {
            coinsService.AddHavenCoins(1, -1);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddToBadUserId()
        {
            coinsService.AddHavenCoins(-1, 3);
        }

        [TestMethod]
        public void TestSubtractHavenCoins()
        {
            var prevcoins = coinsService.getHavenCoins(1);
            Assert.IsTrue(coinsService.SubtractHavenCoins(1, 1));
            Assert.AreEqual(prevcoins - 1, coinsService.getHavenCoins(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubtractNegativeCoins()
        {
            coinsService.SubtractHavenCoins(1, -1);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubtractFromBadUserId()
        {
            coinsService.SubtractHavenCoins(-1, 3);
        }
    }
}
