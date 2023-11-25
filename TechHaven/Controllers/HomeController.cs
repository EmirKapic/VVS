using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHaven.Controllers
{
    public class HomeController : Controller
    {
        private readonly CustomerRecommendation recommendation;
        private readonly ApplicationDbContext context;

        public HomeController(CustomerRecommendation recommendation, ApplicationDbContext context)
        {
            this.recommendation = recommendation;
            this.context = context;
        }

        public IEnumerable<Product> recommendProduct()
        {
            var products = recommendation.RecommendProducts();
            if (products == null)
            {
                return new List<Product>();
            }
            else
            {
                return products;
            }
        }

    }
}