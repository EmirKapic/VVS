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

        private bool ProductExists(int id)
        {
          return (_db.Product?.Any(e => e.Id == id)).GetValueOrDefault();
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
