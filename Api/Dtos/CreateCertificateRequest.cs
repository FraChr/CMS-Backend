namespace Api.Dtos;

public class CreateCertificateRequest
{
    public string Type { get; set; } = "";
    public int Number { get; set; }
    public string NotifiedBody { get; set; } = "";
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
}