using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHaven.Controllers
{
    public class OrdersController : Controller
    {
        private OrdersManager _ordersManager;
        private PaymentManager _paymentManager;

        public OrdersController(OrdersManager ordersManager, PaymentManager paymentManager)
        {
            _ordersManager = ordersManager;
            _paymentManager = paymentManager;
        }

        public IActionResult Index()
        {
            var jsonModel = TempData["orderData"];
            if (jsonModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = JsonConvert.DeserializeObject<OrdersViewModel>(jsonModel.ToString());
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ThankYou(OrdersViewModel model, string selectedCard)
        {
            await _paymentManager.AddNewCard(selectedCard);

            model.order.OrderDate = DateTime.Now;
            var prods = new List<Product>();
            if (model.order.Products == null)
            {
                model.order.Products = new List<Product>();
            }
            foreach(var item in model.cartItems)
            {
                model.order.Products.Add(item.Product);
            }
            await _ordersManager.MakeNewOrder(model.order);

            return View(model);
        }
    }

    
}
