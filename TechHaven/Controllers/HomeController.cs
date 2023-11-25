using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHaven.Controllers
{
    public class HomeController : Controller
    {
        private readonly CustomerRecommendation recommendation;

        public HomeController(CustomerRecommendation recommendation)
        {
            this.recommendation = recommendation;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IEnumerable<Product> recommendProduct()
        {
            var products = recommendation.RecommendProducts();
            if (products == null)
            {
                return new List<Product>();
            }
            else
            {
                return products;
            }
        }
    }
}