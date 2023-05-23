namespace TechHaven.Models
{
    public class DataChangeViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string oldPass { get; set; } = string.Empty;
        public string newPass { get; set; } = string.Empty;
        public string repeatNew { get; set; } = string.Empty;
    }
}
