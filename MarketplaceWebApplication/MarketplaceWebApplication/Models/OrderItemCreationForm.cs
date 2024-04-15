using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebApplication.Models
{
    public class OrderItemCreationForm
    {
        public int OfferId { get; set; }

        public int OrderId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Ціна має бути невід'ємна")]
        [Required(ErrorMessage = "Додайте ціну")]
        public float Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Кількість має бути невід'ємна")]
        [Required(ErrorMessage = "Додайте кількість")]
        public float Quantity { get; set; }

        public string CustomerId { get; set; }

        public int? TransactionId { get; set; }

        public int StatusId { get; set; }

        public DateTime DateOfOrder { get; set; }

        public int PaymentMethodId { get; set; }

        public string? Comment { get; set; }

        public int ShippingCompanyId { get; set; }

        [Required(ErrorMessage = "Додайте країну призначення")]
        public string ArrivalCountry { get; set; } = null!;

        [Required(ErrorMessage = "Додайте місто призначення")]
        public string ArrivalCity { get; set; } = null!;

        [Required(ErrorMessage = "Додайте вулицю призначення")]
        public string ArrivalStreet { get; set; } = null!;

        [Required(ErrorMessage = "Додайте номер призначення")]
        public string ArrivalBuildingNumber { get; set; } = null!;

        [Required(ErrorMessage = "Додайте поштовий код")]
        public string ArrivalZipCode { get; set; } = null!;
    }
}
