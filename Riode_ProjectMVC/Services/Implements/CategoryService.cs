using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Services.Implements;

public class CategoryService:ICategoryService
{
     RiodeContext _context { get; }
    public CategoryService(RiodeContext context)
    {
        _context = context;
    }

    public DbSet<Category> Categories()
    {
        var category=_context.Categories;
        return category;
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
