using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication;

public partial class Chat
{
    public int Id { get; set; }

    public int OfferId { get; set; }

    public DateTime TimeCreated { get; set; }

    public virtual ICollection<Message> Messages { get; } = new List<Message>();

    public virtual Offer Offer { get; set; } = null!;
}
