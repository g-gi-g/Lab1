using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication;

public partial class Message
{
    public int Id { get; set; }

    public byte[]? Photo { get; set; }

    public string? Text { get; set; }

    public DateTime TimeAdded { get; set; }

    public int ChatId { get; set; }

    public int SenderId { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
