using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }
        public string creditCardNumber { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
