using Api.Dtos;
using Api.Model;

namespace Api.Interfaces;

public interface ICertificateService
{
    public Task<List<CertificateView>> GetCertificatesAsync(CertificateFilter filter);
    public Task<Certificate> UploadCertificate(Certificate certificate, IFormFile file);
    public Task<CertificateFileResult> GetCertificateFileAsync(int id);
}