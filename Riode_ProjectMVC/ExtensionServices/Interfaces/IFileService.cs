namespace Riode_ProjectMVC.ExtensionServices.Interfaces;

public interface IFileService
{
    Task SaveAsync(IFormFile file, string path);
    Task<string> UploadAsync(IFormFile file, string path, string contentType = "image", int mb = 2);
    void Delete(string path);
}
