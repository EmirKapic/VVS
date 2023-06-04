using TechHaven.Models;

namespace TechHaven.Services
{
    public class GuestShoppingCart : ShoppingCart
    {
        public override void AddNewProduct(Product newProduct)
        {
            foreach(var prod in Products)
            {
                if (prod.Id == newProduct.Id)
                {
                    return;
                }
            }
            Products.Add(newProduct);
            TotalPrice += newProduct.Price;
        }
        public override void RemoveProduct(Product newProduct)
        {
            var prods = Products.ToList();
            for (int i = 0; i <  Products.Count; i++)
            {
                if (prods[i].Id == newProduct.Id)
                {
                    Products.Remove(prods[i]);
                    TotalPrice -= prods[i].Price;
                }
            }
        }
    }
}
