using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public double? Price { get; set; }
        public int? NumberOfAvailable { get; set; }

        public ICollection<Customer>? Customers { get; set; } = new List<Customer>();
        
        public ICollection<ShoppingCart>? ShoppingCarts { get; set; } = new List<ShoppingCart>();

        public ICollection<Order>? Orders { get; set; } = new List<Order>();

        public Product() { }
    }
}
