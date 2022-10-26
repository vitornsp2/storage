namespace findox.Domain.Models.Dto;

public interface IUserGroupDto
{
    long? Id { get; }
    long? GroupId { get; }
    long? UserId { get; }
}

public class UserGroupDto : IUserGroupDto
{
    public long? Id { get; set; }
    public long? GroupId { get; set; }
    public long? UserId { get; set; }
}