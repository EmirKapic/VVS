using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private readonly FilterMediator _filterMediator;
        public ProductsController(ApplicationDbContext db, CartManager cartManager, ProductManager productManager, FilterMediator filterMediator = null)
        {
            _db = db;
            _cartManager = cartManager;
            _productManager = productManager;
            _filterMediator = filterMediator;
        }

        public async Task<IActionResult> Index(string category = "Laptop", string? searchQuery = null)
        {
            if (searchQuery != null && searchQuery.Any())
            {
                return View(await _productManager.GetProductsContainingString(searchQuery));
            }

            //After filtering / sorting we use this to set showed products instead of getting all of certain category
            //This IF should ONLY be true after method filterProducts of THIS controller. In no other case should this TempData return non-null
            var filteredIds = TempData["filteredProductIds"];


            if (filteredIds != null)
            {
                var ids = JsonConvert.DeserializeObject<List<int>>(filteredIds.ToString());
                return View(await _productManager.GetProductsFromIds(ids));
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
        public async Task<IActionResult> FilterProducts(List<int> idList, IFormCollection formCollection, int priceFrom=0, int priceTo=0)
        {
            var selectedCategories = formCollection["selectedCategory"].ToList();
            var selectedManufacturers = formCollection["selectedManufacturer"].ToList();
            var selectedSortType = Request.Form["selectedSort"].ToString();
            SortType type = DecodeSortType(selectedSortType);

            var currentProducts = await _productManager.GetProductsFromIds(idList);

            _filterMediator.SetConditions(currentProducts, priceFrom, priceTo, selectedCategories, selectedManufacturers, type);
            var newList = _filterMediator.GetFilteredProducts();
            var newIdList = new List<int>();
            newIdList.AddRange(newList.Select(p => p.Id));

            TempData["filteredProductIds"] = JsonConvert.SerializeObject(newIdList);
            
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
