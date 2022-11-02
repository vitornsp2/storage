using findox.Data.Repositories;
using findox.Domain.Interfaces.Repository;
using Npgsql;

namespace findox.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDocumentContentRepository DocumentContentsRepository { get; }
        public IGroupRepository GroupsRepository { get; }
        public IDocumentRepository DocumentsRepository { get; }
        public IUserGroupRepository UserGroupsRepository { get; }
        public IPermissionRepository PermissionsRepository { get; }
        public IUserRepository UsersRepository { get; }

        public UnitOfWork(
                    IDocumentContentRepository documentContentsRepository,
                    IGroupRepository groupsRepository,
                    IDocumentRepository documentsRepository,
                    IUserGroupRepository userGroupsRepository,
                    IPermissionRepository permissionsRepository,
                    IUserRepository usersRepository
        )
        {
            DocumentContentsRepository = documentContentsRepository;
            GroupsRepository = groupsRepository;
            DocumentsRepository = documentsRepository;
            UserGroupsRepository = userGroupsRepository;
            PermissionsRepository = permissionsRepository;
            UsersRepository = usersRepository;
        }
    }
}