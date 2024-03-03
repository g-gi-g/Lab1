namespace MarketplaceWebApplication.Models
{
    public class OrderItemForm
    {
        public int OfferId { get; set; }

        public float Price { get; set; }

        public float Quantity { get; set; }

        public int CustomerId { get; set; }

        public int? TransactionId { get; set; }

        public int StatusId { get; set; }

        public DateTime DateOfOrder { get; set; }

        public int PaymentMethodId { get; set; }

        public string? Comment { get; set; }
    }
}
