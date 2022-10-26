using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Maps;
using findox.Domain.Models.Database;
using findox.Service.Services;
using Npgsql;

namespace findox.Test.End2End
{
    public class StorageDb
    {
        private string _testDbName;

        private NpgsqlConnection _connectionCreateTestDb;

        private NpgsqlConnection _connectionUseTestDb;
        private User? _userAdmin;
        private User? _userManager;
        private User? _userRegular;

        private Document? _documentApiDll;
        private Document? _documentDataDll;
        private Document? _documentDomainDll;
        private Document? _documentServiceDll;
        private Document? _documentTestDll;

        private Group? _groupAdmins;
        private Group? _groupManagers;
        private Group? _groupRegulars;
        private Group? _groupLaughingstocks;

        public StorageDb()
        {
            _testDbName = "storage_db_test";

            _connectionCreateTestDb = new NpgsqlConnection("Host=localhost;Database=postgres;Port=5432;Username=admin;Password=admin1234;Pooling=false");

            _connectionUseTestDb = new NpgsqlConnection($"Host=localhost;Database={_testDbName};Port=5432;Username=admin;Password=admin1234;Pooling=false");

            Environment.SetEnvironmentVariable("STORAGE_CONN_STRING", $"Host=localhost;Database={_testDbName};Port=5432;Username=admin;Password=admin1234;Pooling=false");
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        }

        public async Task RecycleDb()
        {
            await _connectionCreateTestDb.OpenAsync();
            await DropDb();
            await CreateDb();
            await _connectionCreateTestDb.CloseAsync();
            await _connectionCreateTestDb.DisposeAsync();

            await _connectionUseTestDb.OpenAsync();
            await SeedUserAdmin();
            await SeedUserManager();
            await SeedUserRegular();
            await SeedDocumentApiDll();
            await SeedDocumentDataDll();
            await SeedDocumentDomainDll();
            await SeedDocumentServiceDll();
            await SeedDocumentTestDll();
            await SeedGroupAdmins();
            await SeedGroupManagers();
            await SeedGroupRegulars();
            await SeedGroupLaughingstocks();
            await SeedUserGroupAdmin();
            await SeedUserGroupManager();
            await SeedUserGroupRegular();
            await SeedPermissionApiDllAdminUser();
            await SeedPermissionDataDllAdminsGroup();
            await SeedPermissionDomainDllManagerUser();
            await SeedPermissionServiceDllManagersGroup();
            await SeedPermissionTestDllRegularUser();
            await _connectionUseTestDb.CloseAsync();
            await _connectionUseTestDb.DisposeAsync();
        }

        private async Task DropDb()
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"DROP DATABASE IF EXISTS {_testDbName};";
                command.Connection = _connectionCreateTestDb;
                await command.PrepareAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task CreateDb()
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"CREATE DATABASE {_testDbName} WITH TEMPLATE storage_db OWNER admin;";
                command.Connection = _connectionCreateTestDb;
                await command.PrepareAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        #region PUBLIC METHODS

        public User GetUserAdmin()
        {
            if (_userAdmin is null) throw new Exception("Could not return admin user.");
            return _userAdmin;
        }

        public User GetUserManager()
        {
            if (_userManager is null) throw new Exception("Could not return manager user.");
            return _userManager;
        }

        public User GetUserRegular()
        {
            if (_userRegular is null) throw new Exception("Could not return basic user.");
            return _userRegular;
        }

        public string AuthenticateAdmin()
        {
            if (_userAdmin is null)
            {
                throw new Exception("Could not encode admin user token.");
            }
            var tokenService = new TokenService();
            var token = tokenService.Encode(_userAdmin.Id, _userAdmin.Role);
            return token;
        }

        public string AuthenticateManager()
        {
            if (_userManager is null)
            {
                throw new Exception("Could not encode manager user token.");
            }
            var tokenService = new TokenService();
            var token = tokenService.Encode(_userManager.Id, _userManager.Role);
            return token;
        }

        public string AuthenticateRegular()
        {
            if (_userRegular is null)
            {
                throw new Exception("Could not encode basic user token.");
            }
            var tokenService = new TokenService();
            var token = tokenService.Encode(_userRegular.Id, _userRegular.Role);
            return token;
        }

        public Document GetDocumentApiDll()
        {
            if (_documentApiDll is null) throw new Exception("Could not return Api dll document");
            return _documentApiDll;
        }

        public Document GetDocumentDataDll()
        {
            if (_documentDataDll is null) throw new Exception("Could not return Data dll document");
            return _documentDataDll;
        }

        public Document GetDocumentDominaDll()
        {
            if (_documentDomainDll is null) throw new Exception("Could not return Domain dll document");
            return _documentDomainDll;
        }

        public Document GetDocumentServiceDll()
        {
            if (_documentServiceDll is null) throw new Exception("Could not return Service dll document");
            return _documentServiceDll;
        }

        public Document GetDocumentTestDll()
        {
            if (_documentTestDll is null) throw new Exception("Could not return Test dll document");
            return _documentTestDll;
        }

        public Group GetGroupAdmins()
        {
            if (_groupAdmins is null) throw new Exception("Could not return group Admins.");
            return _groupAdmins;
        }

        public Group GetGroupManagers()
        {
            if (_groupManagers is null) throw new Exception("Could not return group Managers.");
            return _groupManagers;
        }

        public Group GetGroupRegulars()
        {
            if (_groupRegulars is null) throw new Exception("Could not return group Regulars.");
            return _groupRegulars;
        }

        public Group GetGroupLaughingstocks()
        {
            if (_groupLaughingstocks is null) throw new Exception("Could not return group Laughingstocks.");
            return _groupLaughingstocks;
        }

        #endregion PUBLIC METHODS

        #region USERS

        private async Task<User> SeedUser(string name, string password, string email, string role)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.users_create('{name}', '{password}', '{email}', '{role}');";
                command.Connection = _connectionUseTestDb;
                await command.PrepareAsync();
                var reader = await command.ExecuteReaderAsync();
                var newUser = new User();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    newUser = map.User(reader);
                }
                await reader.CloseAsync();
                return newUser;
            }
        }

        private async Task SeedUserAdmin()
        {
            _userAdmin = await SeedUser("admin", "passwordadmin", "admin@storagedocument.com", "admin");
        }

        private async Task SeedUserManager()
        {
            _userManager = await SeedUser("manager", "passwordmanager", "manager@storagedocument.com", "manager");
        }

        private async Task SeedUserRegular()
        {
            _userRegular = await SeedUser("regular", "passwordbasic", "regular@storagedocument.com", "regular");
        }

        #endregion USERS

        #region DOCUMENTS

        private async Task<Document> SeedDocument(string filename)
        {
            var contentType = "application/octet-stream";
            byte[] binaryData = await File.ReadAllBytesAsync(filename);
            long adminId = 0;
            if (_userAdmin?.Id is not null && _userAdmin.Id != 0) adminId = _userAdmin.Id;
            Document document = new Document();
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_create('{filename}', '{contentType}', '{adminId}', 'The {filename} file.', 'Files');";
                command.Connection = _connectionUseTestDb;
                await command.PrepareAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    document = map.Document(reader);
                }
                await reader.CloseAsync();
            }
            if (document.Id == 0) throw new Exception("Could not seed document.");
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_content_create('{document.Id}', :binary_data );";
                var npgsqlParameter = new NpgsqlParameter("binary_data", NpgsqlTypes.NpgsqlDbType.Bytea);
                npgsqlParameter.Value = binaryData;
                command.Parameters.Add(npgsqlParameter);
                command.Connection = _connectionUseTestDb;
                await command.PrepareAsync();
                await command.ExecuteScalarAsync();
            }
            return document;
        }

        private async Task SeedDocumentApiDll()
        {
            _documentApiDll = await SeedDocument("findox.Api.dll");
        }

        private async Task SeedDocumentDataDll()
        {
            _documentDataDll = await SeedDocument("findox.Data.dll");
        }

        private async Task SeedDocumentDomainDll()
        {
            _documentDomainDll = await SeedDocument("findox.Domain.dll");
        }

        private async Task SeedDocumentServiceDll()
        {
            _documentServiceDll = await SeedDocument("findox.Service.dll");
        }

        private async Task SeedDocumentTestDll()
        {
            _documentTestDll = await SeedDocument("findox.Test.dll");
        }

        #endregion DOCUMENTS

        #region GROUPS

        private async Task<Group> SeedGroup(string name, string description)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.groups_create('{name}', '{description}');";
                command.Connection = _connectionUseTestDb;
                await command.PrepareAsync();
                var reader = await command.ExecuteReaderAsync();
                var newGroup = new Group();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    newGroup = map.Group(reader);
                }
                await reader.CloseAsync();
                return newGroup;
            }
        }

        private async Task SeedGroupAdmins()
        {
            _groupAdmins = await SeedGroup("Admins", "Group for all administrator users.");
        }

        private async Task SeedGroupManagers()
        {
            _groupManagers = await SeedGroup("Managers", "Group for all manager users.");
        }

        private async Task SeedGroupRegulars()
        {
            _groupRegulars = await SeedGroup("Regulars", "Group for all basic users.");
        }

        private async Task SeedGroupLaughingstocks()
        {
            _groupLaughingstocks = await SeedGroup("Laughingstocks", "The infamous nothingworksright laughingstocks.");
        }

        #endregion GROUPS

        #region USERGROUPS

        private async Task<UserGroup> SeedUserGroup(long groupId, long userId)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.users_groups_create('{groupId}', '{userId}');";
                command.Connection = _connectionUseTestDb;
                await command.PrepareAsync();
                var reader = await command.ExecuteReaderAsync();
                var newUserGroup = new UserGroup();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    newUserGroup = map.UserGroup(reader);
                }
                await reader.CloseAsync();
                return newUserGroup;
            }
        }

        private async Task SeedUserGroupAdmin()
        {
            if (_groupAdmins is null || _userAdmin is null)
            {
                throw new Exception("Could not seed usergroup.");
            }
            await SeedUserGroup(_groupAdmins.Id, _userAdmin.Id);
        }

        private async Task SeedUserGroupManager()
        {
            if (_groupManagers is null || _userManager is null)
            {
                throw new Exception("Could not seed usergroup.");
            }
            await SeedUserGroup(_groupManagers.Id, _userManager.Id);
        }

        private async Task SeedUserGroupRegular()
        {
            if (_groupRegulars is null || _userRegular is null)
            {
                throw new Exception("Could not seed usergroup.");
            }
            await SeedUserGroup(_groupRegulars.Id, _userRegular.Id);
        }

        #endregion USERGROUPS

        #region PERMISSIONS

        private async Task<Permission> SeedPermission(long documentId, long? userId, long? groupId)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.permissions_create('{documentId}'";

                if (userId is not null) command.CommandText += $", '{userId}'";
                else command.CommandText += $", null";

                if (groupId is not null) command.CommandText += $", '{groupId}'";
                else command.CommandText += $", null";

                command.CommandText += ");";
                command.Connection = _connectionUseTestDb;
                await command.PrepareAsync();
                var reader = await command.ExecuteReaderAsync();
                var newPermission = new Permission();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    newPermission = map.Permission(reader);
                }
                await reader.CloseAsync();
                return newPermission;
            }
        }

        private async Task SeedPermissionApiDllAdminUser()
        {
            if (_documentApiDll is null || _userAdmin is null) throw new Exception("Could not create permission.");
            await SeedPermission(_documentApiDll.Id, _userAdmin.Id, null);
        }

        private async Task SeedPermissionDataDllAdminsGroup()
        {
            if (_documentDataDll is null || _groupAdmins is null) throw new Exception("Could not create permission.");
            await SeedPermission(_documentDataDll.Id, null, _groupAdmins.Id);
        }

        private async Task SeedPermissionDomainDllManagerUser()
        {
            if (_documentDomainDll is null || _userManager is null) throw new Exception("Could not create permission.");
            await SeedPermission(_documentDomainDll.Id, _userManager.Id, null);
        }

        private async Task SeedPermissionServiceDllManagersGroup()
        {
            if (_documentServiceDll is null || _groupManagers is null) throw new Exception("Could not create permission.");
            await SeedPermission(_documentServiceDll.Id, null, _groupManagers.Id);
        }

        private async Task SeedPermissionTestDllRegularUser()
        {
            if (_documentTestDll is null || _userRegular is null) throw new Exception("Could not create permission.");
            await SeedPermission(_documentTestDll.Id, _userRegular.Id, null);
        }

        #endregion PERMISSIONS
    }
}