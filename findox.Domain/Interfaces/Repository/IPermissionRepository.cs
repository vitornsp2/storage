using findox.Domain.Models.Database;

namespace findox.Domain.Interfaces.Repository
{
    public interface IPermissionRepository : IBaseRepository
    {
        Task<Permission> Create(Permission permission);
        Task<Permission> ReadById(long id);
        Task<List<Permission>> ReadByDocumentId(long id);
        Task<List<Permission>> ReadByUserId(long id);
        Task<List<Permission>> ReadByGroupId(long id);
        Task<bool> DeleteById(long id);
        Task<bool> DeleteByDocumentId(long id);
        Task<bool> DeleteByUserId(long id);
        Task<bool> DeleteByGroupId(long id);
        Task<int> PermissionMatchCount(Permission permit);
        Task<int> CountByColumnValue(string column, long id);
    }
}