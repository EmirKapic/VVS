using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechHaven.Services;

namespace TechHavenTests.Whitebox
{
    [TestClass]
    public class calculateWithHolidaysTest
    {
        [TestMethod]
        public void calculateWithHolidays_dateIsNull_throwsException() 
        {
            DiscountService discountService = new DiscountService();
            DateTime datum = new DateTime();
            Assert.ThrowsException<ArgumentException>(() => discountService.calculateWithHolidays(100, 50, datum));
        }

        [TestMethod]
        public void calculateWithHolidays_invalidDate_throwsException()
        {
            DiscountService discountService = new DiscountService();
            DateTime datum = new DateTime(2000, 2, 2);
            Assert.ThrowsException<ArgumentException>(() => discountService.calculateWithHolidays(100, 50, datum));
        }

        [TestMethod]
        public void calculateWithHolidays_newYearDiscount_returnsDiscountedPrice()
        {
            DiscountService discountService = new DiscountService();
            DateTime datum = new DateTime(2025, 1, 1);
            Assert.AreEqual( 0,discountService.calculateWithHolidays(100, 50, datum));
        }

        [TestMethod]
        public void calculateWithHolidays_noHolidayDiscount_returnsDiscountedPrice()
        {
            DiscountService discountService = new DiscountService();
            DateTime datum = new DateTime(2025, 2, 1);
            Assert.AreEqual(50, discountService.calculateWithHolidays(100, 50, datum));
        }
    }


}
