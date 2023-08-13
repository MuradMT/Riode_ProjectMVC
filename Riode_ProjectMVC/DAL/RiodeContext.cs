using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Riode_ProjectMVC.Models;
using System.Collections.Generic;

namespace Riode_ProjectMVC.DAL;

public class RiodeContext : IdentityDbContext<AppUser>
{
	public RiodeContext(DbContextOptions<RiodeContext> opt) : base(opt)
	{

	}
	public DbSet<Setting> Settings { get; set; }
	public DbSet<Slider> Sliders { get; set; }
	public DbSet<Badge> Badges { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<ProductBadges> ProductBadges { get; set; }
	public DbSet<ProductImages> ProductImages { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Color> Colors { get; set; }
	public DbSet<ProductColors> ProductColors { get; set; }
	public DbSet<ResetPasswordCode> ResetPasswordCodes { get; set; }
	public DbSet<Review> Reviews { get; set; }
	public DbSet<UserBasket> UserBaskets { get; set; }
}
