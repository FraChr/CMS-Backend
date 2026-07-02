namespace Api.Model;

public class Certificate
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int Number { get; set; }
    public string NotifiedBody { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public string FilePath { get; set; }
}