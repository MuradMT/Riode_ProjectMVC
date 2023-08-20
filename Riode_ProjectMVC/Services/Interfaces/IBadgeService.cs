namespace Riode_ProjectMVC.Services.Interfaces;

public interface IBadgeService
{
    DbSet<Badge> Badges();
    Task SaveChangesAsync();
}
