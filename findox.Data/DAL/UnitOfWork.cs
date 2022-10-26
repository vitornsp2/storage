using findox.Data.Repositories;
using findox.Domain.Interfaces.DAL;
using findox.Domain.Interfaces.Repository;
using Npgsql;

namespace findox.Data.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IDocumentContentRepository? _documentContentRepository;
        private IGroupRepository? _groupRepository;
        private IDocumentRepository? _documentRepository;
        private IUserGroupRepository? _userGroupRepository;
        private IPermissionRepository? _permitRepository;
        private IUserRepository? _userRepository;

        private string _connectionString = $"{Environment.GetEnvironmentVariable("STORAGE_CONN_STRING")}";
        private NpgsqlConnection _connection;
        private NpgsqlTransaction? _transaction;

        private bool disposed;

        public UnitOfWork()
        {
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
        }

        public void Begin()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction is not null) _transaction.Commit();
            if (_transaction is not null && _transaction.Connection is not null) _transaction.Connection.Close();
            if (_connection is not null) _connection.Close();
        }

        public void Rollback()
        {
            if (_transaction is not null) _transaction.Rollback();
            if (_transaction is not null && _transaction.Connection is not null) _transaction.Connection.Close();
        }

        public IDocumentContentRepository documentContents
        {
            get
            {
                if (_documentContentRepository is null) _documentContentRepository = new DocumentContentRepository(_connection);
                return _documentContentRepository;
            }
        }

        public IGroupRepository groups
        {
            get
            {
                if (_groupRepository is null) _groupRepository = new GroupRepository(_connection);
                return _groupRepository;
            }
        }

        public IDocumentRepository documents
        {
            get
            {
                if (_documentRepository is null) _documentRepository = new DocumentRepository(_connection);
                return _documentRepository;
            }
        }

        public IUserGroupRepository userGroups
        {
            get
            {
                if (_userGroupRepository is null) _userGroupRepository = new UserGroupRepository(_connection);
                return _userGroupRepository;
            }
        }

        public IPermissionRepository permissions
        {
            get
            {
                if (_permitRepository is null) _permitRepository = new PermissionRepository(_connection);
                return _permitRepository;
            }
        }

        public IUserRepository users
        {
            get
            {
                if (_userRepository is null) _userRepository = new UserRepository(_connection);
                return _userRepository;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (_transaction is not null && _transaction.Connection is not null)
                {
                    _transaction.Rollback();
                    _transaction.Connection.Close();
                }
                if (_connection is not null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
                disposed = true;
            }
        }

    }
}