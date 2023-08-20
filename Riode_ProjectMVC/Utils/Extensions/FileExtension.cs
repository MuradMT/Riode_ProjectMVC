
namespace Riode_ProjectMVC.Utils.Extensions;
public static class FileExtension
{
    public static bool IsSizeValid(this IFormFile file, int mb)
            => file.IsSizeValid(mb);
    public static bool IsTypeValid(this IFormFile file, string contentType)
        => file.IsTypeValid(contentType);
}
