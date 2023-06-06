using System.ComponentModel.DataAnnotations;

namespace TechHaven.Services
{
    public enum SortType
    {
        [Display(Name="Cheapest First")]
        LowestFirst,
        [Display(Name = "Most Expensive First")]
        HighestFirst,
        [Display(Name = "Alphabetical")]
        Alphabetical
    }
}
