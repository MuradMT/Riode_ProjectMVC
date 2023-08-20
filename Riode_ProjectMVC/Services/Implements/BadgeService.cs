using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Services.Implements
{
    public class BadgeService:IBadgeService
    {
        RiodeContext _context { get; }
        public BadgeService(RiodeContext context)
        {
            _context = context;
        }

        public DbSet<Badge> Badges()
        {
            var badge=_context.Badges;
            return badge;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
