using MarketplaceWebApplication.Data;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebApplication.Models
{
    public class OfferModel : Entity
    {
        public int Id { get; set; }

        public string SellerId { get; set; }

        [Required(ErrorMessage = "Додайте назву")]
        public string Name { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Ціна має бути невід'ємна")]
        public int? Price { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Додайте фото")]
        public IFormFile Photo { get; set; } = null!;

        public int NumberOfOrders { get; set; }

        public int CategoryId { get; set; }

        public DateTime TimeAdded { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsHidden { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Кількість товару має бути більше 0 та ціле")]
        [Required(ErrorMessage = "Введіть кількість товару")]
        public int ItemAmount { get; set; }
    }
}
