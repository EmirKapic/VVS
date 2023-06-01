using TechHaven.Models;

namespace TechHaven.Services
{
    public interface ICartManager
    {
        //Adds new product and returns the updated list
        public Task<ICollection<Product>> AddToCart(Product prod);
        public Task<ICollection<Product>> getAllFromCart();
        //Removes from the list of products in the cart and returns the new list
        public Task<ICollection<Product>> RemoveFromCart(Product prod);
        //Gets the current cart, the users cart if logged in, otherwise the singleton cart that belongs to guests
        public Task<ShoppingCart> GetCurrentCart();
    }
}
