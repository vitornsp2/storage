using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Interfaces.Repository;

namespace findox.Domain.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        IDocumentContentRepository DocumentContentsRepository { get; }
        IGroupRepository GroupsRepository { get; }
        IDocumentRepository DocumentsRepository { get; }
        IUserGroupRepository UserGroupsRepository { get; }
        IPermissionRepository PermissionsRepository { get; }
        IUserRepository UsersRepository { get; }
    }
}