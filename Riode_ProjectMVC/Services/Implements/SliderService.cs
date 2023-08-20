using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Services.Implements;

public class SliderService:ISliderService
{
    public RiodeContext _context { get; }
    public SliderService(RiodeContext context)
    {
        _context = context;
    }

    public DbSet<Slider> Get()
    {
        var data = _context.Sliders;
        return data;
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
