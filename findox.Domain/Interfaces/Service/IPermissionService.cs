using findox.Domain.Models.Service;
using findox.Domain.Models.Dto;

namespace findox.Domain.Interfaces.Service
{
    public interface IPermissionService
    {
        Task<ApiReponse> Create(PermissionDto permissionDto);
        Task<ApiReponse> ReadByDocumentId(long? id);
        Task<ApiReponse> DeleteById(long? id);
        Task<ApiReponse> DeleteByDocumentId(long? id);
        Task<ApiReponse> DeleteByUserId(long? id);
        Task<ApiReponse> DeleteByGroupId(long? id);
    }
}