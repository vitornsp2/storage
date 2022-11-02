namespace findox.Domain.Models.Database;

public class Group : Entity
{
    public Group()
    {
        UserGroups = new HashSet<UserGroup>();
        Permissions = new HashSet<Permission>();
    }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<UserGroup> UserGroups { get; set; }
    public ICollection<Permission> Permissions { get; set; }
}
