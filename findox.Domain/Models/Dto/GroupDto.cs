using findox.Domain.Models.Database;

namespace findox.Domain.Models.Dto;

public interface IGroupDto
{
    long? Id { get; }
    string? Name { get; }
    string? Description { get; }
    DateTime? CreatedDate { get; }
}

public class GroupDto : IGroupDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public interface IGroupAllDto
{
    long? Id { get; }
    string? Name { get; }
    string? Description { get; }
    DateTime? CreatedDate { get; }
    ICollection<UserGroup>? UserGroups { get; }
    ICollection<Permission>? Permissions { get; }
}

public class GroupAllDto : IGroupAllDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
    public ICollection<UserGroup>? UserGroups { get; set; }
    public ICollection<Permission>? Permissions { get; set; }
}
