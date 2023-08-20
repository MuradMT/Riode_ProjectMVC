using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Controllers;
public class HomeController : Controller
{
	IHomeService _service { get; }
	public HomeController(IHomeService service)
	{
		_service = service;
	}
	public IActionResult Index()
	{
		
		return View(_service.GetAll());
	}
}