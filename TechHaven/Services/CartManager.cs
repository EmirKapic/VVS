using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TechHaven.Data;
using TechHaven.Models;

using Microsoft.EntityFrameworkCore;
namespace TechHaven.Services

{
    public class CartManager : ICartManager
    {
        private UserManager<Customer> _userManager;
        private ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartManager(UserManager<Customer> userManager, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor) {
            _userManager = userManager;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
 
        //Adds the product given as parameter to the currently logged-in user
        public async Task AddToCart(Product prod)
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var usr = await _db.Customer
                .Include(u => u.Products)
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .FirstAsync(u => u.Id == usrId);

            if (usr == null) { return; }
            if (usr.ShoppingCart != null && usr.ShoppingCart.Products.Contains(prod)) { return; }

            if (usr.ShoppingCart == null)
            {
                usr.ShoppingCart = new ShoppingCart();
            }
            usr.ShoppingCart.AddNewProduct(prod);
            await _db.SaveChangesAsync();
        }

        public async Task<ICollection<Product>> getAllFromCart()
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            var cart = await _db.ShoppingCart.Include(s => s.Products).Where(s => s.CustomerId == usrId).FirstAsync();

            if (cart == null) { throw new NullReferenceException("Cart is null!"); }
            if (cart.Products != null)
            {
                return cart.Products;
            }
            else return new List<Product>();
        }
    }
}
