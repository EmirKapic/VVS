using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHaven.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly ICartManager _cartManager;
        public ShoppingCartController(UserManager<Customer> usermanager, ApplicationDbContext db, CartManager cartManager, SignInManager<Customer> signInManager)
        {
            _userManager = usermanager;
            _db = db;
            _cartManager = cartManager;
            _signInManager = signInManager;
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
                var res = await _cartManager.RemoveFromCart(prod);
                return Json(new { message = "Sucessfully removed from cart!" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source);
                return Json(new { message = "Couldn't remove from cart!" });
            }
        }
        [Authorize(Roles ="Customer")]
        [HttpPost]
        public IActionResult StartOrder(IList<CartItemViewModel> items)
        {
            //Sta ako korisnik nije ulogovan? Pokusati preko JS-a na frontend ili preko Model-errora server side
            double price = 0;
            List<Product> orderItems = new List<Product>();
            foreach(var item in items)
            {
                price += item.Product.Price;
                orderItems.Add(item.Product);
            }
            var order = new Order()
            {
                CustomerId = _userManager.GetUserId(User),
                Products = orderItems,
                Price = price,
                ShippingAddress = "",
                OrderDate = DateTime.Now
            };

            var orderData = new OrdersViewModel()
            {
                cartItems = items,
                order = order
            };

            TempData["orderData"] = JsonConvert.SerializeObject(orderData);

            return RedirectToAction("Index", "Orders");
        }
    }
}
