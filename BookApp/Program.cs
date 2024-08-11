using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Helpers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationContextConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()  // Enable role management
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  // Ensure this is added
app.UseAuthorization();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using var scope = scopeFactory.CreateScope();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

DbInitializer.Seed(app, roleManager, userManager);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
