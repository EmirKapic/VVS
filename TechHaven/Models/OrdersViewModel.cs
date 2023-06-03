namespace TechHaven.Models
{
    public class OrdersViewModel
    {
        public IList<CartItemViewModel> cartItems { get; set; }
        public Order order { get; set; }
    }
}
