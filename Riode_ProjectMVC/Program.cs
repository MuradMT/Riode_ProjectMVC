global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc.ViewEngines;
global using Riode_ProjectMVC.Models.Base;
global using Riode_ProjectMVC.Models;
global using Microsoft.AspNetCore.Mvc;
global using Riode_ProjectMVC.DAL;
global using Riode_ProjectMVC.ViewModels;
global using NuGet.Packaging; 
global using Microsoft.EntityFrameworkCore;
global using Riode_ProjectMVC.Utils.Extensions;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddService();
builder.Services.AddSession();
builder.Services.AddDbContext<RiodeContext>(opt =>
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("Conn"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
	opt.User.RequireUniqueEmail = true;

	opt.Password.RequiredLength = 8;
	opt.Password.RequireNonAlphanumeric = false;
	opt.Password.RequireLowercase = false;
	opt.Password.RequireUppercase = false;

	opt.Lockout.MaxFailedAccessAttempts = 4;
	opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
	opt.Lockout.AllowedForNewUsers = true;


}).AddDefaultTokenProviders().AddEntityFrameworkStores<RiodeContext>();
builder.Services.Configure<IdentityOptions>(opts =>
{
	opts.SignIn.RequireConfirmedEmail = true;
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
app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
	  name: "default",
	  pattern: "{controller=Home}/{action=Index}/{id?}"
	);
app.Run();
