namespace TechHaven.Models
{
    public class Discount
    {
        public List<Product> Products { get; set; }
        public int NumberOfHavenCoins { get; set; }
        public double AdditionalDiscount { get; set; }
        public double TotalDiscount { get; set; }

        public Discount(List<Product> products, int numberOfHavenCoins, double additionalDiscount, double totalDiscount)
        {
            Products = products;
            NumberOfHavenCoins = numberOfHavenCoins;
            AdditionalDiscount = additionalDiscount;
            TotalDiscount = totalDiscount;
        }
    }
}

