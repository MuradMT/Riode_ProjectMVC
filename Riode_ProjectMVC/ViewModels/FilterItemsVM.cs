namespace Riode_ProjectMVC.ViewModels;
public record FilterItemsVM
{
	public ICollection<Category> Categories { get; set; }
	public ICollection<Color> Colors { get; set; }
	public int MinPrice { get; set; }
	public int MaxPrice { get; set; }
}
