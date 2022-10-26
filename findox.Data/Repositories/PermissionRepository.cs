using findox.Domain.Interfaces.Repository;
using findox.Domain.Maps;
using findox.Domain.Models.Database;
using Npgsql;

namespace findox.Data.Repositories
{
    public class PermissionRepository : BaseRepository, IPermissionRepository
    {
        public PermissionRepository(NpgsqlConnection connection) : base(connection)
        {
            _connection = connection;
        }

        public async Task<Permission> Create(Permission permission)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permissions_create({permission.DocumentId}";

                if (permission.UserId is not null) command.CommandText += $", {permission.UserId}";
                else command.CommandText += $", null";

                if (permission.GroupId is not null) command.CommandText += $", {permission.GroupId}";
                else command.CommandText += $", null";

                command.CommandText += ");";

                var reader = await RunQuery(command);
                var newPermission = new Permission();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    newPermission = map.Permission(reader);
                }
                reader.Close();
                return newPermission;
            }
        }

        public async Task<Permission> ReadById(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permissions_get_by_id({id});";

                var reader = await RunQuery(command);
                var permission = new Permission();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    permission = map.Permission(reader);
                }
                reader.Close();
                return permission;
            }
        }

        public async Task<List<Permission>> ReadByDocumentId(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permissions_get_by_document_id({id});";

                var reader = await RunQuery(command);
                var permissions = new List<Permission>();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    permissions.Add(map.Permission(reader));
                }
                reader.Close();
                return permissions;
            }
        }

        public async Task<List<Permission>> ReadByUserId(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permissions_get_by_user_id({id});";

                var reader = await RunQuery(command);
                var permissions = new List<Permission>();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    permissions.Add(map.Permission(reader));
                }
                reader.Close();
                return permissions;
            }
        }

        public async Task<List<Permission>> ReadByGroupId(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permissions_get_by_group_id({id});";

                var reader = await RunQuery(command);
                var permissions = new List<Permission>();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    permissions.Add(map.Permission(reader));
                }
                reader.Close();
                return permissions;
            }
        }

        public async Task<bool> DeleteById(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permissions_delete_by_id({id});";

                var result = await RunScalar(command);
                var success = false;
                if (result is not null)
                {
                    success = bool.Parse($"{result.ToString()}");
                }
                return success;
            }
        }

        public async Task<bool> DeleteByDocumentId(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permissions_delete_by_document_id({id});";

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
                command.CommandText = $"SELECT * FROM storage.permissions_delete_by_user_id({id});";

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
                command.CommandText = $"SELECT * FROM storage.permissions_delete_by_group_id({id});";

                var result = await RunScalar(command);
                var success = false;
                if (result is not null)
                {
                    success = bool.Parse($"{result.ToString()}");
                }
                return success;
            }
        }

        public async Task<int> PermissionMatchCount(Permission permission)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permission_match_count({permission.DocumentId}";

                if (permission.UserId is not null) command.CommandText += $", {permission.UserId}";
                else command.CommandText += $", null";

                if (permission.GroupId is not null) command.CommandText += $", {permission.GroupId}";
                else command.CommandText += $", null";

                command.CommandText += ");";

                var result = await RunScalar(command);
                int count = 0;
                if (result is not null)
                {
                    count = int.Parse($"{result.ToString()}");
                }
                return count;
            }
        }

        public async Task<int> CountByColumnValue(string column, long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permissions_count_by_column_value_id('{column}', {id});";

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
}
