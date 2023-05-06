using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    [NotMapped]
    public class Guest : User
    {
        public ShoppingCart ShoppingCart { get; set; }

        public Guest()
        {
            ShoppingCart = new ShoppingCart();
        }
    }
}

