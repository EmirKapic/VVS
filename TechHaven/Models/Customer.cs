using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    public class Customer : IdentityUser, Person, User
    {
        [Key]
        override public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? NumberOfHavenCoins { get; set; }

        public ShoppingCart? ShoppingCart { get; set; }

        public List<Product>? Products { get; set; } //Wish-list

        public List<Order>? Orders { get; set; }

        public Customer() { Products = new List<Product>();}
    }
}

