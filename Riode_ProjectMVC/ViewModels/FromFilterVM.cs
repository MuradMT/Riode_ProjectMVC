﻿namespace Riode_ProjectMVC.ViewModels;
public record FromFilterVM
{
	public int MinPrice { get; set; }
	public int MaxPrice { get; set; }
	public List<int> ColorIds { get; set; }
	public int CategoryId { get; set; }
}
