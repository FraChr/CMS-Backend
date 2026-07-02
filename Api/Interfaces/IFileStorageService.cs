namespace Api.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<Stream> GetFileAsync(string fileName);
}