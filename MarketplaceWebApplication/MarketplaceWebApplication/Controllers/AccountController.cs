using MarketplaceWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MarketplaceWebApplication.Data.Identity;
using MarketplaceWebApplication.Data;
using MarketplaceWebApplication.Extensions;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;

namespace MarketplaceWebApplication.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<Data.Identity.User> _userManager;
    private readonly SignInManager<Data.Identity.User> _signInManager;

    private readonly DbmarketplaceContext _context;

    public AccountController(UserManager<Data.Identity.User> userManager, 
        SignInManager<Data.Identity.User> signInManager, DbmarketplaceContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
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
            Data.Identity.User user = new Data.Identity.User 
            { 
                Email = model.Email, 
                UserName = model.UserName, 
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

                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                UserDetails userinfo = new UserDetails { Id = currentUser.Id, Username = user.UserName };

                HttpContext.Session.SetObjectAsJson("UserDetails", userinfo);

                Data.User userCopy = new Data.User
                {
                    Id = currentUser.Id,
                    Email = model.Email,
                    Username = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    DateOfRegistration = currentUser.DateOfRegistration,
                    PhoneNumber = model.PhoneNumber,
                };
                _context.Users.Add(userCopy);
                _context.SaveChanges();

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
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                UserDetails userinfo = new UserDetails { Id = currentUser.Id, Username = currentUser.UserName };

                HttpContext.Session.SetObjectAsJson("UserDetails", userinfo);

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

    public async Task<IActionResult> Logout()
    {
        // видаляємо автентифікаційні куки
        await _signInManager.SignOutAsync();

        HttpContext.Session.SetObjectAsJson("UserDetails", null);

        return RedirectToAction("MainPageView", "Offers");
    }

    public async Task<IActionResult> AccountPage()
    {
        var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

        if (userInfo is null)
        {
            return RedirectToAction("NotLoggedView", "Home", null);
        }

        string userId = userInfo.Id;

        Data.Identity.User user = _userManager.Users.FirstOrDefault(user => user.Id == userId);

        return View(user);
    }

    [HttpGet]
    public IActionResult EditUserInfo()
    {
        var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

        if (userInfo is null)
        {
            return RedirectToAction("NotLoggedView", "Home", null);
        }

        string userId = userInfo.Id;

        Data.Identity.User user = _userManager.Users.FirstOrDefault(user => user.Id == userId);

        EditUserViewModel userData = new EditUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateOfBirth = user.DateOfBirth,
            DateOfRegistration = user.DateOfRegistration,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber
        };

        return View(userData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditUserInfo(EditUserViewModel user)
    {
        if (ModelState.IsValid)
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            string userId = userInfo.Id;

            Data.Identity.User user1 = _userManager.Users.FirstOrDefault(user => user.Id == userId);

            user1.FirstName = user.FirstName;
            user1.LastName = user.LastName;
            user1.DateOfBirth = user.DateOfBirth;
            user1.DateOfRegistration = user.DateOfRegistration;
            user1.UserName = user.UserName;
            user1.PhoneNumber = user.PhoneNumber;

            _userManager.UpdateAsync(user1);

            Data.User user2 = _context.Users.FirstOrDefault(user => user.Id == userId);

            user2.FirstName = user.FirstName;
            user2.LastName = user.LastName;
            user2.DateOfBirth = user.DateOfBirth;
            user2.DateOfRegistration = user.DateOfRegistration;
            user2.Username = user.UserName;
            user2.PhoneNumber = user.PhoneNumber;

            _context.Users.Update(user2);
            _context.SaveChanges();

            return RedirectToAction("AccountPage", "Account");
        }
        return View(user);
    }
}
