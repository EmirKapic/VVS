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

        public ShoppingCart? ShoppingCart { get; set; } = null!;

        public ICollection<Product>? Products { get; set; } = new List<Product>();//Wish-list

        public ICollection<Order>? Orders { get; set; } = new List<Order>();

        public Customer() {}
    }
}

