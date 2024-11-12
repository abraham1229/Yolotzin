using Microsoft.EntityFrameworkCore;
using OrtizAbrahamSprint3.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);

//Add services for the container

//Add authentication cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/SignIn"; //Path to log in page to direct if not logged in
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); //Expiration time
    options.SlidingExpiration = true; //Each time authentication request, resets the time
});

//Add conection to the database
builder.Services.AddDbContext<MyApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connect string not found")));

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
