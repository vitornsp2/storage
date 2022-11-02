using Dapper;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Models.Database;

namespace findox.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository() : base()
    {
    }

    public async Task<User> Create(User user)
    {
        var procedureName = "storage.users_create";
        var param = new { name = user.Name, password = user.Password, email = user.Email, role = user.Role };
        var id = ((long?) await base.ExecuteEscalar(procedureName, param));
        user.Id = id.HasValue ? id.Value : 0;
        return user;
    }

    public async Task<IEnumerable<User>> ReadAll()
    {
        var procedureName = "storage.users_get_all";
        var users = await base.Query(procedureName);
        return users;
    }

    public async Task<User> ReadById(long id)
    {
        var procedureName = "storage.users_get_by_id";
        var param = new { id = id };
        return await base.Get(procedureName, param);
    }

    public async Task<bool?> UpdateById(User user)
    {
        var param = new DynamicParameters();
        var procedureName = "storage.users_update";
        
        param.Add("id", user.Id);
        param.Add("name", string.IsNullOrWhiteSpace(user.Name) ? null : user.Name);
        param.Add("email", string.IsNullOrWhiteSpace(user.Email) ? null : user.Email);
        param.Add("role", string.IsNullOrWhiteSpace(user.Role) ? null : user.Role);
        
        return (bool?)await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<bool?> UpdatePasswordById(User user)
    {
        var procedureName = "storage.users_update_password";
        var param = new { id = user.Id, password = user.Password };
        return (bool?)await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<bool?> DeleteById(long id)
    {
        var procedureName = "storage.users_delete_by_id";
        var param = new { id = id};
        return (bool?)await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<int?> CountByColumnValue(string column, string value)
    {
        var procedureName = "storage.users_count_by_column_value_text";
        var param = new { column_name = column, column_value = value };
        return (int?) await base.ExecuteEscalar(procedureName, param);
    }

    public async Task<User?> Authenticate(User user)
    {
        var procedureName = "storage.users_authenticate";
        var param = new { email = user.Email, password = user.Password };
        var users = await base.Query(procedureName, param);
        return users?.FirstOrDefault();
    }
}