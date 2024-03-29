﻿using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication.Data;

public partial class OfferCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
