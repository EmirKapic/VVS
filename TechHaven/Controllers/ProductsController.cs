﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHaven.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ICartManager _cartManager;
        private readonly IProductManager _productManager;
        public ProductsController(ApplicationDbContext db, CartManager cartManager, ProductManager productManager) {
            _db = db;
            _cartManager = cartManager;
            _productManager = productManager;
        }

        public async Task<IActionResult> Index(string category = "Laptop")
        {
            //After filtering / sorting we use this to set showed products instead of getting all of certain category
            //This IF should ONLY be true after method filterProducts of THIS controller. In no other case should this TempData return non-null
            var filtered = TempData["filteredProducts"] as ICollection<Product>;
            if (filtered != null)
            {
                return View(filtered);
            }
            else
            {
                return View(await _productManager.GetAllByCategory(category));
            }
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
        [HttpPost]
        public IActionResult FilterProducts(IFormCollection formCollection, int priceFrom=0, int priceTo=0)
        {
            var selectedCategories = formCollection["selectedCategory"].ToList();
            var selectedManufacturers = formCollection["selectedManufacturer"].ToList();
            var selectedSortType = Request.Form["selectedSort"].ToString();
            SortType type = DecodeSortType(selectedSortType);
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            if (id == 0)
            {
                return Json(new { message = "No object found with given id!" });
            }
            var prod = await _db.Product.FirstOrDefaultAsync(p => p.Id == id);
            await _cartManager.AddToCart(prod);
            return Json(new { message = "Sucessfully added to cart!" });
        }

        private SortType DecodeSortType(string sortType)
        {
            if (sortType == "HighestFirst")
            {
                return SortType.HighestFirst;
            }
            else if(sortType == "LowestFirst"){
                return SortType.LowestFirst;
            }
            else
            {
                return SortType.Alphabetical;
            }
        }
    }
}
