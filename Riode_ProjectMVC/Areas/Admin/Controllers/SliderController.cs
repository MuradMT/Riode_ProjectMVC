

using Riode_ProjectMVC.ExtensionServices.Interfaces;
using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class SliderController : Controller
{
    ISliderService _service;
    public IHostEnvironment _environment { get; }
    IFileService _fileService;
    public SliderController(ISliderService service, IHostEnvironment environment, IFileService fileService)
    {
        _service = service;
        _environment = environment;
        _fileService = fileService;
    }
    public IActionResult Index()
    {

        return View(_service.Get().ToList());
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Slider slider)
    {
        if (slider.Title == null)
        {
            ModelState.AddModelError("Name", "Title daxil edin");
            return View();
        }
        if (slider == null) return BadRequest();
        if (slider.ImageFile is null)
        {
            ModelState.AddModelError("ImageFile", "Zəhmət olmasa faylı seçin");
            return View();
        }
        var image = slider.ImageFile;
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
        _fileService.SaveAsync(image, Path.Combine(_environment.ContentRootPath, "assets", "images", imagename));
        await _service.Get().AddAsync(slider);
        await _service.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Edit(int? id)
    {
        if (id is null) return BadRequest();
        Slider slider = _service.Get().Where(c => c.Id == id).FirstOrDefault();
        if (slider is null) return NotFound();
        return View(slider);
    }
    [HttpPost]
    public IActionResult Edit(int id, Slider slider)
    {
        Slider item = _service.Get().Find(id);
        if (item is null) return NotFound();
        if (slider.ImageFile != null)
        {
            IFormFile file = slider.ImageFile;
            if (!file.IsTypeValid("image/"))
            {
                ModelState.AddModelError("ImageFile", "Yüklədiyiniz fayl şəkil deyil");
                return View();
            }
            if (file.IsSizeValid(20))
            {
                ModelState.AddModelError("ImageFile", "Yüklədiyiniz fayl 2mb-dan artıq olmamalıdır");
                return View();
            }
            var imagename = Guid.NewGuid().ToString();
            _fileService.UploadAsync(file, Path.Combine("assets", "images", imagename));
            _fileService.SaveAsync(file, Path.Combine(_environment.ContentRootPath, "assets", "images", imagename));
            item.ImageName = imagename;
        }
        item.Title = slider.Title;
        item.SubTitle = slider.SubTitle;
        item.IsLeftSide = slider.IsLeftSide;
        _service.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Delete(int? id)
    {
        if (id is null) return BadRequest();
        var slider = _service.Get().Find(id);
        RemoveFile(Path.Combine("Assets", "Images", slider.ImageName));
        _service.Get().Remove(slider);
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
        var slider = _service.Get().Find(id);
        if (slider is null)
        {
            var response = new
            {
                error = true,
                message = "taplmadi"
            };
            return Json(response);
        }
        if (slider.IsDisable == false)
        {
            slider.IsDisable = true;
        }
        else
        {
            slider.IsDisable = false;
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