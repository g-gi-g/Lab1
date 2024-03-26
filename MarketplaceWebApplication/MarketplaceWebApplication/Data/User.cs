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
    [ValidateDOB(ErrorMessage = "Некоректна дата народження")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Додайте електронну пошту")]
    [RegularExpression(@"[\w._%+=-]+@[\w.-]+\.[a-zA-z]{2,4}", ErrorMessage = "Електронна пошта має бути у форматі X@Y.ZZ")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Додайте пароль")]
    public string Password { get; set; } = null!;

    public DateTime DateOfRegistration { get; set; }

    [Required(ErrorMessage = "Додайте номер телефону")]
    [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Номер телефону має бути у форматі +380XXXXXXXXX")]
    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<SavedOffer> SavedOffers { get; set; } = new List<SavedOffer>();
}

public class ValidateDOBAttribute : ValidationAttribute
{

    private int lowestYear = DateTime.Now.Year - 110;

    private int highestYear = DateTime.Now.Year - 14;

    public ValidateDOBAttribute()
    {
        const string defaultErrorMessage = "Помилкова дата";
        ErrorMessage ??= defaultErrorMessage;
    }

    protected override ValidationResult? IsValid(object? value,
                                         ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult("Вставте дату народження");
        }

        DateOnly dateValue;
        if (!DateOnly.TryParse(value.ToString(), out dateValue))
        {
            return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName));
        }

        if (dateValue.Year < lowestYear)
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName));
        }

        else if (dateValue.Year >= highestYear)
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
}