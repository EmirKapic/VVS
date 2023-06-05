using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Customer>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//Register personal services here

builder.Services.AddScoped<CartManager>();
builder.Services.AddScoped<ProductManager>();
builder.Services.AddScoped<CustomerRecommendation>();
builder.Services.AddScoped<GuestRecommendation>();
builder.Services.AddScoped<OrdersManager>();
builder.Services.AddSingleton<GuestShoppingCart>();
builder.Services.AddScoped<FilterMediator>();
builder.Services.AddScoped<FilterBuilder>();
builder.Services.AddSingleton<ImageFactory>();

//End of personal services

builder.Services.Configure<IdentityOptions>(options =>
{
    //Password options
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 4;
    options.Password.RequireLowercase = false;

    //User options
    options.User.RequireUniqueEmail = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); //return this to controller home when work is finished
app.MapRazorPages();

app.Run();
