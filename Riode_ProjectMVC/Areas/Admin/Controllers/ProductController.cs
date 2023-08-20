using Riode_ProjectMVC.ExtensionServices.Interfaces;
using Riode_ProjectMVC.Services.Interfaces;
using Riode_ProjectMVC.Utils.Extensions;
namespace Riode_ProjectMVC.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
	IProductService _service { get; }
	IBadgeService _bservice { get; }
	ICategoryService _cservice { get; }
	IFileService _fservice { get; }
	public ProductController(IProductService service,IBadgeService bservice,ICategoryService cservice,IFileService fservice)
	{
		_service = service;
		_bservice = bservice;

		_cservice = cservice;
		_fservice = fservice;
	}
	public IActionResult Index()
	{
		var products = _service.Products().Include(p => p.ProductImages).Include(p => p.Category).ToList();

		return View(products);
	}
	public IActionResult Create()
	{
		ViewBag.Badges = _bservice.Badges();
		ViewBag.Colors = _service.Colors();
		ViewBag.Categories = _cservice.Categories();
		return View();
	}
	[HttpPost]
	public IActionResult Create(Product product)
	{
		ViewBag.Badges = _bservice.Badges();
        ViewBag.Colors = _service.Colors();
        ViewBag.Categories = _cservice.Categories();
        if (!ModelState.IsValid)
		{
			return View();
		}
		if (product.MainImg is null)
		{
			ModelState.AddModelError("MainImg", "Zəhmət olmasa şəkil seçin");
			return View();
		}
		if (product.SellPrice < product.CostPrice)
		{
			ModelState.AddModelError("SellPrice", "Satış qiyməti maya dəyərindən kiçik ola bilməz!");
		}
		#region Image
		//other images
		var productImgs = product.OtherImgs;
		List<ProductImages> images = new List<ProductImages>();
		if (productImgs != null)
		{
			foreach (var image in productImgs)
			{
				if (!image.IsTypeValid("image/"))
				{
					ModelState.AddModelError("MainImage", "Yüklədiyiniz fayl şəkil deyil");
					return View();
				}
				if (!image.IsSizeValid(2))
				{
					ModelState.AddModelError("MainImage", "Yüklədiyiniz şəkil 2mb-dan artıq olmamalıdır");
					return View();
				}
			}

			foreach (var image in productImgs)
			{
				var imagename=Guid.NewGuid().ToString();
				_fservice.UploadAsync(image, Path.Combine("assets", "images",imagename));
				_fservice.SaveAsync(image, Path.Combine("assets", "images", imagename));
				images.Add(new()
				{
					ImageName = imagename,
					Product = product,
					IsMain = null
				});

			}
		}
		//main image
		var mainimg = product.MainImg;
		if (!mainimg.IsTypeValid("image/"))
		{
			ModelState.AddModelError("MainImage", "Yüklədiyiniz fayl şəkil deyil");
			return View();
		}
		if (!mainimg.IsSizeValid(2))
		{
			ModelState.AddModelError("MainImage", "File is too big");
			return View();
		}
        var mainimgname = Guid.NewGuid().ToString();
        _fservice.UploadAsync(mainimg, Path.Combine("assets", "images", mainimgname));
        _fservice.SaveAsync(mainimg, Path.Combine("assets", "images", mainimgname));
        images.Add(new()
		{
			ImageName = mainimgname,
			IsMain = true,
			Product = product
		});
		// hover image
		var hoverImg = product.HoverImg;
		if (hoverImg != null)
		{
			if (!hoverImg.IsTypeValid("image/"))
			{
				ModelState.AddModelError("MainImage", "Yüklədiyiniz fayl şəkil deyil");
				return View();
			}
			if (!hoverImg.IsSizeValid(2))
			{
				ModelState.AddModelError("MainImage", "File is too big");
				return View();
			}
            var hoverimgname = Guid.NewGuid().ToString();
            _fservice.UploadAsync(hoverImg, Path.Combine("assets", "images", hoverimgname));
            _fservice.SaveAsync(hoverImg, Path.Combine("assets", "images", hoverimgname));
            images.Add(new()
			{
				ImageName = hoverimgname,
				IsMain = false,
				Product = product
			});

		}
		#endregion

		product.ProductImages = images;
		if (product.ColorIds != null)
		{
			product.ProductColors = new List<ProductColors>();
			foreach (var item in product.ColorIds)
			{
				product.ProductColors.Add(new()
				{
					ColorId = item,
					Product = product
				});
			}
		}
		if (product.BadgeIds != null)
		{
			product.ProductBadges = new List<ProductBadges>();
			foreach (var item in product.BadgeIds)
			{
				product.ProductBadges.Add(new()
				{
					BadgeId = item,
					Product = product
				});
			}
		}

		_service.Products().Add(product);
		 _service.SaveChangesAsync();

		return RedirectToAction(nameof(Index));
	}
	public IActionResult Edit(int? id)
	{
		ViewBag.Badges = _bservice.Badges();
		ViewBag.Colors = _service.Colors();
		ViewBag.Categories = _cservice.Categories();
		if (id is null) return BadRequest();
		var prod = _service.Products().Where(p => p.Id == id)
									.Include(p => p.ProductImages)
									.Include(p => p.ProductBadges)
									.ThenInclude(p => p.Badge)
									.Include(p => p.ProductColors)
									.ThenInclude(p => p.Color)
									.SingleOrDefault();

		if (prod is null) return NotFound();
		return View(prod);
	}
	[HttpPost]
	public IActionResult Edit(int? id, Product product)
	{

		if (id is null || id == 0) return BadRequest();

		var edited = _service.Products().Where(p => p.Id == id)
									.Include(p => p.ProductImages)
									.Include(p => p.ProductBadges)
									.ThenInclude(p => p.Badge)
									.Include(p => p.ProductColors)
									.ThenInclude(p => p.Color)
									.SingleOrDefault();
		ViewBag.Badges = _bservice.Badges();
		ViewBag.Colors = _service.Colors();
		ViewBag.Categories = _cservice.Categories();

		if (edited is null) return NotFound();
		edited.Name = product.Name;
		edited.Brand = product.Brand;
		edited.Description = product.Description;
		edited.CategoryId = product.CategoryId;
		edited.CostPrice = product.CostPrice;
		edited.SellPrice = product.SellPrice;
		edited.DiscountPercent = product.DiscountPercent;
		if (product.ColorIds != null)
		{
			List<ProductColors> productColors = new();
			foreach (var colorId in product.ColorIds)
			{
				productColors.Add(new() { ColorId = colorId, ProductId = product.Id });
			}
			edited.ProductColors = productColors;
		}
		else
		{
			edited.ProductColors.Clear();
		}
		if (product.BadgeIds != null)
		{
			List<ProductBadges> productBadges = new();
			foreach (var badgeId in product.BadgeIds)
			{
				productBadges.Add(new() { BadgeId = badgeId, ProductId = product.Id });
			}
			edited.ProductBadges = productBadges;
		}
		else
		{
			edited.ProductBadges.Clear();
		}
		List<IFormFile> newImages = product.OtherImgs;
		List<ProductImages> images = new List<ProductImages>();
		if (newImages != null)
		{
			foreach (var image in newImages)
			{
				if (!image.IsTypeValid("image/"))
				{
					ModelState.AddModelError("OtherImgs", "Yüklədiyiniz fayl şəkil deyil");
					return View(edited);
				}
				if (!image.IsSizeValid(2))
				{
					ModelState.AddModelError("OtherImgs", "Yüklədiyiniz şəkil 2mb-dan artıq olmamalıdır");
					return View(edited);
				}
				foreach (var img in newImages)
				{
                    var imgname = Guid.NewGuid().ToString();
                    _fservice.UploadAsync(img, Path.Combine("assets", "images", imgname));
                    _fservice.SaveAsync(img, Path.Combine("assets", "images", imgname));
                    images.Add(new()
					{
						ImageName = imgname,
						IsMain = null,
						Product = edited,
					});
				}
				edited.ProductImages.AddRange(images);
			}
		}
		_service.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
	public IActionResult SwitchStatus(int? id)
	{
		if (id is null)
		{
			var response = new
			{
				error = true,
				message = "taplmadi"
			};
			return Json(response);
		}
		var product = _service.Products().Find(id);
		if (product is null)
		{
			var response = new
			{
				error = true,
				message = "taplmadi"
			};
			return Json(response);
		}
		if (product.IsDisable == false)
		{
			product.IsDisable = true;
		}
		else
		{
			product.IsDisable = false;
		}
		_service.SaveChangesAsync();
		return Json(new
		{
			error = false,
			message = "ok"
		});
	}
}