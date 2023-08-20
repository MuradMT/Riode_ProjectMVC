using Riode_ProjectMVC.Services.Implements;
using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Services.Implements;

public class ProductService:IProductService
{
    RiodeContext _context { get; }
    UserManager<AppUser> UserManager { get; }

    public ProductService(RiodeContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        UserManager = userManager;

    }

    public Product Detail(int? id)
    {
        Product prod= _context.Products.Where(p => p.Id == id)
             .Include(p => p.Reviews)
             .ThenInclude(r => r.AppUser)
             .Include(p => p.ProductImages)
             .Include(p => p.ProductColors)
             .ThenInclude(pc => pc.Color)
             .Include(p => p.ProductBadges)
             .ThenInclude(pb => pb.Badge)
             .SingleOrDefault();
        return prod;
    }

    public ICollection<Product> Shop()
    {
        ICollection<Product> products =  _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.ProductBadges)
            .ThenInclude(pb => pb.Badge).Take(12).ToList();
        return products;
    }

    public IQueryable<Product> FilterProduct(FromFilterVM filter)
    {
        IQueryable<Product> products =  _context.Products.Include(p => p.Category)
             .Include(p => p.ProductImages)
             .Include(p => p.ProductBadges)
             .ThenInclude(pb => pb.Badge)
             .Include(p => p.ProductColors)
             .ThenInclude(p => p.Color);
        return products;
    }

    public List<int> ProductIds(FromFilterVM filter)
    {
        var data=  _context.ProductColors.Where(pc => filter.ColorIds.Contains(pc.ColorId)).Select(pc => pc.ProductId).Distinct().ToList();
        return data;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public DbSet<AppUser> Users()
    {
        var user =  _context.Users;
        return user;
    }

    
    public DbSet<Product> Products()
    {
        var product= _context.Products; 
        return product;
    }

    public DbSet<Review> Reviews()
    {
        var review = _context.Reviews;
        return review;
    }
    public DbSet<UserBasket> Baskets()
    {
        var basket = _context.UserBaskets;
        return basket;  
    }

    public DbSet<Color> Colors()
    {
        var color= _context.Colors;
        return color;
    }
}
