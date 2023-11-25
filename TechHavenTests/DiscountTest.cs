using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class DiscountTest
    {
        public DiscountService? discountService;
        [TestInitialize]
        public void Setup()
        {
            discountService = new DiscountService();
        }
        


        public static IEnumerable<object []> TestData2()
        {
            return new List<object[]>()
            {
                new object[]{2000, 17, 1660},
                new object[]{0, 24, 0},
                new object[]{250, 0, 250}
            };
        }

        [TestMethod]
        [DynamicData(nameof(TestData2), DynamicDataSourceType.Method)]
        public void TestCalculateDiscount(int regularPrice, int numberOfCoins, int expected)
        {
            Assert.AreEqual(expected,discountService.calculate(regularPrice, numberOfCoins));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIncorrectParameters()
        {
            discountService.calculate(-1, 0);
        }


        [TestMethod]
        public void TestHolidayDiscount()
        {
            Assert.AreEqual(2000, discountService.calculateWithHolidays(2000, 0, DateTime.Now.AddSeconds(1)));
            DateTime newYears = DateTime.Parse("2024-01-01"); //everyone gets 50%off on new years day
            Assert.AreEqual(1000, discountService.calculateWithHolidays(2000, 0, newYears));
        }

        [TestMethod]
        public void TestHolidayWithCoins()
        {
            Assert.AreEqual(1660, discountService.calculateWithHolidays(2000, 17, DateTime.Now.AddSeconds(1)));
            DateTime newYears = DateTime.Parse("2024-01-01"); //everyone gets 50%off on new years day
            Assert.AreEqual(660, discountService.calculateWithHolidays(2000, 17, newYears));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestHolidayBadDate()
        {
            discountService.calculateWithHolidays(2000, 0, DateTime.Parse("2002-11-15"));
        }

    }
}
