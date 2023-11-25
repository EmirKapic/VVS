using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
		public ProductsController(ApplicationDbContext db, CartManager cartManager, ProductManager productManager)
		{
			_db = db;
			_cartManager = cartManager;
			_productManager = productManager;
		}

        // GET: Products/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Category,Manufacturer,Model,Price,NumberOfAvailable")] Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Product == null)
            {
                return NotFound();
            }

            var product = await _db.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category,Manufacturer,Model,Price,NumberOfAvailable")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(product);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Product == null)
            {
                return NotFound();
            }

            var product = await _db.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
            }
            var product = await _db.Product.FindAsync(id);
            if (product != null)
            {
                _db.Product.Remove(product);
            }
            
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_db.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }

		public async Task<IActionResult> Index(string category = "Laptop", string? searchQuery = null)
		{
            if (User.IsInRole("Administrator"))
            {
				return _db.Product != null ?
						  View(await _db.Product.ToListAsync()) :
						  Problem("Entity set 'ApplicationDbContext.Product'  is null.");
			}

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
			if (prod == null)
			{
				return NotFound();
			}
			return View(prod);
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
			else if (sortType == "LowestFirst")
			{
				return SortType.LowestFirst;
			}
			else
			{
				return SortType.Alphabetical;
			}
		}
	}
}
