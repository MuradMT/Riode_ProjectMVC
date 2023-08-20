using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Controllers;
public class ProductController : Controller
{
	IProductService _service { get; }

	public ProductController(IProductService service)
	{
		_service = service;

	}
	public async Task<IActionResult> Detail(int? id)
	{
		return View(_service.Detail(id));
	}
	public IActionResult Shop()
	{
		return View(_service.Shop());
	}

	[HttpPost]

	public async Task<IActionResult> FilterProduct([FromForm] FromFilterVM filter)
	{
		var products= _service.FilterProduct(filter);
		if (filter.CategoryId != 0)
		{
			products = products.Where(p => p.CategoryId == filter.CategoryId || p.Category.ParentId == filter.CategoryId);
		}
		if (filter.ColorIds != null)
		{
			var productIds =  _service.ProductIds(filter);
			products = products.Where(p => productIds.Contains(p.Id));
		}
		int min = filter.MinPrice;
		int max = filter.MaxPrice;

		if (min <= max && min >= 0)
		{
			products = products.Where(p =>Math.Round( (p.SellPrice * (100 - p.DiscountPercent) / 100) )>= min && Math.Round((p.SellPrice * (100 - p.DiscountPercent) / 100)) <= max);
		}
		ICollection<Product> sendprods = await products.Take(12).ToListAsync();
		return PartialView("_ProductsPartialView", sendprods);

	}

	[HttpPost]
	public async Task<IActionResult> AddReview(Review review)
	{
		if (!User.Identity.IsAuthenticated)
		{
			return RedirectToAction("Login", "Account");
		}
		if (review.Content == null)
		{
			ModelState.AddModelError("", "Please write smth");
		}
		if (!(review.Raiting > 0 && review.Raiting < 5))
		{
			ModelState.AddModelError("", "Choose your rate");
		}
		var user = await _service.Users().FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
		if (user == null) return NotFound();
		var prod = await _service.Products().FirstOrDefaultAsync(p => p.Id == review.ProductId);
		if (prod == null) return NotFound();
		review.Product = prod;
		review.AppUser = user;
		review.CreatedDate = DateTime.UtcNow.AddHours(4);
		_service.Reviews().Add(review);
		await _service.SaveChangesAsync();
		ICollection<Review> comments = await _service.Reviews().Where(r => r.ProductId == prod.Id).Include(r => r.AppUser).ToListAsync();

		return PartialView("_ProductCommentPartialView", comments);
	}
	public async Task<IActionResult> AddBasket(int? id)
	{
		if (id is null) return BadRequest();
		var prod = _service.Products().Include(p => p.ProductImages).FirstOrDefault(p => p.Id == id);
		if (prod is null) return NotFound();
		var userName = User.Identity.Name;
		var user = await _service.Users().FirstOrDefaultAsync(u => u.UserName == userName);
		ICollection<UserBasket> existbasket = _service.Baskets().Where(b => b.AppUserId == user.Id).ToList();
		if (existbasket.Count() != 0)
		{
			if (!existbasket.Any(b => b.ProductId == prod.Id))
			{
				UserBasket basketitem1 = new() { AppUser = user, Product = prod, Count = 1 };

				_service.Baskets().Add(basketitem1);
			}
			else
			{
				var item = existbasket.Where(b => b.ProductId == prod.Id).FirstOrDefault();
				item.Count++;
			}
            await _service.SaveChangesAsync();
            return ViewComponent("Basket");
		}
		UserBasket basketitem = new() { AppUser = user, Product = prod, Count = 1 };
		await _service.Baskets().AddAsync(basketitem);
		await _service.SaveChangesAsync();
		ICollection<UserBasket> basket = _service.Baskets().Where(ub => ub.AppUserId == user.Id).ToList();
		return ViewComponent("Basket");
	}

	public async Task<IActionResult> RemoveBasket(int? id)
	{
		if (id is null) return BadRequest();
		var prod = _service.Products().Include(p => p.ProductImages).FirstOrDefault(p => p.Id == id);
		if (prod is null) return NotFound();
		var userName = User.Identity.Name;
		var user = await _service.Users().FirstOrDefaultAsync(u => u.UserName == userName);
		ICollection<UserBasket> existbasket = _service.Baskets().Where(b => b.AppUserId == user.Id).ToList();
		if (!existbasket.Any()) return NotFound();
		var removeitem = existbasket.Where(b => b.ProductId == prod.Id).ToList();
		foreach (var item in removeitem)
		{
			_service.Baskets().Remove(item);
		}
        await _service.SaveChangesAsync();
        return ViewComponent("Basket");

	}
}