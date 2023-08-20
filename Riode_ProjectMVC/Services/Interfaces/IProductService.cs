namespace Riode_ProjectMVC.Services.Interfaces;

public interface IProductService
{
    Product Detail(int? id);
    ICollection<Product> Shop();
    IQueryable<Product> FilterProduct(FromFilterVM filter);
    List<int> ProductIds(FromFilterVM filter);
    Task SaveChangesAsync();
    DbSet<AppUser> Users();
    DbSet<Product> Products();
    DbSet<Review> Reviews();
    DbSet<UserBasket> Baskets();
    DbSet<Color> Colors();
}
