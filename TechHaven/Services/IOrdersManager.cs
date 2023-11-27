using Microsoft.AspNetCore.Authorization;
using TechHaven.Models;
using System.Collections.Generic;

namespace TechHaven.Services
{
    public interface IOrdersManager
    {
        List<Product> GetProductsFromOrders();
    }
}
