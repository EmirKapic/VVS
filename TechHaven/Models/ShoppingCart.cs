using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        public ICollection<Product>? Products = new List<Product>();
        public double TotalPrice { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public ShoppingCart() { }
    }
}

