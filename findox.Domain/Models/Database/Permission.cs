namespace findox.Domain.Models.Database;

public class Permission : Entity
{
    public long DocumentId { get; set; }
    public long? UserId { get; set; }
    public long? GroupId { get; set; }
    public virtual Group? Group { get; set; }
    public Document Document { get; set; } = null!;
    public User? User { get; set; }
}
