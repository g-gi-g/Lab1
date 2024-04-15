using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication.Data;

public partial class Notification
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Text { get; set; } = null!;

    public bool IsWatched { get; set; }

    public int ClassId { get; set; }

    public DateTime TimeAdded { get; set; }

    public virtual NotificationType Class { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
