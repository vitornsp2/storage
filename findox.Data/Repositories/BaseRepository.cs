using Npgsql;
using findox.Domain.Interfaces.Repository;
using Dapper;
using System.Data;
using findox.Domain.Models.Database;

namespace findox.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {

        private string _connectionString = $"{Environment.GetEnvironmentVariable("STORAGE_CONN_STRING")}";

        public BaseRepository()
        {
        }

        public async Task<T> Get(string procedureName, Object param)
        {
            using NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            return await conn.QueryFirstOrDefaultAsync<T>(sql: procedureName, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<object?> ExecuteEscalar(string procedureName, Object param)
        {
            using NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            return await conn.ExecuteScalarAsync<object?>(sql: procedureName, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> Query(string procedureName, Object? param = null)
        {
            using NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            return await conn.QueryAsync<T>(sql: procedureName, param, commandType: CommandType.StoredProcedure);
        }
    }
}