﻿using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication;

public partial class Offer
{
    public int Id { get; set; }

    public int SellerId { get; set; }

    public string Name { get; set; } = null!;

    public int? Price { get; set; }

    public string? Description { get; set; }

    public byte[] Photo { get; set; } = null!;

    public int NumberOfOrders { get; set; }

    public int CategoryId { get; set; }

    public DateTime TimeAdded { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsHidden { get; set; }

    public int ItemAmount { get; set; }

    public virtual OfferCategory Category { get; set; } = null!;

    public virtual ICollection<Chat> Chats { get; } = new List<Chat>();

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

    public virtual ICollection<SavedOffer> SavedOffers { get; } = new List<SavedOffer>();

    public virtual User Seller { get; set; } = null!;
}
