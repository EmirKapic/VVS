using Microsoft.AspNetCore.Mvc;
using TechHaven.Models;

namespace TechHaven.Controllers
{
    public class OrdersController : Controller
    {
        //Ispravno se ovdje prime itemi
        [HttpPost]
        public IActionResult Index(IList<CartItemViewModel> items)
        {
            return View(items);
        }
    }
}
