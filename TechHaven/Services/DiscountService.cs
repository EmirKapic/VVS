namespace TechHaven.Services
{
    public class DiscountService
    {

        public int calculate(int regularPrice,int numberOfCoins)
        {
            if (regularPrice < 0 || numberOfCoins < 0)
            {
                throw new ArgumentException();
            }
            return (int)(regularPrice - regularPrice * (numberOfCoins / (double)100));
        }


        public int calculateWithHolidays(int regularPrice,int numberOfCoins, DateTime date) {
            if (date == null || date < DateTime.Now)
            {
                throw new ArgumentException();
            }
            double holidayDiscount = 0;
            if (date.Month == 1 && date.Day == 1)
            {
                holidayDiscount = 0.5;
            }
            return (int)(regularPrice - (regularPrice * (numberOfCoins / (double)100)) - regularPrice * holidayDiscount);
        }
    }
}
