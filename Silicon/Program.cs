using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDefaultIdentity<UserEntity>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = false;
    x.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<DataContext>();

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/signin";
    x.Cookie.HttpOnly = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    x.SlidingExpiration = true;
});

builder.Services.AddAuthentication().AddFacebook(x =>
{
    x.AppId = "771287731254854";
    x.AppSecret = "697f58411da15c9bab554e72c5451088";
    x.Fields.Add("first_name");
    x.Fields.Add("last_name");
});

builder.Services.AddAuthentication().AddGoogle(x =>
{
    x.ClientId = "846569615439-n5h04n47f0hcuklob6g0fknofbmbugkg.apps.googleusercontent.com";
    x.ClientSecret = "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";
    x.CallbackPath = "/signin-google";
});


builder.Services.AddScoped<ResponseResult>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserFactory>();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AccountFactory>();

builder.Services.AddScoped<AddressFactory>();
builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<AddressService>();

builder.Services.AddScoped<SavedCoursesFactory>();
builder.Services.AddScoped<SavedCoursesService>();
builder.Services.AddScoped<SavedCoursesRepository>();

builder.Services.AddScoped<CourseFactory>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<CourseRepository>();


var app = builder.Build();
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStatusCodePagesWithRedirects("/404");
app.Run();
