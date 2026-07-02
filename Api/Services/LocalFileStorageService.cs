using Api.Config;
using Api.Interfaces;
using Microsoft.Extensions.Options;

namespace Api.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly FileStorageOptions _options;

    public LocalFileStorageService(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!_options.AllowedExtensions.Contains(extension))
        {
            throw new Exception($"File type {file.ContentType} not supported");
        }
        
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        
        return fileName;
    }

    public async Task<Stream> GetFileAsync(string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);
        return new FileStream(path, FileMode.Open, FileAccess.Read);
        
    }
}