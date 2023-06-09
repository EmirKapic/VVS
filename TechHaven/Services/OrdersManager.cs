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
        private readonly CartManager _cartManager;

        public OrdersManager(ApplicationDbContext db, UserManager<Customer> userManager, IHttpContextAccessor httpContextAccessor, CartManager cartManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _cartManager = cartManager;
        }

        public async Task<List<Product>> GetProductsFromOrders()
        {
			var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
			var user = await _db.Customer
				.Include(c => c.Products)
				.Include(c => c.Orders)
				.ThenInclude(o => o.Products)
				.FirstAsync(c => c.Id == usrId);
			if (user.Orders == null)
			{
				user.Orders = new List<Order>();
			}

            if (user.Orders.Count == 0)
            {
                return new Randomizer(await _db.Product.ToListAsync()).GetRandomized().Take(20).ToList();
            }

            IDictionary<string, int> categoryHistogram = new Dictionary<string, int>();
            foreach(var order in user.Orders)
            {
                foreach(var prod in order.Products)
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
            int highestReps = categoryHistogram.Values.Max();

            var highestRepsPair = categoryHistogram.MaxBy(pair => pair.Value == highestReps);
            var mostCommonCategory = highestRepsPair.Key;

            var prods = await _db.Product.Where(p => p.Category == mostCommonCategory).ToListAsync();

            return new Randomizer(prods).GetRandomized().Take(20).ToList();
		}

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
            //iz nekog razloga se ponekad desi da postoje 2 objekta sa istim kljucem, a nisu isti objekti....
            //Potencijalno zbog serijalizacije/deserijalizacije koja se prije desavala...
            //Zato se ovdje jednostavno trazi novi product zapravo
            //Ovo nije puno manje efikasno iz razloga sto ako je product vec uloadan u aplikaciju 
            //Entity nece ponvo ici u bazu po njega vec ga samo dohvati tako da efikasnost nije narusena van toga sto ima par objekata viska
            Order toBeInserted = new Order();
            foreach(var prod in order.Products)
            {
                toBeInserted.Products.Add(await _db.Product.FirstOrDefaultAsync(p => p.Id == prod.Id));
            }
            toBeInserted.OrderDate = order.OrderDate;
            toBeInserted.ShippingAddress = order.ShippingAddress;
            toBeInserted.Price = order.Price;


            user.Orders.Add(toBeInserted);
            await _cartManager.ClearCart(usrId);
            await _db.SaveChangesAsync();
        }
    }
}
