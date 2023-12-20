using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public Order() { }
    }
}
