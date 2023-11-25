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
        private SignInManager<Customer> _signInManager;
        private ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartManager(UserManager<Customer> userManager, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, SignInManager<Customer> signInManager)
        {
            _userManager = userManager;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }

        //Adds the product given as parameter to the currently logged-in user
        public async Task<ICollection<Product>> AddToCart(Product prod)
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var usr = await _db.Customer
                .Include(u => u.Products)
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .FirstAsync(u => u.Id == usrId);

            if (usr == null) { new List<Product>(); }
            if (usr.ShoppingCart != null && usr.ShoppingCart.Products.Contains(prod)) { new List<Product>(); }

            if (usr.ShoppingCart == null)
            {
                usr.ShoppingCart = new ShoppingCart();
            }
            usr.ShoppingCart.AddNewProduct(prod);
            await _db.SaveChangesAsync();
            return usr.ShoppingCart.Products;
        }

        public async Task<ICollection<Product>> RemoveFromCart(Product prod)
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var usr = await _db.Customer
                .Include(u => u.Products)
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .FirstAsync(u => u.Id == usrId);
            if (usr == null) { throw new NullReferenceException("Shopping cart is null when trying to remove from it!"); }
            //Ako ne postoji cart kako se desi da se pokusa izvaditi nesto iz njega???? -> exception
            if (usr.ShoppingCart == null) { throw new NullReferenceException("Shopping cart is null when trying to remove from it!"); }
            usr.ShoppingCart.RemoveProduct(prod);
            await _db.SaveChangesAsync();
            return usr.ShoppingCart.Products;
        }

        public async Task<ICollection<Product>> getAllFromCart()
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var usr = await _db.Customer
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .FirstAsync(u => u.Id == usrId);
            if (usr == null) { throw new NullReferenceException("User doesn't exist !"); }
            if (usr.ShoppingCart == null)
            {
                usr.ShoppingCart = new ShoppingCart();
            }
            await _db.SaveChangesAsync();
            return usr.ShoppingCart.Products;
        }
        public async Task<ShoppingCart> GetCurrentCart()
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var usr = await _db.Customer
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .FirstAsync(u => u.Id == usrId);
            if (usr == null) { throw new NullReferenceException("User doesn't exist !"); }
            if (usr.ShoppingCart == null)
            {
                usr.ShoppingCart = new ShoppingCart();
            }
            return usr.ShoppingCart;
        }


        public async Task ClearCart(string usrId)
        {
            var usr = await _db.Customer
               .Include(u => u.ShoppingCart)
               .ThenInclude(c => c.Products)
               .FirstAsync(u => u.Id == usrId);
            if (usr == null) { throw new NullReferenceException("User doesn't exist!"); }

            usr.ShoppingCart.Products.Clear();
            usr.ShoppingCart.TotalPrice = 0;
            usr.ShoppingCart.Repetitions.Clear();
            await _db.SaveChangesAsync();
        }

        
    }
}
