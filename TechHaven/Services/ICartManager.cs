using TechHaven.Models;

namespace TechHaven.Services
{
    public interface ICartManager
    {
        //Adds new product and returns the updated list
        public List<Product> AddToCart(Product prod);
        public List<Product> getAllFromCart();
        //Removes from the list of products in the cart and returns the new list
        public List<Product> RemoveFromCart(Product prod);
        //Gets the current cart, the users cart if logged in, otherwise the singleton cart that belongs to guests
        public ShoppingCart GetCurrentCart();
        


        // Adds new product and returns the updated list
        List<Product> addToCart(Product prod);

        // Gets all products from the cart
        List<Product> GetAllFromCart();

        // Removes a product from the cart and returns the updated list
        List<Product> removeFromCart(int id);

        // Gets the current cart, the user's cart if logged in, otherwise the singleton cart that belongs to guests
        ShoppingCart getCurrentCart();
    }
}

