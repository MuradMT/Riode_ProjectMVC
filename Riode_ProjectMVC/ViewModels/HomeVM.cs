namespace Riode_ProjectMVC.ViewModels;
public record HomeVM
{
	public IEnumerable<Slider> Sliders { get; set; }
	public ICollection<Category> Categories { get; set; }
	public ICollection<Product> Products { get; set; }
}
