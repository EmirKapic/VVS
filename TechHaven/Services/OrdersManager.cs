using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    [Authorize]
    public class OrdersManager : IOrdersManager
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersManager(ApplicationDbContext db, UserManager<Customer> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<List<Product>> GetProductsFromOrders()
        {
            throw new NotImplementedException();
        }

        //Order mora biti POTPUNO konfigurisan kada dodje ovdje
        public async Task MakeNewOrder(Order order)
        {
            var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var user = await _db.Customer
                .Include(c => c.Products)
                .Include(c => c.Orders)
                .ThenInclude( o => o.Products)
                .FirstAsync(c => c.Id == usrId);

            if (user.Orders == null)
            {
                user.Orders = new List<Order>();
            }
            //iz nekog razloga 
            Order toBeInserted = new Order();
            foreach(var prod in order.Products)
            {
                toBeInserted.Products.Add(await _db.Product.FirstOrDefaultAsync(p => p.Id == prod.Id));
            }
            toBeInserted.OrderDate = order.OrderDate;
            toBeInserted.ShippingAddress = order.ShippingAddress;
            toBeInserted.Price = order.Price;


            user.Orders.Add(toBeInserted);
            await _db.SaveChangesAsync();
        }
    }
}
