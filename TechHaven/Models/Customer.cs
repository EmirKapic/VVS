namespace TechHaven.Models
{
    public class Customer : Person, User
    {
        public int CustomerId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfHavenCoins { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public List<Product> Wishlist { get; set; }
        public List<Order> Orders { get; set; }

        public Customer(int customerId, string username, string email, string password, string firstName, string lastName, int numberOfHavenCoins, ShoppingCart shoppingCart, List<Product> wishlist, List<Order> orders)
        {
            CustomerId = customerId;
            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            NumberOfHavenCoins = numberOfHavenCoins;
            ShoppingCart = shoppingCart;
            Wishlist = wishlist;
            Orders = orders;
        }

        public Customer() { }
    }
}

