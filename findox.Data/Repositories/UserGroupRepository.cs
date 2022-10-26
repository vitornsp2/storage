using findox.Domain.Interfaces.Repository;
using findox.Domain.Maps;
using findox.Domain.Models.Database;
using Npgsql;

namespace findox.Data.Repositories;

public class UserGroupRepository : BaseRepository, IUserGroupRepository
{
    public UserGroupRepository(NpgsqlConnection connection) : base(connection)
    {
        _connection = connection;
    }

    public async Task<UserGroup> Create(UserGroup userGroup)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_groups_create({userGroup.GroupId}, {userGroup.UserId});";

            var reader = await RunQuery(command);
            var newUserGroup = new UserGroup();
            while (await reader.ReadAsync())
            {
                var map = new DatabaseMap();
                newUserGroup = map.UserGroup(reader);
            }
            reader.Close();
            return newUserGroup;
        }
    }

    public async Task<UserGroup> ReadById(long id)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_groups_get_by_id({id});";

            var reader = await RunQuery(command);
            var userGroup = new UserGroup();
            while (await reader.ReadAsync())
            {
                var map = new DatabaseMap();
                userGroup = map.UserGroup(reader);
            }
            reader.Close();
            return userGroup;
        }
    }

    public async Task<List<UserGroup>> ReadByGroupId(long id)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_groups_get_by_group_id({id});";
            var reader = await RunQuery(command);
            var userGroups = new List<UserGroup>();
            while (await reader.ReadAsync())
            {
                var map = new DatabaseMap();
                userGroups.Add(map.UserGroup(reader));
            }
            reader.Close();
            return userGroups;
        }
    }

    public async Task<List<UserGroup>> ReadByUserId(long id)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_groups_get_by_user_id({id});";
            var reader = await RunQuery(command);
            var userGroups = new List<UserGroup>();
            while (await reader.ReadAsync())
            {
                var map = new DatabaseMap();
                userGroups.Add(map.UserGroup(reader));
            }
            reader.Close();
            return userGroups;
        }
    }

    public async Task<bool> DeleteById(long id)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_groups_delete_by_id({id});";
            var result = await RunScalar(command);
            var success = false;
            if (result is not null)
            {
                success = bool.Parse($"{result.ToString()}");
            }
            return success;
        }
    }

    public async Task<bool> DeleteByGroupId(long id)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_groups_delete_by_group_id({id});";
            var result = await RunScalar(command);
            var success = false;
            if (result is not null)
            {
                success = bool.Parse($"{result.ToString()}");
            }
            return success;
        }
    }

    public async Task<bool> DeleteByUserId(long id)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_groups_delete_by_user_id({id});";
            var result = await RunScalar(command);
            var success = false;
            if (result is not null)
            {
                success = bool.Parse($"{result.ToString()}");
            }
            return success;
        }
    }

    public async Task<int> CountByColumnValue(string column, long id)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_groups_count_by_column_value_id('{column}', {id});";
            var result = await RunScalar(command);
            int count = 0;
            if (result is not null)
            {
                count = int.Parse($"{result.ToString()}");
            }
            return count;
        }
    }

    public async Task<int> CountByGroupAndUser(long groupId, long userId)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_groups_count_by_group_and_user({groupId}, {userId});";
            var result = await RunScalar(command);
            int count = 0;
            if (result is not null)
            {
                count = int.Parse($"{result.ToString()}");
            }
            return count;
        }
    }
}