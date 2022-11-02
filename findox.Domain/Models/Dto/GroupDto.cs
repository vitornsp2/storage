using findox.Domain.Models.Database;

namespace findox.Domain.Models.Dto;


public class GroupDto 
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class GroupAllDto 
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
    public ICollection<UserGroup>? UserGroups { get; set; }
    public ICollection<Permission>? Permissions { get; set; }
}