using System;
using System.Collections.Generic;

namespace MarketplaceWebApplication;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime DateOfRegistration { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<Message> Messages { get; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<Offer> Offers { get; } = new List<Offer>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<SavedOffer> SavedOffers { get; } = new List<SavedOffer>();
}
