using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication.Data;

public partial class SavedOffer
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public int OfferId { get; set; }

    public DateTime TimeAdded { get; set; }

    public virtual Offer Offer { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
