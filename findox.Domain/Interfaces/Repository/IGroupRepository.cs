using findox.Domain.Models.Database;

namespace findox.Domain.Interfaces.Repository;

public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<Group> Create(Group group);
        Task<IEnumerable<Group>> ReadAll();
        Task<Group?> ReadById(long id);
        Task<bool?> UpdateById(Group group);
        Task<bool?> DeleteById(long id);
        Task<int?> CountByColumnValue(string column, string value);
    }
