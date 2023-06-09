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
        private GuestShoppingCart _guestCart;
        public CartManager(UserManager<Customer> userManager, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, SignInManager<Customer> signInManager, GuestShoppingCart guestCart)
        {
            _userManager = userManager;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _guestCart = guestCart;
        }

        //Adds the product given as parameter to the currently logged-in user
        public async Task<ICollection<Product>> AddToCart(Product prod)
        {
            if (!_signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User))
            {
                _guestCart.AddNewProduct(prod);
                return _guestCart.Products;
            }
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
            if (!_signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User))
            {
                _guestCart.RemoveProduct(prod);
                return _guestCart.Products;
            }
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
            if (!_signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User))
            {
                return _guestCart.Products;
            }
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
            if (!_signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User))
            {
                return _guestCart;
            }
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

        public async Task TransferCarts(string usrId)
        {
            var usr = await _db.Customer
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .FirstAsync(u => u.Id == usrId);
            if (usr == null) { throw new NullReferenceException("User doesn't exist!"); }

            if (_guestCart.Products.Any())
            {
                if (usr.ShoppingCart == null)
                {
                    usr.ShoppingCart = new ShoppingCart();
                }
                foreach(var item in _guestCart.Products)
                {
                    usr.ShoppingCart.AddNewProduct(await _db.Product.FirstAsync(p => p.Id == item.Id));
                }
            }

            _guestCart.Products.Clear();
            _guestCart.TotalPrice = 0;
            _guestCart.Repetitions.Clear();
            await _db.SaveChangesAsync();
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
