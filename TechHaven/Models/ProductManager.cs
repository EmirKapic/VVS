namespace TechHaven.Models
{
    public class ProductManager
    {
        public int ProductManagerId { get; set; }
        public List<Product> AddedProducts { get; set; }

        public ProductManager()
        {
            AddedProducts = new List<Product>();
        }
    }
}
