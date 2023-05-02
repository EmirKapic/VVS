namespace TechHaven.Models
{
    public class Guest : User
    {
        public ShoppingCart ShoppingCart { get; set; }

        public Guest()
        {
            ShoppingCart = new ShoppingCart();
        }

        public Guest() { }
    }
}

