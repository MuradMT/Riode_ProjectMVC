using Microsoft.AspNetCore.Mvc.ViewEngines;
using Riode_ProjectMVC.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riode_ProjectMVC.Models;

public class Product:BaseEntity
{
	public string Name { get; set; }
	public string Brand { get; set; }
	public string Description { get; set; }
	public double CostPrice { get; set; }
	public double SellPrice { get; set; }
	public int DiscountPercent { get; set; } = 0;
	public int StockCount { get; set; }
	public int? CategoryId { get; set; }
	public Category Category { get; set; }
	public ICollection<Review> Reviews { get; set; }
	public ICollection<ProductBadges> ProductBadges { get; set; }
	public ICollection<ProductColors> ProductColors { get; set; }
	public ICollection<ProductImages> ProductImages { get; set; }
	[NotMapped]
	public IFormFile MainImg { get; set; }
	[NotMapped]
	public IFormFile HoverImg { get; set; }
	[NotMapped]
	public List<IFormFile> OtherImgs { get; set; }
	[NotMapped]
	public List<int> ColorIds { get; set; }
	[NotMapped]
	public List<int> BadgeIds { get; set; }


}
