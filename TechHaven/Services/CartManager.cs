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
        public List<Product> AddToCart(Product prod)
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var usr = _db.Customer
                .Include(u => u.Products)
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .First(u => u.Id == usrId);

            if (usr == null) { new List<Product>(); }
            if (usr.ShoppingCart != null && usr.ShoppingCart.Products.Contains(prod)) { new List<Product>(); }

            if (usr.ShoppingCart == null)
            {
                usr.ShoppingCart = new ShoppingCart();
            }
            usr.ShoppingCart.AddNewProduct(prod);
            _db.SaveChanges();
            return usr.ShoppingCart.Products.ToList();
        }

        public List<Product> RemoveFromCart(Product prod)
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var usr = _db.Customer
                .Include(u => u.Products)
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .First(u => u.Id == usrId);
            if (usr == null) { throw new NullReferenceException("Shopping cart is null when trying to remove from it!"); }
            //Ako ne postoji cart kako se desi da se pokusa izvaditi nesto iz njega???? -> exception
            if (usr.ShoppingCart == null) { throw new NullReferenceException("Shopping cart is null when trying to remove from it!"); }
            usr.ShoppingCart.RemoveProduct(prod);
            _db.SaveChanges();
            return usr.ShoppingCart.Products.ToList();
        }

        public List<Product> getAllFromCart()
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var usr = _db.Customer
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .First(u => u.Id == usrId);
            if (usr == null) { throw new NullReferenceException("User doesn't exist !"); }
            if (usr.ShoppingCart == null)
            {
                usr.ShoppingCart = new ShoppingCart();
            }
            _db.SaveChanges();
            return usr.ShoppingCart.Products.ToList();
        }
        public ShoppingCart GetCurrentCart()
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var usr = _db.Customer
                .Include(u => u.ShoppingCart)
                .ThenInclude(c => c.Products)
                .First(u => u.Id == usrId);
            if (usr == null) { throw new NullReferenceException("User doesn't exist !"); }
            if (usr.ShoppingCart == null)
            {
                usr.ShoppingCart = new ShoppingCart();
            }
            return usr.ShoppingCart;
        }


        public void ClearCart(string usrId)
        {
            var usr = _db.Customer
               .Include(u => u.ShoppingCart)
               .ThenInclude(c => c.Products)
               .First(u => u.Id == usrId);
            if (usr == null) { throw new NullReferenceException("User doesn't exist!"); }

            usr.ShoppingCart.Products.Clear();
            usr.ShoppingCart.TotalPrice = 0;
            usr.ShoppingCart.Repetitions.Clear();
            _db.SaveChanges();
        }

        // needed for test and shopping cart controller
        private ShoppingCart _cart;

        public CartManager()
        {
            _cart = new ShoppingCart(); // Initializing cart
        }

        public List<Product> addToCart(Product prod)
        {
            _cart.Products.Add(prod);
            return _cart.Products.ToList();
        }

        public List<Product> GetAllFromCart()
        {
            return _cart.Products.ToList();
        }

        public List<Product> removeFromCart(int productId)
        {
            var productToRemove = _cart.Products.FirstOrDefault(p => p.Id == productId);

            if (productToRemove != null)
            {
                _cart.Products.Remove(productToRemove);
            }

            return _cart.Products.ToList();
        }

        public ShoppingCart getCurrentCart()
        {
            return _cart;
        }
    }
}
