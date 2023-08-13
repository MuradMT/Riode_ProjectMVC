using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Riode_ProjectMVC.DAL;
using Riode_ProjectMVC.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RiodeContext>(opt =>
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("Conn"));
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
app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
	  name: "default",
	  pattern: "{controller=Home}/{action=Index}/{id?}"
	);
app.Run();
