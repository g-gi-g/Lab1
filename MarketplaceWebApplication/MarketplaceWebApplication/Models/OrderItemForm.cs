﻿namespace MarketplaceWebApplication.Models
{
    public class OrderItemForm
    {
        public int OfferId { get; set; }

        public int OrderId { get; set; }

        public float Price { get; set; }

        public float Quantity { get; set; }

        public int CustomerId { get; set; }

        public int? TransactionId { get; set; }

        public int StatusId { get; set; }

        public DateTime DateOfOrder { get; set; }

        public int PaymentMethodId { get; set; }

        public string? Comment { get; set; }

        public int ShippingCompanyId { get; set; }

        public string ArrivalCountry { get; set; } = null!;

        public string ArrivalCity { get; set; } = null!;

        public string ArrivalStreet { get; set; } = null!;

        public string ArrivalBuildingNumber { get; set; } = null!;

        public string ArrivalZipCode { get; set; } = null!;

        public string DepartmentCountry { get; set; } = null!;

        public string DepartmentCity { get; set; } = null!;

        public string DepartmentStreet { get; set; } = null!;

        public string DepartmentBuildingNumber { get; set; } = null!;

        public string DepartmentZipCode { get; set; } = null!;
    }
}
