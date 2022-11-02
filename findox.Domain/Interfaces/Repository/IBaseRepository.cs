using Npgsql;

namespace findox.Domain.Interfaces.Repository
{
    public interface IBaseRepository<T>
    {
        Task<object?> ExecuteEscalar(string procedureName, Object item);
        Task<IEnumerable<T>> Query(string procedureName, Object? param = null);
        Task<T> Get(string procedureName, Object param);
    }
}
