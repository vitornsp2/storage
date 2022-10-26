using findox.Domain.Interfaces.Repository;
using findox.Domain.Maps;
using findox.Domain.Models.Database;
using Npgsql;

namespace findox.Data.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(NpgsqlConnection connection) : base(connection)
    {
        _connection = connection;
    }

    public async Task<User> Create(User user)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_create('{user.Name}', '{user.Password}', '{user.Email}', '{user.Role}');";

            var reader = await RunQuery(command);
            var newUser = new User();
            while (await reader.ReadAsync())
            {
                var map = new DatabaseMap();
                newUser = map.User(reader);
            }
            reader.Close();
            return newUser;
        }
    }

    public async Task<List<User>> ReadAll()
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = "SELECT * FROM storage.users_get_all();";

            var reader = await RunQuery(command);
            var users = new List<User>();
            while (await reader.ReadAsync())
            {
                var map = new DatabaseMap();
                users.Add(map.User(reader));
            }
            reader.Close();
            return users;
        }
    }

    public async Task<User> ReadById(long id)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_get_by_id({id});";

            var reader = await RunQuery(command);
            var user = new User();
            while (await reader.ReadAsync())
            {
                var map = new DatabaseMap();
                user = map.User(reader);
            }
            reader.Close();
            return user;
        }
    }

    public async Task<bool> UpdateById(User user)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_update({user.Id}";

            if (user.Name is not null) command.CommandText += $", '{user.Name}'";
            else command.CommandText += $", null";

            if (user.Email is not null) command.CommandText += $", '{user.Email}'";
            else command.CommandText += $", null";

            if (user.Role is not null) command.CommandText += $", '{user.Role}'";
            else command.CommandText += $", null";

            command.CommandText += ");";

            var result = await RunScalar(command);
            var success = false;
            if (result is not null)
            {
                success = bool.Parse($"{result.ToString()}");
            }
            return success;
        }
    }

    public async Task<bool> UpdatePasswordById(User user)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_update_password({user.Id}, '{user.Password}');";

            var result = await RunScalar(command);
            var success = false;
            if (result is not null)
            {
                success = bool.Parse($"{result.ToString()}");
            }
            return success;
        }
    }

    public async Task<bool> DeleteById(long id)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_delete_by_id({id});";

            var result = await RunScalar(command);
            var success = false;
            if (result is not null)
            {
                success = bool.Parse($"{result.ToString()}");
            }
            return success;
        }
    }

    public async Task<int> CountByColumnValue(string column, string value)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_count_by_column_value_text('{column}', '{value}');";

            var result = await RunScalar(command);
            int count = 0;
            if (result is not null)
            {
                count = int.Parse($"{result.ToString()}");
            }
            return count;
        }
    }

    public async Task<User> Authenticate(User user)
    {
        using (var command = new NpgsqlCommand())
        {
            command.CommandText = $"SELECT * FROM storage.users_authenticate('{user.Email}', '{user.Password}');";

            var reader = await RunQuery(command);
            var authenticatedUser = new User();
            while (await reader.ReadAsync())
            {
                var map = new DatabaseMap();
                authenticatedUser = map.User(reader);
            }
            reader.Close();
            return authenticatedUser;
        }
    }
}
