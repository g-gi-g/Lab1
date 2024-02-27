using MarketplaceWebApplication.Data;

namespace MarketplaceWebApplication.Models
{
    public class OfferModel
    {
        public int Id { get; set; }

        public int SellerId { get; set; }

        public string Name { get; set; } = null!;

        public int? Price { get; set; }

        public string? Description { get; set; }

        public IFormFile Photo { get; set; } = null!;

        public int NumberOfOrders { get; set; }

        public int CategoryId { get; set; }

        public DateTime TimeAdded { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsHidden { get; set; }

        public int ItemAmount { get; set; }
    }
}
