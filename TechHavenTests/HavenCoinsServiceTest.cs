using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class HavenCoinsServiceTest
    {
        public HavenCoinsService coinsService;

        [TestInitialize] public void Setup()
        {
            var lines = new string[] { "1,28", "2,12" };
            var fileOperationsMock = new Mock<IFileOperations>();
            fileOperationsMock.Setup(fo => fo.ReadAllLines(It.IsAny<string>())).Returns(lines);
            coinsService = new HavenCoinsService(fileOperationsMock.Object);
        }

        [TestMethod]
        public void TestGetHavenCoins()
        {
            int id = 1;
            var usersCoins = coinsService.getHavenCoins(id);
            Assert.IsTrue(usersCoins >= 0 && usersCoins <= 50);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNonExistantId()
        {
            int id = -3;
            coinsService.getHavenCoins(id);
        }

        [TestMethod]
        public void TestAddHavenCoins()
        {
            int id = 1;
            int coins = 1;
            var prevcoins = coinsService.getHavenCoins(id);
            Assert.IsTrue(coinsService.AddHavenCoins(id, coins));
            Assert.AreEqual(prevcoins + coins, coinsService.getHavenCoins(id));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNegativeCoins()
        {
            int id = 1;
            int coins = -1;
            coinsService.AddHavenCoins(id, coins);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddToBadUserId()
        {
            int id = -1;
            int coins = 3;
            coinsService.AddHavenCoins(id, coins);
        }

        [TestMethod]
        public void TestSubtractHavenCoins()
        {
            int id = 1;
            int coins = 1;
            var prevcoins = coinsService.getHavenCoins(id);
            Assert.IsTrue(coinsService.SubtractHavenCoins(id, coins));
            Assert.AreEqual(prevcoins - coins, coinsService.getHavenCoins(id));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubtractNegativeCoins()
        {
            int id = 1;
            int coins = -1;
            coinsService.SubtractHavenCoins(id, coins);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubtractFromBadUserId()
        {
            int id = -1;
            int coins = 3;
            coinsService.SubtractHavenCoins(id, coins);
        }


        [TestMethod]
        public void TestAddTooMuch()
        {
            int id = 1;
            int coins = 52;
            Assert.IsFalse(coinsService.AddHavenCoins(id, coins));
        }

        [TestMethod]
        public void TestSubtractTooMuch()
        {
            int id = 1;
            int coins = 52;
            Assert.IsFalse(coinsService.SubtractHavenCoins(id, coins));
        }
    }
}
