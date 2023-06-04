using TechHaven.Models;

namespace TechHaven.Services
{
    public interface Analyzer
    {
        public ICollection<Product> products { get; set; }
        public IEnumerable<Product> GetProducts();
        public ICollection<Product> UserHistory { get; set; }
    }
}
