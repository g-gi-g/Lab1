using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication;

public partial class Shipping
{
    public int Id { get; set; }

    public int ShippingCompanyId { get; set; }

    public int OrderId { get; set; }

    public string DepartmentCountry { get; set; } = null!;

    public string DepartmentCity { get; set; } = null!;

    public string DepartmentStreet { get; set; } = null!;

    public string DepartmentBuildingNumber { get; set; } = null!;

    public string DepartmentZipCode { get; set; } = null!;

    public string ArrivalCountry { get; set; } = null!;

    public string ArrivalCity { get; set; } = null!;

    public string ArrivalStreet { get; set; } = null!;

    public string ArrivalBuildingNumber { get; set; } = null!;

    public string ArrivalZipCode { get; set; } = null!;

    public DateTime DateStarted { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ShippingCompany ShippingCompany { get; set; } = null!;
}
