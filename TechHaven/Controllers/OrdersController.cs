using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHaven.Controllers
{
    public class OrdersController : Controller
    {
        public Order GetOrderById(int orderId)
        {
            var order = new Order();
            order.Id = orderId;
            return order;
        }
    }
}
