﻿using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
