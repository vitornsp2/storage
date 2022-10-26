using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace findox.Domain.Models.Service
{
    public enum OutcomeType : int
    {
        Error = 0,
        Fail = 1,
        Success = 2,
    }

    public interface IServiceResponse<T>
    {
        OutcomeType Outcome { get; }
        string? ErrorMessage { get; }
        long? Id { get; }
        T? Item { get; }
        List<T>? List { get; }
        string? Token { get; }
    }

    public class ServiceResponse<T> : IServiceResponse<T>
    {
        public OutcomeType Outcome { get; set; } = OutcomeType.Error;
        public string? ErrorMessage { get; set; }
        public long? Id { get; set; }
        public T? Item { get; set; }
        public List<T>? List { get; set; }
        public string? Token { get; set; }
    }

    public interface IUserServiceResponse : IServiceResponse<IUserDto>
    {
        IUserAllDto? AllDto { get; }
    }

    public class UserServiceResponse : ServiceResponse<IUserDto>, IUserServiceResponse
    {
        public IUserAllDto? AllDto { get; set; }

        public UserServiceResponse()
        {

        }

        public UserServiceResponse(IUserDto user)
        {
            Item = user;
        }

        public UserServiceResponse(List<IUserDto> user)
        {
            List = user;
        }
    }

    public interface IDocumentServiceResponse : IServiceResponse<IDocumentDto>
    {
        byte[]? BinaryData { get; }

        FileContentResult? FileContentResult { get; }
    }

    public class DocumentServiceResponse : ServiceResponse<IDocumentDto>, IDocumentServiceResponse
    {
        public byte[]? BinaryData { get; set; }

        public FileContentResult? FileContentResult { get; set; }

        public DocumentServiceResponse()
        {

        }

        public DocumentServiceResponse(IDocumentDto document)
        {
            Item = document;
        }

        public DocumentServiceResponse(List<IDocumentDto> document)
        {
            List = document;
        }
    }

    public interface IGroupServiceResponse : IServiceResponse<IGroupDto>
    {
        IGroupAllDto? AllDto { get; }
    }

    public class GroupServiceResponse : ServiceResponse<IGroupDto>, IGroupServiceResponse
    {
        public IGroupAllDto? AllDto { get; set; }

        public GroupServiceResponse()
        {

        }

        public GroupServiceResponse(IGroupDto group)
        {
            Item = group;
        }

        public GroupServiceResponse(List<IGroupDto> group)
        {
            List = group;
        }
    }

    public interface IUserGroupServiceResponse : IServiceResponse<IUserGroupDto>
    {

    }

    public class UserGroupServiceResponse : ServiceResponse<IUserGroupDto>, IUserGroupServiceResponse
    {
        public UserGroupServiceResponse()
        {

        }
        public UserGroupServiceResponse(IUserGroupDto userGroup)
        {
            Item = userGroup;
        }

        public UserGroupServiceResponse(List<IUserGroupDto> userGroup)
        {
            List = userGroup;
        }
    }

    public interface IPermissionServiceResponse : IServiceResponse<IPermissionDto>
    {

    }


    public class PermissionServiceResponse : ServiceResponse<IPermissionDto>, IPermissionServiceResponse
    {
        public PermissionServiceResponse()
        {

        }

        public PermissionServiceResponse(IPermissionDto permission)
        {
            Item = permission;
        }


        public PermissionServiceResponse(List<IPermissionDto> permission)
        {
            List = permission;
        }
    }

}