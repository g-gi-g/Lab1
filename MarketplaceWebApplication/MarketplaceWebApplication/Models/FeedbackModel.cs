using MarketplaceWebApplication.Data;

namespace MarketplaceWebApplication.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string? Text { get; set; }

        public int OfferId { get; set; }

        public int UserId { get; set; }

        public DateTime TimeAdded { get; set; }
    }
}
