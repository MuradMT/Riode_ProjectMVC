namespace Riode_ProjectMVC.ViewComponents;

public class FilterViewComponent : ViewComponent
{
	private readonly RiodeContext context;

	public FilterViewComponent(RiodeContext context)
	{
		this.context = context;
	}
	public IViewComponentResult Invoke()
	{
		FilterItemsVM itemsVM = new()
		{
			Categories = context.Categories.Include(c => c.Children).ToList(),
			Colors = context.Colors.ToList(),
			MinPrice = (int)context.Products.OrderBy(p => p.SellPrice).FirstOrDefault().SellPrice,
			MaxPrice = (int)context.Products.OrderByDescending(p => p.SellPrice).FirstOrDefault().SellPrice

		};
		return View(itemsVM);
	}

}