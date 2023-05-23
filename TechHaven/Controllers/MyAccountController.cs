using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Controllers
{
    [Authorize(Roles = "Customer, Administrator")]
    public class MyAccountController : Controller
    {
        public ApplicationDbContext _db;
        public UserManager<Customer> _userManager;
        public SignInManager<Customer> _signInManager;

        public MyAccountController(ApplicationDbContext db, UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }
        
        public async Task<IActionResult> Wishlist()
        {
            var usrId = _userManager.GetUserId(User);
            var usr = await _db.Customer.Include(u => u.Products).FirstAsync(u => u.Id == usrId);
            if (usr.Products != null)
            {
                if (!usr.Products.Any())
                {
                    return View(new List<Product>());
                }
                return View(usr.Products);
            }
            return NotFound();
        }

        
        public async Task<IActionResult> AddToFavorites([FromQuery] int newProdId)
        {
            var newProd = await _db.Product.FirstOrDefaultAsync(x => x.Id == newProdId);
            var usr = await _userManager.GetUserAsync(User);

            var newProduct = new Product();
            //Potrebno je kopirati na ovaj nacin da ne bi oba bili referenca na jedan objekat, zbog cega bi nastala greska u bazi (Zapravo bi bila one-to-one veza)
            //Ako je i dalje nejasno pitati Emira
            if (newProd != null && usr != null)
            {
                newProduct = new Product{
                    Category = newProd.Category,
                    Manufacturer = newProd.Manufacturer,
                    Model = newProd.Model,
                    Price = newProd.Price,
                    NumberOfAvailable = newProd.NumberOfAvailable,
                    CustomerId = usr.Id,
                    Customer = usr
                };
            }
            else
            {
                return NotFound();
            }
            usr.Products.Add(newProduct);
            await _db.SaveChangesAsync();
            return Redirect(Request.Headers.Referer);
        }

        
        public async Task<IActionResult> DropFromFavorites(int id)
        {
            var usr = await _userManager.GetUserAsync(User);
            var prod = await _db.Product.FirstOrDefaultAsync(x => x.Id == id);
            if (usr == null || usr.Products == null || prod == null) { return NotFound(); }
            usr.Products.Remove(prod);
            _db.Product.Remove(prod);
            await _db.SaveChangesAsync();
            return Redirect(Request.Headers.Referer);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Index(DataChangeViewModel newData, string change)
        {
            if (change == "data")
            {
                var usr = await _userManager.GetUserAsync(User);
                usr.FirstName = newData.FirstName;
                usr.LastName = newData.LastName;
                usr.Email = newData.Email;

                await _userManager.UpdateAsync(usr);
                _db.SaveChanges();
                return Redirect(Request.Headers.Referer);
            }
            else if(change == "password")
            {
                var usr = await _userManager.GetUserAsync(User);
                if (!await _userManager.CheckPasswordAsync(usr, newData.oldPass))
                {
                    ModelState.AddModelError("oldPass", "Invalid password");
                }
                if (newData.newPass != newData.repeatNew)
                {
                    ModelState.AddModelError("repeatNew", "Passwords must match!");
                }
                if (!ModelState.IsValid)
                {
                    return View(newData);
                }
                await _userManager.ChangePasswordAsync(usr, newData.oldPass, newData.newPass);
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
