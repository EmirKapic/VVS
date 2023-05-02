namespace TechHaven.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public ShoppingCart Products { get; set; }
        public int Price { get; set; }
        public string ShippingAddress { get; set; }
        public Discount Discount { get; set; }
        public DateTime OrderDate { get; set; }

        [ForeignKey("RecommendationId")]
        public Recommendation Recommendation { get; set; }
        public int RecommendationId { get; set; }

        public Order(ShoppingCart products, int price, string shippingAddress, Discount discount, DateTime orderDate)
        {
            Products = products;
            Price = price;
            ShippingAddress = shippingAddress;
            Discount = discount;
            OrderDate = orderDate;
        }
    }
}
