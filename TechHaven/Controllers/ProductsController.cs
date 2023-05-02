using Microsoft.AspNetCore.Mvc;

namespace TechHaven.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
