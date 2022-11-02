namespace findox.Domain.Models.Dto;

public class DocumentDto
{
    public long? Id { get; set; }
    public string? Filename { get; set; }
    public string? ContentType { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public DateTime? CreateDate { get; set; }
    public long? UserId { get; set; }
}