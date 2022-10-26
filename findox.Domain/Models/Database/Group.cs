namespace findox.Domain.Models.Database;

public class Group
{
    public Group()
        {
            UserGroups = new HashSet<UserGroup>();
            Permissions = new HashSet<Permission>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
        public ICollection<Permission> Permissions { get; set; }
}
