using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    public class Product : ProductPrototype
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

        //Klonira produkt ne klonirajuci Id, time kreiramo novi produkt sa informacijama nad kojim mozemo raditi
        //bez opasnosti da imamo 2x objekat sa istim Id-om (ako se to desi entity goes boom)
        public ProductPrototype Clone()
        {
            Product p = new Product();
            p.Manufacturer = Manufacturer;
            p.Model = Model;
            p.Price = Price;
            p.NumberOfAvailable = NumberOfAvailable;
            p.Customers = Customers;
            p.Orders = Orders;
            return p;  
        }

        public Product() { }


    }
}
