using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [NotMapped]
        public Category Category { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public int NumberOfAvailable { get; set; }

        [ForeignKey("ShoppingCart")]
        public int ShoppingCartId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public Customer Customer { get; set; }


        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }


        public Product(int productId, Category category, string manufacturer, string model, double price, int numberOfAvailable)
        {
            ProductId = productId;
            Category = category;
            Manufacturer = manufacturer;
            Model = model;
            Price = price;
            NumberOfAvailable = numberOfAvailable;
        }

        public Product() { }
    }
}
