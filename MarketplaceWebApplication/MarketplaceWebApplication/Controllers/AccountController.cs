using MarketplaceWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MarketplaceWebApplication.Data.Identity;

namespace MarketplaceWebApplication.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new User 
            { 
                Email = model.Email, 
                UserName = model.Email, 
                FirstName = model.FirstName, 
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                DateOfRegistration = DateTime.Now,
                PhoneNumber = model.PhoneNumber,
            };
            // додаємо користувача
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // установка кукі
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("MainPageView", "Offers");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                // перевіряємо, чи належить URL додатку
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("MainPageView", "Offers");
                }
            }
            else
            {
                ModelState.AddModelError("", "Неправильний логін чи (та) пароль");
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        // видаляємо автентифікаційні куки
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Categories");
    }

}
