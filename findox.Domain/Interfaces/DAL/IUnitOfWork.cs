using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Interfaces.Repository;

namespace findox.Domain.Interfaces.DAL
{
    public interface IUnitOfWork
    {
        IDocumentContentRepository documentContents { get; }
        IGroupRepository groups { get; }
        IDocumentRepository documents { get; }
        IUserGroupRepository userGroups { get; }
        IPermissionRepository permissions { get; }
        IUserRepository users { get; }
        void Begin();
        void Commit();
        void Rollback();
    }
}