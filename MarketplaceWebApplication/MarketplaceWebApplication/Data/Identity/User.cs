using Microsoft.AspNetCore.Identity;

namespace MarketplaceWebApplication.Data.Identity;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public DateTime DateOfRegistration { get; set; }
}
