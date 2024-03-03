using MarketplaceWebApplication.Data;

namespace MarketplaceWebApplication.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int? TransactionId { get; set; }

        public int StatusId { get; set; }

        public DateTime DateOfOrder { get; set; }

        public int PaymentMethodId { get; set; }

        public string? Comment { get; set; }
    }
}
