namespace findox.Domain.Models.Database;

public class User
{
    public User()
    {
        Documents = new HashSet<Document>();
        UserGroups = new HashSet<UserGroup>();
        Permissions = new HashSet<Permission>();
    }
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public ICollection<Document> Documents { get; set; }
    public ICollection<UserGroup> UserGroups { get; set; }
    public ICollection<Permission> Permissions { get; set; }
}
