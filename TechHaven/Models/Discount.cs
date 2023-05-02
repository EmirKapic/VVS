namespace TechHaven.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public List<Product> Products { get; set; }
        public int NumberOfHavenCoins { get; set; }
        public double AdditionalDiscount { get; set; }
        public double TotalDiscount { get; set; }

        public Discount(int discountId, List<Product> products, int numberOfHavenCoins, double additionalDiscount, double totalDiscount)
        {
            DiscountId = discountId;
            Products = products;
            NumberOfHavenCoins = numberOfHavenCoins;
            AdditionalDiscount = additionalDiscount;
            TotalDiscount = totalDiscount;
        }

        public Discount() { }
    }
}

