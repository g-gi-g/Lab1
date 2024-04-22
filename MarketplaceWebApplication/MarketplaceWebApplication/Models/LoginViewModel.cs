using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebApplication.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Ім'я користувача")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Display(Name = "Запам'ятати?")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
