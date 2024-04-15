using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebApplication.Models
{
    public class OrderItemForm
    {
        public int OfferId { get; set; }

        public int OrderId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Ціна має бути невід'ємна")]
        [Required(ErrorMessage = "Додайте ціну")]
        public float Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Кількість товару має бути додатня та ціла")]
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

        [Required(ErrorMessage = "Додайте номер дому призначення")]
        public string ArrivalBuildingNumber { get; set; } = null!;

        [Required(ErrorMessage = "Додайте поштовий код відділення призначення")]
        public string ArrivalZipCode { get; set; } = null!;

        [Required(ErrorMessage = "Додайте країну відправлення")]
        public string DepartmentCountry { get; set; } = null!;

        [Required(ErrorMessage = "Додайте місто відправлення")]
        public string DepartmentCity { get; set; } = null!;

        [Required(ErrorMessage = "Додайте вулицю відправлення")]
        public string DepartmentStreet { get; set; } = null!;

        [Required(ErrorMessage = "Додайте номер дому відправлення")]
        public string DepartmentBuildingNumber { get; set; } = null!;

        [Required(ErrorMessage = "Додайте поштовий код відділення відправлення")]
        public string DepartmentZipCode { get; set; } = null!;
    }
}
