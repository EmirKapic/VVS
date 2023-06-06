using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Controllers
{
    [Authorize(Roles = "Customer, Administrator")]
    public class MyAccountController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<Customer> _userManager;
        private SignInManager<Customer> _signInManager;

        public MyAccountController(ApplicationDbContext db, UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            /*[] imageArray = System.IO.File.ReadAllBytes(@"D:\TechHaven\TechHaven\wwwroot\images\Product Thumbnails\tablets2_img.jpg");
            string base64image = Convert.ToBase64String(imageArray);

            await _db.Image.AddAsync(new Image() { base64Content = base64image, Category = "Tablet" });
            await _db.SaveChangesAsync();*/

            return View();
        }

        public async Task<IActionResult> Order()
        {
            var usrId = _userManager.GetUserId(User);
            var usr = await _db.Customer.Include(u => u.Orders).FirstAsync(u => u.Id == usrId);
            if (usr.Orders != null)
            {
                if (!usr.Orders.Any())
                {
                    return View(new List<Order>());
                }
                return View(usr.Orders);
            }
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
            var usrId = _userManager.GetUserId(User);
            var usr = await _db.Customer.Include(u => u.Products).FirstAsync(u => u.Id == usrId);

            if (usr == null) { return NotFound(); }

            if (usr.Products.Contains(newProd)) { return Json(new { message = "Sucessfully added to favorites!" }); } //Zapravo bi trebao da izadje neki pop-up koji kaze already added to favorites

            usr.Products.Add(newProd);
            await _db.SaveChangesAsync();

            return Json(new { message = "Sucessfully added to favorites!" });
        }

        
        public async Task<IActionResult> DropFromFavorites(int id)
        {
            var usrId = _userManager.GetUserId(User);
            var usr = await _db.Customer.Include(u => u.Products).FirstAsync(u => u.Id == usrId);
            var prod = await _db.Product.FirstOrDefaultAsync(x => x.Id == id);
            if (usr == null || usr.Products == null || prod == null) { return NotFound(); }
            usr.Products.Remove(prod);
            await _db.SaveChangesAsync();
            return Json(new { message = "Sucessfully removed from favorites!" });
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
                if (_db.Customer.Where(c => c.UserName == newData.Email).Any() && (await _userManager.GetUserNameAsync(usr) != newData.Email))
                {
                    ModelState.AddModelError("emptyFields", "Fields cannot be empty");
                    return View(newData);
                }
                usr.FirstName = newData.FirstName;
                usr.LastName = newData.LastName;
                usr.Email = newData.Email;
                usr.UserName = newData.Email;

                await _userManager.UpdateAsync(usr);
                await _db.SaveChangesAsync();
                return Redirect(Request.Headers.Referer);
            }
            else if(change == "password")
            {
                if (newData.oldPass == null || newData.newPass == null || newData.repeatNew == null)
                {
                    ModelState.AddModelError("emptyPassFields", "Polja ne mogu biti prazna");
                    return View(newData);
                }
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
