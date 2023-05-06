using System.ComponentModel.DataAnnotations.Schema;

namespace TechHaven.Models
{
    [NotMapped]
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}