using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Controllers
{
    /*
     * Ovo je samo privremeno rješenje, zapravo ce biti proslijedjena lista Produkata kao parametar metodi Index iz onog kontrolera
     * koji je poziva
     * */
    public class ProductsController : Controller
    {
        public ApplicationDbContext _db;
        public ProductsController(ApplicationDbContext db) {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            //Ovdje treba dodati samo one producte gdje je customerId == null
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
    }
}
