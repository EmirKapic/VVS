using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TechHaven.Models;
using TechHaven.Services;

public class ShoppingCartController : Controller
{
    private readonly ICartManager _cartManager;

    public ShoppingCartController(ICartManager cartManager)
    {
        _cartManager = cartManager;
    }

    public IActionResult Index()
    {
        try
        {
            var products = _cartManager.GetAllFromCart();
            var items = new List<CartItemViewModel>();

            foreach (var product in products)
            {
                items.Add(new CartItemViewModel { Product = product, NumberOfRepetitions = 1 });
            }

            return View(items);
        }
        catch
        {
            return NotFound();
        }
    }

    public IActionResult RemoveFromCart(int id)
    {
        try
        {
            _cartManager.removeFromCart(id);
            return Json(new { message = "Successfully removed from cart!" });
        }
        catch
        {
            return Json(new { message = "Couldn't remove from cart!" });
        }
    }

}
