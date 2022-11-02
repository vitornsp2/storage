
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;

namespace findox.Domain.Interfaces.Service
{
    public interface IUserGroupService
    {
        Task<ApiReponse> Create(UserGroupDto userGroupDto);
        Task<ApiReponse> DeleteById(long? id);
        Task<ApiReponse> DeleteByGroupId(long? id);
        Task<ApiReponse> DeleteByUserId(long? id);
    }
}