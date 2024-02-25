using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication.Data;

public partial class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int OfferId { get; set; }

    public float Price { get; set; }

    public float Quantity { get; set; }

    public virtual Offer Offer { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
