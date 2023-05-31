using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHaven.Controllers
{
    /*
     * Ovo je samo privremeno rješenje, zapravo ce biti proslijedjena lista Produkata kao parametar metodi Index iz onog kontrolera
     * koji je poziva
     * */
    public class ProductsController : Controller
    {
        private ApplicationDbContext _db;
        private ICartManager _cartManager;
        public ProductsController(ApplicationDbContext db, CartManager cartManager) {
            _db = db;
            _cartManager = cartManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Product.ToListAsync());
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var prod = await _db.Product.FirstOrDefaultAsync(p => p.Id == id);
            if(prod == null)
            {
                return NotFound();
            }
            return View(prod);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var prod = await _db.Product.FirstOrDefaultAsync(p => p.Id == id);
            await _cartManager.AddToCart(prod);
            return RedirectToAction("ProductDetails", new {id = id});
        }
    }
}
