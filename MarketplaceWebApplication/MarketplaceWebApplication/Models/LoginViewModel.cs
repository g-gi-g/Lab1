using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebApplication.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Електронна адреса")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Display(Name = "Запам'ятати?")]
    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }
}
