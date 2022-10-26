namespace findox.Domain.Models.Database;

public class UserGroup
{
    public long Id { get; set; }
    public long GroupId { get; set; }
    public long UserId { get; set; }
    public Group Group { get; set; } = null!;
    public User User { get; set; } = null!;
}
