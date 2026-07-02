using Api.Dtos;
using Api.Interfaces;
using Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class CertificateService : ICertificateService
{
    private readonly Context _context;
    private readonly IFileStorageService _fileStorageService;
    
    public CertificateService(Context context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<List<CertificateView>> GetCertificatesAsync(CertificateFilter filter)
    {
        var query = _context.Certificates.AsQueryable();

        if (filter.Number.HasValue)
        {
            query = query.Where(x => x.Number == filter.Number);
        }

        if (!string.IsNullOrWhiteSpace(filter.Type))
        {
            query = query.Where(x => x.Type == filter.Type);
        }

        return await query
            .Select(x => new CertificateView
            {
                Type = x.Type,
                Number = x.Number,
                NotifiedBody = x.NotifiedBody,
                IssueDate = x.IssueDate,
                ExpiryDate = x.ExpiryDate,
            }).ToListAsync();
    }
    
    
    public async Task<Certificate> UploadCertificate(Certificate certificate, IFormFile file)
    {
        try
        {
            var filePath = await _fileStorageService.SaveFileAsync(file);

            certificate.FilePath = filePath;
            
            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            return certificate;
        }
        catch (Exception e)
        {
            throw new Exception("Failed to create certificate", e);
        }
    }

    public async Task<CertificateFileResult> GetCertificateFileAsync(int id)
    {
        var certificate = await _context.Certificates.FindAsync(id);

        if (certificate == null || string.IsNullOrWhiteSpace(certificate.FilePath))
        {
            return null;
        }
        
        var fileStream = await _fileStorageService.GetFileAsync(certificate.FilePath);

        return new CertificateFileResult
        {
            Stream = fileStream,
            FileName = certificate.FilePath,
            ContentType = "application/pdf"
        };
    }
}