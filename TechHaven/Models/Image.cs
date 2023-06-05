using System.ComponentModel.DataAnnotations;

namespace TechHaven.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }

        public string base64Content { get; set; }

    }
}
