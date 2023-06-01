using System.ComponentModel.DataAnnotations;

namespace TechHaven.Models
{
    public class CartItemViewModel
    {
        public Product Product { get; set; }

        [Range(1,100, ErrorMessage= "Must be between 1 and 100")]
        public int NumberOfRepetitions { get; set; }
    }
}
