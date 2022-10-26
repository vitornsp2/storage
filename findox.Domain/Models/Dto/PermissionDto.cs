namespace findox.Domain.Models.Dto;

public interface IPermissionDto
{
    long? Id { get; }
    long? DocumentId { get; }
    long? UserId { get; }
    long? GroupId { get; }
}


public class PermissionDto : IPermissionDto
{
    public long? Id { get; set; }
    public long? DocumentId { get; set; }
    public long? UserId { get; set; }
    public long? GroupId { get; set; }
}