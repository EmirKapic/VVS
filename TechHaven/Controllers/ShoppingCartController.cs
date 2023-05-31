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
        private UserManager<Customer> _userManager;
        private ApplicationDbContext _db;
        private ICartManager _cartManager;
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
                return View(await _cartManager.getAllFromCart());
            }
            catch(NullReferenceException)
            {
                return NotFound();
            }
        }   
    }
}
