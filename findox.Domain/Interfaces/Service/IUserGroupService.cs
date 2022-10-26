
using findox.Domain.Models.Service;

namespace findox.Domain.Interfaces.Service
{
    public interface IUserGroupService
    {
        Task<IUserGroupServiceResponse> Create(IUserGroupServiceRequest request);
        Task<IUserGroupServiceResponse> DeleteById(IUserGroupServiceRequest request);
        Task<IUserGroupServiceResponse> DeleteByGroupId(IUserGroupServiceRequest request);
        Task<IUserGroupServiceResponse> DeleteByUserId(IUserGroupServiceRequest request);
    }
}