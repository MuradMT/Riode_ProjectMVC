namespace Riode_ProjectMVC.ViewComponents;
public class BasketViewComponent : ViewComponent
{
	public BasketViewComponent(RiodeContext context)
	{
		Context = context;
	}

	public RiodeContext Context { get; }

	public IViewComponentResult Invoke()
	{

		var username = User.Identity.Name;

		ICollection<UserBasket> basket = new List<UserBasket>();
		if (username != null)
		{
			var user = Context.Users.Where(u => u.UserName == username).FirstOrDefault();
			basket = Context.UserBaskets.Where(ub => ub.AppUserId == user.Id).Include(b => b.Product).ThenInclude(p => p.ProductImages).ToList();

		}
		return View(basket);
	}
}
