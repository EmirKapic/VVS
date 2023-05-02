namespace TechHaven.Models
{
    public class Product
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public int NumberOfAvailable { get; set; }

        public Product(int id, Category category, string manufacturer, string model, double price, int numberOfAvailable)
        {
            this.Id = id;
            this.Category = category;
            this.Manufacturer = manufacturer;
            this.Model = model;
            this.Price = price;
            this.NumberOfAvailable = numberOfAvailable;
        }
    }

    public enum Category
    {
        Phone,
        Laptop,
        PhoneAccessories,
        Tablet
    }

}
