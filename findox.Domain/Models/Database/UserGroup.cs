namespace findox.Domain.Models.Database;

public class UserGroup : Entity
{
    public long GroupId { get; set; }
    public long UserId { get; set; }
    public Group Group { get; set; } = null!;
    public User User { get; set; } = null!;
}
