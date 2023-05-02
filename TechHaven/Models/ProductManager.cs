namespace TechHaven.Models
{
    public class ProductManager
    {
        public List<Product> AddedProducts { get; set; }

        public ProductManager()
        {
            AddedProducts = new List<Product>();
        }
    }
}
