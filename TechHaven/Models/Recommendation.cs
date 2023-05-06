using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    [NotMapped]
    public class Recommendation
    {
        public Customer Customer { get; set; }
        public List<Order> Orders { get; set; }

        public Recommendation(Customer customer, List<Order> orders)
        {
            Customer = customer;
            Orders = orders;
        }

        public Recommendation() { }
    }
}
