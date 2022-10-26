using findox.Domain.Models.Database;

namespace findox.Domain.Interfaces.Repository;

public interface IUserGroupRepository
{
    Task<UserGroup> Create(UserGroup userGroup);
    Task<UserGroup> ReadById(long id);
    Task<List<UserGroup>> ReadByGroupId(long id);
    Task<List<UserGroup>> ReadByUserId(long id);
    Task<bool> DeleteById(long id);
    Task<bool> DeleteByGroupId(long id);
    Task<bool> DeleteByUserId(long id);
    Task<int> CountByColumnValue(string column, long id);
    Task<int> CountByGroupAndUser(long groupId, long userId);
}
