using Api.Dtos;
using Api.Model;

namespace Api.Interfaces;

public interface ICertificateService
{
    public Task<List<Certificate>> GetCertificatesAsync(CertificateFilter filter);
    public Task<Certificate> UploadCertificate(Certificate certificate, IFormFile file);
    public Task<CertificateFileResult> GetCertificateFileAsync(int id);
}