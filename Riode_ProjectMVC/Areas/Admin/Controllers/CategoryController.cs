using Riode_ProjectMVC.ExtensionServices.Interfaces;
using Riode_ProjectMVC.Services.Interfaces;
using Riode_ProjectMVC.Utils.Extensions;

namespace Riode_ProjectMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
	ICategoryService _service { get; }
	IFileService _fileService { get; }
	public CategoryController(ICategoryService service,IFileService fileService)
	{
		_service = service;
		_fileService = fileService;
	}
	public IActionResult Index()
	{
		return View(_service.Categories().ToList());
	}
	public IActionResult Create()
	{
		ViewBag.Categories = _service.Categories().ToList();
		return View();
	}

	[HttpPost]
	public IActionResult Create(Category category)
	{
		ViewBag.Categories = _service.Categories().ToList();

		if (category.Name == null)
		{
			ModelState.AddModelError("Name", "Adi daxil edin");
			return View();
		}
		if (category == null) return BadRequest();
		if (category.ImageFile is null)
		{
			ModelState.AddModelError("ImageFile", "Zəhmət olmasa faylı seçin");
			return View();
		}
		var image = category.ImageFile;
		if (!image.IsTypeValid("image/"))
		{
			ModelState.AddModelError("ImageFile", "Choose only image");
			return View();

		}
		if (!image.IsSizeValid(20))
		{
			ModelState.AddModelError("ImageFile", "File too big");
			return View();

		}
        var imagename = Guid.NewGuid().ToString();
        _fileService.UploadAsync(image, Path.Combine("assets", "images", imagename));
        _fileService.SaveAsync(image, Path.Combine("assets", "images", imagename));
        _service.Categories().Add(category);
		_service.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
	public IActionResult Edit(int? id)
	{
		ViewBag.Categories = _service.Categories().ToList();

		if (id is null) return BadRequest();
		Category category = _service.Categories().Where(c => c.Id == id).FirstOrDefault();
		if (category is null) return NotFound();
		return View(category);
	}
	[HttpPost]
	public IActionResult Edit(int? id, Category category)
	{
		ViewBag.Categories = _service.Categories().ToList();

		if (id != category.Id || id is null) return BadRequest();
		Category item = _service.Categories().Find(id);
		if (item is null) return NotFound();
		if (category.ImageFile != null)
		{
			IFormFile file = category.ImageFile;
			if (!file.IsTypeValid("image/"))
			{
				ModelState.AddModelError("ImageFile", "Yüklədiyiniz fayl şəkil deyil");
				return View();
			}
			if (!file.IsSizeValid(20))
			{
				ModelState.AddModelError("ImageFile", "Yüklədiyiniz fayl 2mb-dan artıq olmamalıdır");
				return View();
			}
            var imagename = Guid.NewGuid().ToString();
            _fileService.UploadAsync(file, Path.Combine("assets", "images", imagename));
            _fileService.SaveAsync(file, Path.Combine("assets", "images", imagename));
            item.ImageName = imagename;
		}
		item.Name = category.Name;
		_service.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
	public IActionResult Delete(int? id)
	{
		if (id is null) return BadRequest();
		var category = _service.Categories().Find(id);
		RemoveFile(Path.Combine("Assets", "Images", category.ImageName));
        _service.Categories().Remove(category);
		_service.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
	[HttpPost]
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
		var category = _service.Categories().Find(id);
		if (category is null)
		{
			var response = new
			{
				error = true,
				message = "taplmadi"
			};
			return Json(response);
		}
		if (category.IsDisable == false)
		{
			category.IsDisable = true;
		}
		else
		{
			category.IsDisable = false;
		}
		_service.SaveChangesAsync();
		return Json(new
		{
			error = false,
			message = "ok"
		});

	}
	public void RemoveFile(string path)
	{
		_fileService.Delete(path);
	}

}