using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MarketplaceWebApplication.Data;
using MarketplaceWebApplication.Data.Identity;
using MarketplaceWebApplication.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbmarketplaceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(DbmarketplaceContext))));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IdentityContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("IdentityConnection")
    ));
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<MarketplaceWebApplication.Data.Identity.User, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<MarketplaceWebApplication.Data.Identity.User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database." + DateTime.Now.ToString());
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication(); //підключення автентифікації

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Offers}/{action=MainPageView}/{id?}");
    pattern: "{controller=Offers}/{action=MainPageView}/{id?}");

app.Run();
