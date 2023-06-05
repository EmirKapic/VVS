using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
    public class ImageFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Dictionary<string, Image> images = new Dictionary<string, Image>();

        public ImageFactory(IServiceScopeFactory scopeFactory) 
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<Image> GetImage(string category = "Laptop")
        {
            if (images.ContainsKey(category))
            {
                return images[category];
            }
            using(var scope = _scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                Image newImage = await _db.Image.Where(i => i.Category == category).FirstAsync();
                images[category] = newImage;
                return newImage;
            }
        }
    }
}
