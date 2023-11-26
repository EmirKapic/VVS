using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Authorization;

namespace TechHaven.Services
{
    [Authorize]
    public class OrdersManager : IOrdersManager
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CartManager _cartManager;

        public OrdersManager(ApplicationDbContext db, UserManager<Customer> userManager, IHttpContextAccessor httpContextAccessor, CartManager cartManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _cartManager = cartManager;
        }

        public List<Product> GetProductsFromOrders()
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var user = _db.Customer
                .Include(c => c.Products)
                .Include(c => c.Orders)
                .ThenInclude(o => o.Products)
                .First(c => c.Id == usrId);

            if (user.Orders == null)
            {
                user.Orders = new List<Order>();
            }

            if (user.Orders.Count == 0)
            {
                return new Randomizer(_db.Product.ToList()).GetRandomized().Take(20).ToList();
            }

            IDictionary<string, int> categoryHistogram = new Dictionary<string, int>();
            foreach (var order in user.Orders)
            {
                foreach (var prod in order.Products)
                {
                    if (!categoryHistogram.ContainsKey(prod.Category))
                    {
                        categoryHistogram.Add(prod.Category, 1);
                    }
                    else
                    {
                        categoryHistogram[prod.Category]++;
                    }
                }
            }

            var mostCommonCategory = categoryHistogram.OrderByDescending(pair => pair.Value).FirstOrDefault().Key;

            if (mostCommonCategory == null)
            {
                // Handle the case where no category is found
                return new List<Product>();
            }

            var prods = _db.Product.Where(p => p.Category == mostCommonCategory).ToList();

            return new Randomizer(prods).GetRandomized().Take(20).ToList();
        }

        public void MakeNewOrder(Order order)
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var user = _db.Customer
                .Include(c => c.Products)
                .Include(c => c.Orders)
                .ThenInclude(o => o.Products)
                .First(c => c.Id == usrId);

            if (user.Orders == null)
            {
                user.Orders = new List<Order>();
            }

            Order toBeInserted = new Order();
            foreach (var prod in order.Products)
            {
                toBeInserted.Products.Add(_db.Product.FirstOrDefault(p => p.Id == prod.Id));
            }

            toBeInserted.OrderDate = order.OrderDate;
            toBeInserted.ShippingAddress = order.ShippingAddress;
            toBeInserted.Price = order.Price;

            user.Orders.Add(toBeInserted);
            _cartManager.ClearCart(usrId);
            _db.SaveChanges();
        }


    }
}