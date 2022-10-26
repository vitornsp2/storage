namespace findox.Domain.Models.Dto;

public interface IDocumentDto
{
    long? Id { get; }
    string? Filename { get; }
    string? ContentType { get; }
    string? Description { get; }
    string? Category { get; }
    DateTime? CreateDate { get; }
    long? UserId { get; }
}

public class DocumentDto : IDocumentDto
{
    public long? Id { get; set; }
    public string? Filename { get; set; }
    public string? ContentType { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public DateTime? CreateDate { get; set; }
    public long? UserId { get; set; }
}