using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using TechHaven.Models;

namespace TechHaven.Services
{
    public interface IOrdersManager
    {
        public Task MakeNewOrder(Order order);

        public Task<List<Product>> GetProductsFromOrders();
    }
}
