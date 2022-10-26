

using findox.Domain.Models.Service;

namespace findox.Domain.Interfaces.Service
{
    public interface IPermissionService
    {
        Task<IPermissionServiceResponse> Create(IPermissionServiceRequest request);
        Task<IPermissionServiceResponse> ReadByDocumentId(IPermissionServiceRequest request);
        Task<IPermissionServiceResponse> DeleteById(IPermissionServiceRequest request);
        Task<IPermissionServiceResponse> DeleteByDocumentId(IPermissionServiceRequest request);
        Task<IPermissionServiceResponse> DeleteByUserId(IPermissionServiceRequest request);
        Task<IPermissionServiceResponse> DeleteByGroupId(IPermissionServiceRequest request);
    }
}