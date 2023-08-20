namespace Riode_ProjectMVC.Services.Interfaces;

public interface ISliderService
{
    DbSet<Slider> Get();
    Task SaveChangesAsync();
}
