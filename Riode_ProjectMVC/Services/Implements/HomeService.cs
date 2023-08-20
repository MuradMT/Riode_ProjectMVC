using Riode_ProjectMVC.Services.Implements;
using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Services.Implements;

public class HomeService:IHomeService
{
    RiodeContext _context { get; }
    public HomeService(RiodeContext context)
    {
        _context = context;
    }

    public HomeVM GetAll()
    {
        HomeVM homeVM = new HomeVM();
        homeVM.Sliders = _context.Sliders.Where(s => s.IsDisable == false);
        ICollection<Product> products = _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.ProductBadges)
            .ThenInclude(p => p.Badge).ToList();
        homeVM.Categories = _context.Categories.Where(c => c.IsDisable == false && c.ParentId == null).OrderByDescending(c => c.Products.Count()).Take(4).ToList();
        homeVM.Products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Include(p => p.Reviews)
            .Include(p => p.ProductBadges)
            .ThenInclude(p => p.Badge).ToList();
        return homeVM;
    }
}
