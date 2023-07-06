using System.Text;
using Infrastructure.AutoMapperProfiles;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(conf => conf.UseNpgsql(connection));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAccountService, AccountService>();


builder.Services.AddAutoMapper(typeof(ServiceProfile));
//jwt config
builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
    {
        config.Password.RequiredLength = 4;
        config.Password.RequireDigit = false; // must have at least one digit
        config.Password.RequireNonAlphanumeric = false; // must have at least one non-alphanumeric character
        config.Password.RequireUppercase = false; // must have at least one uppercase character
        config.Password.RequireLowercase = false;  // must have at least one lowercase character
    })
    //for registering usermanager and signinmanger
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,option =>
    {
        option.LoginPath = "/account/login";
        option.AccessDeniedPath = "/account/accessDenied";
        option.Cookie.Name = "AuthCookie";

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.UseEndpoints(endpoints => 
{
    endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
