using Riode_ProjectMVC.ExtensionServices.Implements;
using Riode_ProjectMVC.ExtensionServices.Interfaces;
using Riode_ProjectMVC.Services.Implements;
using Riode_ProjectMVC.Services.Interfaces;

namespace Riode_ProjectMVC.Utils.Extensions
{
    public static class ServiceRegistrationExtension
    {
        public static void AddService(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBadgeService, BadgeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IFileService, FileService>();
        }
    }
}
