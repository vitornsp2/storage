using Dapper;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Models.Database;

namespace findox.Data.Repositories;

public class GroupRepository : BaseRepository<Group>, IGroupRepository
{
    public GroupRepository() : base()
    {
    }

    public async Task<Group> Create(Group group)
    {
        var procedureName = "storage.groups_create";

        var param = new DynamicParameters();

        param.Add("name", group.Name);
        param.Add("description", string.IsNullOrWhiteSpace(group.Description) ? null : group.Description);

        var id = ((long?)await base.ExecuteEscalar(procedureName, param));
        group.Id = id.HasValue ? id.Value : 0;
        return group;
    }

    public async Task<IEnumerable<Group>> ReadAll()
    {
        var procedureName = "storage.groups_get_all";
        var groups = await base.Query(procedureName);
        return groups;
    }

    public async Task<Group?> ReadById(long id)
    {
        var procedureName = "storage.groups_get_by_id";
        var param = new { id = id };
        var groups = await base.Query(procedureName, param);
        return groups?.FirstOrDefault();
    }

    public async Task<bool?> UpdateById(Group group)
    {
        var param = new DynamicParameters();
        var procedureName = "storage.users_update";

        param.Add("id", group.Id);
        param.Add("name", string.IsNullOrWhiteSpace(group.Name) ? null : group.Name);
        param.Add("description", string.IsNullOrWhiteSpace(group.Description) ? null : group.Description);

        return (bool?)await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<bool?> DeleteById(long id)
    {
        var procedureName = "storage.groups_delete_by_id";
        var param = new { id = id };
        return (bool?)await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<int?> CountByColumnValue(string column, string value)
    {
        var procedureName = "storage.groups_count_by_column_value_text";
        var param = new { column_name = column, column_value = value };
        return (int?)await base.ExecuteEscalar(procedureName, param);
    }
}
