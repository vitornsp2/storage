using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Dto;
using Microsoft.AspNetCore.Http;

namespace findox.Domain.Models.Service
{
    public interface IServiceRequest<T>
    {
        long? Id { get; }
        T? Item { get; }
        System.Security.Claims.ClaimsPrincipal? PrincipalUser { get; }
    }

    public class ServiceRequest<T> : IServiceRequest<T>
    {
        public long? Id { get; set; }

        public T? Item { get; set; }

        public System.Security.Claims.ClaimsPrincipal? PrincipalUser { get; set; }
    }

    public interface IUserServiceRequest : IServiceRequest<IUserDto>
    {
        IUserSessionDto? UserSessionDto { get; }
    }

    public class UserServiceRequest : ServiceRequest<IUserDto>, IUserServiceRequest
    {
        public IUserSessionDto? UserSessionDto { get; set; }

        public UserServiceRequest()
        {
            Item = new UserDto();
        }

        public UserServiceRequest(IUserDto user)
        {
            Item = user;
        }

        public UserServiceRequest(IUserSessionDto user)
        {
            UserSessionDto = user;
        }

        public UserServiceRequest(long id)
        {
            Id = id;
        }
    }

    public interface IDocumentServiceRequest : IServiceRequest<IDocumentDto>
    {
        IFormCollection? Metadata { get; }
        IFormFile? File { get; }
    }

    public class DocumentServiceRequest : ServiceRequest<IDocumentDto>, IDocumentServiceRequest
    {
        public IFormCollection? Metadata { get; set; }
        public IFormFile? File { get; set; }

        public DocumentServiceRequest()
        {
            Item = new DocumentDto();
        }

        public DocumentServiceRequest(IDocumentDto document)
        {
            Item = document;
        }

        public DocumentServiceRequest(long id)
        {
            Id = id;
        }

        public DocumentServiceRequest(IFormCollection metadata, IFormFile file, System.Security.Claims.ClaimsPrincipal principalUser)
        {
            Metadata = metadata;
            File = file;
            PrincipalUser = principalUser;
        }
    }

    public interface IGroupServiceRequest : IServiceRequest<IGroupDto>
    {

    }

    public class GroupServiceRequest : ServiceRequest<IGroupDto>, IGroupServiceRequest
    {
        public GroupServiceRequest()
        {
            Item = new GroupDto();
        }
        public GroupServiceRequest(IGroupDto group)
        {
            Item = group;
        }

        public GroupServiceRequest(long id)
        {
            Id = id;
        }
    }

    public interface IUserGroupServiceRequest : IServiceRequest<IUserGroupDto>
    {

    }

    public class UserGroupServiceRequest : ServiceRequest<IUserGroupDto>, IUserGroupServiceRequest
    {
        public UserGroupServiceRequest()
        {
            Item = new UserGroupDto();
        }

        public UserGroupServiceRequest(IUserGroupDto userGroup)
        {
            Item = userGroup;
        }

        public UserGroupServiceRequest(long id)
        {
            Id = id;
        }
    }

    public interface IPermissionServiceRequest : IServiceRequest<IPermissionDto>
    {

    }

    public class PermissionServiceRequest : ServiceRequest<IPermissionDto>, IPermissionServiceRequest
    {
        public PermissionServiceRequest()
        {
            Item = new PermissionDto();
        }

        public PermissionServiceRequest(IPermissionDto permission)
        {
            Item = permission;
        }

        public PermissionServiceRequest(long id)
        {
            Id = id;
        }
    }
}