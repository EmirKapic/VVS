using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHaven.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly UserManager<Customer> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly ICartManager _cartManager;
        public ShoppingCartController(UserManager<Customer> usermanager, ApplicationDbContext db, CartManager cartManager)
        {
            _userManager = usermanager;
            _db = db;
            _cartManager = cartManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                //Mora se castati u List umjesto ICollection jer mi treba indeksiranje u iducem dijelu
                List<Product> prods = (List<Product>)await _cartManager.getAllFromCart();
                List<CartItemViewModel> items = new List<CartItemViewModel>();
                foreach (var prod in prods)
                {
                    items.Add(new CartItemViewModel { Product = prod , NumberOfRepetitions = 1}) ;
                }
                return View(items);
            }
            catch(NullReferenceException)
            {
                return NotFound();
            }
        }
        
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            try
            {
                var prod = await _db.Product.FirstAsync(p => p.Id == id);
                await _cartManager.RemoveFromCart(prod);
                return RedirectToAction("Index");
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
    }
}
