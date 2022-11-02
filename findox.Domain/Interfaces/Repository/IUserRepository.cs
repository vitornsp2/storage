using findox.Domain.Models.Database;

namespace findox.Domain.Interfaces.Repository;

public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> Create(User user);
        Task<IEnumerable<User>> ReadAll();
        Task<User> ReadById(long id);
        Task<bool?> UpdateById(User user);
        Task<bool?> UpdatePasswordById(User user);
        Task<bool?> DeleteById(long id);
        Task<int?> CountByColumnValue(string column, string value);
        Task<User?> Authenticate(User user);
    }