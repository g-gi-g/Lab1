using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication;

public partial class ShippingCompany
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Shipping> Shippings { get; } = new List<Shipping>();
}
