namespace findox.Domain.Models.Database;

public class Document : Entity
{
    public Document()
    {
        Permissions = new HashSet<Permission>();
    }
    public string Filename { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public long UserId { get; set; }
    public User UserPostedNavigation { get; set; } = null!;
    public DocumentContent DocumentContent { get; set; } = null!;
    public ICollection<Permission> Permissions { get; set; }
}
