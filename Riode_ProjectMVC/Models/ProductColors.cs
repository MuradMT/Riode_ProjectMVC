﻿namespace Riode_ProjectMVC.Models;
public class ProductColors
{
	public int Id { get; set; }
	public int ProductId { get; set; }
	public Product Product { get; set; }
	public int ColorId { get; set; }
	public Color Color { get; set; }
}
