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
    [Authorize(Roles = "Customer, Administrator")]
    public class MyAccountController : Controller
    {
        private IProductManager _productManager;
        private List<Product> _products;
        
        public MyAccountController(IProductManager productManager)
        {
            _productManager = productManager;
            _products = _productManager.GetAllProducts();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Wishlist()
        {
            if (!_products.Any())
            {
                return View(new List<Product>());
            }

            return View(_products);
        }
        
        public IActionResult AddToFavorites(int newProdId)
        {
            var newProd = _products.FirstOrDefault(x => x.Id == newProdId);
            
            if (_products.Contains(newProd))
            { 
                return Json("Product already in favorites!");
            }

            _products.Add(newProd);

            return Json("Sucessfully added to favorites!");
        }
        
        public IActionResult DropFromFavorites(int prodId)
        {
            var prod = _products.FirstOrDefault(x => x.Id == prodId);
            _products.Remove(prod);
            
            return Json("Sucessfully removed from favorites!");
		}
    }
}
