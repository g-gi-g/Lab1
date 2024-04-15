using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication.Data;

public partial class Order
{
    public int Id { get; set; }

    public string CustomerId { get; set; }

    public int? TransactionId { get; set; }

    public int StatusId { get; set; }

    public DateTime DateOfOrder { get; set; }

    public int PaymentMethodId { get; set; }

    public string? Comment { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();

    public virtual OrderStatus Status { get; set; } = null!;
}
