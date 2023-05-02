namespace TechHaven.Models
{
    public class Customer : Person, User
    {
        public int NumberOfHavenCoins { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public List<Product> Wishlist { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

