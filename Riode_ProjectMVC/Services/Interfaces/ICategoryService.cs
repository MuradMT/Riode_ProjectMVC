namespace Riode_ProjectMVC.Services.Interfaces;

public interface ICategoryService
{
    DbSet<Category> Categories();
    Task SaveChangesAsync();
}
