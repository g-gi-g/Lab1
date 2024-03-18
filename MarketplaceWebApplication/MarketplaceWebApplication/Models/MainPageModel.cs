using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebApplication.Models
{
    public class MainPageModel
    {
        public List<SelectListItem> drpCategories { get; set; }

        [Display(Name = "Categories")]
        public int[] CategoriesIds { get; set; }

        public string? SearchWord { get; set; }
    }
}
