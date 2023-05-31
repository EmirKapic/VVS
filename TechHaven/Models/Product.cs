using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; } = String.Empty;
        public string Manufacturer { get; set; } = String.Empty;
        public string Model { get; set; } = String.Empty;
        public double Price { get; set; } = 0;
        public int NumberOfAvailable { get; set; } = 0;

        public ICollection<Customer>? Customers { get; set; } = new List<Customer>();
        
        public ICollection<ShoppingCart>? ShoppingCarts { get; set; } = new List<ShoppingCart>();

        public ICollection<Order>? Orders { get; set; } = new List<Order>();

        public Product() { }
    }
}
