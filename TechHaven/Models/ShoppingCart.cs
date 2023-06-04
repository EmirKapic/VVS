using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public double TotalPrice { get; set; } = 0;

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public List<int> Repetitions = new();

        public virtual void AddNewProduct(Product newProduct)
        {
            if (!Products.Contains(newProduct))
            {
                Products.Add(newProduct);
                TotalPrice += newProduct.Price;
            }
        }
        public virtual void RemoveProduct(Product newProduct)
        {
            if (Products.Contains(newProduct))
            {
                Products.Remove(newProduct);
                TotalPrice -= newProduct.Price;
            }
        }

        public ShoppingCart() { }
    }
}

