using BookApp.Mapping;
using BookApp.Repository;
using BookApp.Services;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Helpers;
using Persistence.Repositories;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServices;
using Service.Abstractions.Interfaces.IServises;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationContextConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddIdentity<AppUser, IdentityRole>(options => { options.SignIn.RequireConfirmedAccount = true;  options.User.RequireUniqueEmail = true; })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));


builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<ISoldService, SoldService>();
builder.Services.AddScoped<IRentService, RentService>();
builder.Services.AddScoped<IUserCartService, UserCartService>();
builder.Services.AddScoped<IPaymentFormService, PaymentFormService>();
builder.Services.AddScoped<ICartService, CartService>();


builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Account/Login";
});



var app = builder.Build();

// Error handling and status code pages
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Global error handling, redirects to /Error
    app.UseStatusCodePagesWithReExecute("/Error/{0}"); // Handles status codes (e.g., 404)
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // For development environment
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



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}" 
    );

app.MapFallbackToController("InvalidUrl", "Error");

app.MapRazorPages();

//Seed Data In DataBase 
await DbInitializer.Seed(app, roleManager, userManager);

app.Run();
