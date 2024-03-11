using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebApplication.Data;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Додайте логін")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Додайте ім'я")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Додайте фамілію")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Додайте дату народження")]
    //[Range(typeof(DateTime), "1924-01-01", nameof(DateTime.Today), ErrorMessage = "Неправильна дата")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Додайте електронну пошту")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Додайте пароль")]
    public string Password { get; set; } = null!;

    public DateTime DateOfRegistration { get; set; }

    [Required(ErrorMessage = "Додайте номер телефону")]
    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<SavedOffer> SavedOffers { get; set; } = new List<SavedOffer>();
}
