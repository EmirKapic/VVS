using TechHaven.Models;

namespace TechHaven.Services
{
    public interface ICartManager
    {
        public Task AddToCart(Product prod);
        public Task<ICollection<Product>> getAllFromCart();
    }
}
