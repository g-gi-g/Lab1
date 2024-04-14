using MarketplaceWebApplication.Data;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebApplication.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Додайте ім'я")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Додайте фамілію")]
        [Display(Name = "Фамілія")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Додайте дату народження")]
        [Display(Name = "Дата народження")]
        [ValidateDOB(ErrorMessage = "Некоректна дата народження")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Додайте електронну пошту")]
        [Display(Name = "Електронна адреса")]
        [RegularExpression(@"[\w._%+=-]+@[\w.-]+\.[a-zA-z]{2,4}", ErrorMessage = "Електронна пошта має бути у форматі X@Y.ZZ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Додайте пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;

        [Required]
        [Display(Name = "Повторіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; } = null!;

        [Required(ErrorMessage = "Додайте номер телефону")]
        [Display(Name = "Номер телефону")]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Номер телефону має бути у форматі +380XXXXXXXXX")]
        public string PhoneNumber { get; set; } = null!;
    }
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
