using TechHaven.Models;

namespace TechHaven.Services
{
    public class GuestShoppingCart : ShoppingCart
    {
        public override void AddNewProduct(Product newProduct)
        {
            if (!Products.Contains(newProduct))
            {
                Products.Add(newProduct);
                TotalPrice += newProduct.Price;
            }
        }
        public override void RemoveProduct(Product newProduct)
        {
            foreach (var prod in Products)
            {
                if (prod.Id == newProduct.Id)
                {
                    Products.Remove(prod);
                    TotalPrice -= prod.Price;
                }
            }
        }
    }
}
