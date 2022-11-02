using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;

namespace findox.Domain.Interfaces.Service
{
    public interface IUserService : IBaseService
    {
        Task<ApiReponse> Create(UserDto userDto);
        Task<ApiReponse> ReadAll();
        Task<ApiReponse> ReadById(long? id);
        Task<ApiReponse> Update(UserDto userDto);
        Task<ApiReponse> UpdatePassword(UserDto userDto);
        Task<ApiReponse> DeleteById(long? id);
        Task<ApiReponse> CreateSession(UserSessionDto userSessionDto);
    }
}