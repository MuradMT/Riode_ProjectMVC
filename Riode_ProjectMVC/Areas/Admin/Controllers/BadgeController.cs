using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Areas.Admin.Controllers;
[Area("Admin")]
public class BadgeController : Controller
{
	IBadgeService _service { get; }
	public BadgeController(IBadgeService service)
	{
		_service = service;
	}
	public IActionResult Index()
	{
		return View(_service.Badges());
	}
	public IActionResult Create()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> Create(Badge badge)
	{
		if (String.IsNullOrWhiteSpace(badge.Name))
		{
			ModelState.AddModelError("Name", "Adi daxil edin");
			return View();
		}
		_service.Badges().Add(badge);
		_service.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null) return BadRequest();
		var deleted = _service.Badges().Where(b => b.Id == id).FirstOrDefault();
		if (deleted is null) return NotFound();
		_service.Badges().Remove(deleted);
		await _service.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
	public IActionResult Edit(int? id)
	{
		if (id == null) return BadRequest();
		var edited = _service.Badges().Where(b => b.Id == id).FirstOrDefault();
		if (edited is null) return NotFound();
		return View(edited);
	}
	[HttpPost]
	public async Task<IActionResult> Edit(int id, Badge badge)
	{
		Badge edited = _service.Badges().Where(b => b.Id == id).FirstOrDefault();
		if (edited is null) return NotFound();
		if (String.IsNullOrWhiteSpace(badge.Name))
		{
			ModelState.AddModelError("Name", "Adi daxil edin");
			return View();
		}
		edited.Name = badge.Name;
		await _service.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
}