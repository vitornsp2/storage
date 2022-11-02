using findox.Domain.Interfaces.Repository;
using findox.Domain.Models.Database;

namespace findox.Data.Repositories;

public class UserGroupRepository : BaseRepository<UserGroup>, IUserGroupRepository
{
    public UserGroupRepository() : base()
    {
    }

    public async Task<UserGroup> Create(UserGroup userGroup)
    {
        var procedureName = "storage.users_groups_create";
        var param = new { group_id = userGroup.GroupId, user_id = userGroup.UserId };
        var id = ((long?) await base.ExecuteEscalar(procedureName, param));
        userGroup.Id = id.HasValue ? id.Value : 0;
        return userGroup;
    }

    public async Task<UserGroup?> ReadById(long id)
    {
        var procedureName = "storage.users_groups_get_by_id";
        var param = new { id = id };
        var userGroups = await base.Query(procedureName, param);
        return userGroups?.FirstOrDefault();
    }

    public async Task<IEnumerable<UserGroup>> ReadByGroupId(long id)
    {
        var procedureName = "storage.users_groups_get_by_group_id";
        var param = new { group_id = id };
        return await base.Query(procedureName, param);
    }

    public async Task<IEnumerable<UserGroup>> ReadByUserId(long id)
    {
        var procedureName = "storage.users_groups_get_by_user_id";
        var param = new { user_id = id };
        return await base.Query(procedureName, param);
    }

    public async Task<bool?> DeleteById(long id)
    {
        var procedureName = "storage.users_groups_delete_by_id";
        var param = new { id = id};
        return (bool?)await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<bool?> DeleteByGroupId(long id)
    {
        var procedureName = "storage.users_groups_delete_by_group_id";
        var param = new { group_id = id};
        return (bool?)await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<bool?> DeleteByUserId(long id)
    {
        var procedureName = "storage.users_groups_delete_by_user_id";
        var param = new { user_id = id};
        return (bool?)await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<int?> CountByColumnValue(string column, long id)
    {
        var procedureName = "storage.users_groups_count_by_column_value_id";
        var param = new { column_name = column, column_value = id };
        return (int?) await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<int?> CountByGroupAndUser(long groupId, long userId)
    {
        var procedureName = "storage.users_groups_count_by_group_and_user";
        var param = new { id_group = groupId, id_user = userId };
        return (int?) await base.ExecuteEscalar(procedureName, param);
    }
}