using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication.Data;

public partial class Feedback
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string? Text { get; set; }

    public int OfferId { get; set; }

    public int UserId { get; set; }

    public DateTime TimeAdded { get; set; }

    public virtual Offer Offer { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
