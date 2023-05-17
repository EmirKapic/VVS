using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        public ApplicationDbContext _db;
        public UserManager<Customer> _userManager;
        public MyAccountController(ApplicationDbContext db, UserManager<Customer> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }
        
        public IActionResult Wishlist()
        {
            return View();
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddToFavorites([FromQuery] int newProdId)
        {
            var usrId = _userManager.GetUserId(User);
            var newProduct = await _db.Product.FirstOrDefaultAsync(x => x.Id == newProdId);
            var usr = _db.Customer.Include(c => c.Products).FirstOrDefault(c=> c.Id == usrId);
            //Console.WriteLine("\n\n\n\n" + usr.Products[1] + "\n\n\n\n");
            if (newProduct == null || usr == null)
            {
                return NotFound();
            }
            usr.Products.Add(newProduct);

            if (ModelState.IsValid)
            {
                
                await _db.SaveChangesAsync();
                return Redirect(Request.Headers.Referer);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
