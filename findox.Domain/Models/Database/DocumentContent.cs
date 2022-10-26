namespace findox.Domain.Models.Database;

public class DocumentContent
{
    public long Id { get; set; }
    public long DocumentId { get; set; }
    public byte[] Data { get; set; } = null!;
    public Document Document { get; set; } = null!;
}
