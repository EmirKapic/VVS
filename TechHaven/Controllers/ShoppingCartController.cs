using Microsoft.AspNetCore.Mvc;

namespace TechHaven.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
