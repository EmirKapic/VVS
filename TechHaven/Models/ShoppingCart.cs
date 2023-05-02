namespace TechHaven.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        private List<Product> products = new List<Product>();
        private double totalPrice;

        public void AddProduct(Product product)
        {
            products.Add(product);
            totalPrice += product.Price;
        }

        public void RemoveProduct(Product product)
        {
            products.Remove(product);
            totalPrice -= product.Price;
        }

        public IReadOnlyList<Product> GetProducts()
        {
            return products.AsReadOnly();
        }

        public double GetTotalPrice()
        {
            return totalPrice;
        }
    }
}

