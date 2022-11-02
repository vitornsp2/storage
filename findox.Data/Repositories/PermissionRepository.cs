using Dapper;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Models.Database;

namespace findox.Data.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository() : base()
        {
        }

        public async Task<Permission> Create(Permission permission)
        {
            var procedureName = "storage.permissions_create";
            var param = new DynamicParameters();
            param.Add("document_id", permission.DocumentId);
            param.Add("user_id", permission.UserId.HasValue ? permission.UserId : null);
            param.Add("group_id", permission.GroupId.HasValue ? permission.GroupId : null);
            var id = ((long?) await base.ExecuteEscalar(procedureName, param));
            permission.Id = id.HasValue ? id.Value : 0;

            return permission;
        }

        public async Task<Permission?> ReadById(long id)
        {
            var procedureName = "storage.permissions_get_by_id";
            var param = new { id = id };
            var permission = await base.Query(procedureName, param);
            return permission?.FirstOrDefault();
        }

        public async Task<IEnumerable<Permission>> ReadByDocumentId(long id)
        {

            var procedureName = "storage.permissions_get_by_document_id";
            var param = new { document_id = id };
            var permissions = await base.Query(procedureName);
            return permissions;
        }

        public async Task<IEnumerable<Permission>> ReadByUserId(long id)
        {
            var procedureName = "storage.permissions_get_by_user_id";
            var param = new { id_user = id };
            var permissions = await base.Query(procedureName, param);
            return permissions;
        }

        public async Task<IEnumerable<Permission>> ReadByGroupId(long id)
        {
            var procedureName = "storage.permissions_get_by_group_id";
            var param = new { group_id = id };
            var permissions = await base.Query(procedureName, param);
            return permissions;
        }

        public async Task<bool?> DeleteById(long id)
        {
            var procedureName = "storage.permissions_delete_by_id";
            var param = new { id = id};
            return (bool?)await base.ExecuteEscalar(procedureName, param);
        }

        public async Task<bool?> DeleteByDocumentId(long id)
        {
            var procedureName = "storage.permissions_delete_by_document_id";
            var param = new { id_document = id};
            return (bool?)await base.ExecuteEscalar(procedureName, param);
        }

        public async Task<bool?> DeleteByUserId(long id)
        {
            var procedureName = "storage.permissions_delete_by_user_id";
            var param = new { id_user = id};
            return (bool?)await base.ExecuteEscalar(procedureName, param);
        }

        public async Task<bool?> DeleteByGroupId(long id)
        {
            var procedureName = "storage.permissions_delete_by_group_id";
            var param = new { id_group = id};
            return (bool?)await base.ExecuteEscalar(procedureName, param);
        }

        public async Task<int?> PermissionMatchCount(Permission permission)
        {
            var procedureName = "storage.permission_match_count";
            var param = new DynamicParameters();
            param.Add("id_document", permission.DocumentId);
            param.Add("id_user", permission.UserId.HasValue ? permission.UserId : null);
            param.Add("id_group", permission.GroupId.HasValue ? permission.GroupId : null);

            return (int?) await base.ExecuteEscalar(procedureName, param);
        }

        public async Task<int?> CountByColumnValue(string column, long id)
        {
            var procedureName = "storage.permissions_count_by_column_value_id";
            var param = new { column_name = column, column_value = id };
            return (int?) await base.ExecuteEscalar(procedureName, param);
        }
    }
}
